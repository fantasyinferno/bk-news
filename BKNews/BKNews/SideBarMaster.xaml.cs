using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BKNews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideBarMaster : ContentPage
    {
        public ListView ListView;

        public SideBarMaster()
        {
            InitializeComponent();

            BindingContext = new SideBarMasterViewModel();
            ListView = MenuItemsListView;
        }

        class SideBarMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<SideBarMenuItem> MenuItems { get; set; }
            
            public SideBarMasterViewModel()
            {
                MenuItems = new ObservableCollection<SideBarMenuItem>(new[]
                {
                    new SideBarMenuItem { Id = 0, Title = "Page 1" },
                    new SideBarMenuItem { Id = 1, Title = "Page 2" },
                    new SideBarMenuItem { Id = 2, Title = "Page 3" },
                    new SideBarMenuItem { Id = 3, Title = "Page 4" },
                    new SideBarMenuItem { Id = 4, Title = "Page 5" },
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}