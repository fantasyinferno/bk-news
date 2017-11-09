using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Xamarin.Forms;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BKNews
{
    class NewsViewModel : INotifyPropertyChanged
    {
        // mixed collection of news
        public ObservableCollection<News> NewsCollection { get; private set; }
        // command to bind with button
        public ICommand ScrapeCommand { get; set; }
        // list for storing scrapers
        public List<IScrape> Scrapers;
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
        public async Task ScrapeToCollectionAsync()
        {
            IsBusy = true;
            foreach (var scraper in Scrapers)
            {
                try
                {
                    var list = await scraper.Scrape();
                    // individually add each item to the list (because we have to use ObservableCollection)
                    foreach (var item in list)
                    {
                        NewsCollection.Add(item);
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
        }
        public NewsViewModel()
        {
            Scrapers = new List<IScrape>
            {
                new OISPScraper(),
                new AAOScraper(),
                new HCMUTScraper()
            };
            NewsCollection = new ObservableCollection<News>();
            // command for button
            ScrapeCommand = new Command(async () =>
            {
                await ScrapeToCollectionAsync();
            });
        }
    }
}