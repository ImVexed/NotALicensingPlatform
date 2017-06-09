using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
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
        private RNGCryptoServiceProvider cRandom = new RNGCryptoServiceProvider(DateTime.Now.ToString());
        public List<sClient> Clients = new List<sClient>();
        private List<KeyValuePair<string, MethodInfo>> RemotingMethods = new List<KeyValuePair<string, MethodInfo>>();
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

                if (oAttr.Any())
                {
                    NLCCall thisAttr = oAttr[0] as NLCCall;

                    if (RemotingMethods.Where(x => x.Key == thisAttr.Identifier).Any())
                        throw new Exception("There are more than one function inside the SharedClass with the same Identifier!");

                    Log(String.Format("Identifier {0} MethodInfo link created...", thisAttr.Identifier), Color.Green);
                    RemotingMethods.Add(new KeyValuePair<string, MethodInfo>(thisAttr.Identifier, mI));
                }
            }

            sSocket.BeginAccept(AcceptCallback, null);
        }

        /// <summary>
        /// Stop listening for connections and close server.
        /// </summary>
        public void Stop()
        {
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
            int iSIndex = Clients.Count;
            sClient sC = new sClient();
            sC.cSocket = client;
            sC.sCls = new SharedClass(client.RemoteEndPoint.ToString());
            sC.eCls = null;
            sC.bSize = new byte[4];
            BeginEncrypting(ref sC);
            Clients.Add(sC);
            sC.cSocket.BeginReceive(sC.bSize, 0, sC.bSize.Length, SocketFlags.None, RetrieveCallback, iSIndex);
            sSocket.BeginAccept(AcceptCallback, null);
        }

        private void RetrieveCallback(IAsyncResult iAR)
        // Handshake + Encryption is handled outside of this callback, so any message that makes it here is expected to be a method call/move.
        {
            int iSC = (int)iAR.AsyncState;

            try
            {
                SocketError sE;

                if (Clients[iSC].cSocket.EndReceive(iAR, out sE) == 0 || sE != SocketError.Success)
                {
                    Log(String.Format("Client IP: {0} has disconnected...", Clients[iSC].cSocket.RemoteEndPoint.ToString()), Color.Orange, true);

                    Clients[iSC].cSocket.Close();
                    Clients[iSC].sCls.Dispose();
                    Clients.RemoveAt(iSC);
                    //GC.Collect(0); //For immediate disposal
                    return;
                }
                byte[] cBuffer = new byte[BitConverter.ToInt32(Clients[iSC].bSize, 0)];

                Clients[iSC].bSize = new byte[4];
                Clients[iSC].cSocket.Receive(cBuffer);

                Log(String.Format("Receiving {0} bytes...", cBuffer.Length), Color.Cyan);

                if (cBuffer.Length <= 0)
                    throw new Exception("Received null buffer from client!");

                cBuffer = Clients[iSC].eCls.AES_Decrypt(cBuffer);

                object[] oMsg = BinaryFormatterSerializer.Deserialize(cBuffer);

                if (!oMsg[0].Equals(Headers.HEADER_CALL) && !oMsg[0].Equals(Headers.HEADER_MOVE)) // Check to make sure it's a method call/move
                    throw new Exception("Ahhh it's not a call or move, everyone run!");

                object[] oRet = new object[2];

                oRet[0] = Headers.HEADER_RETURN;

                MethodInfo mI = RemotingMethods.Find(x => x.Key == oMsg[1] as string).Value;

                if (mI == null)
                    throw new Exception("Client called method that does not exist in Shared Class! (Did you remember the [NLCCall] Attribute?)");

                oRet[1] = mI.Invoke(Clients[iSC].sCls, oMsg.Skip(2).Take(oMsg.Length - 2).ToArray());

                Log(String.Format("Client IP: {0} called Remote Identifier: {1}", Clients[iSC].cSocket.RemoteEndPoint.ToString(), oMsg[1] as string), Color.Cyan);

                if (oRet[1] == null && oMsg[0].Equals(Headers.HEADER_MOVE))
                    Console.WriteLine("Method {0} returned null! Possible mismatch?", oMsg[1] as string);

                BlockingSend(Clients[iSC], oRet);
                Clients[iSC].cSocket.BeginReceive(Clients[iSC].bSize, 0, Clients[iSC].bSize.Length, SocketFlags.None, RetrieveCallback, iSC);
            }
            catch
            {
                try
                {
                    Log(String.Format("Client IP: {0} has caused an exception...", Clients[iSC].cSocket.RemoteEndPoint.ToString()), Color.Orange, true);
                }
                catch { };
                try
                {
                    Clients[iSC].cSocket.Close();
                }
                catch { };
                try
                {
                    Clients[iSC].sCls.Dispose();
                }
                catch { };
                try
                {
                    Clients.RemoveAt(iSC);
                }
                catch { };
            }
        }

        private void BeginEncrypting(ref sClient sC)
        {
            byte[] sSymKey;
            CngKey sCngKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP521);
            byte[] sPublic = sCngKey.Export(CngKeyBlobFormat.EccPublicBlob);

            BlockingSend(sC, Headers.HEADER_HANDSHAKE, sPublic);

            object[] oRecv = BlockingReceive(sC);

            if (!oRecv[0].Equals(Headers.HEADER_HANDSHAKE))
                sC.cSocket.Disconnect(true);

            byte[] cBuf = oRecv[1] as byte[];

            using (var sAlgo = new ECDiffieHellmanCng(sCngKey))
            using (CngKey cPubKey = CngKey.Import(cBuf, CngKeyBlobFormat.EccPublicBlob))
                sSymKey = sAlgo.DeriveKeyMaterial(cPubKey);

            sC.eCls = new Encryption(sSymKey, HASH_STRENGTH.MEDIUM);
        }

        private void BlockingSend(sClient sC, params object[] param)
        {
            byte[] bSend = BinaryFormatterSerializer.Serialize(param);
            if (sC.eCls != null)
                bSend = sC.eCls.AES_Encrypt(bSend);
            else
                bSend = Compress(bSend);

            Log(String.Format("Sending {0} bytes...", bSend.Length), Color.Cyan);

            sC.cSocket.Send(BitConverter.GetBytes(bSend.Length));
            sC.cSocket.Send(bSend);
        }

        private object[] BlockingReceive(sClient sC)
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

            if (sC.eCls != null)
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
}