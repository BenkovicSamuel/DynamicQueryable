using System;
using DynamicQueryable.Core.Enums;
using DynamicQueryable.Core.Models;

namespace DynamicQueryable.Core.Clauses
{
    public interface IClauseQueryFilter
    {
        string Alias { get; set; }
        ClauseType ClauseType { get; set; }
        object ParseValue(string value, Type type, IDynamicQueryableProfile profile);
    }
}