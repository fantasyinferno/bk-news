using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
namespace BKNews
{   
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsPage : ContentPage
	{
        // is the user logged in?
        bool authenticated = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Refresh items only when authenticated.
            if (authenticated)
            {
                Debug.WriteLine("Authenticated");
            }
        }
        async void FacebookLoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
        }
        async void GoogleLoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
        }
        async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.LogoutAsync();
        }
        public NewsPage (string category, IScrape scraper)
		{
			InitializeComponent ();
            BindingContext = new NewsViewModel(category, scraper);
        }
        // Open a browser every time an item is tapped
        public void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            var news = (News)e.Item;
            this.Navigation.PushAsync(new InAppBrowser(news.NewsUrl));
            ((ListView)sender).SelectedItem = null;
        }
    }
}