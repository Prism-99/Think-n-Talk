using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;
using StardewModdingAPI.Events;
using StardewModdingAPI;

namespace SDV_Speaker.Speaker
{

    class BubbleGuyManager
    {
        private readonly BubbleRecorder Recorder;
        private readonly string sSpirteDirectory;
        public bool IsBubbleVisible = false;
        private string sRecordingLastLocation = "";
        private int iRecordingLastX;
        private int iRecordingLastY;
        public IModHelper oHelper;
        public IMonitor oMonitor;
        public List<SpeakerItem> CurrentRecording => Recorder.CurrentRecording;
        public Dictionary<string, List<SpeakerItem>> Recordings => Recorder.Recordings;

        public BubbleGuyManager(string sSavePath, string sSpriteDir, IModHelper helper,IMonitor monitor)
        {
            oHelper = helper;
            oMonitor = monitor;
            Recorder = new BubbleRecorder(sSavePath, oHelper);
            sSpirteDirectory = sSpriteDir;
        }
        public void AddBubbleGuy(bool isThink, string sText)
        {
            RemoveBubbleGuy(false);
            Game1.currentLocation.characters.Add(new BubbleGuy(isThink, sText, sSpirteDirectory));
            IsBubbleVisible = true;
        }
        public void RemoveBubbleGuy(bool bAllLocations)
        {
            if (bAllLocations) { }
            else
            {
                if (Game1.currentLocation.getCharacterFromName("BubbleGuy") is BubbleGuy oGuy)
                {
                    Game1.currentLocation.characters.Remove(oGuy);
                }
            }
            IsBubbleVisible = false;
        }
        public BubbleRecorder.RecorderStatus RecorderStatus => Recorder.Status;
        public bool LoadSave(string sSaveName)
        {
            Recorder.CurrentRecording = Recorder.Recordings[sSaveName];

            return true;
        }
        public void LoadRecordings()
        {
            Recorder.LoadRecordings();
        }
        public bool SaveRecording(string sSaveName)
        {
            Recorder.SaveRecording(sSaveName);
            return true;
        }
        public bool SaveRecordings()
        {
            Recorder.SaveRecordings();

            return true;
        }
        public void Play()
        {
            foreach (SpeakerItem oitem in Recorder.CurrentRecording)
            {
                oitem.MarkHit = false;
            }
            oHelper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            Recorder.Play();
        }
        public void Stop()
        {
            if (Recorder.Status == BubbleRecorder.RecorderStatus.Playing)
            {
                oHelper.Events.GameLoop.UpdateTicked -= GameLoop_UpdateTicked;
            }

        }
        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (string.IsNullOrEmpty(sRecordingLastLocation) || sRecordingLastLocation != Game1.currentLocation.name.Value
                || Game1.player.getTileX() != iRecordingLastX || Game1.player.getTileY() != iRecordingLastY)
            {
                sRecordingLastLocation = Game1.currentLocation.name.Value;
                iRecordingLastX = Game1.player.getTileX();
                iRecordingLastY = Game1.player.getTileY();

                foreach (SpeakerItem oItem in Recorder.CurrentRecording)
                {
                    if (!oItem.MarkHit && oItem.Location == sRecordingLastLocation && oItem.TileX == iRecordingLastX && oItem.TileY == iRecordingLastY)
                    {
                        RemoveBubbleGuy(false);
                        if (!oItem.IsClear)
                        {
                            Game1.currentLocation.characters.Add(new BubbleGuy(oItem.IsThink, oItem.Text, sSpirteDirectory));
                        }
                        oItem.MarkHit = true;
                    }
                }
            }
        }

    }
}
