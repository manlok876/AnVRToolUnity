using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class PositionWidget : MonoBehaviour
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

    void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        if (ptr == null && text == null)
        {
            return;
        }
        if (!ptr.IsActive)
        {
            return;
        }
        //text.SetText(ptr.Position.ToString());
        Vector3 v1 = ptr.Position;
        Vector3 v2 = ptr.BaseCursor.Position;
        //string v3 = ptr.FocusTarget.ToString();
        var res = ptr.Result;
        var det = res.Details;
        var point = det.Point;
        text.SetText(point.ToString());
    }
}
