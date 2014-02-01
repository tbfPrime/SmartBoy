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
        Fingerprint fp = new Fingerprint();
        LookupAcoustID ac_id = new LookupAcoustID();
        PlannerUtilities_1 util = new PlannerUtilities_1();
        System.Threading.Timer timer;
        
        string path, currentHash, currentFingerprint, currentDuration;
        string Track_MBID;

        bool songChanged = false;
        bool id36 = false;


        public Planner(){
            sensor = new Sensor();
            paceMaker();
        }

        public void paceMaker(){
            TimerCallback tcb = sensor.sense;
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

    }
}
