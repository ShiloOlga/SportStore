using SportStore.Domain.Entities;
using System.Data.Entity;

namespace SportStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
