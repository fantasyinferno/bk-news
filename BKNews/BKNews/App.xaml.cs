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
        NewsPage NewsPage;
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
<<<<<<< HEAD
            MainPage = new NavigationPage(new BKNews.NewsPage());
=======
            NewsPage = new NewsPage();
            MainPage = new NavigationPage(NewsPage)
            {
                BarBackgroundColor = Color.Blue
            };
>>>>>>> 0f422e36cb725586c4345d0c493751c3bf2aaf80
        }

        protected override void OnStart ()
		{
            // Handle when your app starts
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
