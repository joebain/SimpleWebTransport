using System.IO;
using System.Security.Authentication;
using UnityEngine;

namespace JamesFrowen.SimpleWeb
{

    public class SslConfigLoader
    {
        internal struct Cert
        {
            public string path;
            public string password;
        }
        public static SslConfig Load(bool sslEnabled, string sslCertJson, SslProtocols sslProtocols)
        {
            // dont need to load anything if ssl is not enabled
            if (!sslEnabled)
                return default;

            string certJsonPath = sslCertJson;

            Cert cert = LoadCertJson(certJsonPath);

            return new SslConfig(
                enabled: sslEnabled,
                sslProtocols: sslProtocols,
                certPath: cert.path,
                certPassword: cert.password
            );
        }

        internal static Cert LoadCertJson(string certJsonPath)
        {
#if UNITY_WEBGL
            string json = Resources.Load<TextAsset>(certJsonPath).text;
#else
            string json = File.ReadAllText(certJsonPath);
#endif
            Cert cert = JsonUtility.FromJson<Cert>(json);

            if (string.IsNullOrEmpty(cert.path))
            {
                throw new InvalidDataException("Cert Json didnt not contain \"path\"");
            }
            if (string.IsNullOrEmpty(cert.password))
            {
                // password can be empty
                cert.password = string.Empty;
            }

            return cert;
        }
    }
}
