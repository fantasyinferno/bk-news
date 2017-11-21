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
	public partial class BookmarkPage : ContentPage
	{
		public BookmarkPage ()
		{
			InitializeComponent ();
            BindingContext = new BookmarkViewModel();
        }
        void OnItemTapped(object sender, ItemTappedEventArgs args)
        {
            var news = (News)args.Item;
            Device.OpenUri(new Uri(news.NewsUrl));
            ((ListView)sender).SelectedItem = null;
        }
    }
}