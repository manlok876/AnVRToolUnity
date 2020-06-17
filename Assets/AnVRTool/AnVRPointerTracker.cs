using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

namespace AnVRTool
{
    public class AnVRPointerTracker : IAnVRDataProvider<Vector3>
    {
        public IMixedRealityPointer linkedPointer
        {
            get;
            private set;
        }

        public bool isActive => (linkedPointer != null && linkedPointer.Result != null);

        public Vector3 GetData()
        {
            if (isActive)
            {
                return linkedPointer.Result.Details.Point;
                //return linkedPointer.BaseCursor.Position;
            }
            return Vector3.zero;
        }

        public virtual void Initialize(IMixedRealityPointer pointer)
        {
            linkedPointer = pointer;
            
        }
    }
}
