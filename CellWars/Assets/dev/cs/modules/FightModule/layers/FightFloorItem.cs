using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightFloorItem : FightBaseItem
{
	public FloorInfo floorInfo;

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
				iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);
				iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W * 0.9f, PosMgr.CELL_W * 0.9f);
            }
        }
        get { return _icon; }
    }
}
