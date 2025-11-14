using System.Linq;
using DynamicQueryable.AspNetCore.Filter.FilterAttributes;
using DynamicQueryable.AspNetCore.Swagger;
using DynamicQueryable.Core.Enums;
using DynamicQueryable.Core.Models;
using DynamicQueryable.Extensions;
using DynamicQueryable.Sample.EfCore.Contexts;
using DynamicQueryable.Sample.EfCore.Dtos;
using DynamicQueryable.Sample.EfCore.Entities;
using Microsoft.AspNetCore.Mvc;


namespace DynamicQueryable.Sample.EfCore.Controllers
{
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IDynamicQueryableContext _autoQueryableContext;
        private readonly IDynamicQueryableProfile _profile;

        public ProductController(IDynamicQueryableContext autoQueryableContext, IDynamicQueryableProfile profile)
        {
            _autoQueryableContext = autoQueryableContext;
            _profile = profile;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <example>http://localhost:5000/api/products</example>
        /// <example>http://localhost:5000/api/products?select=name&top=50&skip=10</example>
        /// <param name="context"></param>
        /// <returns></returns>
        
        [DynamicQueryable]
        [HttpGet]
        public IQueryable Get([FromServices] DynamicQueryableDbContext context)
        {
            return context.Product;
        }
        
        [DynamicQueryable(DisAllowedClauses = ClauseType.Select)]
        [HttpGet("with_disallow")]
        public IQueryable GetWithDisallow([FromServices] DynamicQueryableDbContext context)
        {
            return context.Product;
        }
        
        
        [DynamicQueryable(DefaultToSelect = "name")]
        [HttpGet("with_default")]
        public IQueryable GetWithDefault([FromServices] DynamicQueryableDbContext context)
        {
            return context.Product;
        }

        /// <summary>
        /// </summary>
        /// <example>http://localhost:5000/api/products/with_dto_projection</example>
        /// <example>http://localhost:5000/api/products/with_dto_projection?select=name,category.name</example>
        /// <param name="context"></param>
        /// <returns></returns>
        //[DynamicQueryable]
        [HttpGet("with_dto_projection")]
        public IQueryable GetWithDtoProjection([FromServices] DynamicQueryableDbContext context)
        {
            return context.Product.Select(p => new ProductDto
            {
                Name = p.Name,
                Category = new ProductCategoryDto
                {
                    Name = p.ProductCategory.Name
                }
            });
        }
        
        [HttpGet("swagger_without_aq_attr")]
        [DynamicQueryableSwagger]
        public IQueryable GetSwaggerWithoutAqAttr([FromServices] DynamicQueryableDbContext context)
        {
            return context.Product.Select(p => new ProductDto
            {
                Name = p.Name,
                Category = new ProductCategoryDto
                {
                    Name = p.ProductCategory.Name
                }
            }).DynamicQueryable(_autoQueryableContext);
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [HttpGet("disallow")]
        public dynamic GetWithNotAllowedClauses([FromServices] DynamicQueryableDbContext context)
        {
           _profile.AllowedClauses = ClauseType.Select | ClauseType.Skip | ClauseType.OrderBy | ClauseType.OrderByDesc | ClauseType.WrapWith | ClauseType.Filter;
            _profile.MaxToTake = 5;

//                MaxToSkip = 5,
//                MaxDepth = 2,
////                    SelectableProperties = new[] { "name", "color" },
//                DisAllowedConditions = ConditionType.Contains | ConditionType.Less,
//                SortableProperties = new []{"color"},
//                AllowedWrapperPartType = WrapperPartType.Count
//            };
            return context.Product.DynamicQueryable(_autoQueryableContext);
        }
    }
}
