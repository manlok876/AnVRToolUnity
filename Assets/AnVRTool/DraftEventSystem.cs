using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using UnityEngine.EventSystems;

public class DraftEventSystem : MonoBehaviour, IMixedRealityEventSystem
{
    public List<GameObject> EventListeners => throw new System.NotImplementedException();

    public string Name { get; }

    public uint Priority { get; }

    public BaseMixedRealityProfile ConfigurationProfile { get; }

    public void Destroy() {}

    public void Disable() {}

    public void Dispose() {}

    public void Enable() {}

    public void HandleEvent<T>(BaseEventData eventData, ExecuteEvents.EventFunction<T> eventHandler) where T : IEventSystemHandler
    {
        
    }

    public void Initialize()
    {
        
    }

    public void LateUpdate() {}

    public void Register(GameObject listener)
    {
        throw new System.NotImplementedException();
    }

    public void RegisterHandler<T>(IEventSystemHandler handler) where T : IEventSystemHandler
    {
        
    }

    public void Reset() {}

    public void Unregister(GameObject listener)
    {
        throw new System.NotImplementedException();
    }

    public void UnregisterHandler<T>(IEventSystemHandler handler) where T : IEventSystemHandler
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IMixedRealityService.Update() {}
}
