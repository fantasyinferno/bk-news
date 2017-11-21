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
using BKNews;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;
namespace BKNews.Droid
{
	[Activity (Label = "BKNews", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation) ]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        // Define an authenticated user.
        private MobileServiceUser user;
        public async Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await NewsManager.DefaultManager.CurrentClient.LoginAsync(this,
                    provider, Constants.URLScheme);
                if (user != null)
                {
                    message = string.Format("You are now signed-in as {0}. Token: {1}",
                        user.UserId, user.MobileServiceAuthenticationToken);
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
            System.Diagnostics.Debug.WriteLine(user.MobileServiceAuthenticationToken);
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-ZUMO-AUTH", user.MobileServiceAuthenticationToken);
                HttpResponseMessage response;
                response = await client.GetAsync(Constants.ApplicationURL + @"/.auth/me");
                var responseString = await response.Content.ReadAsStringAsync();
                JToken token = JToken.Parse(responseString);
                System.Diagnostics.Debug.WriteLine(token[0]["user_claims"]);
                var userClaims = token[0]["user_claims"];
                string avatarUrl = null;
                string name = null;
                List<Info> yourInfo = JsonConvert.DeserializeObject<List<Info>>(userClaims.ToString());
                if (provider == MobileServiceAuthenticationProvider.Facebook)
                {
                    avatarUrl = "http://graph.facebook.com/" + yourInfo.Find(info => info.typ == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").val + "/picture?type=normal";
                    name = yourInfo.Find(info => info.typ == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").val;
                }
                else if (provider == MobileServiceAuthenticationProvider.Google)
                {
                    avatarUrl = yourInfo.Find(info => info.typ == "picture").val;
                    name = yourInfo.Find(info => info.typ == "name").val;
                }
                User.CurrentUser.Id = user.UserId;
                User.CurrentUser.Name = name;
                User.CurrentUser.AvatarUrl = avatarUrl;
                User.CurrentUser.Authenticated = true;
                // get the users bookmarks from the database
                var collection = await NewsManager.DefaultManager.GetNewsForUser(User.CurrentUser.Id);
                foreach(var item in collection)
                {
                    User.CurrentUser.Bookmarks.Add(item);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return success;

        }
        public async Task<bool> LogoutAsync()
        {
            // delete cookies
            CookieManager.Instance.RemoveAllCookie();
            await NewsManager.DefaultManager.CurrentClient.LogoutAsync();

            // user has logged out so set this to null
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

