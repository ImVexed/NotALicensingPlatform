using NotLiteCode.Network;
using Security;
using System.Threading.Tasks;

namespace Client
{
    class RemoteProvider
    {
        public Task<bool> Login(string username, byte[] password) =>
            Task.FromResult(client.RemoteCall<bool>("Login", username, password, hwid.Result));

        public Task<bool> Register(string username, byte[] password, string keyid) =>
            Task.FromResult(client.RemoteCall<bool>("Register", username, password, hwid.Result, keyid));

        public Task<string> ProtectedFunction() =>
            Task.FromResult(client.RemoteCall<string>("ProtectedFunction"));

        private readonly NotLiteCode.Client client;

        private Task<byte[]> hwid;

        public RemoteProvider(string IP, int Port)
        {
            var socket = new NLCSocket(true, true);

            Task.Run(FingerPrint.Get).ContinueWith(x => hwid = x);

            client = new NotLiteCode.Client(socket, true);

            client.Connect(IP, Port);
        }
    }
}
