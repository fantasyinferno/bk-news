using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace BKNews
{
    public interface IAuthenticate
    {
        // authenticate with the chosen provider, e.g "google", "facebook", "twitter"...
        Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider);
        Task<bool> LogoutAsync();
    }
}
