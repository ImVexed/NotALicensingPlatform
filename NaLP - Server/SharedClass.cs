using System;
using System.IO;

namespace NaLP___Server
{
    internal class SharedClass : IDisposable
    {
        private bool isLoggedIn = false;

        [NLCCall("GetBase")]
        public byte[] GetBase()
        {
            return File.ReadAllBytes("NaLP - Login.exe");
        }

        [NLCCall("Login")]
        public bool Login(string username, string password)
        {
            return isLoggedIn = (username == "Admin" && password == "secure");
        }

        [NLCCall("1337")]
        public string SecureFunction()
        {
            if (isLoggedIn)
                return "Harambe was an inside meme";
            else
                return "You're not allowed to call this function!"; // Do whatever you want here, disconnect client, dispose, send a bluescreen, idk im not ur mom.
        }

        public void Dispose()
        {
        }
    }
}