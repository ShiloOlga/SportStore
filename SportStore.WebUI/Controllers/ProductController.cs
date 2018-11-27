using SportStore.Domain.Abstract;
using SportStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        public ViewResult List(string category, int page = 1)
        {
            var model = new ProductsListViewModel
            {
                Products = _repository.Products
                  .Where(p => category == null || p.Category == category)
                  .OrderBy(p => p.ProductId)
                  .Skip((page - 1) * PageSize)
                  .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null 
                        ? _repository.Products.Count()
                        : _repository.Products.Count(p => p.Category == category)
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
            return (product != null)
                ? File(product.ImageData, product.ImageMimeType)
                : null;
        }
    }
}