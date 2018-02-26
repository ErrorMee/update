using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class BallBaseItem : MonoBehaviour
{

    private float _zrotate;
    public float zrotate
    {
        set
        {
            if (_zrotate != value)
            {
                _zrotate = value;
                RectTransform itemRect = GetComponent<RectTransform>();
                itemRect.localRotation = Quaternion.Euler(0, 0, _zrotate);
            }
        }
        get { return _zrotate; }
    }
}
