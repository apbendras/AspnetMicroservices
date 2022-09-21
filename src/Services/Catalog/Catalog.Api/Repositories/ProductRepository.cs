
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this._catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext) );
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, id);

            var deletedResult = await _catalogContext
                .Products
                .DeleteOneAsync(filter);

            return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id) => await
                _catalogContext
                .Products
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Category, category);

            return await
                _catalogContext
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Name, name);

            return await
                _catalogContext
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await
                _catalogContext
                .Products
                .Find(x => true)
                .ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult =  await _catalogContext.Products.ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}

