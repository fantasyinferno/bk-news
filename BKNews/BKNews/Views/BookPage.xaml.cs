using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
namespace BKNews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh items only when authenticated.
        }

        public BookPage(string UserId)
        {
            //            InitializeComponent();
            BindingContext = new Bookmark(UserId);

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