namespace SportStore.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        bool Authentificate(string username, string password);
    }
}