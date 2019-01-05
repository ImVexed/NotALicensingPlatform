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
            [Option("sqlite", MetaValue = "sqlite", HelpText = "Use SQLite DB Provider at specified data path")]
            public string SQLite { get; set; }

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

                           provider = new Sqlite(o.SQLite);
                       }

                       //TODO: Furure provider support?

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
