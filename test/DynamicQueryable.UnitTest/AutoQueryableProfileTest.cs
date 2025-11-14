using System;
using System.Collections.Generic;
using System.Linq;
using AutoQueryable.Core.Clauses;
using AutoQueryable.Core.Clauses.ClauseHandlers;
using AutoQueryable.Core.CriteriaFilters;
using AutoQueryable.Core.Enums;
using AutoQueryable.Core.Models;
using AutoQueryable.Extensions;
using AutoQueryable.UnitTest.Mock;
using AutoQueryable.UnitTest.Mock.Entities;
using FluentAssertions;
using Xunit;

namespace AutoQueryable.UnitTest
{
    public class AutoQueryableProfileTest
    {
        private readonly SimpleQueryStringAccessor _queryStringAccessor;
        private readonly IAutoQueryableProfile _profile;
        private readonly IAutoQueryableContext _autoQueryableContext;

        public AutoQueryableProfileTest()
        {
            var settings = new AutoQueryableSettings {DefaultToTake = 10};
            _profile = new AutoQueryableProfile(settings);
            _queryStringAccessor = new SimpleQueryStringAccessor();
            var selectClauseHandler = new DefaultSelectClauseHandler();
            var orderByClauseHandler = new DefaultOrderByClauseHandler();
            var wrapWithClauseHandler = new DefaultWrapWithClauseHandler();
            var clauseMapManager = new ClauseMapManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var clauseValueManager = new ClauseValueManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var criteriaFilterManager = new CriteriaFilterManager();
            var defaultAutoQueryHandler = new AutoQueryHandler(_queryStringAccessor,criteriaFilterManager ,clauseMapManager ,clauseValueManager, _profile);
            _autoQueryableContext = new AutoQueryableContext(defaultAutoQueryHandler);
        }

        [Fact]
        public void AllowOnlyOneClause()
        {
            using (var context = new AutoQueryableDbContext())
            {
                DataInitializer.InitializeSeed(context);
                _queryStringAccessor.SetQueryString("select=productId");

                _profile.AllowedClauses = ClauseType.Select;

                var query = context.Product.AutoQueryable(_autoQueryableContext) as IQueryable<object>;


                query.Count().Should().Be(DataInitializer.DefaultToTakeCount);
                var first = query.First();

                var propertiesCount = first.GetType().GetProperties().Length;
                propertiesCount.Should().Be(1);

                var name = first.GetType().GetProperty("productId").GetValue(first);
                name.Should().NotBeNull();
            }
        }

        [Fact]
        public void AllowMultipleClauses()
        {
            using (var context = new AutoQueryableDbContext())
            {
                DataInitializer.InitializeSeed(context);

                _queryStringAccessor.SetQueryString("select=productId&top=10&skip=100");
                _profile.AllowedClauses = ClauseType.Select | ClauseType.Top;

                var query = context.Product.AutoQueryable(_autoQueryableContext) as IQueryable<object>;

                query.Count().Should().Be(10);
                var first = query.First();

                var propertiesCount = first.GetType().GetProperties().Length;
                propertiesCount.Should().Be(1);

                var productid = first.GetType().GetProperty("productId").GetValue(first);
                productid.Should().Be(101);
            }
        }

        [Fact]
        public void AllowCaseInsensitiveClauses()
        {
            using (var context = new AutoQueryableDbContext())
            {
                DataInitializer.InitializeSeed(context);

                var autoQueryableContext = PrepareAutoQuery(_queryStringAccessor);
                _queryStringAccessor.SetQueryString($"NameContains:i=MyspecialProduct&Select=*");

                _profile.AllowedClauses = ClauseType.OrderBy | ClauseType.OrderByDesc | ClauseType.Filter | ClauseType.Select | ClauseType.WrapWith;

                var query = context.Product.AutoQueryable(_autoQueryableContext) as IQueryable<object>;

                query.Count().Should().Be(2);
                var first = query.First();
                
                var productid = first.GetType().GetProperty("ProductId").GetValue(first);
                productid.Should().Be(1001);
            }
        }

