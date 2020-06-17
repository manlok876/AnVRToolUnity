using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnVRTool
{
    public class AnVRPointerManager : MonoBehaviour
    {
        private HashSet<IMixedRealityPointer> managedPointers = 
            new HashSet<IMixedRealityPointer>();

        private AnVRDataCollector collector = null;

        void Start()
        {
            collector = FindObjectOfType<AnVRDataCollector>();
        }

        void Update()
        {
            UpdatePointerList();
        }

        void UpdatePointerList()
        {
            var allPointers = PointerUtils.GetPointers();
            foreach (var pointer in allPointers)
            {
                if (!managedPointers.Contains(pointer))
                {
                    AddPointer(pointer);
                }
            }
        }

        void AddPointer(IMixedRealityPointer pointer)
        {
            managedPointers.Add(pointer);
            if (collector != null)
            {
                var tracker = new AnVRPointerTracker();
                tracker.Initialize(pointer);
                collector.AddVectorTrack(pointer.PointerName, tracker);
            }
        }
    }
}
