using System.Collections.Generic;
using System.Threading.Tasks;

namespace BKNews
{
    interface IScrape
    {
        Task<List<News>> Scrape();
        Task<List<News>> Scrape(int i);
    }
}
