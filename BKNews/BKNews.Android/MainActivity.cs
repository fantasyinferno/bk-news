using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Android.Webkit;

namespace BKNews.Droid
{
	[Activity (Label = "BKNews", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)
    , IntentFilter(new[] { Android.Content.Intent.ActionView },
        DataScheme = "bkexpress",
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user;
        public async Task<bool> AuthenticateAsync()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await NewsManager.DefaultManager.CurrentClient.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Google, Constants.URLScheme);
                if (user != null)
                {
                    message = string.Format("you are now signed-in as {0}.",
                        user.UserId);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            System.Diagnostics.Debug.WriteLine(message);
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }
        public async Task<bool> LogoutAsync()
        {
            CookieManager.Instance.RemoveAllCookie();
            await NewsManager.DefaultManager.CurrentClient.LogoutAsync();
            user = null;
            return true;
        }
        protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

            // authenticate signin form
            App.Init((IAuthenticate)this);
            LoadApplication (new BKNews.App ());
		}
	}
}

