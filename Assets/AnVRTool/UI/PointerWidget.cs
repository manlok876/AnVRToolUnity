using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class PointerWidget : MonoBehaviour
{
    public IMixedRealityPointer linkedPointer
    {
        get { return ptr; }
        set
        {
            ptr = value;
            UpdateRefs();
        }
    }
    private IMixedRealityPointer ptr = null;

    [SerializeField]
    private NameWidget nameWidget = null;
    [SerializeField]
    private TypeWidget typeWidget = null;
    [SerializeField]
    private PositionWidget positionWidget = null;

    void Start()
    {
        UpdateRefs();
    }

    void UpdateRefs()
    {
        if (nameWidget != null)
        {
            nameWidget.linkedPointer = ptr;
        }
        if (typeWidget != null)
        {
            typeWidget.linkedPointer = ptr;
        }
        if (positionWidget != null)
        {
            positionWidget.linkedPointer = ptr;
        }
    }
}
