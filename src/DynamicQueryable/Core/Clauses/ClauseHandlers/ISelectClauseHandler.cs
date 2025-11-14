using System.Collections.Generic;
using DynamicQueryable.Core.Models;

namespace DynamicQueryable.Core.Clauses.ClauseHandlers
{
    public interface ISelectClauseHandler : IClauseHandler<ICollection<SelectColumn>>
    {

    }
}