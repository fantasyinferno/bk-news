using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace BKNews
{
    class MainFeedPageViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<MainFeedPageGroup> LatestNews { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand BookmarkCommand { get; set; }
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
                        User.CurrentUser.Bookmarks.Add(news);
                        news.IsBookmarkedByUser = true;
                    } else
                    {
                        // remove from the database
                        await NewsManager.DefaultManager.DeleteNewsUserAsync(newsUser);
                        User.CurrentUser.Bookmarks.RemoveWhere((n) => n.Id == newsUser.NewsId);
                        news.IsBookmarkedByUser = false;
                        // remove from the Bookmarks property of CurrentUser
                    }
                }
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
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
        public MainFeedPageViewModel()
        {
            LatestNews = new ObservableCollection<MainFeedPageGroup>()
            {
                new MainFeedPageGroup("HCMUT >", "HCMUT"),
                new MainFeedPageGroup("Văn phòng đào tạo quốc tế >", "OISP"),
                new MainFeedPageGroup("Phòng Đào Tạo >", "AAO"),
            };
            RefreshCommand = new Command(GetLatestNews);
            ShareCommand = new Command<News>(ShareAsync);
            BookmarkCommand = new Command<News>(BookmarkAsync);
            GetLatestNews();
        }
        public async void GetLatestNews()
        {
            try
            {
                IsBusy = true;
                List<Task> tasks = new List<Task>()
                {
                    Task.Run(async () =>
                    {
                         ObservableCollection<News> collection;
                        collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("HCMUT", 0, 5);
                        for (int i = 1; i < collection.Count; ++i)
                        {
                            if (User.CurrentUser.Bookmarks.Contains(collection[i])){
                                collection[i].IsBookmarkedByUser = true;
                            }
                            LatestNews[0].Add(collection[i]);
                        }
                        if (User.CurrentUser.Bookmarks.Contains(collection[0])){
                            collection[0].IsBookmarkedByUser = true;
                        }
                        LatestNews[0].FirstNews = (collection != null && collection.Count > 0) ? collection[0] : null;
                    }),
                    Task.Run(async () =>
                    {
                        ObservableCollection<News> collection;
                        collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("OISP", 0, 5);
                        for (int i = 1; i < collection.Count; ++i)
                        {
                            if (User.CurrentUser.Bookmarks.Contains(collection[i])){
                                collection[i].IsBookmarkedByUser = true;
                            }
                            LatestNews[1].Add(collection[i]);
                        }
                        if (User.CurrentUser.Bookmarks.Contains(collection[0])){
                            collection[0].IsBookmarkedByUser = true;
                        }
                        LatestNews[1].FirstNews =  (collection != null && collection.Count > 0) ? collection[0] : null;
                    }),
                    Task.Run(async () =>
                    {
                        ObservableCollection<News> collection;
                        collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("AAO", 0, 5);
                        for (int i = 1; i < collection.Count; ++i)
                        {
                            if (User.CurrentUser.Bookmarks.Contains(collection[i])){
                                collection[i].IsBookmarkedByUser = true;
                            }
                            LatestNews[2].Add(collection[i]);
                        }
                        if (User.CurrentUser.Bookmarks.Contains(collection[0])){
                            collection[0].IsBookmarkedByUser = true;
                        }
                        LatestNews[2].FirstNews =  (collection != null && collection.Count > 0) ? collection[0] : null;
                    })
                };
                Task.WaitAll(tasks.ToArray());
            } catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            } finally
            {
                IsBusy = false;
            }

        }


    }
   
    public class MainFeedPageGroup : ObservableCollection<News>
    {
        public string Title { get; set; }
        public string ShortName { get; set; } //will be used for jump lists
        public string Subtitle { get; set; }
        public ICommand GroupHeaderTappedCommand { get; set; }
        public News _firstNews;
        public News FirstNews
        {
            get
            {
                return _firstNews;
            }
            set
            {
                if (_firstNews != value)
                {
                    _firstNews = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("FirstNews"));
                }
            }
        }
        public void OnGroupHeaderTapped()
        {
            Debug.WriteLine("WTF");
            if (FirstNews != null)
            {
                Device.OpenUri(new Uri(FirstNews.NewsUrl));
            }
        }
        public MainFeedPageGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
            FirstNews = null;
            GroupHeaderTappedCommand = new Command(OnGroupHeaderTapped);
        }
    }
}
