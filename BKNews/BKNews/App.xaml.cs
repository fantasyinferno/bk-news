using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace BKNews
{
    
	public partial class App : Application
	{
        CategoryPage CategoryPage;
        MainFeedPage MainFeedPage;
        // Initialize authenticator
        public static IAuthenticate Authenticator { get; private set; }
        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
        // Property to check if there is connectivity
        public static bool IsConnected
        {
            get
            {
                if (!CrossConnectivity.IsSupported)
                {
                    return true;
                }
                return CrossConnectivity.Current.IsConnected;
            }
        }
		public App ()
		{
			InitializeComponent();
            CategoryPage = new CategoryPage();
            MainFeedPage = new MainFeedPage();
            MainPage = CategoryPage;
        }

        protected async override void OnStart ()
		{
            // Handle when your app starts
            if (App.IsConnected)
            {
                // scrape if there is a internet connection
                // connectivityErrorPage.IsVisible = false;
                foreach (var page in CategoryPage.Children)
                {
                    if (page.GetType() == typeof(NewsPage)) {
                        var newsViewModel = (NewsViewModel)page.BindingContext;
                        await newsViewModel.ScrapeToCollectionAsync();
                    }
                }
                var mainFeedViewModel = (MainFeedPageViewModel)MainFeedPage.BindingContext;
                mainFeedViewModel.GetLatestNews();
            }
            else
            {
                // don't scrape and display an "Oops!" page
                // connectivityErrorPage.IsVisible = true;
            }
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
