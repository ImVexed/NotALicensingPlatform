using NotLiteCode.Server;
using Server.Database;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Server.NLC
{
    internal class SharedClass : IDisposable
    {
        User user;

        [NLCCall("Login")]
        public bool Login(string username, byte[] password, byte[] hwid)
        {
            // Ensure a user isn't already logged in
            if (this.user != null)
                return false;

            var user = RemoteProvider.Db.GetUser(username);

            if (user == null)
                return false;

            if (!user.CheckPassword(password) ||
                !user.CheckHWID(hwid) ||
                !user.IsActive())
                return false;

            this.user = user;

            return true;
        }

        [NLCCall("Register")]
        public bool Register(string username, byte[] password, byte[] hwid, string keyid)
        {
            // Ensure a user isn't already logged in
            if (this.user != null)
                return false;

            // Check if a user already exists with this username
            if (RemoteProvider.Db.GetUser(username) != null)
                return false;

            var key = RemoteProvider.Db.GetKey(keyid);

            if (key == null)
                return false;

            var user = new User(username, password, hwid);

            using (var sha = SHA256.Create())
            {
                var hash = new byte[32];

                if (!sha.TryComputeHash(user.Password, hash, out _))
                    return false;

                user.Password = hash;
            }

            user.AddKey(key);

            if (!RemoteProvider.Db.DeleteKey(key) ||
                !RemoteProvider.Db.CreateUser(user))
                return false;

            this.user = user;

            return true;
        }

        [NLCCall("Logout")]
        public bool Logout()
        {
            if (this.user == null)
                return false;

            this.user = null;

            return true;
        }

        [NLCCall("ProtectedFunction")]
        public string ProtectedFunction()
        {
            if (this.user == null)
                return "Unauthorized!";
            else
                return "You have access to the secret data " + this.user.Username;
        }

        public void Dispose()
        {
        }
    }
}