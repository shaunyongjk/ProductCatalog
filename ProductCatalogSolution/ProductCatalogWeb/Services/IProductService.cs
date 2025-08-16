using System.Collections.Generic;
using System.Threading.Tasks;
using ProductCatalogWeb.Models;

namespace ProductCatalogWeb.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
}
