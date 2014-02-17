using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SmartBoy
{
    class Sensor
    {
        System.Windows.Forms.Integration.WindowsFormsHost host;
        PlayerConnector rm;
        
        public Sensor()
        {
            Console.WriteLine("Sensor | Constructor");
            // creates an instance WMP.
            InitPlayerInstance();
        }

        public System.Windows.Forms.Integration.WindowsFormsHost GetHost
        {
            get
            {
                return host;
            }
        }

        private void InitPlayerInstance()
        {

            // Create the interop host control.
            host = new System.Windows.Forms.Integration.WindowsFormsHost();

            // Create the ActiveX control.
            //AxWMPLib.AxWindowsMediaPlayer axWmp = new AxWMPLib.AxWindowsMediaPlayer();
            rm = new PlayerConnector();

            // Assign the ActiveX control as the host control's child.
            host.Child = rm;
        }

        // sense function.
        public void sensev2() {
            try
            {
                Console.WriteLine("Sensor | sensev2");
                if (((WMPLib.IWMPPlayer4)rm.GetOcx()).playState == WMPLib.WMPPlayState.wmppsPlaying) {
                    var x1 = ((WMPLib.IWMPPlayer4)rm.GetOcx()).currentMedia.sourceURL;

                    // Assign current song filepath to static variable  
                    CurrentSongData.filePath = @x1; 
                }
            }
            catch (Exception) { }
        }
    }
}
