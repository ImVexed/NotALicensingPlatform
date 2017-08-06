using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using static NaLP___Server.Encryption;

namespace NaLP___Server
{
    public static class Headers
    {
        public const byte HEADER_CALL = 0x01;
        public const byte HEADER_RETURN = 0x02;
        public const byte HEADER_HANDSHAKE = 0x03;
        public const byte HEADER_MOVE = 0x04;
    }

    public class Server
    {
        private RNGCryptoServiceProvider cRandom = new RNGCryptoServiceProvider();
        public Dictionary<EndPoint, sClient> Clients = new Dictionary<EndPoint, sClient>();
        private SemaphoreSlim ssLimiter = new SemaphoreSlim(10);
        private CancellationTokenSource tkCancel = new CancellationTokenSource();
        private Dictionary<string, MethodInfo> RemotingMethods = new Dictionary<string, MethodInfo>();
        private Socket sSocket = null;
        private Action<string, Color> aLog = null;

        #region Variables

        /// <summary>
        /// Port for the server to listen on, changing this variable while the server is running will have no effect.
        /// </summary>
        public int iPort { get; set; }

        /// <summary>
        /// The number of maximum pending connections in queue.
        /// </summary>
        public int iBacklog { get; set; }

        /// <summary>
        /// If true, debugging information will be output to the console.
        /// </summary>
        public bool bDebugLog { get; set; } = false;

        #endregion Variables

