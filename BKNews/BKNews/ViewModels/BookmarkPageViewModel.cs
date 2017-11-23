using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System;
using System.ComponentModel;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System.Collections.Generic;

namespace BKNews
{
    class BookmarkPageViewModel: INotifyPropertyChanged
    {
        // the results that are displayed to the user
        public ObservableCollection<News> Collection { get; set; }
        // the actual results, to be displayed as the user scrolls down
        public ICommand RefreshCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand BookmarkCommand { get; set; }
        public BookmarkPageViewModel()
        {
            BookmarkCommand = new Command<News>(BookmarkAsync);
            ShareCommand = new Command<News>(ShareAsync);
            Collection = new ObservableCollection<News>();
            foreach(var item in User.CurrentUser.Bookmarks)
            {
                Collection.Add(item);
            }
            User.CurrentUser.UserChanged += RecheckNews;
        }
        // event to reload when User is changed
        void RecheckNews(object sender, EventArgs args)
        {
            Collection.Clear();
            foreach (var item in User.CurrentUser.Bookmarks)
            {
                Collection.Add(item);
            }
        }
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
                        Collection.Remove(news);
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
        public async void ShareAsync(News news)
        {
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
    }
}
