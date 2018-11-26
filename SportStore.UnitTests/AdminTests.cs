using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        public class AdminMock
        {
            public IProductRepository Repository { get; private set; }
            public Mock<IProductRepository> Mock { get; private set; }

            public AdminMock()
            {
                var mock = new Mock<IProductRepository>();
                mock.Setup(m => m.Products).Returns(new Product[]{
                    new Product {ProductId = 1, Name = "P1", Category = "Cat1" },
                    new Product {ProductId = 2, Name = "P2", Category = "Cat2" },
                    new Product {ProductId = 3, Name = "P3", Category = "Cat1" },
                    new Product {ProductId = 4, Name = "P4", Category = "Cat2" },
                    new Product {ProductId = 5, Name = "P5", Category = "Cat3" },
                });
                Repository = mock.Object;
                Mock = mock;
            }
        }

        [TestMethod]
        public void Index_Contains_All_Products()
        {
            var repo = new AdminMock().Repository;
            var target = new AdminController(repo);

            var result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            Assert.AreEqual(5, result.Length);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Products()
        {
            var repo = new AdminMock().Repository;
            var target = new AdminController(repo);

            var p1 = target.Edit(1).ViewData.Model as Product;
            var p2 = target.Edit(2).ViewData.Model as Product;
            var p3 = target.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.ProductId);
            Assert.AreEqual(2, p2.ProductId);
            Assert.AreEqual(3, p3.ProductId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistant_Product()
        {
            var repo = new AdminMock().Repository;
            var target = new AdminController(repo);

            var p = target.Edit(14).ViewData.Model as Product;

            Assert.IsNull(p);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            var mock = new AdminMock();
            var repo = mock.Repository;
            var target = new AdminController(repo);
            var p = new Product { Name = "Test" };

            var result = target.Edit(p);

            mock.Mock.Verify(o => o.Save(p));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            var mock = new AdminMock();
            var repo = mock.Repository;
            var target = new AdminController(repo);
            target.ModelState.AddModelError("err", "err");
            var p = new Product { Name = "Test" };

            var result = target.Edit(p);

            mock.Mock.Verify(o => o.Save(It.IsAny<Product>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Product()
        {
            var mock = new AdminMock();
            var repo = mock.Repository;
            var target = new AdminController(repo);
            var p = repo.Products.ToArray()[2];

            var result = target.Delete(p.ProductId);

            mock.Mock.Verify(o => o.Delete(p.ProductId));
        }
    }
}
