using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Server.Misc
{
    internal class Helpers
    {
        public static X509Certificate2 GenerateSelfSignedCert(string CertificateName, string HostName)
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddDnsName(HostName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={CertificateName}");

            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

                request.CertificateExtensions.Add(sanBuilder.Build());

                var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

                return new X509Certificate2(certificate.Export(X509ContentType.Pfx));
            }
        }

        private static readonly object LogLock = new object();

        public static void Log(string message, ConsoleColor color)
        {
            lock (LogLock)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[{0}] ", DateTime.Now.ToLongTimeString());
                Console.ForegroundColor = color;
                Console.Write("{0}{1}", message, Environment.NewLine);
                Console.ResetColor();
            }
        }

        public static string ToHumanReadableString(TimeSpan t)
        {
            if (t.TotalSeconds <= 1)
            {
                return $@"{t:s\.ff} seconds";
            }
            if (t.TotalMinutes <= 1)
            {
                return $@"{t:%s} seconds";
            }
            if (t.TotalHours <= 1)
            {
                return $@"{t:%m} minutes";
            }
            if (t.TotalDays <= 1)
            {
                return $@"{t:%h} hours";
            }

            return $@"{t:%d} days";
        }
    }
}