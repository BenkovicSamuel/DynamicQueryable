using System.Linq;
using DynamicQueryable.Core.Clauses;

namespace DynamicQueryable.Core.Models
{
    public interface IDynamicQueryableContext
    {
        dynamic GetDynamicQuery<T>(IQueryable<T> query) where T : class;
        IClauseValueManager ClauseValueManager { get; }
        IQueryable<dynamic> TotalCountQuery { get; }
        string QueryString { get; }
    }

}