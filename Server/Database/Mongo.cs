using MongoDB.Driver;

namespace Server.Database
{
    internal class Mongo : IDBProvider
    {
        private readonly IMongoCollection<Key> keys;
        private readonly IMongoCollection<User> users;

        public Mongo(string ConnectionString)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase("nalp");

            users = db.GetCollection<User>("Users");
            keys = db.GetCollection<Key>("Keys");
        }

        public bool CreateKey(Key key)
        {
            try
            {
                keys.InsertOne(key);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateUser(User user)
        {
            try
            {
                users.InsertOne(user);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteKey(Key key)
        {
            var filter = Builders<Key>.Filter.Eq(x => x.Identifier, key.Identifier);
            var result = keys.DeleteOne(filter);

            return result.IsAcknowledged && result.DeletedCount == 1;
        }

        public Key GetKey(string id)
        {
            var filter = Builders<Key>.Filter.Eq(x => x.Identifier, id);

            return keys.Find(filter).FirstOrDefault();
        }

        public User GetUser(string username)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Username, username);

            return users.Find(filter).FirstOrDefault();
        }

        public bool UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Username, user.Username);
            var result = users.ReplaceOne(filter, user);

            return result.IsAcknowledged && result.ModifiedCount == 1;
        }
    }
}