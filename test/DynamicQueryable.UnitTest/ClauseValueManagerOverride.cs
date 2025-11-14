using AutoQueryable.Core.Clauses;
using AutoQueryable.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoQueryable.UnitTest
{
    public class ClauseValueManagerOverride : IClauseValueManager
    {
        private readonly IClauseValueManager _defaultClauseValueManager;

        public ClauseValueManagerOverride(IClauseValueManager defaultClauseValueManager)
        {
            _defaultClauseValueManager = defaultClauseValueManager;
        }

        /// <summary>
        /// This is why this class exists
        /// </summary>
        /// <param name="type"></param>
        public void SetDefaults(Type type)
        {
            var selectBeforeDefaults = Select?.ToList();

            _defaultClauseValueManager.SetDefaults(type);

            //Remove default selection, this will keep included statements from EF core intact unless explicitly overriden
            Select = selectBeforeDefaults ?? new List<SelectColumn>();
        }

        public bool First
        {
            get { return _defaultClauseValueManager.First; }
            set { _defaultClauseValueManager.First = value; }
        }

        public string GroupBy
        {
            get { return _defaultClauseValueManager.GroupBy; }
            set { _defaultClauseValueManager.GroupBy = value; }
        }

        public bool Last
        {
            get { return _defaultClauseValueManager.Last; }
            set { _defaultClauseValueManager.Last = value; }
        }

        public Dictionary<string, bool> OrderBy
        {
            get { return _defaultClauseValueManager.OrderBy; }
            set { _defaultClauseValueManager.OrderBy = value; }
        }

        public int? Page
        {
            get { return _defaultClauseValueManager.Page; }
            set { _defaultClauseValueManager.Page = value; }
        }

        public int? PageSize
        {
            get { return _defaultClauseValueManager.PageSize; }
            set { _defaultClauseValueManager.PageSize = value; }
        }

        public ICollection<SelectColumn> Select
        {
            get { return _defaultClauseValueManager.Select; }
            set { _defaultClauseValueManager.Select = value; }
        }

        public int? Skip
        {
            get { return _defaultClauseValueManager.Skip; }
            set { _defaultClauseValueManager.Skip = value; }
        }

        public int? Take
        {
            get { return _defaultClauseValueManager.Take; }
            set { _defaultClauseValueManager.Take = value; }
        }

        public int? Top
        {
            get { return _defaultClauseValueManager.Top; }
            set { _defaultClauseValueManager.Top = value; }
        }

        public IEnumerable<string> WrapWith
        {
            get { return _defaultClauseValueManager.WrapWith; }
            set { _defaultClauseValueManager.WrapWith = value; }
        }

    }
}
