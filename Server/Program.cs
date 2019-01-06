using CommandLine;
using HumanDateParser;
using Server.Database;
using Server.Misc;
using System;
using System.Diagnostics;

namespace Server
{
    class Program
    {
        class Options
        {
            [Option("sqlite", MetaValue = "sqlite", HelpText = "Use SQLite provider at specified data path")]
            public string SQLite { get; set; }

            [Option("emptysqlite", MetaValue = "emptysqlite", HelpText = "Path to empty pre-formatted sqlite3 db to use in case one cannot be found in the data path")]
            public string EmptySQLite { get; set; }

            [Option("mongo", MetaValue = "mongo", HelpText = "Use Mongo provider with specified connection string")]
            public string Mongo { get; set; }

            [Option("postgresql", MetaValue = "postgresql", HelpText = "Use PostgreSQL provider with specified connection string")]
            public string PostgreSQL { get; set; }

            [Option("genkey", MetaValue = "genkey", HelpText = "Generate a new key for specified duration ex. '5 months' '1 day'")]
            public string GenKey { get; set; }

            [Option('p', "port", MetaValue = "port", HelpText = "Port to listen on", Default = (short)1337)]
            public short Port { get; set; }
        }


        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       IDBProvider provider = default(IDBProvider);

                       if(!string.IsNullOrEmpty(o.SQLite))
                       {
                           Helpers.Log($"Using SQLite database provider at {o.SQLite}", ConsoleColor.Green);

                           provider = new Sqlite(o.SQLite, o.EmptySQLite);
                       }

                       if(!string.IsNullOrEmpty(o.Mongo))
                       {
                           Helpers.Log($"Using Mongo with connection string: {o.Mongo}", ConsoleColor.Green);

                           provider = new Mongo(o.Mongo);
                       }

                       if(!string.IsNullOrEmpty(o.PostgreSQL))
                       {
                           Helpers.Log($"Using PostgreSQL with connection string: {o.PostgreSQL}", ConsoleColor.Green);

                           provider = new Postgresql(o.PostgreSQL);
                       }

                       if(provider == default(IDBProvider))
                       {
                           Helpers.Log("No DB provider specified! See --help", ConsoleColor.Red);

                           return;
                       }

                       var rp = new RemoteProvider(provider);

                       if(!string.IsNullOrEmpty(o.GenKey))
                       {
                           var dateEnd = DateParser.Parse(o.GenKey);

                           if(dateEnd == default(DateTime) || DateTime.Now > dateEnd)
                           {
                               Helpers.Log($"{o.GenKey} is not a valid time duration", ConsoleColor.Red);

                               return;
                           }

                           var duration = dateEnd - DateTime.Now;

                           rp.GenerateKey(duration);
                       }
                       else
                       {
                           rp.Start(o.Port);

                           Helpers.Log($"Server listening on port {o.Port}", ConsoleColor.Green);

                           Process.GetCurrentProcess().WaitForExit();
                       }
                   });
        }
    }
}
