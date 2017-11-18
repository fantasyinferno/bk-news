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

    }
}
