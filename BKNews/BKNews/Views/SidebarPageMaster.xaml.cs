using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BKNews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SidebarPageMaster : ContentPage
    {
        public ListView ListView;

        public SidebarPageMaster()
        {
            InitializeComponent();

            BindingContext = new SidebarPageMasterViewModel();
            ListView = MenuItemsListView;
        }
        async void FacebookLoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.AuthenticateAsync(MobileServiceAuthenticationProvider.Facebook);
            UpdateUserInfo();
        }
        async void GoogleLoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
            UpdateUserInfo();
        }
        async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.LogoutAsync();
            UpdateUserInfo();
        }
        async void UpdateUserInfo()
        {
            
        }
        class SidebarPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<SidebarPageMenuItem> MenuItems { get; set; }

           
            public SidebarPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<SidebarPageMenuItem>(new[]
                {
                    new SidebarPageMenuItem { Id = 0, Title = "BKExpress", TargetType = typeof(MainFeedPage) },
                    new SidebarPageMenuItem { Id = 0, Title = "Thông báo", TargetType = typeof(NotificationPage) },
                    new SidebarPageMenuItem { Id = 1, Title = "HCMUT", TargetType = typeof(NewsPage) },
                    new SidebarPageMenuItem { Id = 3, Title = "AAO", TargetType = typeof(NewsPage) },
                    new SidebarPageMenuItem { Id = 4, Title = "OISP", TargetType = typeof(NewsPage) },
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