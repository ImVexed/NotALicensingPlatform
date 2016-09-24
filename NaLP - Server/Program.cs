using System;
using System.Diagnostics;

namespace NaLP___Server
{
    internal class Program
    {
        private static Server server = new Server();

        private static void Main(string[] args)
        {
            Console.Title = "NaLP Server";
            server.bDebugLog = true;
            server.Start();

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}