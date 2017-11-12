using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BKNews
{
    class MainFeedPageViewModel
    {
        ObservableCollection<News> AAONews;
        ObservableCollection<News> OISPNews;
        ObservableCollection<News> HCMUTNews;

        public MainFeedPageViewModel()
        {
            AAONews = new ObservableCollection<News>();
            OISPNews = new ObservableCollection<News>();
            HCMUTNews = new ObservableCollection<News>();
        }
        public async void GetLatestNews()
        {
            IEnumerable<News> collection;
            AAONews.Clear();
            collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("AAO", 0, 5);
            foreach (var item in collection)
            {
                AAONews.Add(item);
            }
            OISPNews.Clear();
            collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("OISP", 0, 5);
            foreach (var item in collection)
            {
                OISPNews.Add(item);
            }
            HCMUTNews.Clear();
            collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync("HCMUT", 0, 5);
            foreach (var item in collection)
            {
                HCMUTNews.Add(item);
            }

        }
    }
}
