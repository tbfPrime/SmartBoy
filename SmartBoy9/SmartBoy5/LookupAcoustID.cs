using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class LookupAcoustID
    {
        string recordingsID = "";
        string duration, fingerprint, lookupURL, JsonContent, checkStatus;
        string baseURL = "http://api.acoustid.org/v2/lookup?client=8XaBELgH&meta=recordingids";

        public string GetFingerprint
        {
            get
            {
                return fingerprint;
            }
        }

        public string GetDuration
        {
            get
            {
                return duration;
            }
        }




        public string LookUp(string fp)
        {
            string temp = fp;
            duration = getBetween(fp, "DURATION=", "FINGERPRINT=");
            fingerprint = getEnd(fp, "FINGERPRINT=");
            GetRec_ID();
            return recordingsID;
        }


        public string LookUp(string fp, string durn)
        {
            duration = durn;
            fingerprint = fp;
            GetRec_ID();
            return recordingsID;
        }

        private void GetRec_ID()
        {
            lookupURL = baseURL + "&duration=" + duration + "&fingerprint=" + fingerprint;
            JsonContent = acoustID_Content(lookupURL);
            checkStatus = getBetween(JsonContent, "status\": \"", "\"");
            if (checkStatus == "ok")
            {
                JsonContent = getBetween(JsonContent, "recordings", "]");
                recordingsID = getBetween(JsonContent, "id\": \"", "\"");
            }
        }

        private string acoustID_Content(string url)
        {
            GetWebClient fetcher = new GetWebClient();
            return fetcher.GetWebString(url);
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static string getEnd(string strSource, string strStart)
        {
            int Start, End;
            if (strSource.Contains(strStart))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.Length;
                return strSource.Substring(Start, strSource.Length - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
