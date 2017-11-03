using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BKNews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InAppBrowser : ContentPage
	{
        public InAppBrowser()
        {
            InitializeComponent();
        }
		public InAppBrowser (string newsUrl)
		{
			InitializeComponent ();
            webView.Source = newsUrl;
		}
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await progressBar.ProgressTo(0.9, 900, Easing.SpringIn);
        }
        void WebOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            progressBar.IsVisible = true;
        }
        void WebOnNavigated(object sender, WebNavigatingEventArgs e)
        {
            progressBar.IsVisible = false;
        }
	}
}