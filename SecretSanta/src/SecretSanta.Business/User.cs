using System;
using System.Collections.Generic;

namespace SecretSanta.Business
{
    public class User
    {
        public int Id { get; }
        public string FirstName
        {
            get => _FirstName;
            set => _FirstName = AssertIsNotNullOrWhitespace(value);
        }
        private string _FirstName = "INVALID";
        public string LastName
        {
            get => _LastName;
            set => _LastName = AssertIsNotNullOrWhitespace(value);
        }
        private string _LastName = "INVALID";
        public IEnumerable<Gift> Gifts
        {
            get => _Gifts;
            set => _Gifts = AssertIsNotNullOrWhitespace(value);
        }
        private IEnumerable<Gift> _Gifts= new List<Gift>();

        public User(int id, string firstName, string lastName, IEnumerable<Gift> gifts)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gifts = gifts;

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
