using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System.Linq;
using System.Web;
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
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
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