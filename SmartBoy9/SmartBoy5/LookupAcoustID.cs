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
        string lookupURL, JsonContent, checkStatus;
        string baseURL = "http://api.acoustid.org/v2/lookup?client=8XaBELgH&meta=recordingids";
        
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
    }
}
