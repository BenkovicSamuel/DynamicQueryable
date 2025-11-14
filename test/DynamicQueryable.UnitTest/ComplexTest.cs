using DynamicQueryable.Core.Clauses;
using DynamicQueryable.Core.Clauses.ClauseHandlers;
using DynamicQueryable.Core.CriteriaFilters;
using DynamicQueryable.Core.Models;
using DynamicQueryable.UnitTest.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DynamicQueryable.Extensions;
using FluentAssertions;
using DynamicQueryable.UnitTest.Mock.Entities;

namespace DynamicQueryable.UnitTest
{
    public class ComplexTest
    {
        private readonly SimpleQueryStringAccessor _queryStringAccessor;
        private readonly IDynamicQueryableProfile _profile;
        private readonly IDynamicQueryableContext _autoQueryableContext;

        public ComplexTest()
        {
            var settings = new DynamicQueryableSettings { DefaultToTake = 10, UseBaseType = true };
            _profile = new DynamicQueryableProfile(settings);
            _queryStringAccessor = new SimpleQueryStringAccessor();
            var selectClauseHandler = new DefaultSelectClauseHandler();
            var orderByClauseHandler = new DefaultOrderByClauseHandler();
            var wrapWithClauseHandler = new DefaultWrapWithClauseHandler();
            var clauseMapManager = new ClauseMapManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var clauseValueManager = new ClauseValueManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var criteriaFilterManager = new CriteriaFilterManager();
            var defaultDynamicQueryHandler = new DynamicQueryHandler(_queryStringAccessor, criteriaFilterManager, clauseMapManager, clauseValueManager, _profile);
            _autoQueryableContext = new DynamicQueryableContext(defaultDynamicQueryHandler);
        }
        [Fact]
        public void SelectAllProducts()
        {
            using (var context = new DynamicQueryableDbContext())
            {

                _queryStringAccessor.SetQueryString("nameContains:i=MySpecialProduct&select=MyComplexClass");

                DataInitializer.InitializeSeed(context);
                var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<Product>;
                query.Last().MyComplexClass.Count().Should().Be(1);
            }
        }
    }
}
