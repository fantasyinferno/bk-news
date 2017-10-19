using System;

namespace BKNews
{
    // News class for abstracting news (obviously)
    class News
    {
        public News(string title, string desc, string author, string newsUrl, string imageUrl, DateTime createdAt, DateTime updatedAt)
        {
            Title = title;
            Desc = desc;
            Author = author;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            NewsUrl = newsUrl;
            ImageUrl = imageUrl;
        }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string NewsUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
