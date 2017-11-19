using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Xamarin.Forms;
using System.ComponentModel;
//adb connect 169.254.138.177 
using System.Threading.Tasks;

namespace BKNews
{
    class Bookmark
    {
        // UserId
        public string _UserId;
        public string Category = "abc";
        // mixed collection of news
        public ObservableCollection<News> NewsBookmark { get; private set; }
        // command to bind with button
        public ICommand Load { get; set; }
        // IsRefreshing property of ListView
        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }

            }
        }
        // propagate property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // load items from database with pagination
        public async void LoadFromDatabaseAsync()
        {
            try
            {
                // Get Id from Droid, IOS, UWP
                //
                //_UserId = BKNews.Droid.MainActivity.getId();
                //

                var collection = await NewsManager.DefaultManager.GetNewsForUser(_UserId);
                foreach (var item in collection)
                {
                    NewsBookmark.Add(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public Bookmark(string userId)
        {
            _UserId = userId;
            NewsBookmark = new ObservableCollection<News>();
            // load bookmark from database
            Load = new Command(LoadFromDatabaseAsync);
            Debug.WriteLine(_UserId);
            LoadFromDatabaseAsync();
        }
    }
}
