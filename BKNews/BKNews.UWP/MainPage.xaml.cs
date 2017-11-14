using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using BKNews;
using System.Diagnostics;
namespace BKNews.UWP
{
    public sealed partial class MainPage: IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user;

        public async Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            string message = string.Empty;
            var success = false;

            try
            {
                // Sign in with Google login using a server-managed flow.
                if (user == null)
                {
                    user = await NewsManager.DefaultManager.CurrentClient
                        .LoginAsync(provider, Constants.URLScheme);
                    if (user != null)
                    {
                        success = true;
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                        Debug.WriteLine(user.MobileServiceAuthenticationToken);
                    }
                }

            }
            catch (Exception ex)
            {
                message = string.Format("Authentication Failed: {0}", ex.Message);
            }

            // Display the success or failure message.
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return success;
        }
        public async Task<bool> LogoutAsync()
        {
            await NewsManager.DefaultManager.CurrentClient.LogoutAsync();
            user = null;
            return true;
        }
        public MainPage()
        {
            this.InitializeComponent();
            BKNews.App.Init(this);
            LoadApplication(new BKNews.App());
        }

    }
}
