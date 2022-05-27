using VisaLib.Models;

namespace VisaLib;
public interface IVisaService
{
    Task<VisaTestResponseModel> HelloWordVisaAsync();
    Task<Models.COFDS.Response> getVisaCOFR(Models.COFDS.ApiRequest request);
}