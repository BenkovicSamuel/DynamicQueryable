using System;

namespace DynamicQueryable.Core.CriteriaFilters
{
    public interface IQueryableFilter
    {
        string Alias { get; set; }
        int Level { get; set; }
        IFormatProvider FormatProvider { get; set; }
    }
}