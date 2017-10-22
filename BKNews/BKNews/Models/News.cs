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
        DateTime newsDate;
        // Construct JSON properties for sending to Azure Mobile Services
        [JsonProperty(PropertyName = "id")]
        public string Id { get => id; set => id = value; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get => title; set => title = value; }
        [JsonProperty(PropertyName = "desc")]
        public string Desc { get => desc; set => desc = value; }
        [JsonProperty(PropertyName = "author")]
        public string Author { get => author; set => author = value; }
        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get => imageUrl; set => imageUrl = value; }
        [JsonProperty(PropertyName = "newsUrl")]
        public string NewsUrl { get => newsUrl; set => newsUrl = value; }
        [JsonProperty(PropertyName = "newsDate")]
        public DateTime NewsDate { get => newsDate; set => newsDate = value; }

        public News(string title, string desc, string author, string newsUrl, string imageUrl, DateTime newsDate)
        {
            Title = title;
            Desc = desc;
            Author = author;
            NewsUrl = newsUrl;
            ImageUrl = imageUrl;
            NewsDate = newsDate;
        }
    }
}
