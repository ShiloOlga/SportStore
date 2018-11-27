using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            var p2 = new Product
            {
                ProductId = 2,
                Name = "P2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product {ProductId = 1, Name = "P1", Category = "Cat1" },
                p2,
                new Product {ProductId = 3, Name = "P3", Category = "Cat1" },
            }.AsQueryable());
            var target = new ProductController(mock.Object);

            var result = target.GetImage(2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(p2.ImageMimeType, ((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_Id()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product {ProductId = 1, Name = "P1", Category = "Cat1" },
                new Product {ProductId = 3, Name = "P3", Category = "Cat1" },
            }.AsQueryable());
            var target = new ProductController(mock.Object);

            var result = target.GetImage(2);

            Assert.IsNull(result);
        }
    }
}
