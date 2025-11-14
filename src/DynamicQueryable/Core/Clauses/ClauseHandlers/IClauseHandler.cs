using System;
using DynamicQueryable.Core.Models;

namespace DynamicQueryable.Core.Clauses.ClauseHandlers
{
    public interface IClauseHandler<T> where T : class 
    {
        T Handle(string queryStringPart, Type type = default, IDynamicQueryableProfile profile = null);
    }
}