using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
namespace BKNews
{
    class NewsViewModel
    {
        // mixed collection of news
        public ObservableCollection<News> NewsCollection { get; private set; }
        // command to bind with button
        public ICommand ScrapeCommand { get; set; }
        // list for storing scrapers
        public List<IScrape> Scrapers;
        void onItemTapped()
        {

        }
        public NewsViewModel()
        {
            Scrapers = new List<IScrape>();
            Scrapers.Add(new AAOScraper());
            Scrapers.Add(new OISPScraper());
            NewsCollection = new ObservableCollection<News>();
            ScrapeCommand = new Command(() =>
            {
                foreach (var scraper in Scrapers)
                {
                    try
                    {
                        var list = scraper.Scrape();
                        // individually add each item to the list (because we have to use ObservableCollection)
                        foreach (var item in list)
                        {
                            NewsCollection.Add(item);
                        }
                    } catch(Exception e)
                    {
                        // do nothing
                    }

                }
            });
        }
    }
}
