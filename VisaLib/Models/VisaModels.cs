namespace VisaLib.Models;
#region Visa
#region Test
public record VisaTestRequestModel(string Pan, int Score, int cvv2);
public record VisaTestResponseModel(string timestamp, string message, VisaError? error = null);
public record VisaError(string code, string message);
#endregion Test
public class VisaSettingsRaw
{
    public string Url { get; set; }
    public string CertPem { get; set; }
    public string KeyPem { get; set; }
    public string Cert { get; set; }
    public string CertPassword { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public MLERawModel MLE { get; set; }
}
public class MLERawModel
{
    public string key { get; set; }
    public string client { get; set; }
    public string server { get; set; }
}
public record VisaSettings(string Url, string CertPem, string KeyPem, string Cert, string CertPassword, string User, string Password, MLEModel MLE)
{
    public string CertPasswordDecrypted = ZUtil.ZCrypt.DecUserKey(CertPassword);
    public string PasswordDecrypted = ZUtil.ZCrypt.DecUserKey(Password);
}
#region Message level Encrypt
public record MLEModel(string key, string client, string server);
public class EncryptedPayload
{
    public string encData { get; set; }
}

#endregion Message level Encrypt
#endregion Visa
