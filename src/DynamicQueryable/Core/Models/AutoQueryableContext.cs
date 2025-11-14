using System.Linq;
using DynamicQueryable.Core.Clauses;

namespace DynamicQueryable.Core.Models
{
    public class DynamicQueryableContext : IDynamicQueryableContext
    {
   private readonly IDynamicQueryHandler _dynamicQueryHandler;
        public IQueryable<dynamic> TotalCountQuery { get; private set; }
        public IClauseValueManager ClauseValueManager { get; private set; }
        public string QueryString { get; private set; }

    public DynamicQueryableContext(IDynamicQueryHandler dynamicQueryHandler)
        {
       _dynamicQueryHandler = dynamicQueryHandler;
        }

        public dynamic GetDynamicQuery<T>(IQueryable<T> query) where T : class
        {
          var result = _dynamicQueryHandler.GetDynamicQuery(query);
 TotalCountQuery = _dynamicQueryHandler.TotalCountQuery;
     ClauseValueManager = _dynamicQueryHandler.ClauseValueManager;
   QueryString = _dynamicQueryHandler.QueryString;
            return result;
        }
    }
}