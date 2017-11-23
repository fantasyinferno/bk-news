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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BKNews.UWP
{
    public sealed partial class MainPage: IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user;

        public async Task<bool> AuthenticateAsync(MobileServiceAuthenticationProvider provider)
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await NewsManager.DefaultManager.CurrentClient.LoginAsync(
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
