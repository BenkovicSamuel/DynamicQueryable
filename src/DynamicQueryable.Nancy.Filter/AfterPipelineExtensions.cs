using System;
using System.IO;
using System.Linq;
using DynamicQueryable.Core.Models;
using DynamicQueryable.Extensions;
using Nancy;
using Newtonsoft.Json;

namespace DynamicQueryable.Nancy.Filter
{
    public static class AfterPipelineExtensions
    {
        public static void DynamicQueryable(this AfterPipeline afterPipeline, NancyContext context, IQueryable<dynamic> query, IDynamicQueryableContext autoQueryableContext)
        {
            context.Items.Add("autoqueryable-query", query);
            afterPipeline += ctx =>
            {
                if (query == null) throw new Exception("Unable to retrieve value of IQueryable from context result.");
                ctx.Response.Contents = stream =>
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var result = query.DynamicQueryable(autoQueryableContext).ToAutoQueryListResult(autoQueryableContext);
                        writer.Write(JsonConvert.SerializeObject(result));
                    }
                };
            };
        }
    }
}