using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisaLib.Models.COFDS
{
    public record ApiRequest(RequestData requestData);
    public record Request(RequestData requestData, RequestHeader requestHeader);
    public record RequestHeader(string requestMessageId, string messageDateTime);
    public record RequestData(long[] pANs, string group);
}
