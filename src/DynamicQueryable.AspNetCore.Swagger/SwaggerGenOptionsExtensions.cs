using Swashbuckle.AspNetCore.SwaggerGen;

namespace DynamicQueryable.AspNetCore.Swagger
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void AddDynamicQueryable(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.OperationFilter<DynamicQueryableOperationFilter>();
        }
    }
}