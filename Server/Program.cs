using Server.Database;
using System;
using System.Diagnostics;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            var rp = new RemoteProvider(new Sqlite("data/"));
            rp.Start();

            Console.WriteLine("Server Started!");

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
