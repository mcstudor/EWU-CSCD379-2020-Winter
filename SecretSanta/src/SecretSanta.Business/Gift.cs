using System;
using System.Collections.Generic;

namespace SecretSanta.Business
{
    public class Gift
    {
        public int Id { get; }
        public string Title { get => _Title; set => _Title = AssertIsNotNullOrWhitespace(value); }
        private string _Title = "INVALID";
        public string Description { get => _Description; set => _Description = AssertIsNotNullOrWhitespace(value); }
        private string _Description = "INVALID";
        public string Url { get => _Url; set => _Url = AssertIsNotNullOrWhitespace(value); }
        private string _Url = "INVALID";
        public User User { get => _User; set => _User = AssertIsNotNullOrWhitespace(value);  }
        private User _User = new User(-1, "INVALID", "INVALID", new List<Gift>());
        public Gift(int id, string title, string description, string url, User user)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            User = user;
        }

        private T AssertIsNotNullOrWhitespace<T>(T value) =>
            value switch
            {
                null => throw new ArgumentException($"{nameof(value)} cannot be empty.", nameof(value)),
                string temp when string.IsNullOrWhiteSpace(temp) =>
                throw new ArgumentException($"{nameof(value)} cannot be only whitespace.", nameof(value)),
                _ => value
            };

    }
}
