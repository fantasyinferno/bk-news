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
        public class CurrentUser
        {
            public string Name { get; set; }
            public string AvatarURL { get; set; }
        }
        CurrentUser currentUser = new CurrentUser();
		// is the user logged in?
		public static bool authenticated = false;
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
			//MainPage = new NavigationPage(MainFeedPage)
			//{
			//    Title = "BKExpress",
			//    BarBackgroundColor=Color.Blue,
			//    Icon="Assets/logo.png"
			//};
			MainPage = new SidebarPage();
		}


		protected async override void OnStart ()
		{
            await ScrapingSystem.ScrapeAll();
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
