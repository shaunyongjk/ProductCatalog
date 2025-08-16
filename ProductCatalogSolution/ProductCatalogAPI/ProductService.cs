using System.Collections.Concurrent;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    public class ProductService
    {
        private readonly HttpClient _http;
        private readonly ApiSettings _settings;
        private readonly ConcurrentQueue<DateTime> _timestamps = new();

        public ProductService(HttpClient http, IOptions<ApiSettings> opts)
        {
            _http = http;
            _settings = opts.Value;
            _http.BaseAddress = new Uri(_settings.VendorBaseUrl);
            _http.DefaultRequestHeaders.Add("X-Api-Key", _settings.Key);
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var now = DateTime.UtcNow;
            _timestamps.Enqueue(now);

            // keep only last 5 timestamps
            while (_timestamps.Count > 5)
                _timestamps.TryDequeue(out _);

            // if 5 calls in last 60s → throttle
            if (_timestamps.Count == 5
                && now - _timestamps.First() < TimeSpan.FromSeconds(60))
            {
                throw new InvalidOperationException("Rate limit exceeded.");
            }

            var res = await _http.GetAsync("product/get-product-list");
            res.EnsureSuccessStatusCode();

            var items = await JsonSerializer.DeserializeAsync<List<ProductDto>>(
                await res.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return items!
                .Select(p => { p.Price = Math.Round(p.BasePrice * 1.2m, 2); return p; })
                .ToList();
        }
    }
}
