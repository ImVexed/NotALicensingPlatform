using MongoDB.Driver;

namespace Server.Database
{
    internal class Mongo : IDBProvider
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<Key> keyCol;
        private readonly IMongoCollection<User> userCol;

        public Mongo(string ConnectionString)
        {
            mongoClient = new MongoClient(ConnectionString);
            database = mongoClient.GetDatabase("nalp");
            userCol = database.GetCollection<User>("Users");
            keyCol = database.GetCollection<Key>("Keys");
        }

        public bool CreateKey(Key key)
        {
            try
            {
                keyCol.InsertOne(key);
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
                userCol.InsertOne(user);
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
            var result = keyCol.DeleteOne(filter);
            if (!result.IsAcknowledged)
            {
                return false;
            }
            return result.DeletedCount == 1;
        }

        public Key GetKey(string id)
        {
            var filter = Builders<Key>.Filter.Eq(x => x.Identifier, id);
            var key = keyCol.Find(filter).FirstOrDefault();
            return key;
        }

        public User GetUser(string username)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Username, username);
            var user = userCol.Find(filter).FirstOrDefault();
            return user;
        }

        public bool UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Username, user.Username);
            var result = userCol.ReplaceOne(filter, user);
            if (!result.IsAcknowledged)
            {
                return false;
            }
            return result.ModifiedCount == 1;
        }
    }
}