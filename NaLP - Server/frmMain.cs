using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NaLP___Server
{
    public partial class frmMain : Form
    {
        public static frmMain frmStatic;
        private clsSettings settings = default(clsSettings);
        private Server server = default(Server);
        public static object xmlLock = new object();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            frmStatic = this;
        }

        public static T ReadXML<T>(string path)
        {
            try
            {
                object ret;
                using (var file = new FileStream(path, FileMode.Open))
                {
                    ret = new XmlSerializer(typeof(T)).Deserialize(file);
                    file.Close();
                }
                return (T)ret;
            }
            catch { return default(T); }
        }

        public static bool WriteXML(object obj, string path)
        {
            try
            {
                using (var file = new FileStream(path, FileMode.Create))
                {
                    new XmlSerializer(obj.GetType()).Serialize(file, obj);
                    file.Close();
                }

                return true;
            }
            catch { return false; }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            rTxtLog.AppendText("Reading settings.cfg...", Color.Orange);

            if ((settings = ReadXML<clsSettings>("settings.xml")) == default(clsSettings))
            {
                rTxtLog.AppendText("Failed to parse settings.xml!", Color.Red);
                return;
            }

            rTxtLog.AppendText("settings.cfg parsed!", Color.Green);
            rTxtLog.AppendText("Creating server instance...", Color.Orange);

            Encryption.iCompressionLevel = settings.iCompressionLevel;
            server = new Server((int)numPort.Value, settings.iMaxQueueLength);

            rTxtLog.AppendText("Server created!", Color.Green);
            rTxtLog.AppendText("Starting server...", Color.Orange);

            server.bDebugLog = true;
            server.Start(rTxtLog.AppendText);

            rTxtLog.AppendText("Server started!", Color.Green);
        }

        private void btnStop_Click(object sender, EventArgs e) =>
            server.Stop();

        private void itemKick_Click(object sender, EventArgs e)
        {
            var cEP = (EndPoint)dgvConnections.SelectedCells[0].OwningRow.Cells[1].Value;

            server.Clients[cEP].sCls.Dispose();
            server.Clients[cEP].cSocket.Close();
            server.Clients.Remove(cEP);
        }

        private void itemBan_Click(object sender, EventArgs e)
        {
            var cEP = (EndPoint)dgvConnections.SelectedCells[0].OwningRow.Cells[1].Value;
            File.AppendAllText("BannedIPs.txt", (dgvConnections.SelectedCells[0].OwningRow.Cells[1].Value as string).Split(':')[0] + Environment.NewLine);

            if (server.Clients[cEP].sCls.isLoggedIn)
            {
                File.AppendAllText("BannedHWIDs.txt", Encoding.UTF8.GetString(server.Clients[cEP].sCls.thisClient.bHWID) + Environment.NewLine);
                File.Delete(string.Format("Clients\\{0}.xml", server.Clients[cEP].sCls.thisClient.sUsername));
            }

            server.Clients[cEP].sCls.Dispose();
            server.Clients[cEP].cSocket.Close();
            server.Clients.Remove(cEP);
        }
    }

    public static class clsResults
    {
        public const int SUCCESS = 0x00;
        public const int UNKNOWN_ERROR = 0x01;
        public const int ERR_USER_EXISTS = 0x02;
        public const int INVALID_KEY = 0x03;
        public const int HWID_MISMATCH = 0x04;
        public const int INVALID_USERNAME = 0x05;
        public const int INVALID_PASSWORD = 0x06;
        public const int SUCCESS_EXPIRED = 0x07;
        public const int BANNED = 0x08;
    }

    public class clsSettings
    {
        public int iCompressionLevel { get; set; } // 1 or 3
        public int iMaxQueueLength { get; set; }   // min should be around 5
    }

    public class clsKeys
    {
        public List<clsKeyEntry> lKeys { get; set; }
    }

    public class clsKeyEntry
    {
        public string sKey { get; set; }
        public int iValidFor { get; set; }
    }

    public class clsClient
    {
        public string sUsername { get; set; }
        public byte[] bPassword { get; set; }
        public byte[] bHWID { get; set; }
        public List<clsUsedKey> lHistory { get; set; }
    }

    public class clsUsedKey
    {
        public DateTime dtActivation { get; set; }
        public string sKey { get; set; }
        public int iValidFor { get; set; }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string message, Color clr)
        {
            box.PerformSafely(() => box.SelectionLength = 0);
            box.PerformSafely(() => box.SelectionColor = Color.DarkGray);
            box.PerformSafely(() => box.AppendText(string.Format("[{0}] ", DateTime.Now.ToShortTimeString())));

            box.PerformSafely(() => box.SelectionLength = 0);
            box.PerformSafely(() => box.SelectionColor = clr);
            box.PerformSafely(() => box.AppendText(message + Environment.NewLine));
            box.PerformSafely(() => box.SelectionColor = box.ForeColor);
        }
    }

    public static class CrossThreadExtensions
    {
        public static void PerformSafely(this Control target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public static T1 PerformSafely<T1>(this Control target, Func<T1> action)
        {
            if (target.InvokeRequired)
            {
                return (T1)target.Invoke(action);
            }
            else
            {
                return action();
            }
        }

        public static void PerformSafely<T1, T2>(this Control target, Action<T1, T2> action, T1 p1, T2 p2)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, p1, p2);
            }
            else
            {
                action(p1, p2);
            }
        }
    }
}