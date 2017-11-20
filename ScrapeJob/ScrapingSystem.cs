using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeJob
{
    class ScrapingSystem
    {

        Dictionary<string, IScrape> Scrapers = new Dictionary<string, IScrape> { { "AAO", new AAOScraper() }, { "OISP", new OISPScraper() }, { "HCMUT", new HCMUTScraper() } };
        public ObservableCollection<News> Updates = new ObservableCollection<News>();
        public static ScrapingSystem System = new ScrapingSystem();
        public async Task Scrape(string category)
        {
            try
            {
                var Scraper = this.Scrapers[category];
                List<News> updates = new List<News>();
                // Scrape first 1000 pages if possible
                for (int i = 1; i < 1000; ++i)
                {
                    Console.WriteLine("Scraping page {0} of {1}.", i, category);
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
        public void ScrapeAll()
        {
            List<Task> allTasks = new List<Task>();
            foreach(var key in this.Scrapers.Keys)
            {
                allTasks.Add(this.Scrape(key));
            }
            Task.WaitAll(allTasks.ToArray());
        }
    }
}
