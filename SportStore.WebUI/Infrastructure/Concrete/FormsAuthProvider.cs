using SportStore.WebUI.Infrastructure.Abstract;
using System.Web.Security;

namespace SportStore.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authentificate(string username, string password)
        {
            var result = FormsAuthentication.Authenticate(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }
    }
}