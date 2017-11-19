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
    public partial class SidebarPage : MasterDetailPage
    {
        public SidebarPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SidebarPageMenuItem;
            if (item == null)
                return;
            Page page;
            if (item.TargetType == typeof(NewsPage))
            {
                page = new NewsPage(item.Title);
            } else {
                page = (Page)Activator.CreateInstance(item.TargetType);
            }
            page.Title = item.Title;
            // navigation with search and notification buttons
            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}