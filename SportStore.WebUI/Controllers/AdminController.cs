using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository _repository;

        public AdminController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: Admin
        public ViewResult Index()
        {
            return View(_repository.Products);
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        public ViewResult Edit(int productId)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductId == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.Save(product);
                TempData["message"] = $"{product.Name} has been saved.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            var product = _repository.Delete(productId);
            if (product != null)
            {
                TempData["message"] = $"{product.Name} has been deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}