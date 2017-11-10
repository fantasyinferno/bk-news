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
	public partial class AuthenticationPage : ContentPage
	{
		public AuthenticationPage ()
		{
			InitializeComponent ();
		}

        async void OnButtonClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            //await DisplayAlert("Wrong Password or Invalid Username",
            //  "Please try again or sign up",
            //"OK");
            await Navigation.PushAsync(new NewsPage());
        }

        async void OnButtonClicked0(object sender, EventArgs args)
        {
            Button button = (Button)sender;

            //await DisplayAlert("Wrong Password or Invalid Username",
            //  "Please try again or sign up",
            //"OK");
            await Navigation.PushAsync(new NewsPage());
        }
    }
}