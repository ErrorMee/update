using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightBaseItem : MonoBehaviour
{
    public Image iconImage;

	public FightLayerType type;

    protected int _icon;
    public int icon
    {
        set
        {
            _icon = value;

            if (_icon <= 0)
            {
                Debug.LogWarning("_icon " + _icon);
                iconImage.overrideSprite = null;
            }
            else
            {
				iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);
            }
        }
        get { return _icon; }
    }

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
		get{return _zrotate;}
	}

    protected virtual void Awake()
    {
        iconImage = transform.Find("icon").GetComponent<Image>();
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }
}
