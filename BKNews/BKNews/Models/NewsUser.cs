﻿using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

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
        public string NewsId { get { return newsId; } set { newsId = value; } }
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get { return userId; } set { userId = value; } }

        public NewsUser(string newsId, string userId)
        {
            this.NewsId = newsId;
            this.UserId = userId;
        }
    }
}
