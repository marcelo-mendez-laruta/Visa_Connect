using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using VisaLib.Helpers;
using VisaLib.Models;
using COFDS = VisaLib.Models.COFDS;

namespace VisaLib;
public class VisaService : IVisaService
{
    public HttpClient _VisaClient;
    public IConfiguration _configuration;
    public VisaSettings visaSettings;
    public IHelpers _visaHelpers;
    public VisaService(IConfiguration configuration, IHelpers visahelpers)
    {
        _visaHelpers = visahelpers;
        #region AppSettings Visa Variables
        _configuration = configuration;
        var visaSettingsRaw = _configuration.GetSection("VisaSettings").Get<VisaSettingsRaw>();
        MLEModel mLEModel = new MLEModel(visaSettingsRaw.MLE.key, visaSettingsRaw.MLE.client, visaSettingsRaw.MLE.server);
        visaSettings = new VisaSettings(visaSettingsRaw.Url, visaSettingsRaw.CertPem, visaSettingsRaw.KeyPem, visaSettingsRaw.Cert, visaSettingsRaw.CertPassword, visaSettingsRaw.User, visaSettingsRaw.Password, mLEModel);
        #endregion AppSettings Visa Variables
        #region Visa Connection
        var certPem = visaSettings.CertPem;
        var keyPem = visaSettings.KeyPem;
        var cert = visaSettings.Cert;
        var certPassword = visaSettings.CertPasswordDecrypted;
        var user = visaSettings.User;
        var password = visaSettings.PasswordDecrypted;
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(new X509Certificate2(cert, certPassword));
        _VisaClient = new HttpClient(handler);
        _VisaClient.BaseAddress = new Uri(visaSettings.Url);
        var authString = Encoding.ASCII.GetBytes(user + ":" + password);
        _VisaClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authString));
        _VisaClient.DefaultRequestHeaders.Add("keyId", visaSettings.MLE.key);
        _VisaClient.DefaultRequestHeaders.Add("ex-correlation-id", _visaHelpers.RandomString(12) + "_SC");
        #endregion
    }

    public async Task<COFDS.Response> getVisaCOFR(COFDS.ApiRequest request)
    {
        COFDS.Response response;
        try
        {
            COFDS.RequestHeader requestHEAD = new COFDS.RequestHeader(_visaHelpers.RandomString(30), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            COFDS.Request finalrequest = new COFDS.Request(request.requestData, requestHEAD);
            var requestRAW = JsonConvert.SerializeObject(finalrequest);
            string Encryptedpayload = _visaHelpers.getEncryptedPayload(requestRAW, visaSettings.MLE.server, visaSettings.MLE.key);
            HttpContent payload = new StringContent(Encryptedpayload, Encoding.UTF8, "application/json");
            var responseRAW = await _VisaClient.PostAsync("cofds-web/v1/datainfo", payload);
            string DecryptedPayload = _visaHelpers.GetDecryptedPayload(await responseRAW.Content.ReadAsStringAsync(), visaSettings.MLE.client);
            response = JsonConvert.DeserializeObject<COFDS.Response>(DecryptedPayload)!;

            return response!;
        }
        catch (Exception ex)
        {
            COFDS.Status status = new COFDS.Status();
            status.statusCode = "500";
            status.statusDescription = "Error al conectarse";
            response = new COFDS.Response();
            response.status = status;
            return response;
        }
    }

    public async Task<VisaTestResponseModel> HelloWordVisaAsync()
    {
        try
        {
            VisaTestResponseModel response = await _VisaClient.GetFromJsonAsync<VisaTestResponseModel>("vdp/helloworld");
            return response!;
        }
        catch (Exception ex)
        {
            var response = new VisaTestResponseModel("", "", new VisaError("00", "Error de conexion"));
            return response;
        }

    }
}
