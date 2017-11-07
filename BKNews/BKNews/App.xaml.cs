using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BKNews
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
	public partial class App : Application
	{
        public static IAuthenticate Authenticator { get; private set; }
        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
		public App ()
		{
			InitializeComponent();
            MainPage = new NavigationPage(new NewsPage())
            {
                BarBackgroundColor = Color.Blue
            };
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
