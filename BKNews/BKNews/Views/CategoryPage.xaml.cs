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
    public partial class CategoryPage : TabbedPage
    {
        public CategoryPage ()
        {
            InitializeComponent();
            this.Children.Add(new NewsPage("HCMUT", new HCMUTScraper()));
            this.Children.Add(new NewsPage("AAO", new AAOScraper()));
            this.Children.Add(new NewsPage("OISP", new OISPScraper()));
        }
    }
}