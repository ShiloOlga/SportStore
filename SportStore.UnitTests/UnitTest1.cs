using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.HtmlHelpers;
using SportStore.WebUI.Models;

namespace SportStore.UnitTests
{
    public class ProductRepositoryMock
    {
        public IProductRepository ProductRepository { get; private set; }
        public ProductController ProductController { get; private set; }

        public ProductRepositoryMock()
        {
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"},
            });
            ProductRepository = mock.Object;
            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            ProductController = controller;
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            var repoMock = new ProductRepositoryMock();

            var result = (ProductsListViewModel)repoMock.ProductController.List(2).Model;

            var resultArray = result.Products.ToArray();
            Assert.IsTrue(resultArray.Length == 2);
            Assert.AreEqual(resultArray[0].Name, "P4");
            Assert.AreEqual(resultArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper htmlHelper = null;
            var pagingInfo = new PagingInfo { CurrentPage = 2, ItemsPerPage = 10, TotalItems = 28 };
            Func<int, string> pageUriDelegate = i => "Page" + i;

            var result = htmlHelper.PageLinks(pagingInfo, pageUriDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            var repoMock = new ProductRepositoryMock();

            var result = (ProductsListViewModel)repoMock.ProductController.List(2).Model;

            var pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }
    }
}
