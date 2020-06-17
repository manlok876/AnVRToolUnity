using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnVRTool
{
    public class AnVRSessionData 
    {
        public Dictionary<string, List<Tuple<float, Vector3>>> vectorTracks =
            new Dictionary<string, List<Tuple<float, Vector3>>>();

        public void AddVectorTrack(string trackName)
        {
            if (vectorTracks.ContainsKey(trackName))
            {
                return;
            }
            vectorTracks.Add(trackName, new List<Tuple<float, Vector3>>());
        }

        public List<Tuple<float, Vector3>> GetVectorTrackData(string trackName)
        {
            return vectorTracks[trackName];
        }

        public void RemoveVectorTrack(string trackName)
        {
            vectorTracks.Remove(trackName);
        }

        public Dictionary<string, System.Type> GetAllTracks()
        {
            var result = new Dictionary<string, System.Type>();

            foreach (var trackName in vectorTracks.Keys)
            {
                result.Add(trackName, typeof(Vector3));
            }

            return result;
        }
    }
}