        /// <summary>
        /// Initializes the NLC Server. Credits to Killpot :^)
        /// </summary>
        /// <param name="port">Port for server to listen on.</param>
        public Server(int port = 1337, int maxBacklog = 5)
        {
            this.iPort = port;
            this.iBacklog = maxBacklog;

            sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Start server and begin listening for connections.
        /// </summary>
        public void Start(Action<string, Color> aLog)
        {
            this.aLog = aLog;

            sSocket.Bind(new IPEndPoint(IPAddress.Any, iPort));
            sSocket.Listen(iBacklog);

            foreach (MethodInfo mI in typeof(SharedClass).GetMethods())
            {
                object[] oAttr = mI.GetCustomAttributes(typeof(NLCCall), false);

                if (oAttr.Length > 0)
                {
                    NLCCall thisAttr = oAttr[0] as NLCCall;

                    if (RemotingMethods.TryGetValue(thisAttr.Identifier, out Out<MethodInfo>.NULL) == true)
                        throw new Exception("There are more than one function inside the SharedClass with the same Identifier!");

                    Log(String.Format("Identifier {0} MethodInfo link created...", thisAttr.Identifier), Color.Green);
                    RemotingMethods.Add(thisAttr.Identifier, mI);
                }
            }

            if (RemotingMethods.Count() == 0)
                throw new Exception("No remote methods found inside the SharedClass");
            sSocket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Stop listening for connections and close server.
        /// </summary>
        public void Stop()
        {
            tkCancel.Cancel();
            if (sSocket == null)
                throw new Exception("Server is not running...");
            sSocket.Close();
        }

        private void AcceptCallback(IAsyncResult iAR)
        //Our method for accepting clients
        {
            Socket client = null;
            try
            {
                client = sSocket.EndAccept(iAR);
            }
            catch { }

            if (Array.Exists(File.ReadAllLines("BannedIPs.txt"), x => x == client.RemoteEndPoint.ToString().Split(':')[0]))
            {
                client.Close();
                sSocket.BeginAccept(AcceptCallback, null);
                return;
            }
            Log(String.Format("Client connected from IP: {0}", client.RemoteEndPoint.ToString()), Color.Green, true);
            sClient sC = new sClient();
            sC.cSocket = client;
            sC.sCls = new SharedClass(client.RemoteEndPoint);
            sC.eCls = null;
            sC.bSize = new byte[4];
            BeginEncrypting(ref sC);
            Clients.Add(client.RemoteEndPoint, sC);
            Clients[client.RemoteEndPoint].cSocket.BeginReceive(sC.bSize, 0, sC.bSize.Length, SocketFlags.None, RetrieveCallback, client.RemoteEndPoint);
            sSocket.BeginAccept(AcceptCallback, null);
        }

        private void RetrieveCallback(IAsyncResult iAR)
        // Handshake + Encryption is handled outside of this callback, so any message that makes it here is expected to be a method call/move.
        {
            EndPoint cEP;
            if (!ssLimiter.Wait(10 * 1000, tkCancel.Token))
            {
                if (tkCancel.Token.IsCancellationRequested)
                    return;
                else
                {
                    Log("Semaphore wait time has exceeded 10 seconds! Aborting!", Color.Red, true);
                    cEP = (EndPoint)iAR.AsyncState;
                    Clients[cEP].cSocket.BeginReceive(Clients[cEP].bSize, 0, Clients[cEP].bSize.Length, SocketFlags.None, RetrieveCallback, cEP);
                    return;
                }
            }
            cEP = (EndPoint)iAR.AsyncState;

            try
            {
                SocketError sE;
                if (Clients[cEP].cSocket.EndReceive(iAR, out sE) == 0 || sE != SocketError.Success)
                {
                    Log(String.Format("Client IP: {0} has disconnected...", Clients[cEP].cSocket.RemoteEndPoint.ToString()), Color.Orange, true);

                    Clients[cEP].cSocket.Close();
                    Clients[cEP].sCls.Dispose();
                    Clients.Remove(cEP);
                    //GC.Collect(0); //For immediate disposal
                    return;
                }
                byte[] cBuffer = new byte[BitConverter.ToInt32(Clients[cEP].bSize, 0)];

                Clients[cEP].bSize = new byte[4];
                Clients[cEP].cSocket.Receive(cBuffer);

                Log(String.Format("Receiving {0} bytes...", cBuffer.Length), Color.Cyan);

                if (cBuffer.Length <= 0)
                    throw new Exception("Received null buffer from client!");

                cBuffer = Clients[cEP].eCls.AES_Decrypt(cBuffer);

                object[] oMsg = BinaryFormatterSerializer.Deserialize(cBuffer);

                if (!oMsg[0].Equals(Headers.HEADER_CALL) && !oMsg[0].Equals(Headers.HEADER_MOVE)) // Check to make sure it's a method call/move
                    throw new Exception("Ahhh it's not a call or move, everyone run!");

                object[] oRet = new object[2];

                oRet[0] = Headers.HEADER_RETURN;

                MethodInfo mI;

                if (RemotingMethods.TryGetValue(oMsg[1] as string, out mI) == false)
                    throw new Exception("Client called method that does not exist in Shared Class! (Did you remember the [NLCCall] Attribute?)");

                oRet[1] = mI.Invoke(Clients[cEP].sCls, oMsg.Subset(2, oMsg.Length - 2));

                Log(String.Format("Client IP: {0} called Remote Identifier: {1}", Clients[cEP].cSocket.RemoteEndPoint.ToString(), oMsg[1] as string), Color.Cyan);

                if (oRet[1] == null && oMsg[0].Equals(Headers.HEADER_MOVE))
                    Console.WriteLine("Method {0} returned null! Possible mismatch?", oMsg[1] as string);

                BlockingSend(Clients[cEP], oRet);
                Clients[cEP].cSocket.BeginReceive(Clients[cEP].bSize, 0, Clients[cEP].bSize.Length, SocketFlags.None, RetrieveCallback, cEP);
            }
            catch
            {
                try
                {
                    Log(String.Format("Client IP: {0} has caused an exception...", Clients[cEP].cSocket.RemoteEndPoint.ToString()), Color.Orange, true);
                }
                catch { };
                try
                {
                    Clients[cEP].cSocket.Close();
                }
                catch { };
                try
                {
                    Clients[cEP].sCls.Dispose();
                }
                catch { };
                try
                {
                    Clients.Remove(cEP);
                }
                catch { };
            }
            ssLimiter.Release();
        }

        private void BeginEncrypting(ref sClient sC)
        {
            byte[] sSymKey;
            CngKey sCngKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP521);
            byte[] sPublic = sCngKey.Export(CngKeyBlobFormat.EccPublicBlob);

            BlockingSend(sC, Headers.HEADER_HANDSHAKE, sPublic);

            object[] oRecv = BlockingReceive(sC, true);

            if (!oRecv[0].Equals(Headers.HEADER_HANDSHAKE))
            {
                sC.cSocket.Disconnect(true);
                return;
            }

            byte[] cBuf = oRecv[1] as byte[];

            using (var sAlgo = new ECDiffieHellmanCng(sCngKey))
            using (CngKey cPubKey = CngKey.Import(cBuf, CngKeyBlobFormat.EccPublicBlob))
                sSymKey = sAlgo.DeriveKeyMaterial(cPubKey);

            sC.eCls = new Encryption(sSymKey, HASH_STRENGTH.MEDIUM);
        }

        private void BlockingSend(sClient sC, params object[] param)
        {
            byte[] bSend = BinaryFormatterSerializer.Serialize(param);
            if ((byte)param[0] != (byte)Headers.HEADER_HANDSHAKE) // Only allow compression if we're in handshake
                bSend = sC.eCls.AES_Encrypt(bSend);
            else
                bSend = Compress(bSend);

            Log(String.Format("Sending {0} bytes...", bSend.Length), Color.Cyan);

            sC.cSocket.Send(BitConverter.GetBytes(bSend.Length));
            sC.cSocket.Send(bSend);
        }

        private object[] BlockingReceive(sClient sC, bool onlydecompress = false)
        {
            byte[] bSize = new byte[4];
            sC.cSocket.Receive(bSize);

            byte[] sBuf = new byte[BitConverter.ToInt32(bSize, 0)];
            int iReceived = 0;

            while (iReceived < sBuf.Length)
            {
                iReceived += sC.cSocket.Receive(sBuf, iReceived, sBuf.Length - iReceived, SocketFlags.None);
            }

            Log(String.Format("Receiving {0} bytes...", sBuf.Length), Color.Cyan);

            if (!onlydecompress)
                sBuf = sC.eCls.AES_Decrypt(sBuf);
            else
                sBuf = Decompress(sBuf);

            return BinaryFormatterSerializer.Deserialize(sBuf);
        }

        private void Log(string message, Color color, bool force = false)
        {
            if (!bDebugLog && !force)
                return;

            aLog(message, color);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class NLCCall : Attribute
    {
        public readonly string Identifier;

        public NLCCall(string Identifier)
        {
            this.Identifier = Identifier;
        }
    }

    public class sClient
    {
        public Socket cSocket;
        public SharedClass sCls;
        public Encryption eCls;
        public byte[] bSize;
    }

    public static partial class LinqFaster
    {
        public static T[] Subset<T>(this T[] source, int start, int length)
        {
            if (length < 0)
                length = 0;

            var result = new T[length];
            Array.Copy(source, start, result, 0, length);
            return result;
        }
    }

    public static class Out<T>
    {
        [ThreadStatic]
        public static T NULL;
    }
}