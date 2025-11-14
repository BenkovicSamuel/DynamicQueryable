using System;
using System.Collections.Generic;
using System.Linq;
using DynamicQueryable.Core.Clauses;
using DynamicQueryable.Core.Clauses.ClauseHandlers;
using DynamicQueryable.Core.CriteriaFilters;
using DynamicQueryable.Core.Models;
using DynamicQueryable.Extensions;
using DynamicQueryable.Helpers;
using DynamicQueryable.UnitTest.Mock;
using DynamicQueryable.UnitTest.Mock.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace DynamicQueryable.UnitTest
{
    public class BaseTypeTest
    {
        private readonly SimpleQueryStringAccessor _queryStringAccessor;
        private IDynamicQueryableProfile _profile;
        private readonly IDynamicQueryableContext _autoQueryableContext;

        public BaseTypeTest()
        {
            var settings = new DynamicQueryableSettings {DefaultToTake = 10};
            _profile = new DynamicQueryableProfile(settings);
            _queryStringAccessor = new SimpleQueryStringAccessor();
            var selectClauseHandler = new DefaultSelectClauseHandler();
            var orderByClauseHandler = new DefaultOrderByClauseHandler();
            var wrapWithClauseHandler = new DefaultWrapWithClauseHandler();
            var clauseMapManager = new ClauseMapManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var clauseValueManager = new ClauseValueManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var criteriaFilterManager = new CriteriaFilterManager();
            _autoQueryableContext = new DynamicQueryableContext(new DynamicQueryHandler( _queryStringAccessor,criteriaFilterManager ,clauseMapManager ,clauseValueManager, _profile));
        }

        [Fact]
        public void SelectAllProducts()
        {
            using (var context = new DynamicQueryableDbContext())
            {
                DataInitializer.InitializeSeed(context);
                var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
                query.Count().Should().Be(DataInitializer.DefaultToTakeCount);
            }
        }
        
        [Fact]
        public void CreateAqWithUseBaseTypeAndUnSelectable_Query_CheckIfResultsDoesNotContainsUnselectabe()
        {
            using (var context = new DynamicQueryableDbContext())
            {
                _queryStringAccessor.SetQueryString("namecontains:i=product");
                DataInitializer.InitializeSeed(context);
                _profile.UseBaseType = true;
                _profile.UnselectableProperties = typeof(Product).GetProperties().Where(p => p.Name != "Name")
                    .Select(p => p.Name).ToArray();
                var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
                var products = (query as IEnumerable<Product>)?.ToList();
                products.Should().NotBeNull();
                products.Should().NotContain(p => p.Color != null || p.Id != 0);
                products.Count().Should().Be(DataInitializer.DefaultToTakeCount);
            }
        }
    }
}