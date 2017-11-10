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
    class NewsViewModel : INotifyPropertyChanged
    {
        // offset
        public int Offset { get; set; }
        // category's name
        public string Category { get; set; }
        // mixed collection of news
        public ObservableCollection<News> NewsCollection { get; private set; }
        // command to bind with button
        public ICommand ScrapeCommand { get; set; }
        public ICommand LoadMore { get; set; }
        // list for storing scrapers
        public IScrape Scraper;
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
        // Scrape function. Once done, sent the scraped news to NewsCollection.
        public async Task ScrapeToCollectionAsync(bool append = false)
        {
            IsBusy = true;
            try
            {
                var list = await Scraper.Scrape();
                // individually add each item to the list (because we have to use ObservableCollection)
                if (append)
                {
                    foreach (var item in list)
                    {
                        NewsCollection.Add(item);
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        NewsCollection.Insert(0, item);
                    }
                }
            }
            catch (Exception e)
            {
                // do nothing
                Debug.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public NewsViewModel(String category, IScrape scraper)
        {
            Category = category;
            Scraper = scraper;
            NewsCollection = new ObservableCollection<News>();
            // command for button
            ScrapeCommand = new Command(async () =>
            {
                await ScrapeToCollectionAsync();
            });

            LoadMore = new Command(async () =>
            {
                await ScrapeToCollectionAsync(true);
            });
            LoadNextItems();
        }
        public void LoadNextItems(int num = 0)
        {
            NewsManager.DefaultManager.GetNewsAsync();
        }
    }
}