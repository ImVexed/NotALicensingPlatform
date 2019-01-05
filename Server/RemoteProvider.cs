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
        public static IDBProvider DB;
        private readonly Server<SharedClass> server;

        public RemoteProvider(IDBProvider db)
        {
            var socket = new NLCSocket(true, Helpers.GenerateSelfSignedCert("nalp", "localhost"));

            DB = db;

            server = new Server<SharedClass>(socket);
            server.OnServerClientConnected += (x, y) => Helpers.Log($"NLC: Client {y.Client} connected!", ConsoleColor.Green);
            server.OnServerClientDisconnected += (x, y) => Helpers.Log($"NLC: Client {y.Client} disconnected!", ConsoleColor.Yellow);
            server.OnServerExceptionOccurred += (x, y) => Helpers.Log($"NLC: Exception Occured! {y.Exception}", ConsoleColor.Red);
        }

        public void GenerateKey(TimeSpan Duration)
        {
            var key = new Key(Duration);

            if(DB.CreateKey(key))
            {
                Helpers.Log($"Key {key.Identifier} for {Helpers.ToHumanReadableString(key.ValidFor)} has been successfully created", ConsoleColor.Green);
            }
            else
            {
                Helpers.Log($"Key {key.Identifier} could not be created", ConsoleColor.Red);
            }
        }

        public void Start(int Port)
        {
            server.Start(Port);
        }
    }
}
