using System.Linq;
using DynamicQueryable.AspNetCore.Filter.FilterAttributes;
using DynamicQueryable.Sample.EfCore.Contexts;
using Microsoft.AspNetCore.Mvc;
using DynamicQueryable.Core.Enums;

namespace DynamicQueryable.Sample.EfCore.Controllers
{
    [Route("odata/products")]
    public class ProductOdataController
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <example>http://localhost:5000/api/products</example>
        /// <example>http://localhost:5000/api/products?select=name&top=50&skip=10</example>
        /// <param name="context"></param>
        /// <returns></returns>
        //[DynamicQueryable(ProviderType = ProviderType.OData)]
        [HttpGet]
        public IQueryable Get([FromServices] DynamicQueryableDbContext context)
        {
            return context.Product;
        }
    }
}