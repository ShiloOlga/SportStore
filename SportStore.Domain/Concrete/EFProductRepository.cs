using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System.Collections.Generic;

namespace SportStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext _context = new EFDbContext();
        public IEnumerable<Product> Products => _context.Products;
    }
}
