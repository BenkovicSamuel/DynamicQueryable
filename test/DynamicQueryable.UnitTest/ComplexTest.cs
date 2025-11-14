using AutoQueryable.Core.Clauses;
using AutoQueryable.Core.Clauses.ClauseHandlers;
using AutoQueryable.Core.CriteriaFilters;
using AutoQueryable.Core.Models;
using AutoQueryable.UnitTest.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoQueryable.Extensions;
using FluentAssertions;
using AutoQueryable.UnitTest.Mock.Entities;

namespace AutoQueryable.UnitTest
{
    public class ComplexTest
    {
        private readonly SimpleQueryStringAccessor _queryStringAccessor;
        private readonly IAutoQueryableProfile _profile;
        private readonly IAutoQueryableContext _autoQueryableContext;

        public ComplexTest()
        {
            var settings = new AutoQueryableSettings { DefaultToTake = 10, UseBaseType = true };
            _profile = new AutoQueryableProfile(settings);
            _queryStringAccessor = new SimpleQueryStringAccessor();
            var selectClauseHandler = new DefaultSelectClauseHandler();
            var orderByClauseHandler = new DefaultOrderByClauseHandler();
            var wrapWithClauseHandler = new DefaultWrapWithClauseHandler();
            var clauseMapManager = new ClauseMapManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var clauseValueManager = new ClauseValueManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, _profile);
            var criteriaFilterManager = new CriteriaFilterManager();
            var defaultAutoQueryHandler = new AutoQueryHandler(_queryStringAccessor, criteriaFilterManager, clauseMapManager, clauseValueManager, _profile);
            _autoQueryableContext = new AutoQueryableContext(defaultAutoQueryHandler);
        }
        [Fact]
        public void SelectAllProducts()
        {
            using (var context = new AutoQueryableDbContext())
            {

                _queryStringAccessor.SetQueryString("nameContains:i=MySpecialProduct&select=MyComplexClass");

                DataInitializer.InitializeSeed(context);
                var query = context.Product.AutoQueryable(_autoQueryableContext) as IQueryable<Product>;
                query.Last().MyComplexClass.Count().Should().Be(1);
            }
        }
    }
}
