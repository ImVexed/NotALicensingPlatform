using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;

namespace Server.Database
{
    public class User
    {
        [Key] [BsonId]
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public List<Key> Keys { get; set; }
        public bool Banned { get; set; }

        public User(string Username, byte[] Password)
        {
            this.Username = Username;
            this.Password = Password;
            this.Keys = new List<Key>();
            this.Banned = false;
        }

        public bool CheckPassword(byte[] password)
        {
            using (var sha = SHA256.Create())
            {
                var hash = new byte[32];

                if (!sha.TryComputeHash(password, hash, out _))
                    return false;

                return hash.SequenceEqual(this.Password);
            }
        }

        public void AddKey(Key key)
        {
            key.ActivatedOn = DateTime.Now;

            this.Keys.Add(key);
        }

        public bool IsActive()
        {
            return this.Keys.Any(x => x.ActivatedOn + x.ValidFor > DateTime.Now);
        }
    }

    public class Key
    {
        [Key] [BsonId]
        public string Identifier { get; set; }
        public TimeSpan ValidFor { get; set; }
        public DateTime ActivatedOn { get; set; }

        public Key(TimeSpan ValidFor, string Identifier = null)
        {
            this.Identifier = Identifier ?? Guid.NewGuid().ToString();
            this.ValidFor = ValidFor;
        }
    }
}