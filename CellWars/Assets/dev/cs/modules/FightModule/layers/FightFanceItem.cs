using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightFanceItem : FightBaseItem
{
	public WallInfo wall_info;

    public new int icon
    {
        set
        {
            _icon = value;

            if (_icon <= 0)
            {
                iconImage.overrideSprite = null;
            }
            else
            {
                iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/wall.ab", "wall_" + _icon);
                iconImage.rectTransform.sizeDelta = new Vector2(95, 26);
            }
        }
        get { return _icon; }
    }
}
