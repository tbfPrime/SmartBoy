using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class MB_Lookup_URL_Generator
    {
        string rootURL = "http://musicbrainz.org/ws/2/";


        public string RecordingLookupURL(string id)
        {
            string recordingURL;
            return recordingURL = rootURL + "recording/" + id + "?inc=releases+artists";
        }

        public string ArtistLookupURL(string id)
        {
            string artistURL;
            return artistURL = rootURL + "artist/" + id;
        }
    }
}
