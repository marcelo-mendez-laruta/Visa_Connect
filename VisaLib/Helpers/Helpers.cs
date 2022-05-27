using Jose;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using VisaLib.Models;

namespace VisaLib.Helpers
{
    public class Helpers : IHelpers
    {
        private static Random random = new Random();
        public string getEncryptedPayload(string requestBody, string mleServerPublicCertificate, string keyId)
        {
            RSA clientCertificate = new X509Certificate2(mleServerPublicCertificate).GetRSAPublicKey();
            DateTime now = DateTime.UtcNow;
            long unixTimeMilliseconds = new DateTimeOffset(now).ToUnixTimeMilliseconds();
            IDictionary<string, object> extraHeaders = new Dictionary<string, object>{
                {"kid", keyId},{"iat",unixTimeMilliseconds}
            };
            string token = JWT.Encode(requestBody, clientCertificate, JweAlgorithm.RSA_OAEP_256, JweEncryption.A128GCM, null, extraHeaders);
            return "{\"encData\":\"" + token + "\"}";
        }

        public string GetDecryptedPayload(string encryptedPayload, string mleClientPrivateKey)
        {
            var jsonPayload = JsonConvert.DeserializeObject<EncryptedPayload>(encryptedPayload);
            return JWT.Decode(jsonPayload.encData, ImportPrivateKey(mleClientPrivateKey));
        }

        private static RSA ImportPrivateKey(string privateKeyFile)
        {
            var pemValue = System.Text.Encoding.Default.GetString(File.ReadAllBytes(privateKeyFile));
            var pr = new PemReader(new StringReader(pemValue));
            var keyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair.Private);

            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);

            return rsa;
        }
        public string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz1234567890";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
