using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace BKNews
{
    // News class for abstracting news (obviously)
    public class NewsUser
    {
        string id;
        string newsId;
        string userId;
        public ObservableCollection<News> NewsBookmark { get; private set; }
        // Construct JSON properties for sending to Azure Mobile Services
        [JsonProperty(PropertyName = "id")]
        public string Id { get { return id; } set { id = value; } }
        [JsonProperty(PropertyName = "newsId")]
        public string NewsId { get { return id; } set { id = value; } }
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get { return id; } set { id = value; } }

        public NewsUser(string newsId, string userId)
        {
            this.NewsId = newsId;
            this.UserId = userId;
        }

        public bool bookmark(News news)
        {
            foreach (var pnews in NewsBookmark)
            {
                if (pnews.NewsUrl == news.NewsUrl)
                {
                    return false;
                }
            }
            NewsBookmark.Add(news);
            return true;
        }

        public bool unBookmark(News news)
        {
            foreach (var pnews in NewsBookmark)
            {
                if (pnews.NewsUrl == news.NewsUrl)
                {
                    NewsBookmark.Remove(pnews);
                    return true;
                }
            }
            return false;
        }


    }
}
