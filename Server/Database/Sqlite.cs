using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Server.Database
{
    public class Sqlite : IDBProvider
    {
        public static string DataDirectory = "data/";

        private readonly UserContext db;

        public Sqlite(string DataDirectory)
        {
            Sqlite.DataDirectory = DataDirectory;

            db = new UserContext();

            if (!db.Database.CanConnect())
                throw new Exception($"Failed to open database at {Path.Combine(Environment.CurrentDirectory, DataDirectory, "nalp.db")}");
        }

        public bool CreateUser(User user)
        {
            db.Users.Add(user);

            try
            {
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public User GetUser(string username)
        {
            var user = db.Users.Find(username);

            if(user != null)
                db.Entry(user).Collection(x => x.Keys).Load();

            return user;
        }

        public bool UpdateUser(User user)
        {
            db.Users.Update(user);

            try
            {
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateKey(Key key)
        {
            db.Keys.Add(key);

            try
            {
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Key GetKey(string id)
        {
            return db.Keys.Find(id);
        }

        public bool DeleteKey(Key key)
        {
            db.Keys.Remove(key);

            try
            {
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    internal class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Key> Keys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Path.Combine(Sqlite.DataDirectory, "nalp.db")}");
        }
    }
}