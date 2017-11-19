using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace BKNews
{
    static class ScrapingSystem
    {

        static Dictionary<string, IScrape> Scrapers;
        public static ObservableCollection<News> Updates;
        static ScrapingSystem()
        {
            Scrapers = new Dictionary<string, IScrape> { { "AAO", new AAOScraper() }, { "OISP", new OISPScraper() }, { "HCMUT", new HCMUTScraper() }, { "PGS", new PGSScraper() } };
            Updates = new ObservableCollection<News>();
        }
        public async static Task ScrapeAsync(string category)
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
        public static async Task ScrapeAllAsync()
        {
            foreach(var key in Scrapers.Keys)
            {
                await ScrapeAsync(key);
            }
        }
        /// <summary>
        /// Scrape tasks periodically with a specified period.
        /// </summary>
        /// <param name="period">The period after with to scrape</param>
        /// <param name="cancellationToken">The cancellation token which can be used to stop the task</param>
        /// <returns></returns>
        public static async Task StartPeriodicScrapeJobAsync(TimeSpan period, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(period, cancellationToken);
                if (!cancellationToken.IsCancellationRequested)
                {
                    Debug.WriteLine("Job started at {0}.", DateTime.Now);
                    await ScrapeAllAsync();
                }
            }
        }
    }
}
