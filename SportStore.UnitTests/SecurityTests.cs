using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportStore.WebUI.Controllers;
using SportStore.WebUI.Infrastructure.Abstract;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportStore.UnitTests
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authentificate("admin", "secret")).Returns(true);
            var model = new LoginViewModel { Username = "admin", Password = "secret" };
            var target = new AccountController(mock.Object);

            var result = target.Login(model, "/MyUrl");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_invalid_Credentials()
        {
            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authentificate("badUser", "badPassword")).Returns(false);
            var model = new LoginViewModel { Username = "badUser", Password = "badPassword" };
            var target = new AccountController(mock.Object);

            var result = target.Login(model, "/MyUrl");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
