using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnVRTool
{
    public class AnVRDataCollector : MonoBehaviour
    {
        [SerializeField]
        private float recordingStartDelay = 0.01f;
        [SerializeField]
        private float recordingPeriod = 1f;
        public bool isRecording { get; private set; } = false;

        protected Dictionary<string, IAnVRDataProvider<Vector3>> vectorDataProviders = 
            new Dictionary<string, IAnVRDataProvider<Vector3>>();

        public AnVRSessionData sessionData { get; set; } = new AnVRSessionData();

        void Start()
        {
            StartRecording();
        }

        public void StartRecording()
        {
            isRecording = true;
            InvokeRepeating("PollProviders", recordingStartDelay, recordingPeriod);
        }

        public void StopRecording()
        {
            isRecording = false;
            CancelInvoke("PollProviders");
        }

        void PollProviders()
        {
            //Debug.Log("Poll");
            foreach (var provider in vectorDataProviders)
            {
                sessionData.vectorTracks[provider.Key].Add(
                    new Tuple<float, Vector3>(Time.time, provider.Value.GetData()) );
            }
        }

        public void AddOrUpdateVectorTrack(string trackName, IAnVRDataProvider<Vector3> provider)
        {
            if (vectorDataProviders.ContainsKey(trackName))
            {
                UpdateVectorTrack(trackName, provider);
            }
            else
            {
                AddVectorTrack(trackName, provider);
            }
        }

        public void AddVectorTrack(string trackName, IAnVRDataProvider<Vector3> provider)
        {
            if (vectorDataProviders.ContainsKey(trackName))
            {
                return;
            }
            vectorDataProviders.Add(trackName, provider);
            sessionData.AddVectorTrack(trackName);
        }

        public void UpdateVectorTrack(string trackName, IAnVRDataProvider<Vector3> provider)
        {
            if (!vectorDataProviders.ContainsKey(trackName))
            {
                return;
            }
            vectorDataProviders[trackName] = provider;
        }

        public void RemoveVectorTrack(string trackName)
        {
            vectorDataProviders.Remove(trackName);
            sessionData.RemoveVectorTrack(trackName);
        }

        public List<Tuple<float, Vector3>> GetVectorTrackData(string trackName)
        {
            return sessionData.GetVectorTrackData(trackName);
        }

        public Dictionary<string, System.Type> GetAllTracks()
        {
            var result = new Dictionary<string, System.Type>();

            foreach (var trackName in vectorDataProviders.Keys)
            {
                result.Add(trackName, typeof(Vector3));
            }

            return result;
        }
    }
}
