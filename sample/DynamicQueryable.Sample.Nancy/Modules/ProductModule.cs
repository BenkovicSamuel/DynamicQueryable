using System.Collections.Generic;
using DynamicQueryable.Extensions;
using Nancy;
using DynamicQueryable.Core.Models;
using DynamicQueryable.Nancy.Filter;
using DynamicQueryable.Sample.Nancy.Contexts;

namespace DynamicQueryable.Sample.Nancy.Modules
{
    public sealed class ProductModule : NancyModule
    {
        public ProductModule(DynamicQueryableDbContext dbContext, IDynamicQueryableContext autoQueryableContext, NancyQueryStringAccessor queryStringAccessor, NancyContext nancyContext) : base("/products")
        {
            queryStringAccessor.SetQueryString(nancyContext.Request.Url.Query);

            Get<dynamic>("/", args => Response.AsJson(dbContext.Product.DynamicQueryable(autoQueryableContext).ToAutoQueryListResult(autoQueryableContext) as ICollection<object>));

            Get<dynamic>("/withfilter", args =>
            {
                After.DynamicQueryable(Context, dbContext.Product, autoQueryableContext);
                return "";
            });
        }
    }
}