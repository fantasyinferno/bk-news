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
    class BookmarkViewModel: INotifyPropertyChanged
    {
        // the results that are displayed to the user
        public HashSet<News> Collection { get; private set; }
        // the actual results, to be displayed as the user scrolls down
        public ICommand RefreshCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand BookmarkCommand { get; set; }
        public BookmarkViewModel()
        {
            Collection = User.CurrentUser.Bookmarks;
            BookmarkCommand = new Command<News>(BookmarkAsync);
            ShareCommand = new Command<News>(ShareAsync);
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
                        news.IsBookmarkedByUser = false;
                        // remove from the Bookmarks property of CurrentUser
                        User.CurrentUser.Bookmarks.RemoveWhere((n) => n.Id == newsUser.NewsId);
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
