using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace BKNews.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        private MobileServiceUser user;

        public async Task<bool> AuthenticateAsync()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    user = await NewsManager.DefaultManager.CurrentClient
                        .LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
                        MobileServiceAuthenticationProvider.Google, Constants.URLScheme);
                    if (user != null)
                    {
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

            return success;
        }
        public async Task<bool> LogoutAsync()
        {
            foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
            {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
            }
            await NewsManager.DefaultManager.CurrentClient.LogoutAsync();
            user = null;
            return true;
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return NewsManager.DefaultManager.CurrentClient.ResumeWithURL(url);
        }
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
            App.Init(this);
			LoadApplication (new BKNews.App ());

			return base.FinishedLaunching (app, options);
		}
	}
}
