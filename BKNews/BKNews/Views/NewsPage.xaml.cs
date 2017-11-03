using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
namespace BKNews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsPage : ContentPage
	{
		public NewsPage ()
		{
			InitializeComponent ();
            BindingContext = new NewsViewModel();
        }
        // Open a browser every time an item is tapped
        public void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            var news = (News)e.Item;
            this.Navigation.PushAsync(new InAppBrowser(news.NewsUrl));
            ((ListView)sender).SelectedItem = null;
        }
    }
}