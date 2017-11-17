using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace BKNews
{
    static class ScrapingSystem
    {

        static Dictionary<string, IScrape> Scrapers;
        public static ObservableCollection<News> Updates;
        static ScrapingSystem()
        {
            Scrapers = new Dictionary<string, IScrape> { { "AAO", new AAOScraper() }, { "OISP", new OISPScraper() }, { "HCMUT", new HCMUTScraper() } };
            Updates = new ObservableCollection<News>();
        }
        public async static Task Scrape(string category)
        {
            try
            {
                var Scraper = Scrapers[category];
                List<News> updates = new List<News>();
                // Scrape first 1000 pages if possible
                for (int i = 1; i < 1000; ++i)
                {
                    var list = await Scraper.Scrape(i);
                    // individually add each item to the list (because we have to use ObservableCollection)
                    foreach (var item in list)
                    {
                        // should fail on duplicates
                        await NewsManager.DefaultManager.SaveNewsAsync(item);
                        Updates.Add(item);
                    }
                }

            }
            catch (InvalidOperationException)
            {
                // duplicates
            }
            catch (Exception)
            {
                // do nothing 
            }
        }
        public static async Task ScrapeAll()
        {
            foreach(var key in Scrapers.Keys)
            {
                Scrape(key);
            }
        }
    }
}
