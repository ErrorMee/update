using System;

using UnityEngine;
using UnityEngine.UI;

public class TipImage : Image
{
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return false;
    }
}