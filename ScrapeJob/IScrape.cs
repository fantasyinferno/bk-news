using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScrapeJob
{
    public interface IScrape
    {
        Task<List<News>> Scrape(int i);
    }
}
