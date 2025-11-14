using DynamicQueryable.Core.Enums;

namespace DynamicQueryable.Core.Clauses
{
    public interface IClauseMapManager
    {
        void Init();
        IClauseQueryFilter FindClauseQueryFilter(string queryParameterKey);
        IClauseQueryFilter GetClauseQueryFilter(ClauseType clauseType);
    }
}