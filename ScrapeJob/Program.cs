using System;

namespace ScrapeJob
{
    class Program
    {
        static void Main(string[] args)
        {
            // ScrapeAll() function to scrape all news asynchronously (from all sources at once)
            ScrapingSystem.System.ScrapeAll();
        }
    }
}
