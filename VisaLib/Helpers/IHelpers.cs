using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VisaLib.Helpers
{
    public interface IHelpers
    {
        string getEncryptedPayload(string requestBody, string mleServerPublicCertificate, string keyId);
        string GetDecryptedPayload(string encryptedPayload, string mleClientPrivateKey);
        string RandomString(int length);
    }
}
