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
	public partial class TeaserPage : ContentPage
	{
		public TeaserPage ()
		{
			InitializeComponent ();
		}

        async public void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {

            var imageSender = (Image)sender;
            // watch the monkey go from color to black&white!
            await Navigation.PushAsync(new AuthenticationPage());
        }
    }
}