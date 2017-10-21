using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace BKNews
{
    class AAOScraper: IScrape
    {
        public List<News> Scrape()
        {
            var url = @"http://www.aao.hcmut.edu.vn/index.php?route=catalog/thongbao";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//ul[@class=\"slider-items slider-line-1\"]/li");

            List<News> list = new List<News>();
            foreach(var node in nodes)
            {
                // get title, url and createdAt
                var title = node.SelectSingleNode(".//h3").InnerHtml;
                var newsUrl = node.SelectSingleNode(".//a").Attributes["href"].Value;
                // delete parenthesises at the beginning and end of date
                var createdAtString = node.SelectSingleNode(".//span").InnerHtml.Trim(new char[]{'(', ')'});
                var imageUrl = node.SelectSingleNode(".//div[@class=\"img-box-item\"]//img").Attributes["src"].Value;
                DateTime createdAt = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                News news = new News(title, "Click to see more details", "Phòng đào tạo", newsUrl, imageUrl, createdAt, createdAt);
                list.Add(news);
            }
            return list;
        }
    }
    class OISPScraper : IScrape
    {
        public List<News> Scrape()
        {
            var url = @"http://oisp.hcmut.edu.vn/tin-tuc.html";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"itemListPrimary\"]//div[@class=\"catItemBody\"]");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title, url and createdAt
                var title = node.SelectSingleNode(".//div[@class=\"catItemTitle\"]//a").InnerText;
//                title = title.TrimStart(char(10));
                var desc = node.SelectSingleNode(".//div[@class=\"catItemIntroText\"]").InnerText;
                // prepend rootUrl to the scraped urls (because the website uses relative urls)
                string rootUrl = "http://oisp.hcmut.edu.vn/";
                var imageUrl = rootUrl + node.SelectSingleNode(".//span[@class=\"catItemImage\"]//img").Attributes["src"].Value;
                var newsUrl = rootUrl + node.SelectSingleNode(".//a").Attributes["href"].Value;
                // TODO: extract date string and convert it to DateTime
                News news = new News(title, "Click to see more details", "OISP", newsUrl, imageUrl, DateTime.Now, DateTime.Now);
                list.Add(news);
            }
            return list;
        }
    }
}
