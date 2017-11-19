using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
namespace BKNews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class BookmarkPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh items only when authenticated.
        }

        public BookmarkPage(string userId)
        {
//            InitializeComponent();
            BindingContext = new Bookmark(userId);
        }
        // Open a browser every time an item is tapped
        public void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            var news = (News)e.Item;
            Device.OpenUri(new Uri(news.NewsUrl));
            ((ListView)sender).SelectedItem = null;
        }
    }
}
