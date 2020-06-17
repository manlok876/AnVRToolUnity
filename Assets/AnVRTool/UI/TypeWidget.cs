using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class TypeWidget : MonoBehaviour
{
    public IMixedRealityPointer linkedPointer
    {
        get { return ptr; }
        set
        {
            ptr = value;
            UpdateText();
        }
    }
    private IMixedRealityPointer ptr = null;

    [SerializeField]
    private TextMeshProUGUI text = null;

    void Start()
    {
        UpdateText();
    }

    void UpdateText()
    {
        if (ptr != null && text != null)
        {
            text.SetText(ptr.GetType().Name);
        }
    }
}
