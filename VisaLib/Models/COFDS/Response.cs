using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisaLib.Models.COFDS
{
    public class Merchant
    {
        public string acctNumOld4Digit { get; set; }
        public string mCC { get; set; }
        public string mrchName { get; set; }
        public string tokenReqstrId { get; set; }
        public string totalTranCount { get; set; }
        public string lastMrchTranDt { get; set; }
        public string vAUUpdateStatus { get; set; }
        public string vAULastUpdateDate { get; set; }
        public string tokenPANReplacementStatus { get; set; }
        public string paymentFacilitatorId { get; set; }
        public string sponsoredMerchantId { get; set; }
        public string tokenPANReplacementDate { get; set; }
        public string cardAcceptorId { get; set; }
        public string confidenceInd { get; set; }
        public string mrchDbaId { get; set; }
        public string mrchAddr { get; set; }
        public string mrchPhoneNum { get; set; }
        public string mrchURL { get; set; }
        public string lastTranAmt { get; set; }
        public string mrchDbaName { get; set; }
        public string lastTranAmtUSD { get; set; }
        public string lastTranCurrency { get; set; }
        public string lastTranDateTime { get; set; }
        public List<TranTypeDetail> tranTypeDetails { get; set; }
    }

    public class PanData
    {
        public string pAN { get; set; }
        public string panResponseMsg { get; set; }
        public List<Merchant> merchants { get; set; }
    }

    public class PanList
    {
        public PanData panData { get; set; }
    }

    public class ResponseData
    {
        public string group { get; set; }
        public List<PanList> panList { get; set; }
    }

    public class ResponseHeader
    {
        public int numRecordsReturned { get; set; }
        public string requestMessageId { get; set; }
        public string messageDateTime { get; set; }
        public string responseMessageId { get; set; }
    }

    public class Response
    {
        public Status status { get; set; }
        public ResponseData responseData { get; set; }
        public ResponseHeader responseHeader { get; set; }
    }

    public class Status
    {
        public string statusCode { get; set; }
        public string statusDescription { get; set; }
    }

    public class TranTypeDetail
    {
        public string tranType { get; set; }
        public int tranCount { get; set; }
        public string lastTranAmt { get; set; }
        public string lastTranDateTime { get; set; }
        public string lastTranCurrency { get; set; }
        public string lastTranAmtUSD { get; set; }
    }


}
