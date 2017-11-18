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
        }
        async void GoogleLoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.AuthenticateAsync(MobileServiceAuthenticationProvider.Google);
        }
        async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                App.authenticated = await App.Authenticator.LogoutAsync();
        }
    }
}