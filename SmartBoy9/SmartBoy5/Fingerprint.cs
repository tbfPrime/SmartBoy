using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SmartBoy
{
    public class Fingerprint
    {
        StringUtil tools = new StringUtil();
        string output;

        public string CreateFingerprint(string filename)
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "fpcalc.exe";
            p.StartInfo.Arguments = " \"" + filename + "\"";
            p.Start();

            // Read the output stream first and then wait.
            output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        public void CreateFingerprintv2()
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "fpcalc.exe";
            p.StartInfo.Arguments = " \"" + CurrentSongData.filePath + "\"";
            p.Start();

            // Read the output stream first and then wait.
            output = p.StandardOutput.ReadToEnd();

            CurrentSongData.duration = tools.getBetween(output, "DURATION=", "FINGERPRINT=");
            CurrentSongData.fingerprint = tools.getEnd(output, "FINGERPRINT=");

            p.WaitForExit();
        }
        
    }
}
