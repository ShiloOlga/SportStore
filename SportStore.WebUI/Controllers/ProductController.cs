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

        public ViewResult List(int page = 1)
        {
            var model = new ProductsListViewModel
            {
                Products = _repository.Products
                  .OrderBy(p => p.ProductId)
                  .Skip((page - 1) * PageSize)
                  .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Products.Count()
                }
            };
            return View(model);
        }
    }
}