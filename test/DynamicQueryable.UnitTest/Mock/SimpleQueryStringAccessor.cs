using DynamicQueryable.Core.Models;
using DynamicQueryable.Core.Models.QueryStringAccessors;

namespace DynamicQueryable.UnitTest.Mock
{
    public class SimpleQueryStringAccessor : BaseQueryStringAccessor
    {
        public void SetQueryString(string queryString)
        {
            QueryString = queryString;
        }
    }
}
