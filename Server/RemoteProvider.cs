using NotLiteCode.Network;
using NotLiteCode.Server;
using Server.Database;
using Server.Misc;
using Server.NLC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class RemoteProvider
    {
        public static IDBProvider Db;
        private readonly Server<SharedClass> server;

        public RemoteProvider(IDBProvider db)
        {
            var socket = new NLCSocket(true, Helpers.GenerateSelfSignedCert("nalp", "localhost"));

            Db = db;

            //var key = new Key(new TimeSpan(7, 0, 0, 0), "key1");
            //db.CreateKey(key);

            server = new Server<SharedClass>(socket);
            server.OnServerClientConnected += (x, y) => Log($"Client {y.Client} connected!", ConsoleColor.Green);
            server.OnServerClientDisconnected += (x, y) => Log($"Client {y.Client} disconnected!", ConsoleColor.Yellow);
            server.OnServerExceptionOccurred += (x, y) => Log($"Exception Occured! {y.Exception}", ConsoleColor.Red);
        }

        public void Start()
        {
            server.Start();
        }

        private static void Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[{0}] ", DateTime.Now.ToLongTimeString());
            Console.ForegroundColor = color;
            Console.Write("{0}{1}", message, Environment.NewLine);
            Console.ResetColor();
        }
    }
}
