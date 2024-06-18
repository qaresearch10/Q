using Org.BouncyCastle.Crypto.Tls;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using static Q.Web.Q;


namespace Q.Common
{
    public class CertificateManager
    {
        public static bool IsCertificateInstalled(string certificateFile)
        {
            X509Certificate2 cert = CreateNewCertificate(certificateFile);
            using (X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                foreach (var storeCert in store.Certificates)
                {
                    if (storeCert.Thumbprint == cert.Thumbprint)
                    {
                        return true;
                    }
                }
            }
            return false;
        }    

        public static bool IsCertificateFromIssuerInstalled(string issuer)
        {
            X509Certificate2Collection certificates = GetAllInstalledCertificatesFromIssuer(issuer);
            return certificates?.Count > 0;
        }

        public static void InstallProxyCertificate(string certificateFile)
        {
            if (File.Exists(certificateFile))
            {
                X509Certificate2 cert = CreateNewCertificate(certificateFile);
                using (var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(cert);
                    store.Close();
                }
            }
            else
            {
                logger.Error("Cannot install a certificate. File does not exists.");
            }
        }

        public static void UninstallAllCertificatesFromIssuer(string issuer)
        {
            X509Certificate2Collection certificates = GetAllInstalledCertificatesFromIssuer(issuer);
            foreach (X509Certificate2 certificate in certificates)
            {
                RemoveProxyCertificate(certificate);
            }
        }

        public static void RemoveProxyCertificate(X509Certificate2 certificate)
        {
            using (X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadWrite);
                store.Remove(certificate);
                store.Close();
            }
        }

        private static X509Certificate2 CreateNewCertificate(string certificateFile)
        {
            return new X509Certificate2(X509Certificate2.CreateFromCertFile(certificateFile));
        }

        private static X509Certificate2Collection GetAllInstalledCertificatesFromIssuer(string issuer)
        {
            X509Certificate2Collection certificates = new X509Certificate2Collection();
            using (var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                foreach (var cert in store.Certificates)
                {
                    if (cert.Issuer.Contains(issuer))
                    {
                        certificates.Add(cert);
                    }
                }
            }
            return certificates;
        }

        public static void InstallOrUpdateCertificateIfRequired(string certificateFile)
        {
            X509Certificate2 newCert = CreateNewCertificate(certificateFile);
            X509Certificate2Collection installedCertificates = GetAllInstalledCertificatesFromIssuer(newCert.Issuer);

            if (installedCertificates.Count == 0)
            {
                InstallProxyCertificate(certificateFile);
            }
            else
            {
                foreach (var installedCert in installedCertificates)
                {
                    if (installedCert.Thumbprint != newCert.Thumbprint)
                    {
                        RemoveProxyCertificate(installedCert);
                        InstallProxyCertificate(certificateFile);
                    }
                }
            }
        }
    }
}