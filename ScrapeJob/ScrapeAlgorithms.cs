﻿using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ScrapeJob
{

    class AAOScraper : IScrape
    {


        public async Task<List<News>> Scrape(int i)
        {
            string url;
            if (i < 2)
            {
                url = @"http://www.aao.hcmut.edu.vn/index.php?route=catalog/thongbao";
            }
            else
            {
                url = @"http://www.aao.hcmut.edu.vn/index.php?route=catalog/thongbao&page=" + i.ToString();
            }
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
       

        public async Task<List<News>> Scrape(int i)
        {
            string url;
            if (i < 2)
            {
                url = @"http://oisp.hcmut.edu.vn/tin-tuc.html";
            }
            else
            {
                url = @"http://oisp.hcmut.edu.vn/tin-tuc.html?start=" + ((i - 1) * 10).ToString();
            }
            //Console.WriteLine(url);
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@id=\"itemListPrimary\"]//div[@class=\"catItemBody\"]");
            //Console.WriteLine("Scrape nodes");
            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                // get title, url and createdAt

                var title = node.SelectSingleNode(".//div[@class=\"catItemTitle\"]//a").InnerText.Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });
            //    Console.WriteLine(title);
                var desc = node.SelectSingleNode(".//div[@class=\"catItemIntroText\"]").InnerText;
            //    Console.WriteLine(desc);
                desc = desc.Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });
                while (desc.IndexOf("&nbsp;") != -1)
                {
                    desc = desc.Insert(desc.IndexOf("&nbsp;"), " ");
                    desc = desc.Remove(desc.IndexOf("&nbsp;"), 6);
                }
                // prepend rootUrl to the scraped urls (because the website uses relative urls)
                string rootUrl = "http://oisp.hcmut.edu.vn";
                var imageUrl = rootUrl + node.SelectSingleNode(".//span[@class=\"catItemImage\"]//img").Attributes["src"].Value;
            //    Console.WriteLine(imageUrl);
                var newsUrl = rootUrl + node.SelectSingleNode(".//a").Attributes["href"].Value;
            //    Console.WriteLine(newsUrl);
                // TODO: extract date string and convert it to DateTime
                var timenode = node.SelectSingleNode("//div[@class=\"catItemDateCreated\"]").InnerText;
            //    Console.WriteLine(timenode);
                string createdAtString = timenode.Remove(0, timenode.IndexOf(',') + 2).Trim(new char[] { (char)9, (char)10, (char)11, (char)32 });
            //    Console.WriteLine(createdAtString);
                createdAtString = createdAtString.Remove(createdAtString.IndexOf("Th"), 6);
                if (createdAtString.Length==9)
                {
                    createdAtString = createdAtString.Insert(3, "0");
                }
            //    Console.WriteLine(createdAtString);
            //    Console.WriteLine(createdAtString.Length);
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd MM yyyy", null);
            //    Console.WriteLine("end scraping");

                News news = new News(title, desc, "OISP", newsUrl, imageUrl, newsDate, "OISP");
                list.Add(news);
            }
            return list;
        }
    }
   
    class HCMUTScraper : IScrape
    {
        
        public async Task<List<News>> Scrape(int i)
        {
            string url;
            if (i < 2)
            {
                url = @"http://www.hcmut.edu.vn/vi/newsletter/category/tin-tuc/";
            }
            else
            {
                url = @"http://www.hcmut.edu.vn/vi/newsletter/category/tin-tuc/" + (3 * (i - 1)).ToString();
            }
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
        

        public async Task<List<News>> Scrape(int i)
        {
            string url;
            if (i<2)
            {
                url = @"http://www.pgs.hcmut.edu.vn/vi/thong-bao";
            }
            else
            {
                url = @"http://www.pgs.hcmut.edu.vn/vi/thong-bao?start=" + ((i - 1) * 10).ToString();
            }
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//li[@class=\"item-thongbao\"]");
            int count = 0;

            List<News> list = new List<News>();
            foreach (var node in nodes)
            {
                if (count > 9)
                    break;
                count++;
                // get title
                var title = node.SelectSingleNode(".//h3//a").InnerText;
            //    Console.WriteLine(title);
                // get desc
                var desc = "";
            //    Console.WriteLine(desc);

                // get url
                var imageUrl = @"http://www.pgs.hcmut.edu.vn/media/k2/items/cache/8c65de010bd08c28dd62a66cc800ec57_L.jpg";
            //    Console.WriteLine(imageUrl);
                var newsUrl = node.SelectSingleNode(".//h3//a").Attributes["href"].Value;
                newsUrl = "http://www.pgs.hcmut.edu.vn" + newsUrl;
            //    Console.WriteLine(newsUrl);
                // Get DateTime
                var createdAtString = node.SelectSingleNode(".//span[@class=\"date\"]").InnerText;
            //    Console.WriteLine(createdAtString);
                createdAtString = createdAtString.Remove(0, 6);
            //    Console.WriteLine(createdAtString);
                DateTime newsDate = DateTime.ParseExact(createdAtString, "dd/MM/yyyy", null);
                // Create News
                News news = new News(title, desc, "PGS", newsUrl, imageUrl, newsDate, "PGS");
                list.Add(news);
            }
            return list;
        }
    }

}