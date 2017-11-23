using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Xamarin.Forms;
using System.ComponentModel;
//adb connect 169.254.138.177 
using System.Threading.Tasks;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace BKNews
{
    class NewsViewModel : INotifyPropertyChanged
    {
        // skip & step
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 5;
        // category's name
        public string Category { get; set; }
        // mixed collection of news
        public ObservableCollection<News> NewsCollection { get; private set; }
        // command to bind with button
        public ICommand RefreshCommand { get; set; }
        public ICommand LoadMore { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand BookmarkCommand { get; set; }
        public async void BookmarkAsync(News news)
        {
            try
            {
                if (User.CurrentUser.Authenticated)
                {
                    // bookmark or unbookmark if the user is authenticated
                    NewsUser newsUser = new NewsUser(news.Id, User.CurrentUser.Id);

                    if (!User.CurrentUser.Bookmarks.Contains(news))
                    {
                        await NewsManager.DefaultManager.SaveNewsUserAsync(newsUser);
                        news.IsBookmarkedByUser = true;
                        User.CurrentUser.Bookmarks.Add(news);
                    }
                    else
                    {
                        // remove from the database
                        await NewsManager.DefaultManager.DeleteNewsUserAsync(newsUser);
                        User.CurrentUser.Bookmarks.RemoveWhere((n) => n.Id == newsUser.NewsId);
                        news.IsBookmarkedByUser = false;
                        // remove from the Bookmarks property of CurrentUser
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        // list for storing scrapers
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
                Debug.WriteLine("{0} {1}", Skip, Take);
                IsBusy = true;
                var collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync(Category, Skip, Take);
                foreach (var item in collection)
                {
                    if (User.CurrentUser.Bookmarks.Contains(item))
                    {
                        item.IsBookmarkedByUser = true;
                    }
                    NewsCollection.Add(item);
                }
                Skip += Take;
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            } finally
            {
                IsBusy = false;
            }
        }
        void RecheckNews(object sender, EventArgs args)
        {
            foreach(var item in NewsCollection)
            {
                if (User.CurrentUser.Bookmarks.Contains(item))
                {
                    item.IsBookmarkedByUser = true;
                }
                else
                {
                    item.IsBookmarkedByUser = false;
                }
            }
        }
        public async void RefreshAsync()
        {
            NewsCollection.Clear();
            Skip = 0;
            LoadFromDatabaseAsync();
        }
        public async void ShareAsync(News news)
        {
            Debug.WriteLine("ShareAsync tapped");
            if (!CrossShare.IsSupported)
            {
                return;
            }
            await CrossShare.Current.Share(new ShareMessage
            {
                Title = news.Title,
                Text = news.Desc,
                Url = news.NewsUrl
            });
        }
        public NewsViewModel(String category)
        {
            Category = category;
            NewsCollection = new ObservableCollection<News>();
            // load more when pull to refresh
            RefreshCommand = new Command(RefreshAsync);
            // load more at the end of the list
            LoadMore = new Command(LoadFromDatabaseAsync);
            // take 5 news from database 
            ShareCommand = new Command<News>(ShareAsync);
            BookmarkCommand = new Command<News>(BookmarkAsync);
            User.CurrentUser.UserChanged += RecheckNews;
            LoadFromDatabaseAsync();
        }
    }
}