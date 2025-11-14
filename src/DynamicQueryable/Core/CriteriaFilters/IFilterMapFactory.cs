using System.Collections.Generic;

namespace DynamicQueryable.Core.CriteriaFilters
{
    public interface IReZisFilterMapFactory
    {
        ICollection<IQueryableTypeFilter> InitializeMap();
    }
}