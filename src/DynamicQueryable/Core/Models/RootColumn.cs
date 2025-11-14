using System;

namespace DynamicQueryable.Core.Models
{
    public class RootColumn : SelectColumn
    {
        public RootColumn(Type type) : base("", "", type)
        {
        }
    }
}