using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProductCatalogWeb.Models;
using ProductCatalogWeb.Services;
using Polly;
using Polly.RateLimit;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Bind ApiSettings
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings")
);

// Register HttpClient + Rate-Limit Policy
builder.Services
    .AddHttpClient<IProductService, ProductService>()
    .ConfigureHttpClient((sp, client) =>
    {
        var opts = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
        client.BaseAddress = new Uri(opts.BaseUrl);
        client.DefaultRequestHeaders.Add("X-Api-Key", opts.ApiKey);
    })
    .AddPolicyHandler(
        Policy.RateLimitAsync<HttpResponseMessage>(5, TimeSpan.FromSeconds(60))
    );

builder.Services.AddControllersWithViews();
var app = builder.Build();

// … middleware, routing, etc.

app.Run();
