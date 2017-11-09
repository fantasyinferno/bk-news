using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
namespace BKNews
{
    class NewsViewModel
    {
        // collection of news' title
        public ObservableCollection<String> NewsTitle { get; private set; }
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
            Scrapers.Add(new HCMUTScraper());
            NewsCollection = new ObservableCollection<News>();
            NewsTitle = new ObservableCollection<String>();
            // command for button
            ScrapeCommand = new Command(async () =>
            {
                foreach (var scraper in Scrapers)
                {
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
                    {
                        // do nothing

                    }
                }
            });
        }
    }
}
