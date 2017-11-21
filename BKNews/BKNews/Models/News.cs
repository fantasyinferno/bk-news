using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.ComponentModel;

namespace BKNews
{
    // News class for abstracting news (obviously)
    public class News: INotifyPropertyChanged
    {
        // propagate property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
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
        // is this news bookmarked by the user?
        private bool _isBookmarkedByUser = false;
        public bool IsBookmarkedByUser
        {
            get
            {
                return _isBookmarkedByUser;
            }
            set
            {
                if (_isBookmarkedByUser != value)
                {
                    _isBookmarkedByUser = value;
                    OnPropertyChanged("IsBookmarkedByUser");
                }
            }
        }
        public News(string title, string desc, string author, string newsUrl, string imageUrl, DateTime newsDate, string type)
        {
            this.Title = title;
            this.Desc = desc;
            this.Author = author;
            this.NewsUrl = newsUrl;
            this.ImageUrl = imageUrl;
            this.NewsDate = newsDate;
            this.Type = type;
        }
    }
}