        [Fact]
        public void AllowCaseSpecialClauses()
        {
            using (var context = new AutoQueryableDbContext())
            {
                DataInitializer.InitializeSeed(context);

                var autoQueryableContext = PrepareAutoQuery(_queryStringAccessor);
                _queryStringAccessor.SetQueryString($"NameContains:i=1,3&Select=*");

                _profile.AllowedClauses = ClauseType.OrderBy | ClauseType.OrderByDesc | ClauseType.Filter | ClauseType.Select | ClauseType.WrapWith;

                var query = context.Product.AutoQueryable(_autoQueryableContext) as IQueryable<object>;

                query.Count().Should().Be(1);
            }
        }

        [Fact]
        public void PlusCharInNameCase()
        {
            using (var context = new AutoQueryableDbContext())
            {
                DataInitializer.InitializeSeed(context);

                var autoQueryableContext = PrepareAutoQuery(_queryStringAccessor);
                _queryStringAccessor.SetQueryString($"ColorContains:i=green + red");

                _profile.AllowedClauses = ClauseType.OrderBy | ClauseType.OrderByDesc | ClauseType.Filter | ClauseType.Select | ClauseType.WrapWith;

                var query = context.Product.AutoQueryable(_autoQueryableContext) as IQueryable<object>;

                query.Count().Should().Be(1);
                var first = query.First();

                var productid = first.GetType().GetProperty("ProductId").GetValue(first);
                productid.Should().Be(1005);
            }
        }

        [Fact]
        public void SortTest()
        {
            using (var context = new AutoQueryableDbContext())
            {
                DataInitializer.InitializeSeed(context);
               
                var autoQueryableContext = PrepareAutoQuery(_queryStringAccessor);
                _queryStringAccessor.SetQueryString($"orderby=Weight,ListPrice");

                //_profile.AllowedClauses = ClauseType.OrderBy | ClauseType.OrderByDesc;

                var query = context.Product.AutoQueryable(autoQueryableContext) as IQueryable<Product>;

                var result = query?.ToList();
                var manuallySorted = context.Product.ToList().OrderBy(x => x.Weight).ThenBy(product => product.ListPrice).ToList();

                result.Select(x => new { x.ListPrice, x.Weight }).Should()
                    .ContainInOrder(manuallySorted.Select(x => new { x.ListPrice, x.Weight }));
            }
        }

        //this test is currently not supported....
        [Fact]
        public void OrderByCategory()
        {
            using (var context = new AutoQueryableDbContext())
            {

                DataInitializer.InitializeSeed(context);

                var autoQueryableContext = PrepareAutoQuery(_queryStringAccessor);
                _queryStringAccessor.SetQueryString("orderby=productExtension.name");
                var query = context.Product.AutoQueryable(autoQueryableContext) as IQueryable<Product>;
                var list = query?.ToList();
                list.First().ProductExtension.Name.Should().NotBe("aaa");
                var second = query.Last();
                list.First().ProductExtension.Name.Should().NotBe("zzz");
            }
        }

        private const ClauseType AllowedClauses = ClauseType.OrderBy | ClauseType.OrderByDesc | ClauseType.Filter | ClauseType.Select | ClauseType.WrapWith;

        private static AutoQueryableContext PrepareAutoQuery(SimpleQueryStringAccessor queryStringAccessor)
        {
            var settings = new AutoQueryableSettings();
            settings.AllowedClauses = AllowedClauses;
            settings.UseBaseType = true;
            settings.MaxDepth = 2;
            settings.DefaultToTake = 0;
            settings.DefaultOrderBy = null;
            IAutoQueryableProfile profile = new AutoQueryableProfile(settings);
            var selectClauseHandler = new DefaultSelectClauseHandler();
            var orderByClauseHandler = new DefaultOrderByClauseHandler();
            var wrapWithClauseHandler = new DefaultWrapWithClauseHandler();
            var clauseMapManager = new ClauseMapManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, profile);
            var clauseValueManager = new ClauseValueManagerOverride(new ClauseValueManager(selectClauseHandler,
                orderByClauseHandler, wrapWithClauseHandler, profile));
            var criteriaFilterManager = new CriteriaFilterManager();
            var defaultAutoQueryHandler = new AutoQueryHandler(queryStringAccessor, criteriaFilterManager, clauseMapManager, clauseValueManager, profile);
            return new AutoQueryableContext(defaultAutoQueryHandler);
        }
    }
}
