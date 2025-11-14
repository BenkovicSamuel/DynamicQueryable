using System;
using System.Collections.Generic;
using DynamicQueryable.Core.Models;

namespace DynamicQueryable.Core.Clauses.ClauseHandlers
{
    public class DefaultOrderByClauseHandler : IOrderByClauseHandler
    {
        public Dictionary<string, bool> Handle(string orderByQueryStringPart, Type type = default, IDynamicQueryableProfile profile = null)
        {
            var orderByValues = new Dictionary<string, bool>();
            foreach(var q in orderByQueryStringPart.Split(','))
            {
                if(q.StartsWith("-"))
                {
                    orderByValues.Add(q.Trim('-'), true);
                }else
                {
                    orderByValues.Add(q.Trim('+'), false);
                }
            }
            return orderByValues;
        }
    }
}