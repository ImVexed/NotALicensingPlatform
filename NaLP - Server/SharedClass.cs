using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace NaLP___Server
{
    public class SharedClass : IDisposable
    {
       
        public bool isLoggedIn = false;
        public int iRowIndex = 0;
        public bool isExpired = true;
        public clsClient thisClient; // If you need to modify passwords, username or whatever
        public clsUsedKey thisKey;   // If you need to get subscription time or whatever

        [NLCCall("GetBase")]
        public byte[] GetBase()
        {
            return File.ReadAllBytes("NaLP - Login.exe");
        }

        [NLCCall("GetBase")]
        public byte[] GetKey()
        {

        }
        [NLCCall("Login")]
        public int Login(string username, string password, byte[] hwid)
        {
            try
            {
                if (Array.Exists(File.ReadAllLines("BannedHWIDs.txt"), x => x == Encoding.UTF8.GetString(hwid)))
                    return clsResults.BANNED;

                if (isLoggedIn)
                    if (isExpired)
                        return clsResults.SUCCESS_EXPIRED;
                    else
                        return clsResults.SUCCESS;

                if (!File.Exists(string.Format("Clients\\{0}.xml", username)))
                    return clsResults.INVALID_USERNAME;

                clsClient client = frmMain.ReadXML<clsClient>(string.Format("Clients\\{0}.xml", username));

                if (!SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).SequenceEqual(client.bPassword))
                    return clsResults.INVALID_PASSWORD;

                if (!hwid.SequenceEqual(client.bHWID))
                    return clsResults.HWID_MISMATCH;

                isLoggedIn = true;
                thisClient = client;

                foreach (clsUsedKey uKey in client.lHistory)
                    if (DateTime.Now.CompareTo(uKey.dtActivation.Add(new TimeSpan(uKey.iValidFor, 0, 0, 0))) <= 0)
                    {
                        isExpired = false;
                        thisKey = uKey;
                        UpdateInfo();
                        return clsResults.SUCCESS;
                    }

                isExpired = true;
                UpdateInfo();
                return clsResults.SUCCESS_EXPIRED;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                return clsResults.UNKNOWN_ERROR;
            }
        }

        [NLCCall("Register")]
        public int Register(string username, byte[] password, byte[] publicKey, byte[] hwid, string key) // Require a key on activation to prevent spam
        {
            try
            {
                if (Array.Exists(File.ReadAllLines("BannedHWIDs.txt"), x => x == Encoding.UTF8.GetString(hwid)))
                    return clsResults.BANNED;

                clsKeys keys = frmMain.ReadXML<clsKeys>("keys.xml");
                clsKeyEntry thisKey = keys.lKeys.Find(x => x.sKey == key);

                if (thisKey == default(clsKeyEntry))
                    return clsResults.INVALID_KEY;

                if (File.Exists(string.Format("Clients\\{0}.xml", username)))
                    return clsResults.ERR_USER_EXISTS;

                clsClient client = new clsClient();
                clsUsedKey thisUsedKey = new clsUsedKey() { sKey = thisKey.sKey, iValidFor = thisKey.iValidFor, dtActivation = DateTime.Now };
                client.sUsername = username;
                client.bHWID = hwid;
                client.lHistory = new List<clsUsedKey>();
                client.lHistory.Add(thisUsedKey);
                client.bPassword = SHA256.Create().ComputeHash(password);

                isLoggedIn = true;
                isExpired = false;
                thisClient = client;

                keys.lKeys.Remove(thisKey);
                this.thisKey = thisUsedKey;

                lock (frmMain.xmlLock)
                    frmMain.WriteXML(keys, "keys.xml");

                UpdateInfo();

                return frmMain.WriteXML(client, string.Format("Clients\\{0}.xml", username)) ? clsResults.SUCCESS : clsResults.UNKNOWN_ERROR;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                return clsResults.UNKNOWN_ERROR;
            }
        }

        [NLCCall("ActivateKey")]
        public int ActivateKey(string key)
        {
            if (isLoggedIn == false || thisClient == default(clsClient))
                return clsResults.UNKNOWN_ERROR;

            clsKeys keys = frmMain.ReadXML<clsKeys>("keys.xml");
            clsKeyEntry thisKey = keys.lKeys.Find(x => x.sKey == key);

            if (thisKey == default(clsKeyEntry))
                return clsResults.INVALID_KEY;

            thisClient.lHistory.Add(new clsUsedKey() { sKey = thisKey.sKey, iValidFor = thisKey.iValidFor, dtActivation = DateTime.Now });
            keys.lKeys.Remove(thisKey);

            lock (frmMain.xmlLock)
                frmMain.WriteXML(keys, "keys.xml");

            return clsResults.SUCCESS;
        }

        [NLCCall("1337")]
        public string SecureFunction()
        {
            if (isLoggedIn)
                if (isExpired)
                    return "Hey, your account is expired!";
                else
                    return "Harambe was an inside meme";
            else
                return "You're not allowed to call this function!"; // Do whatever you want here, disconnect client, dispose, send a bluescreen, idk im not ur mom.
        }

        public void UpdateInfo()
        {
            //frmMain.frmStatic.PerformSafely(() => frmMain.frmStatic.dgvConnections.Rows.SharedRow(iRowIndex).Cells[0].Value = thisClient.sUsername);
            //if (thisKey == default(clsUsedKey))
            //    frmMain.frmStatic.PerformSafely(() => frmMain.frmStatic.dgvConnections.Rows.SharedRow(iRowIndex).Cells[2].Value = 0);
            //else
            //    frmMain.frmStatic.PerformSafely(() => frmMain.frmStatic.dgvConnections.Rows.SharedRow(iRowIndex).Cells[2].Value = ((int)thisKey.dtActivation.Add(new TimeSpan(thisKey.iValidFor, 0, 0, 0)).Subtract(DateTime.Now).TotalDays).ToString());
        }

        public SharedClass(EndPoint cEP)
        {
            iRowIndex = frmMain.frmStatic.PerformSafely(() => frmMain.frmStatic.dgvConnections.Rows.Add(string.Empty, cEP, string.Empty));
        }

        public void Dispose()
        {
            frmMain.frmStatic.PerformSafely(() => frmMain.frmStatic.dgvConnections.Rows.RemoveAt(iRowIndex));
        }
    }
}