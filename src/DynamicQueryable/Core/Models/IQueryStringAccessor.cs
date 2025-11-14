using System.Collections.Generic;

namespace DynamicQueryable.Core.Models
{
    public interface IQueryStringAccessor
    {
        string QueryString { get; }
        ICollection<QueryStringPart> QueryStringParts { get; }
    }
}