﻿using System;
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
        // is the user logged in?
        public static bool authenticated = false;
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
            MainFeedPage = new MainFeedPage();
            MainPage = new NavigationPage(MainFeedPage)
            {
                Title = "BKExpress",
                BarBackgroundColor=Color.Blue,
                Icon="Assets/logo.png"
            };
        }

        protected async override void OnStart ()
		{
            // Handle when your app starts
            var mainFeedViewModel = (MainFeedPageViewModel)MainFeedPage.BindingContext;

            if (App.IsConnected)
            {
                // scrape if there is a internet connection
                // connectivityErrorPage.IsVisible = false;
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
