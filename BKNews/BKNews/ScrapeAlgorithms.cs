using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace BKNews
{

    class AAOScraper: IScrape
    {
        public async Task<List<News>> Scrape()
        {
            var url = @"http://www.aao.hcmut.edu.vn/index.php?route=catalog/thongbao";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
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
        public async Task<List<News>> Scrape()
        {
            var url = @"http://oisp.hcmut.edu.vn/tin-tuc.html";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"itemListPrimary\"]//div[@class=\"catItemBody\"]");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title, url and createdAt

                var title = node.SelectSingleNode(".//div[@class=\"catItemTitle\"]//a").InnerText.Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });

                var desc = node.SelectSingleNode(".//div[@class=\"catItemIntroText\"]").InnerText;
                desc = desc.Trim(new char[] { (char)9, (char)10, (char)11, (char)32});
                while (desc.IndexOf("&nbsp;") != -1)
                {
                    desc = desc.Insert(desc.IndexOf("&nbsp;"), " ");
                    desc = desc.Remove(desc.IndexOf("&nbsp;"), 6);
                }
                // prepend rootUrl to the scraped urls (because the website uses relative urls)
                string rootUrl = "http://oisp.hcmut.edu.vn";
                var imageUrl = rootUrl + node.SelectSingleNode(".//span[@class=\"catItemImage\"]//img").Attributes["src"].Value;
                var newsUrl = rootUrl + node.SelectSingleNode(".//a").Attributes["href"].Value;
                // TODO: extract date string and convert it to DateTime
                var timenode = node.SelectSingleNode("//div[@class=\"catItemDateCreated\"]").InnerText;
                string createdAtString = timenode.Remove(0, timenode.IndexOf(',') + 2).Trim(new char[] {(char) 9, (char) 10, (char) 11, (char) 32});
                createdAtString = createdAtString.Remove(createdAtString.IndexOf("Th"), 6);
                DateTime createdAt = DateTime.ParseExact(createdAtString, "dd MM yyyy", null);

                News news = new News(title, desc + "\nClick to see more details", "OISP", newsUrl, imageUrl, createdAt, createdAt);
                list.Add(news);
            }
            return list;
        }
    }
/*
    class HCMUTScraper : IScrape
    {
        public async Task<List<News>> Scrape()
        {
            var url = @"http://www.hcmut.edu.vn/vi/newsletter/category/tin-tuc/";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class=\"panel_1_content\"]");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title
                var title = node.SelectSingleNode(".//h3").InnerText;
                title = title.Remove(title.IndexOf(" &nbsp"));
                // get desc

                var desc = node.SelectSingleNode(".//p").InnerText;
                // get url
                var imageUrl = node.SelectSingleNode(".//a//img").Attributes["src"].Value;
                var newsUrl = node.SelectSingleNode(".//a").Attributes["href"].Value;
                // Get DateTime
                var webNode = new HtmlWeb();
                var docNode = await webNode.LoadFromWebAsync(newsUrl);
                var timenode = docNode.DocumentNode.SelectSingleNode("//p[@class=\"date\"]").InnerText;
                string createdAtString = timenode.Remove(timenode.IndexOf(',') - 6).Remove(0, timenode.IndexOf(':') + 2);
                DateTime createdAt = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                // Create News
                News news = new News(title, desc + "\nClick to see more details", "HCMUT", newsUrl, imageUrl, createdAt, createdAt);
                list.Add(news);
            }
            return list;
        }
    }
    */
    class HCMUTScraper : IScrape
    {
        public async Task<List<News>> Scrape()
        {
            var url = @"http://www.hcmut.edu.vn/vi/newsletter/category/tin-tuc/";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class=\"panel_1_content\"]");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title
                var title = node.SelectSingleNode(".//h3").InnerText;
                title = title.Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });
                while (title.IndexOf("&nbsp;") != -1)
                {
                    title = title.Insert(title.IndexOf("&nbsp;"), " ");
                    title = title.Remove(title.IndexOf("&nbsp;"), 6);
                }
                // get desc
                var desc = node.SelectSingleNode(".//p").InnerText;
                desc = desc.Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });
                while (desc.IndexOf("&nbsp;") != -1)
                {
                    desc = desc.Insert(desc.IndexOf("&nbsp;"), " ");
                    desc = desc.Remove(desc.IndexOf("&nbsp;"), 6);
                }
                // get url
                var imageUrl = node.SelectSingleNode(".//a//img").Attributes["src"].Value;
                var newsUrl = node.SelectSingleNode(".//a").Attributes["href"].Value;
                // Get DateTime
                var webNode = new HtmlWeb();
                var docNode = webNode.Load(newsUrl);
                var timenode = docNode.DocumentNode.SelectSingleNode("//p[@class=\"date\"]").InnerText;
                string createdAtString = timenode.Remove(timenode.IndexOf(',')-6).Remove(0, timenode.IndexOf(':') + 2);
                DateTime createdAt = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                // Create News
                News news = new News(title, desc + "\nClick to see more details", "HCMUT", newsUrl, imageUrl, createdAt, createdAt);
                list.Add(news);
            }
            return list;
        }
    }

}
