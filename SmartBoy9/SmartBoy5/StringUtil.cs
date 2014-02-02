using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class StringUtil
    {
        public string getBetween(string strSource, string strStart, string strEnd){
            int Start, End;

            if (strSource.Contains(strStart) && strSource.Contains(strEnd)){
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            } else {
                return "";
            }
        }

        public string getEnd(string strSource, string strStart){
            int Start, End;

            if (strSource.Contains(strStart)){
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.Length;
                return strSource.Substring(Start, strSource.Length - Start);
            } else {
                return "";
            }
        }

        public string getBetweenNA(string strSource, string strStart, string strEnd){
            int Start, End;

            try{
                if (strSource.Contains(strStart) && strSource.Contains(strEnd)){
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return strSource.Substring(Start, End - Start);
                } else {
                    return "N/A";
                }
            } catch {
                return "N/A";
            }
        }
    }
}
