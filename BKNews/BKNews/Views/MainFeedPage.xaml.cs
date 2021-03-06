﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;

namespace BKNews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainFeedPage : ContentPage
	{
        public MainFeedPage ()
		{
			InitializeComponent ();
            BindingContext = new MainFeedPageViewModel();
		}

        // Open a browser every time an item is tapped
        public void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Debug.WriteLine(e);
            var news = (News)e.Item;
            Device.OpenUri(new Uri(news.NewsUrl));
            ((ListView)sender).SelectedItem = null;
        }
        public void OnListViewHeaderItemTapped(object sender, EventArgs e)
        {
            Debug.WriteLine(e);
        }
    }
}