using SportStore.Domain.Entities;
using System.Collections.Generic;

namespace SportStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
    }
}
