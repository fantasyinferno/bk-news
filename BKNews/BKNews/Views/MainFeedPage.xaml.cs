using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Microsoft.WindowsAzure.MobileServices;

namespace BKNews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainFeedPage : ContentPage
	{
		public MainFeedPage ()
		{
			InitializeComponent ();
            BindingContext = new MainFeedPageViewModel();
		}
        async void FacebookLoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
        }
        async void GoogleLoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
        }
        async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.LogoutAsync();
        }
    }
}