using NotLiteCode.Network;
using Security;
using System.Threading.Tasks;

namespace Client
{
    class RemoteProvider
    {
        public async Task<bool> Login(string username, byte[] password) =>
            await client.RemoteCall<bool>("Login", username, password);

        public async Task<bool> Register(string username, byte[] password, string keyid) =>
            await client.RemoteCall<bool>("Register", username, password, keyid);

        public async Task<bool> Logout() =>
            await client.RemoteCall<bool>("Logout");

        public async Task<string> ProtectedFunction() =>
            await client.RemoteCall<string>("ProtectedFunction");

        private readonly NotLiteCode.Client client;

        public RemoteProvider(string IP, int Port)
        {
            var socket = new NLCSocket(true, true);

            client = new NotLiteCode.Client(socket, true);

            client.Connect(IP, Port);
        }
    }
}
