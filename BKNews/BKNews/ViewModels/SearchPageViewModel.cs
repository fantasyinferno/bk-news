using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System;
using System.ComponentModel;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Microsoft.WindowsAzure.MobileServices;

namespace BKNews
{
    class SearchPageViewModel: INotifyPropertyChanged
    {
        // the results that are displayed to the user
        public ObservableCollection<News> SearchCollection { get; private set; }
        private int Skip { get; set; }
        private string SearchTerm { get; set; }
        string _headerString = "- Kết quả -";
        public string HeaderString
        {
            get
            {
                return _headerString;
            }
            set
            {
                if (_headerString != value)
                {
                    _headerString = value;
                    OnPropertyChanged("HeaderString");
                }

            }
        }
        public ICommand SearchCommand { get; private set; }
        public ICommand LoadMoreCommand { get; private set; }
        public ICommand ShareCommand { get; set; }
        public ICommand BookmarkCommand { get; set; }
        public SearchPageViewModel()
        {
            Skip = 0;
            SearchCollection = new ObservableCollection<News>();
            LoadMoreCommand = new Command(LoadMore);
            SearchCommand = new Command<string>(async (text) => await Search(text));
            ShareCommand = new Command<News>(ShareAsync);
            BookmarkCommand = new Command<News>(BookmarkAsync);
            User.CurrentUser.UserChanged += RecheckNews;
        }
        void RecheckNews(object sender, EventArgs args)
        {
            foreach(var item in SearchCollection)
            {
                if (User.CurrentUser.Bookmarks.Contains(item))
                {
                    item.IsBookmarkedByUser = true;
                } else
                {
                    item.IsBookmarkedByUser = false;
                }
            }
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
        async Task Search(string text)
        {
            SearchTerm = text;
            Skip = 0;
            try
            {
                SearchCollection.Clear();
                IQueryResultEnumerable<News> items = await NewsManager.DefaultManager.GetNewsAsync((news) => news.Title.ToLower().Contains(SearchTerm.ToLower()), Skip, 5);
                if (items != null)
                {
                    HeaderString = "- " + items.TotalCount + " kết quả -";
                    foreach (var item in items)
                    {
                        SearchCollection.Add(item);
                    }
                    Skip += 5;
                } else
                {
                    HeaderString = "- Không có kết quả -";
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        public async void LoadMore()
        {
            Skip += 5;
            IQueryResultEnumerable<News> items = await NewsManager.DefaultManager.GetNewsAsync((news) => news.Title.ToLower().Contains(SearchTerm.ToLower()), Skip, 5);
            foreach(var item in items)
            {
                if (User.CurrentUser.Bookmarks.Contains(item))
                {
                    item.IsBookmarkedByUser = true;
                }
                SearchCollection.Add(item);
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
    }
}
