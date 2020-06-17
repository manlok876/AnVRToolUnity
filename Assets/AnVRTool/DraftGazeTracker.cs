using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraftGazeTracker : MonoBehaviour
{
    private Transform raycastCamera = null;
    void Start()
    {
        
    }

    void Update()
    {
        if (raycastCamera != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastCamera.position, raycastCamera.forward, out hit))
            {
                Debug.Log("Hit surface at " + hit.point.ToString());
            }
        }
        else
        {
            raycastCamera = GameObject.Find("UIRaycastCamera").transform;
        }
    }
}
