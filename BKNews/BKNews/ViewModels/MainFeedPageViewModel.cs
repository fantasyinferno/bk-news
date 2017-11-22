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
                        news.IsBookmarkedByUser = true;
                        User.CurrentUser.Bookmarks.Add(news);
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
                new MainFeedPageGroup("Phòng Đào Tạo Sau Đại Hội >", "PGS"),
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
                var collection = await NewsManager.DefaultManager.GetLatestNews();
                Debug.WriteLine(collection);
                for (int i = 0; i < collection.Count; ++i)
                {
                    if (User.CurrentUser.Bookmarks.Contains(collection[i]))
                    {
                        collection[i].IsBookmarkedByUser = true;
                    }
                    if (i % 5 == 0)
                    {
                        LatestNews[i / 5].FirstNews = (collection != null && collection.Count > 0) ? collection[i] : null;
                    } else
                    {
                        LatestNews[i / 5].Add(collection[i]);
                    }
                }
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
