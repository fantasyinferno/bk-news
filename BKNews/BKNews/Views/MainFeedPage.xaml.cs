﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BKNews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainFeedPage : ContentPage
	{
		public MainFeedPage ()
		{
			InitializeComponent ();
            this.BindingContext = new MainFeedPageViewModel();

            
		}
	}
}