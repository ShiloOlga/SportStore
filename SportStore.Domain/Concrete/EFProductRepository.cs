using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System.Collections.Generic;

namespace SportStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext _context = new EFDbContext();
        public IEnumerable<Product> Products => _context.Products;

        public void Save(Product product)
        {
            if (product.ProductId == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                var entry = _context.Products.Find(product.ProductId);
                if (entry != null)
                {
                    entry.Name = product.Name;
                    entry.Description = product.Description;
                    entry.Price = product.Price;
                    entry.Category = product.Category;
                    entry.ImageData = product.ImageData;
                    entry.ImageMimeType = product.ImageMimeType;
                }
            }
            _context.SaveChanges();
        }

        public Product Delete(int productId)
        {
            var entry = _context.Products.Find(productId);
            if (entry != null)
            {
                _context.Products.Remove(entry);
                _context.SaveChanges();
            }
            return entry;
        }
    }
}
