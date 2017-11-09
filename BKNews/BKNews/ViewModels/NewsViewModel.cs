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
    class NewsViewModel: INotifyPropertyChanged
    {
        // collection of news' title
        public ObservableCollection<String> NewsTitle { get; private set; }
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
<<<<<<< HEAD
            Scrapers = new List<IScrape>();
            Scrapers.Add(new AAOScraper());
            Scrapers.Add(new OISPScraper());
            Scrapers.Add(new HCMUTScraper());
            NewsCollection = new ObservableCollection<News>();
            NewsTitle = new ObservableCollection<String>();
            // command for button
            ScrapeCommand = new Command(async () =>
=======
            var changed = PropertyChanged;
            if (changed != null)
>>>>>>> 0f422e36cb725586c4345d0c493751c3bf2aaf80
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
<<<<<<< HEAD
                    try
                    {
                        
                        var list = await scraper.Scrape();

                        // individually add each item to the list (because we have to use ObservableCollection)
                        foreach (var item in list)
                        {
                        
                            await NewsManager.DefaultManager.SaveTaskAsync(item);

                            
                            if (NewsTitle.IndexOf(item.Title)<0)
                            {
                                NewsTitle.Add(item.Title);
                                NewsCollection.Add(item);
                            }
                            
                        }
                        
                        var list2 = await scraper.Scrape(2);
                        foreach (var item in list2)
                        {
                            if (NewsTitle.IndexOf(item.Title) < 0)
                            {
                                NewsTitle.Add(item.Title);
                                NewsCollection.Add(item);
                                await NewsManager.DefaultManager.SaveTaskAsync(item);
                            }

                        }

                    } catch(Exception e)
=======
                    var list = await scraper.Scrape();
                    // individually add each item to the list (because we have to use ObservableCollection)
                    foreach (var item in list)
>>>>>>> 0f422e36cb725586c4345d0c493751c3bf2aaf80
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
