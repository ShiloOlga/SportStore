using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportStore.Domain.Entities;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Models;
using System.Linq;

namespace SportStore.UnitTests
{
    public class CartMock
    {
        public Product P1 { get; private set; }
        public Product P2 { get; private set; }
        public Product P3 { get; private set; }
        public Cart Cart { get; private set; }

        public CartMock()
        {
            P1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            P2 = new Product { ProductId = 2, Name = "P2", Price = 50M };
            P3 = new Product { ProductId = 3, Name = "P3"};
            Cart = new Cart();
        }
    }

    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            var mock = new CartMock();
            var cart = mock.Cart;

            cart.AddItem(mock.P1, 1);
            cart.AddItem(mock.P2, 1);
            var result = cart.Lines.ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(mock.P1, result[0].Product);
            Assert.AreEqual(mock.P2, result[1].Product);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            var mock = new CartMock();
            var cart = mock.Cart;

            cart.AddItem(mock.P1, 1);
            cart.AddItem(mock.P2, 1);
            cart.AddItem(mock.P1, 10);
            var result = cart.Lines.OrderBy(p => p.Product.ProductId).ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(11, result[0].Quantity);
            Assert.AreEqual(1, result[1].Quantity);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            var mock = new CartMock();
            var cart = mock.Cart;

            cart.AddItem(mock.P1, 1);
            cart.AddItem(mock.P2, 3);
            cart.AddItem(mock.P3, 5);
            cart.AddItem(mock.P2, 1);
            cart.Remove(mock.P2);

            Assert.AreEqual(0, cart.Lines.Count(p => p.Product == mock.P2));
            Assert.AreEqual(2, cart.Lines.Count());
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            var mock = new CartMock();
            var cart = mock.Cart;

            cart.AddItem(mock.P1, 1);
            cart.AddItem(mock.P2, 1);
            cart.AddItem(mock.P1, 3);
            var result = cart.ComputeTotalValue();

            Assert.AreEqual(450M, result);
        }

        [TestMethod]
        public void Can_Clear_Content()
        {
            var mock = new CartMock();
            var cart = mock.Cart;

            cart.AddItem(mock.P1, 1);
            cart.AddItem(mock.P2, 1);
            cart.Clear();

            Assert.AreEqual(0, cart.Lines.Count());
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            var mock = new ProductRepositoryMock();
            var cart = new Cart();
            var target = new CartController(mock.ProductRepository);

            target.AddToCart(cart, 1, null);

            Assert.AreEqual(1, cart.Lines.Count());
            Assert.AreEqual(1, cart.Lines.ToArray()[0].Product.ProductId);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            var mock = new ProductRepositoryMock();
            var cart = new Cart();
            var target = new CartController(mock.ProductRepository);

            var result = target.AddToCart(cart, 2, "url");

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("url", result.RouteValues["returnUrl"]);
        }

        [TestMethod]
        public void Can_View_Cart_Content()
        {
            var cart = new Cart();
            var target = new CartController(null);

            var result = (CartIndexViewModel)target.Index(cart, "url").ViewData.Model;

            Assert.AreSame(cart, result.Cart);
            Assert.AreEqual("url", result.ReturnUrl);
        }
    }
}
