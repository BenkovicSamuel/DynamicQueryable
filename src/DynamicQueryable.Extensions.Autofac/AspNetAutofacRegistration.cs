using System;
using Autofac;
using DynamicQueryable.Core.Clauses;
using DynamicQueryable.Core.Clauses.ClauseHandlers;
using DynamicQueryable.Core.CriteriaFilters;
using DynamicQueryable.Core.Models;

namespace DynamicQueryable.Extensions.Autofac
{
    public static class AspNetAutofacRegistration
    {
        public static void RegisterDynamicQueryable(this ContainerBuilder builder, Action<DynamicQueryableSettings> handler = null)
        {
            var settings = new DynamicQueryableSettings();
            handler?.Invoke(settings);
            builder.RegisterType<DynamicQueryableContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.Register(c => settings).As<DynamicQueryableSettings>().SingleInstance();
            builder.RegisterType<DynamicQueryableProfile>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DynamicQueryHandler>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ClauseValueManager>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CriteriaFilterManager>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ClauseMapManager>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DefaultSelectClauseHandler>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DefaultOrderByClauseHandler>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DefaultWrapWithClauseHandler>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }

}
