using Core.Entities;
using Core.Interfaces;
using Infrastrcture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Migrations
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        async Task<Product> IProductRepository.GetProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(p=>p.ProductBrand)
            .Include(p=>p.ProductType)
            .FirstOrDefaultAsync(p=>p.Id==id);
        }

        async Task<IReadOnlyList<Product>> IProductRepository.GetProductsAsync()
        {
            return await _context.Products
            .Include(p=>p.ProductBrand)
            .Include(p=>p.ProductType)
            .ToListAsync();
        }
    }
}