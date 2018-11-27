using SportStore.WebUI.Infrastructure.Abstract;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthProvider _authProvider;

        public AccountController(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_authProvider.Authentificate(viewModel.Username, viewModel.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "incorrect username or password");
                }
            }
            return View();
        }
    }
}