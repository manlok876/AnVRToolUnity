using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

public class DraftInputSystemListener : MonoBehaviour
{
    private MixedRealityToolkit MRTKInstance = null;
    private IMixedRealityInputSystem inputSystem = null;

    private class DraftHandler : IMixedRealityTouchHandler
    {
        public void OnTouchCompleted(HandTrackingInputEventData eventData)
        {
            Debug.Log("Touch ended at: " + eventData.InputData.ToString());
        }

        public void OnTouchStarted(HandTrackingInputEventData eventData)
        {
            Debug.Log("Touch started at: " + eventData.InputData.ToString());
        }

        public void OnTouchUpdated(HandTrackingInputEventData eventData)
        {
            
        }
    }

    void Start()
    {
        MRTKInstance = MixedRealityToolkit.Instance;
        inputSystem = CoreServices.InputSystem;
        
        if (inputSystem != null)
        {
            inputSystem.RegisterHandler<IMixedRealityTouchHandler>(new DraftHandler());
        }
    }

    void Update()
    {
        
    }
}
