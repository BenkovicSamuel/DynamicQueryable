using System;
using DynamicQueryable.AspNetCore.Filter.FilterAttributes;

namespace DynamicQueryable.AspNetCore.Swagger
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DynamicQueryableSwaggerAttribute : Attribute, IDynamicQueryableAttribute
    {
    }
}