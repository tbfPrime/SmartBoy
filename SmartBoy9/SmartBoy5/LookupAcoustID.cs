using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class LookupAcoustID
    {
        StringUtil tools = new StringUtil();
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
            duration = tools.getBetween(fp, "DURATION=", "FINGERPRINT=");
            fingerprint = tools.getEnd(fp, "FINGERPRINT=");
            GetRec_ID();
            return recordingsID;
        }


        public string LookUp(string fp, string durn)
        {
            GetRec_ID();
            return recordingsID;
        }

        private void GetRec_ID()
        {
            lookupURL = baseURL + "&duration=" + duration + "&fingerprint=" + fingerprint;
            JsonContent = acoustID_Content(lookupURL);
            checkStatus = tools.getBetween(JsonContent, "status\": \"", "\"");
            if (checkStatus == "ok")
            {
                JsonContent = tools.getBetween(JsonContent, "recordings", "]");
                recordingsID = tools.getBetween(JsonContent, "id\": \"", "\"");
            }
        }

        private string acoustID_Content(string url)
        {
            GetWebClient fetcher = new GetWebClient();
            return fetcher.GetWebString(url);
        }

        // New Code

        public void GetRec_IDv2()
        {
            Console.WriteLine("LookupAcoustID | GetRec_IDv2 | Initializing...");
            lookupURL = baseURL + "&duration=" + CurrentSongData.duration + "&fingerprint=" + CurrentSongData.fingerprint;
            JsonContent = new GetWebClient().GetWebString(lookupURL);
            checkStatus = tools.getBetween(JsonContent, "status\": \"", "\"");

            Console.WriteLine("LookupAcoustID | GetRec_IDv2 | checkStatus: " + checkStatus);
            if (checkStatus == "ok")
            {
                JsonContent = tools.getBetween(JsonContent, "recordings", "]");
                CurrentSongData.trackMBID = tools.getBetween(JsonContent, "id\": \"", "\"");
            }
            Console.WriteLine("LookupAcoustID | GetRec_IDv2 | Finalizing...");

            // Data Log.
            Console.WriteLine("LookupAcoustID | GetRec_IDv2 | CurrentSongData.trackMBID: " + CurrentSongData.trackMBID);
        }

        //
    }
}
