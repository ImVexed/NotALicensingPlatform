using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace NotALicensingPlatform
{
    internal class Program
    {
        private static Client client = new Client("localhost", 1337);

        private static void Main(string[] args)
        {
            Console.Title = "NaLP Client";
            client.Start();

            var vAsm = Assembly.Load(client.GetBase());
            var vEP = vAsm.EntryPoint;
            var vSC = vAsm.GetType(vEP.DeclaringType.FullName).GetMethod("SetClient");
            var vInst = vAsm.CreateInstance(vEP.Name);
            vSC.Invoke(vInst, new object[] { client }); // Pass our client to the Login GUI first
            vEP.Invoke(vInst, null);                    // Now properly invoke and draw GUI

            Process.GetCurrentProcess().WaitForExit();
        }
    }
}