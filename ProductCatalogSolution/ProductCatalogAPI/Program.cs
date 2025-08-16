using ProductCatalogAPI.Models;
using ProductCatalogAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Load settings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// 2. HttpClient + ProductService
builder.Services.AddHttpClient<ProductService>();

// 3. CORS to allow front-end calls
builder.Services.AddCors(o => o.AddPolicy("AllowAll",
    p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// 4. Controllers
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();
