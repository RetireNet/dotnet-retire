using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRetireRuntimeHostedService(o => o.CheckInterval = 10000);
builder.WebHost.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration
        .MinimumLevel.Debug()
        .WriteTo.Console(new CompactJsonFormatter()));

var app = builder.Build();
app.UseRuntimeVulnerabilityReport();
await app.RunAsync();

