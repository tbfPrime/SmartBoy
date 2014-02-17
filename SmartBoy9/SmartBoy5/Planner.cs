using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartBoy
{
    class Planner
    {
        Sensor sensor;
        MB_Recording recordingLookup = new MB_Recording();
        MB_Artist artistLookup = new MB_Artist();
        LookupAcoustID ac_id = new LookupAcoustID();
        PlannerUtilities_1 util = new PlannerUtilities_1();
        System.Threading.Timer timer;
        
        private bool fnLock = false;
        private bool lyricsLock = false;
        private string previousPath;
        int desc;
        
        public Planner(){
            Console.WriteLine("Planner | Constructor");

            // Initialize Sensor
            sensor = new Sensor();

            // Planner Pacemaker-Beat.
            paceMaker();
        }

        public void paceMaker()
        {
            Console.WriteLine("Planner | paceMaker");

            // Call to Core every 4000ms (4 secs.)
            TimerCallback tcb = Core;
            timer = new System.Threading.Timer(tcb, 0, 4000, 4000);
        }

        // Return host from Sensor.
        public System.Windows.Forms.Integration.WindowsFormsHost Host
        {
            get
            {
                return sensor.GetHost;
            }
        }

        // New Code

        private void Core(object state) {
            Console.WriteLine("Planner | Core | Pacemaker-Beat");

            sensor.sensev2(); // Activate Sensor
            
            if (!fnLock && SongCheckv2()) {
                Console.WriteLine("Planner | Core | Inside Condition.");
                try {
                    // Locks the function to prevent Hanging.
                    fnLock = true;
                    Console.WriteLine("Planner | Core | Core Locked!");

                    // DB instance created.
                    CurrentSongData.db = new TestContext();

                    Console.WriteLine("Planner | Core | db variable generated");

                    SongPlanv2();

                    if (PlannerUtilities_1.InternetGetConnectedState(out desc, 0) && !lyricsLock)
                    {
                        Console.WriteLine("\nStage------------------ 3 ------------------\n");
                        new LyricsFetch().LyricsPlan();
                        Console.WriteLine("CurrentSongData.LyricsLineCount: " + CurrentSongData.LyricsLineCount);

                        // Updating the gui again.
                        CurrentSongData.UpdateTags();

                        Console.WriteLine("\nStage------------------ 4 ------------------\n");
                        CurrentSongData.Commit();
                    }
                    else
                    {
                        Console.WriteLine("Planner | Core | Offline Commit to DB");
                        CurrentSongData.CommitOffline();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Outermost Exception Caught. | Exception: " + e);
                }
                finally
                {
                    Console.WriteLine("\nStage------------------ 5 ------------------\n");
                    Console.WriteLine("Planner | Core | Finally!");
                    CurrentSongData.db.Dispose();
                    Console.WriteLine("Planner | Core | db Disposed!");
                }
            }
        }

        private bool SongCheckv2() {
            Console.WriteLine("Planner | SongCheck");

            if (CurrentSongData.filePath != previousPath) {

                // Reset all Variables to null.
                CurrentSongData.resetVariables();

                Console.WriteLine("Planner | SongCheck | PathChanged");
                Console.WriteLine("Planner | SongCheck | Old filepath: " + previousPath);
                Console.WriteLine("Planner | SongCheck | New filepath: " + CurrentSongData.filePath);

                previousPath = CurrentSongData.filePath;
                CurrentSongData.filePathHash = util.CreateHash(previousPath);
                lyricsLock = false;
                return true;
            } else {
                return false;
            }
        }

        private void SongPlanv2() {
            Console.WriteLine("\nStage------------------ 2 ------------------\n");
            Console.WriteLine("Planner | SongPlanv2 | Initializing...");

            // Check if hash exists
            try
            {
                if (CurrentSongData.db.ID_SB.Any(u => u.Hash == CurrentSongData.filePathHash))
                {
                    Console.WriteLine("Planner | SongPlanv2 | Hash exists");

                    // Check for previous lookup
                    if (!util.TrackMBID_6_plusv2() && PlannerUtilities_1.InternetGetConnectedState(out desc, 0))
                    {

                        Console.WriteLine("Planner | SongPlanv2 | Offline to Online Data shift");
                        Console.WriteLine("Planner | SongPlanv2 | Song being searched online for the first time.");

                        util.FetchTrackMBIDv2();

                        // check if mbID is received and in proper length
                        if (CurrentSongData.trackMBID.Length == 36)
                        {
                            Console.WriteLine("Planner | SongPlanv2 | Flushing old and loading new Content.");
                            util.FlushLocalInfov2(); // Clear Previous Offline Data

                            // Call effector to search online.
                            Effector();
                            CurrentSongData.UpdateTags();
                        }
                    }
                    else 
                    {
                        Console.WriteLine("Planner | SongPlanv2 | Song has been searched online before.");
                        lyricsLock = true;
                        CurrentSongData.PullFromDB();
                    }
                }
                else
                {
                    // Generate fingerprint.
                    new Fingerprint().CreateFingerprintv2();
                    Console.WriteLine("Planner | SongPlanv2 | Fingerprint Generated.");

                    // first time lookup
                    if (PlannerUtilities_1.InternetGetConnectedState(out desc, 0)) // Check Internet Availability.
                    {
                        Console.WriteLine("Planner | SongPlanv2 | First Time Lookup.");

                        // Looking up AcoustID for Track MusicBrainz ID.
                        Console.WriteLine("Planner | SongPlanv2 | Looking up AcoustID for TrackMBID.");
                        ac_id.GetRec_IDv2();
                        Console.WriteLine("Planner | SongPlanv2 | AcoustID Lookup Completed.");

                        if (CurrentSongData.trackMBID.Length == 36)
                        {
                            // Call effector to search online.
                            Effector();
                        }
                    }
                    // no internet available. Offline storage.
                    else
                    {
                        Console.WriteLine("Planner | SongPlanv2 | No Internet, Offline Storage.");
                        util.Offline_Storagev2();
                    }
                }
                
                

                // Pull Album Art from Song.
                CurrentSongData.albumArt = new Taggot(CurrentSongData.filePath).GetPictures;

                // if album art is available it will enter this check.
                if (!CurrentSongData.defaultAlbumArt)
                {
                    Console.WriteLine("Planner | SongPlanv2 | Dominant Color");
                    //CurrentSongData.dominantColor = "#444444";
                    //CurrentSongData.contrastColor = "#000000";
                    CurrentSongData.dominantColor = new GenerateColorCode().AvgColorCode(new Taggot(CurrentSongData.filePath).GetPictureBitmap);
                    CurrentSongData.contrastColor = new GenerateColorCode().DarkerColor(CurrentSongData.dominantColor);

                    //CurrentSongData.dominantColor = new GenerateColorCode().CalculateDominantColor(CurrentSongData.albumArt);
                }
                else
                {
                    // Code to choose default Album Art and set default colors.
                }

                // Gui is updated here.
                CurrentSongData.UpdateTags();

                fnLock = false;
                Console.WriteLine("Planner | SongPlanv2 | Core UnLocked!");
            }
            catch (Exception e) 
            {
                Console.WriteLine("Planner | SongPlanv2 | catch | Exception: " + e); 
            }
        }


        // Fetch Module 
        private void Effector()
        {
            Console.WriteLine("Planner | Effector | Initializing...");

            Console.WriteLine("Planner | SongPlanv2 | Looking up MusicBrainz.");

            Console.WriteLine("Planner | Effector | Recording Lookup.");
            recordingLookup.LookUpv2(); // first time lookup Track info from MusicBrainz

            Console.WriteLine("Planner | Effector | Artist Lookup.");
            artistLookup.LookUpv2(); // first time lookup Artist info from MusicBrainz

            Console.WriteLine("Planner | Effector | Wikipedia Lookup.");
            util.ActivateWikiv2(); // first time lookup Wikipedia

            Console.WriteLine("Planner | Effector | Finalizing...");
        }
        //
    }
}
