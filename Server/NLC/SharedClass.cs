using NotLiteCode.Server;
using Server.Database;
using Server.Misc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Server.NLC
{
    internal class SharedClass : IDisposable
    {
        User user;

        [NLCCall("Login")]
        public bool Login(string username, byte[] password)
        {
            // Ensure a user isn't already logged in
            if (this.user != null)
                return false;

            var user = RemoteProvider.DB.GetUser(username);

            if (user == null)
                return false;

            if (!user.CheckPassword(password))
            {
                Helpers.Log($"{username} tried to log in with an invalid password", ConsoleColor.Yellow);
                return false;
            }

            if(!user.IsActive())
            {
                Helpers.Log($"{username} tried to log in without an active subscription", ConsoleColor.Yellow);
                return false;
            }

            this.user = user;

            Helpers.Log($"{username} has successfully logged in", ConsoleColor.Green);

            return true;
        }

        [NLCCall("Register")]
        public bool Register(string username, byte[] password, string keyid)
        {
            // Ensure a user isn't already logged in
            if (this.user != null)
            {
                Helpers.Log($"{username} tried to register an account while still logged in", ConsoleColor.Yellow);

                return false;
            }

            // Check if a user already exists with this username
            if (RemoteProvider.DB.GetUser(username) != null)
            {
                Helpers.Log($"A register attempt was made for an account that already exists for {username}", ConsoleColor.Yellow);

                return false;
            }

            var key = RemoteProvider.DB.GetKey(keyid);

            if (key == null)
            {
                Helpers.Log($"{username} tried to register an account with an invalid key", ConsoleColor.Yellow);

                return false;
            }

            var user = new User(username, password);

            using (var sha = SHA256.Create())
            {
                var hash = new byte[32];

                if (!sha.TryComputeHash(user.Password, hash, out _))
                    return false;

                user.Password = hash;
            }

            user.AddKey(key);

            if (!RemoteProvider.DB.DeleteKey(key) ||
                !RemoteProvider.DB.CreateUser(user))
                return false;

            this.user = user;

            Helpers.Log($"{username} has successfully registered a new account", ConsoleColor.Green);

            return true;
        }

        [NLCCall("Logout")]
        public bool Logout()
        {
            if (this.user == null)
            {
                Helpers.Log("A user tried to logout without being logged in", ConsoleColor.Yellow);

                return false;
            }

            Helpers.Log($"{this.user.Username} has successfully logged out", ConsoleColor.Green);

            this.user = null;

            return true;
        }

        [NLCCall("ProtectedFunction")]
        public string ProtectedFunction()
        {
            if (this.user == null)
            {
                Helpers.Log($"An unauthorized user attempted to call the protected function", ConsoleColor.Yellow);

                return "Unauthorized!";
            }
            else
            {
                Helpers.Log($"{this.user.Username} has called the protected function", ConsoleColor.Green);

                return "You have access to the secret data " + this.user.Username;
            }

        }

        public void Dispose()
        {
            this.user = null;
        }
    }
}