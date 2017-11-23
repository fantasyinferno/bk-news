using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        public async Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
               user = await NewsManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
                    provider, Constants.URLScheme);
                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine("NOW THE USING IS NOT NULL");
                    message = string.Format("You are now signed-in as {0}.",
                        user.UserId);
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
                    // get the users bookmarks from the database
                    var collection = await NewsManager.DefaultManager.GetNewsForUser(User.CurrentUser.Id);
                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            item.IsBookmarkedByUser = true;
                            User.CurrentUser.Bookmarks.Add(item);
                        }
                    }
                    success = true;
                }

                System.Diagnostics.Debug.WriteLine("WE ARE OUT!");
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
