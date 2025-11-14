using DynamicQueryable.Core.Models.QueryStringAccessors;

namespace DynamicQueryable.Core.Models
{
    public class TestQueryStringAccessor : BaseQueryStringAccessor
    {
        public TestQueryStringAccessor(string queryString)
        {
            QueryString = queryString;
        }
    }
}
