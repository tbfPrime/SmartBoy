﻿using System;
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
        Fingerprint fp = new Fingerprint();
        LookupAcoustID ac_id = new LookupAcoustID();
        PlannerUtilities_1 util = new PlannerUtilities_1();
        System.Threading.Timer timer;
        
        string path, currentHash, currentFingerprint, currentDuration;
        string Track_MBID;

        bool songChanged = false;
        bool id36 = false;


        // New Code

        private bool fnLock = false;
        private string previousPath;

        //



        public Planner(){
            Console.WriteLine("Planner | Constructor");
            sensor = new Sensor();
            paceMaker();
        }

        public void paceMaker()
        {
            Console.WriteLine("Planner | paceMaker");
            TimerCallback tcb = Core;
            timer = new System.Threading.Timer(tcb, 0, 1000, 1000);
        }

        public bool SongChanged
        {
            get
            {
                SongCheck();

                return songChanged;
            }

            set
            {
                songChanged = value;
            }
        }

        public string CurrentPath()
        {
            return path;
        }

        public string CurrentHash
        {
            get
            {
                return currentHash;
            }
        }

        public System.Windows.Forms.Integration.WindowsFormsHost Host
        {
            get
            {
                return sensor.GetHost;
            }
        }

        private void SongCheck()
        {
            if (path != sensor.CurrentMusicPath)
            {
                path = sensor.CurrentMusicPath;
                currentHash = util.CreateHash(path);
                SongChanged = true;
            }
            else
            SongChanged = false;
        }

        #region SmartBoy BrainZzz

        public void SongPlan()
        {
            if (util.HashExists(currentHash))
            {
                if (util.TrackMBID_6_plus(currentHash))
                { }
                else
                {
                    if (util.CheckForInternetConnection())
                    {
                        effector_phase1_Reload();   
                    }
                }
            }
            else
            {
                Track_MBID = ac_id.LookUp(fp.CreateFingerprint(path));
                currentFingerprint = ac_id.GetFingerprint;
                currentDuration = ac_id.GetDuration;

                if (util.CheckForInternetConnection() && Track_MBID.Length == 36)
                {
                    id36 = true;
                    effector_phase1();
                    
                }

                if (!id36)
                {
                    util.Offline_Storage(path, currentHash, currentFingerprint, currentDuration);
                }

                id36 = false;
            }
        }

        private void effector_phase1_Reload()
        {
            Track_MBID = util.FetchTrackMBID(currentHash);
            if (Track_MBID.Length == 36)
            {
                util.FlushLocalInfo(currentHash);
                recordingLookup.MB_Recording_LookUp(Track_MBID);
                util.ID_StorageReload(currentHash, Track_MBID);
                artistLookup.MB_Artist_Lookup(Track_MBID);
            }
        }

        private void effector_phase1()
        {

            recordingLookup.MB_Recording_LookUp(Track_MBID);
            util.ID_Storage36(currentHash, Track_MBID, currentFingerprint, currentDuration);
            artistLookup.MB_Artist_Lookup(Track_MBID);

        }

        public void WikiPlan()
        {
            if (!util.CheckForInternetConnection())
            { }
            else
                util.ActivateWiki(currentHash);
                
        }

        #endregion


        // New Code

        private void Core(object state) {
            Console.WriteLine("Planner | Core | Beat");

            sensor.sensev2(); // Activate Sensor
            
            if (!fnLock && SongCheckv2()) {
                Console.WriteLine("Planner | Core | Inside Condition.");
                try {
                    fnLock = true;
                    CurrentSongData.db = new TestContext();
                    SongPlanv2();
                    CurrentSongData.UpdateTags();
                }
                catch (Exception)
                {
                    Console.WriteLine("Outermost Exception Caught.");
                }
                finally
                {
                    CurrentSongData.db.Dispose();
                }
            }
        }

        private bool SongCheckv2() {
            Console.WriteLine("Planner | SongCheck");

            if (CurrentSongData.filePath != previousPath) {
                Console.WriteLine("Planner | SongCheck | PathChanged");

                previousPath = CurrentSongData.filePath;
                CurrentSongData.filePathHash = util.CreateHash(previousPath);
                return true;
            } else {
                return false;
            }
        }

        private void SongPlanv2() {
            Console.WriteLine("Planner | SongPlanv2");
            // Check if hash exists
            if (CurrentSongData.db.ID_SB.Any(u => u.Hash == CurrentSongData.filePathHash)){
                // Check for previous lookup
                if (!util.TrackMBID_6_plusv2() && util.CheckForInternetConnection())
                {
                    util.FetchTrackMBIDv2();
                    // check if mbID is received and in proper length
                    if (CurrentSongData.trackMBID.Length == 36)
                    {
                        util.FlushLocalInfov2(); // Clear Previous Offline Data
                        recordingLookup.LookUpv2();// Lookup Track info from MusicBrainz
                        artistLookup.LookUpv2(); // Lookup Artist info from MusicBrainz
                        util.ActivateWikiv2(); // Lookup Wikipedia
                    }
                }
            }
            // first time lookup
            else if (util.CheckForInternetConnection())
            {
                // Generate fingerprint.
                new Fingerprint().CreateFingerprintv2();
                ac_id.GetRec_IDv2();

                if (CurrentSongData.trackMBID.Length == 36)
                {
                    recordingLookup.LookUpv2(); // first time lookup Track info from MusicBrainz
                    artistLookup.LookUpv2(); // first time lookup Artist info from MusicBrainz
                    util.ActivateWikiv2(); // first time lookup Wikipedia
                }
            }
            // no internet available. Offline storage.
            else {
                util.Offline_Storagev2();
            }

            // Pull Album Art from Song.
            CurrentSongData.albumArt = new Taggot().GetPictures;
            if (CurrentSongData.albumArt != null)
            {
                CurrentSongData.dominantColor = new GenerateColorCode().CalculateDominantColor(CurrentSongData.albumArt);
                //CurrentSongData.contrastColor = new GenerateColorCode().ContrastColor(CurrentSongData.albumArt);
            }
            else
            { 
                // Code to choose default Album Art and set default colors.
            }
            fnLock = false;
        }

        //
















    }
}
