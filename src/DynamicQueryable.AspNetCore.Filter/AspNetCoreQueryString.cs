using System;
using System.Collections.Generic;
using System.Text;
using DynamicQueryable.Core.Models;
using DynamicQueryable.Core.Models.QueryStringAccessors;
using Microsoft.AspNetCore.Http;

namespace DynamicQueryable.AspNetCore.Filter
{
    public class AspNetCoreQueryStringAccessor : BaseQueryStringAccessor
    {
        public AspNetCoreQueryStringAccessor(IHttpContextAccessor httpContextAccessor)
        {
            var queryString = httpContextAccessor.HttpContext.Request.QueryString.Value;

            QueryString = Uri.UnescapeDataString(queryString ?? "");

            _setQueryParts();
        }
    }
}
