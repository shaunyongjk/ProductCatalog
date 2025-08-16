using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Polly.RateLimit;
using ProductCatalogWeb.Models;

namespace ProductCatalogWeb.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http) => _http = http;

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        try
        {
            var response = await _http.GetAsync("product/get-product-list");

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new InvalidOperationException("Server rate limit reached. Try again later.");
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<IEnumerable<Product>>(stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? Array.Empty<Product>();
        }
        catch (RateLimitRejectedException)
        {
            throw new InvalidOperationException("Client rate limit exceeded. Wait and retry.");
        }
    }
}
