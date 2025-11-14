using System;
using System.Collections.Generic;
using System.Linq;
using DynamicQueryable.Core.Clauses;
using DynamicQueryable.Core.Clauses.ClauseHandlers;
using DynamicQueryable.Core.CriteriaFilters;
using DynamicQueryable.Core.Models;
using DynamicQueryable.Extensions;
using DynamicQueryable.UnitTest.Mock;
using DynamicQueryable.UnitTest.Mock.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace DynamicQueryable.UnitTest
{
    public class FilterTest
    {
        private readonly SimpleQueryStringAccessor _queryStringAccessor;
        private readonly IDynamicQueryableContext _autoQueryableContext;
        private DynamicQueryableDbContext context;

        public FilterTest()
        {
            var settings = new DynamicQueryableSettings();
            IDynamicQueryableProfile profile = new DynamicQueryableProfile(settings);
            _queryStringAccessor = new SimpleQueryStringAccessor();
            var selectClauseHandler = new DefaultSelectClauseHandler();
            var orderByClauseHandler = new DefaultOrderByClauseHandler();
            var wrapWithClauseHandler = new DefaultWrapWithClauseHandler();
            var clauseMapManager = new ClauseMapManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, profile);
            var clauseValueManager = new ClauseValueManager(selectClauseHandler, orderByClauseHandler, wrapWithClauseHandler, profile);
            var criteriaFilterManager = new CriteriaFilterManager();
            var defaultDynamicQueryHandler = new DynamicQueryHandler(_queryStringAccessor, criteriaFilterManager, clauseMapManager, clauseValueManager, profile);
            _autoQueryableContext = new DynamicQueryableContext(defaultDynamicQueryHandler);

            this.context = new DynamicQueryableDbContext();
            DataInitializer.InitializeSeed(context);
        }


        [Fact]
        public void IdEquals5()
        {
            _queryStringAccessor.SetQueryString("productid=5");

            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(1);
            var first = query.First();
            var id = first.GetType().GetProperty("ProductId").GetValue(first);
            id.Should().Be(5);
        }

        [Fact]
        public void IdEquals3Or4Or5()
        {

            _queryStringAccessor.SetQueryString("productid=3/,4/,5");

            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(3);

            foreach (var product in query)
            {
                var id = int.Parse(product.GetType().GetProperty("ProductId").GetValue(product).ToString());
                id.Should().BeOneOf(3, 4, 5);
            }
        }
        [Fact]
        public void ProductCateqoryIdEquals1()
        {

            _queryStringAccessor.SetQueryString("ProductCategory.ProductCategoryId=1");

            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(DataInitializer.LoopProductSampleCountDiv0 + DataInitializer.ProductSampleExtraCount);
        }

        [Fact]
        public void IdEquals3And4()
        {

            _queryStringAccessor.SetQueryString("productid=3&productid=4");

            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(0);
        }

        [Fact]
        public void IdEquals3Or4And5Or6()
        {

            _queryStringAccessor.SetQueryString("productid=3/,4&productid=5/,6");

            DataInitializer.InitializeSeed(context);
            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(0);
        }
        [Fact]
        public void NullablePropertyEquals()
        {

            _queryStringAccessor.SetQueryString("name=Product 23");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(1);
        }
        [Fact]
        public void NameEqualsNull()
        {

            _queryStringAccessor.SetQueryString("name=null");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(DataInitializer.LoopProductSampleCountDiv0);
            query.Should().OnlyContain(product => product.GetType().GetProperty("Name").GetValue(product) == null);
        }
        [Fact]
        public void NameNotEqualsNull()
        {

            _queryStringAccessor.SetQueryString("name!=null");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(DataInitializer.LoopProductSampleCountDiv1 + DataInitializer.ProductSampleExtraCount);
            query.Should().OnlyContain(product => product.GetType().GetProperty("Name").GetValue(product) != null);
        }

        [Fact]
        public void NullableValueContains()
        {

            _queryStringAccessor.SetQueryString("namecontains=Product");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(DataInitializer.LoopProductSampleCountDiv1);
        }

        [Fact]
        public void ContainsIgnoreCase()
        {

            _queryStringAccessor.SetQueryString("namecontains:i=proDuct");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            //each %2 have name product x
            query.Count().Should().Be(DataInitializer.LoopProductSampleCountDiv0);
        }
        [Fact]
        public void StartsWith()
        {

            _queryStringAccessor.SetQueryString("namestartsWith=Prod");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(DataInitializer.LoopProductSampleCount / 2);
        }
        [Fact]
        public void NotStartsWith()
        {

            _queryStringAccessor.SetQueryString("nameStartsWith!=Prod");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(DataInitializer.LoopProductSampleCountDiv0 + DataInitializer.ProductSampleExtraCount);
        }
        [Fact]
        public void StartsWithIgnoreCase()
        {

            _queryStringAccessor.SetQueryString("namestartsWith:i=prod");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(DataInitializer.LoopProductSampleCount / 2);
        }
        [Fact]
        public void NotStartsWithIgnoreCase()
        {

            const string nameCheck = "prodUct 10";
            _queryStringAccessor.SetQueryString($"namestartsWith:i!={nameCheck}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Should().OnlyContain(product =>
                product.GetType().GetProperty("Name").GetValue(product) == null
                || !product.GetType().GetProperty("Name").GetValue(product).ToString().StartsWith(nameCheck, StringComparison.OrdinalIgnoreCase));
        }
        [Fact]
        public void NullableValueEndsWith()
        {

            _queryStringAccessor.SetQueryString("nameEndsWith=119");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(1);
        }
        [Fact]
        public void EndsWithIgnoreCase()
        {

            _queryStringAccessor.SetQueryString("nameEndsWith:i=cT 459");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Count().Should().Be(1);
        }
        [Fact]
        public void NotEndsWithIgnoreCase()
        {

            const string nameCheck = "dUcT 100";
            _queryStringAccessor.SetQueryString($"nameEndsWith:i!={nameCheck}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;
            query.Should().OnlyContain(product =>
                product.GetType().GetProperty("Name").GetValue(product) == null
                || !product.GetType().GetProperty("Name").GetValue(product).ToString().EndsWith(nameCheck, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void RowGuidEqualsGuidString()
        {

            _queryStringAccessor.SetQueryString($"rowguid={DataInitializer.GuidString}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(DataInitializer.ProductSampleCount);
            var first = query.First();
            var id = first.GetType().GetProperty("Rowguid").GetValue(first);
            id.Should().Be(Guid.Parse(DataInitializer.GuidString));
        }

        [Fact]
        public void ColorEqualsRed()
        {

            _queryStringAccessor.SetQueryString("color=red");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be((DataInitializer.LoopProductSampleCountDiv0 + DataInitializer.ProductSampleExtraCount -1));
        }

        [Fact]
        public void ColorEqualsRedOrBlack()
        {

            _queryStringAccessor.SetQueryString("color=red/,black");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(DataInitializer.LoopProductSampleCount + 4);
        }

        [Fact]
        public void SellStartDateEqualsTodayJsonFormatted()
        {

            var todayJsonFormated = DateTime.Today.ToString("yyyy-MM-dd");
            _queryStringAccessor.SetQueryString($"SellStartDate={todayJsonFormated}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(1);
            var first = query.First();
            var sellStartDate = DateTime.Parse(first.GetType().GetProperty("SellStartDate").GetValue(first).ToString());
            sellStartDate.Should().Be(DateTime.Today);
        }

        [Fact]
        public void SellStartDateEqualsTodayOrTodayPlus8HourJsonFormatted()
        {

            var todayJsonFormated = DateTime.Today.ToString("yyyy-MM-dd");
            var todayPlus8HourJsonFormated = DateTime.Today.AddHours(8).ToString("yyyy-MM-ddThh:mm:ss");
            _queryStringAccessor.SetQueryString($"SellStartDate={todayJsonFormated}/,{todayPlus8HourJsonFormated}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(2);
            foreach (var product in query)
            {
                var sellStartDate = DateTime.Parse(product.GetType().GetProperty("SellStartDate").GetValue(product).ToString());
                sellStartDate.Should().BeOneOf(DateTime.Today, DateTime.Today.AddHours(8));
            }
        }

        [Fact]
        public void SalesOrderDetailUnitPriceEquals2()
        {

            _queryStringAccessor.SetQueryString("SalesOrderDetail.UnitPrice=2");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(1);
        }

        [Fact]
        public void SalesOrderDetailUnitProductIdEquals1()
        {

            _queryStringAccessor.SetQueryString("SalesOrderDetail.Product.ProductId=1");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(1);
        }

        [Fact]
        public void DateEquals()
        {

            _queryStringAccessor.SetQueryString($"SellStartDate={DateTime.Today.AddHours(8 * 2):o}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(1);
            var first = query.First();
            var id = int.Parse(first.GetType().GetProperty("ProductId").GetValue(first).ToString());
            id.Should().Be(3);
        }

        [Fact]
        public void DateLessThan()
        {

            _queryStringAccessor.SetQueryString($"SellStartDate<{DateTime.Today.AddHours(8 * 2):o}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(2);
        }

        [Fact]
        public void DateLessThanEquals()
        {

            _queryStringAccessor.SetQueryString($"SellStartDate<={DateTime.Today.AddHours(8 * 2):o}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(3);
        }

        [Fact]
        public void DateGreaterThan()
        {

            _queryStringAccessor.SetQueryString($"SellStartDate>{DateTime.Today.AddHours(8 * 2):o}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(DataInitializer.ProductSampleCount - 3);
        }

        [Fact]
        public void DateGreaterThanEquals()
        {

            _queryStringAccessor.SetQueryString($"SellStartDate>={DateTime.Today.AddHours(8 * 2):o}");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(DataInitializer.ProductSampleCount - 2);
        }

        [Fact]
        public void DateYearEquals2010()
        {

            _queryStringAccessor.SetQueryString("SellStartDate:Year=2010");


            DataInitializer.AddDateTimeSeeds(context);
            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;


            query.Count().Should().Be(1);
        }
        [Fact]
        public void DateYearShouldNotEqual2011()
        {

            _queryStringAccessor.SetQueryString("SellStartDate:Year=2011");


            DataInitializer.AddDateTimeSeeds(context);
            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(0);
        }

        [Fact]
        public void FilterWithDecimalPoints_Query_ResultsCountShouldBeOne()
        {

            _queryStringAccessor.SetQueryString("ListPrice=1.6");


            var query = context.Product.DynamicQueryable(_autoQueryableContext) as IQueryable<object>;

            query.Count().Should().Be(1);
        }
    }
}