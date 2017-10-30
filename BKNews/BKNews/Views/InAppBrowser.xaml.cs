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
		public InAppBrowser (string newsUrl)
		{
			InitializeComponent ();
            webView.Source = newsUrl;
		}
	}
}