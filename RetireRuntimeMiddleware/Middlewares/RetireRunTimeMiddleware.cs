﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RetireRuntimeMiddleware.Clients;
using RetireRuntimeMiddleware.Clients.Models;

namespace RetireRuntimeMiddleware.Middlewares
{
    internal class RetireRunTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ReportGenerator _client;

        public RetireRunTimeMiddleware(RequestDelegate next)
        {
            _next = next;
            var releaseMetadataClient = new ReleaseMetadataClient();
            _client = new ReportGenerator(releaseMetadataClient);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var report = await _client.GetReport(AppRunTimeDetails.Build());

            var json = JsonConvert.SerializeObject(report, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            context.Response.OnStarting(state =>
            {
                context.Response.ContentType = "application/json";
                return Task.CompletedTask;
            }, null);

            await context.Response.WriteAsync(json);
        }


    }
}
