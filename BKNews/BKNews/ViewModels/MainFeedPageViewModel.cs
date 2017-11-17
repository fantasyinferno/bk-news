using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
namespace BKNews
{
    class MainFeedPageViewModel
    {
        public ObservableCollection<MainFeedPageGroup> LatestNews { get; set; }
        public ICommand RefreshCommand { get; set; }
        public MainFeedPageViewModel()
        {
            LatestNews = new ObservableCollection<MainFeedPageGroup>()
            {
                new MainFeedPageGroup("Phòng Đào Tạo >", "AAO"),
                new MainFeedPageGroup("Văn phòng đào tạo quốc tế >", "OISP"),
                new MainFeedPageGroup("HCMUT >", "HCMUT")
            };
            RefreshCommand = new Command(async () =>
            {
                await GetLatestNews();
            });
        }
        public async Task GetLatestNews()
        {

            ObservableCollection<News> collection;
            collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("AAO", 0, 5);
            for (int i = 1; i < collection.Count; ++i)
            {
                LatestNews[0].Add(collection[i]);
            }
            LatestNews[0].FirstNews = collection.Count > 0 ? collection[0] : null;
            collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("OISP", 0, 5);
            for (int i = 1; i < collection.Count; ++i)
            {
                LatestNews[1].Add(collection[i]);
            }
            LatestNews[1].FirstNews = collection.Count > 0 ? collection[1] : null;

            collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("HCMUT", 0, 5);
            for (int i = 1; i < collection.Count; ++i)
            {
                LatestNews[2].Add(collection[i]);
            }
            LatestNews[2].FirstNews = collection.Count > 0 ? collection[2] : null;

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
            Device.OpenUri(new Uri(FirstNews.NewsUrl));
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
