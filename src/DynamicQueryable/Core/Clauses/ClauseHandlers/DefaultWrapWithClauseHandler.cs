using System;
using System.Collections.Generic;
using System.Linq;
using DynamicQueryable.Core.Models;

namespace DynamicQueryable.Core.Clauses.ClauseHandlers
{
    public class DefaultWrapWithClauseHandler : IWrapWithClauseHandler
    {
        public IEnumerable<string> Handle(string wrapWithQueryStringPart, Type type = default, IDynamicQueryableProfile profile = null)
        {
            return wrapWithQueryStringPart.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower());
        }
    }
}
