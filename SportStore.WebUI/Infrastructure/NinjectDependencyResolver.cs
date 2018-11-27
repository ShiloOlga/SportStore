using Moq;
using Ninject;
using SportStore.Domain.Abstract;
using SportStore.Domain.Concrete;
using SportStore.Domain.Entities;
using SportStore.WebUI.Infrastructure.Abstract;
using SportStore.WebUI.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IProductRepository>().To<EFProductRepository>();

            var settings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            _kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", settings);
            _kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}