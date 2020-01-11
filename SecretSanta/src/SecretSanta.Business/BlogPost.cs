using System;

namespace SecretSanta.Business
{
    public class BlogPost
    {
        public string Title { get; }
        private string _Content = string.Empty;

        public string Content
        {
            get => _Content;
            set => _Content = value ?? throw new ArgumentNullException(nameof(value));
        }

        public DateTime Date { get; }
        public string Author { get; }

        public BlogPost(string title, string content, DateTime date, string author)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content;
            Date = date;
            Author = author ?? throw new ArgumentNullException(nameof(author));
        }
    }
}
