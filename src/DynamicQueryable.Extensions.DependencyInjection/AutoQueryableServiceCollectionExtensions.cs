using System;
using DynamicQueryable.AspNetCore.Filter;
using DynamicQueryable.AspNetCore.Filter.FilterAttributes;
using DynamicQueryable.Core.Clauses;
using DynamicQueryable.Core.Clauses.ClauseHandlers;
using DynamicQueryable.Core.CriteriaFilters;
using DynamicQueryable.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DynamicQueryable.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extension methods for setting up DynamicQueryable Framework related services in an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
    /// </summary>
    public static class DynamicQueryableServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the minimum essential DynamicQueryable services to the specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />. Additional services
        /// including default clause handlers.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add services to.</param>
        /// <param name="handler">An <see cref="T:System.Action`1" /> to configure the provided <see cref="T:DynamicQueryable.Core.Models.DynamicQueryableProfile" />.</param>
        /// <returns>An <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> that can be used to further other services.</returns>
        public static IServiceCollection AddDynamicQueryable(this IServiceCollection services, Action<DynamicQueryableSettings> handler = null)
        {
            var settings = new DynamicQueryableSettings();
            handler?.Invoke(settings);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services.AddScoped<IDynamicQueryableContext, DynamicQueryableContext>()
                .AddSingleton(_ => settings)
                .AddScoped<IDynamicQueryableProfile, DynamicQueryableProfile>()
                .AddScoped<IDynamicQueryHandler, DynamicQueryHandler>()
                .AddScoped<IClauseValueManager, ClauseValueManager>()
                .AddScoped<ICriteriaFilterManager, CriteriaFilterManager>()
                .AddScoped<IClauseMapManager, ClauseMapManager>()
                .AddScoped<ISelectClauseHandler, DefaultSelectClauseHandler>()
                .AddScoped<IOrderByClauseHandler, DefaultOrderByClauseHandler>()
                .AddScoped<IWrapWithClauseHandler, DefaultWrapWithClauseHandler>()
                .AddScoped<IQueryStringAccessor, AspNetCoreQueryStringAccessor>()
                .AddScoped<DynamicQueryableFilter>();
        }
    }
}