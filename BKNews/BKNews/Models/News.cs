using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace BKNews
{
    // News class for abstracting news (obviously)
    public class News
    {
        string id;
        string title;
        string desc;
        string author;
        string imageUrl;
        string newsUrl;
        string type;
        DateTime newsDate;
        // Construct JSON properties for sending to Azure Mobile Services
        [JsonProperty(PropertyName = "id")]
        public string Id { get { return id; } set { id = value; } }
        [JsonProperty(PropertyName = "title")]
        public string Title { get { return title; } set { title = value; } }
        [JsonProperty(PropertyName = "desc")]
        public string Desc { get { return desc; } set { desc = value; } }
        [JsonProperty(PropertyName = "author")]
        public string Author { get { return author; }  set { author = value; } }
        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get { return imageUrl; } set { imageUrl = value; } }
        [JsonProperty(PropertyName = "newsUrl")]
        public string NewsUrl { get { return newsUrl; } set { newsUrl = value; } }
        [JsonProperty(PropertyName = "newsDate")]
        public DateTime NewsDate { get { return newsDate; } set { newsDate = value; } }
        [JsonProperty(PropertyName = "type")]
        public string Type { get { return type; } set { type = value; } }

        public News(string title, string desc, string author, string newsUrl, string imageUrl, DateTime newsDate, string type)
        {
            Title = title;
            Desc = desc;
            Author = author;
            NewsUrl = newsUrl;
            ImageUrl = imageUrl;
            NewsDate = newsDate;
            Type = type;
        }
    }
}
