using System;
using DynamicQueryable.Core.Clauses;
using DynamicQueryable.Core.Clauses.ClauseHandlers;
using DynamicQueryable.Core.CriteriaFilters;
using DynamicQueryable.Core.Models;
using Nancy.TinyIoc;

namespace DynamicQueryable.Extensions.TinyIocContainer
{
    public static class DynamicQueryableServiceCollectionExtensions
    {
        public static void RegisterDynamicQueryable(this TinyIoCContainer container, Action<DynamicQueryableSettings> handler = null)
        {
            var settings = new DynamicQueryableSettings();
            handler?.Invoke(settings);
            container.Register<IDynamicQueryableContext, DynamicQueryableContext>();
            container.Register(settings);
            container.Register<IDynamicQueryableProfile, DynamicQueryableProfile>();
            container.Register<IDynamicQueryHandler, DynamicQueryHandler>();
            container.Register<IClauseValueManager, ClauseValueManager>();
            container.Register<ICriteriaFilterManager, CriteriaFilterManager>();
            container.Register<IClauseMapManager, ClauseMapManager>();
            container.Register<ISelectClauseHandler, DefaultSelectClauseHandler>();
            container.Register<IOrderByClauseHandler, DefaultOrderByClauseHandler>();
            container.Register<IWrapWithClauseHandler, DefaultWrapWithClauseHandler>();
        }
    }
}