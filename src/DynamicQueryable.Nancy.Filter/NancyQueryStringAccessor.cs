using DynamicQueryable.Core.Models.QueryStringAccessors;

namespace DynamicQueryable.Nancy.Filter
{
    public class NancyQueryStringAccessor : BaseQueryStringAccessor
    {
        public void SetQueryString(string queryString)
        {
            QueryString = queryString;
        }
    }
}
