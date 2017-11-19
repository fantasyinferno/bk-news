﻿using HtmlAgilityPack;
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
                // get the de entitized value because ampersands in the URL get encoded in the original value
                var newsUrl = node.SelectSingleNode(".//a").Attributes["href"].DeEntitizeValue;
                // delete parenthesises at the beginning and end of date
                var createdAtString = node.SelectSingleNode(".//span").InnerHtml.Trim(new char[]{'(', ')'});
                var imageUrl = node.SelectSingleNode(".//div[@class=\"img-box-item\"]//img").Attributes["src"].Value;
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                News news = new News(title, "...", "Phòng đào tạo", newsUrl, imageUrl, newsDate, "AAO");
                list.Add(news);
            }
            return list;
        }

        public async Task<List<News>> Scrape(int i)
        {
            var url = @"http://www.aao.hcmut.edu.vn/index.php?route=catalog/thongbao&page="+i.ToString();
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//ul[@class=\"slider-items slider-line-1\"]/li");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title, url and createdAt
                var title = node.SelectSingleNode(".//h3").InnerHtml;
                var newsUrl = node.SelectSingleNode(".//a").Attributes["href"].DeEntitizeValue;
                // delete parenthesises at the beginning and end of date
                var createdAtString = node.SelectSingleNode(".//span").InnerHtml.Trim(new char[] { '(', ')' });
                var imageUrl = node.SelectSingleNode(".//div[@class=\"img-box-item\"]//img").Attributes["src"].Value;
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                News news = new News(title, null, "Phòng đào tạo", newsUrl, imageUrl, newsDate, "AAO");
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
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd MM yyyy", null);
                News news = new News(title, desc, "OISP", newsUrl, imageUrl, newsDate, "OISP");
                list.Add(news);
            }
            return list;
        }

        public async Task<List<News>> Scrape(int i)
        {
            var url = @"http://oisp.hcmut.edu.vn/tin-tuc.html?start=" + ((i-1)*10).ToString();
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"itemListPrimary\"]//div[@class=\"catItemBody\"]");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title, url and createdAt

                var title = node.SelectSingleNode(".//div[@class=\"catItemTitle\"]//a").InnerText.Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });

                var desc = node.SelectSingleNode(".//div[@class=\"catItemIntroText\"]").InnerText;
                desc = desc.Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });
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
                string createdAtString = timenode.Remove(0, timenode.IndexOf(',') + 2).Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });
                createdAtString = createdAtString.Remove(createdAtString.IndexOf("Th"), 6);
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd MM yyyy", null);

                News news = new News(title, desc, "OISP", newsUrl, imageUrl, newsDate, "OISP");
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
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                // Create News
                News news = new News(title, desc, "HCMUT", newsUrl, imageUrl, newsDate, "HCMUT");
                list.Add(news);
            }
            return list;
        }

        public async Task<List<News>> Scrape(int i)
        {
            var url = @"http://www.hcmut.edu.vn/vi/newsletter/category/tin-tuc/" + (3*(i-1)).ToString();
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
                string createdAtString = timenode.Remove(timenode.IndexOf(',') - 6).Remove(0, timenode.IndexOf(':') + 2);
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                // Create News
                News news = new News(title, desc, "HCMUT", newsUrl, imageUrl, newsDate, "HCMUT");
                list.Add(news);
            }
            return list;
        }
    }

    class PGSScraper : IScrape
    {
        public async Task<List<News>> Scrape()
        {
            var url = @"http://www.pgs.hcmut.edu.vn/vi/thong-bao";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//li[@class=\"item-thongbao\"]");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title
                var title = node.SelectSingleNode(".//h3//a").InnerText;
         //       var title = "LỊCH THU HỌC PHÍ HK1/2017-2018 LẦN 2";
                // get desc
                var desc = "";

                // get url
                var imageUrl = @"http://www.pgs.hcmut.edu.vn/media/k2/items/cache/8c65de010bd08c28dd62a66cc800ec57_L.jpg";
                var newsUrl = node.SelectSingleNode(".//h3//a").Attributes["href"].Value;
         //       var newsUrl = @"http://www.pgs.hcmut.edu.vn/vi/thong-bao/thong-tin-chung/item/1848-lich-thu-hoc-phi-hk1-2017-2018-lan-2";
                newsUrl = "http://www.pgs.hcmut.edu.vn" + newsUrl;
                // Get DateTime
                var createdAtString = node.SelectSingleNode(".//span[@class=\"date\"]").InnerText;
                createdAtString = createdAtString.Remove(0, 5);
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                // Create News
                News news = new News(title, desc, "PGS", newsUrl, imageUrl, newsDate, "PGS");
                list.Add(news);
            }
            return list;
        }

        public async Task<List<News>> Scrape(int i)
        {
            var url = @"http://www.pgs.hcmut.edu.vn/vi/thong-bao?start=" + ((i)*10).ToString();
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//li[@class=\"item-thongbao\"]");

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title
                var title = node.SelectSingleNode(".//h3//a").InnerText;
                // get desc
                var desc = "";

                // get url
                var imageUrl = @"http://www.pgs.hcmut.edu.vn/media/k2/items/cache/8c65de010bd08c28dd62a66cc800ec57_L.jpg";
                var newsUrl = node.SelectSingleNode(".//h3//a").Attributes["href"].Value;
                newsUrl = "http://www.pgs.hcmut.edu.vn" + newsUrl;
                // Get DateTime
                var createdAtString = node.SelectSingleNode(".//span[@class=\"date\"]").InnerText;
                createdAtString = createdAtString.Remove(0, 5);
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                // Create News
                News news = new News(title, desc, "PGS", newsUrl, imageUrl, newsDate, "PGS");
                list.Add(news);
            }
            return list;
        }
    }

}
