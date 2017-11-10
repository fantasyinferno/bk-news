using System.Collections.Generic;
using System.Threading.Tasks;

namespace BKNews
{
    public interface IScrape
    {
        Task<List<News>> Scrape();
    }
}
