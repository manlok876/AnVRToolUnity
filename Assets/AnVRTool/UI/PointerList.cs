using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class PointerList : MonoBehaviour
{
    [SerializeField]
    private PointerWidget widgetPrefab = null;
    private IEnumerable<IMixedRealityPointer> pointers;
    //private PointerWidget[] widgets = null;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            UpdateList();
        }
    }

    void UpdateList()
    {
        ClearList();
        pointers = PointerUtils.GetPointers();
        foreach (var pointer in pointers)
        {
            PointerWidget newWidget = Instantiate(widgetPrefab);
            newWidget.transform.SetParent(transform, false);
            newWidget.linkedPointer = pointer;
        }
    }

    void ClearList()
    {
        PointerWidget[] widgets = GetComponentsInChildren<PointerWidget>();
        foreach (var widget in widgets)
        {
            Destroy(widget.gameObject);
        }
    }
}
