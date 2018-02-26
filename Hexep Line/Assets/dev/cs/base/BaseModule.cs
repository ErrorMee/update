using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BaseModule : MonoBehaviour
{

    protected virtual void Awake()
    {
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    protected virtual void UpdateView()
    {
        
    }
}