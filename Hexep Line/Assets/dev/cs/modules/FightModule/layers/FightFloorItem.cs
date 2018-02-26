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
				iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + _icon);
				iconImage.rectTransform.sizeDelta = new Vector2(PosUtil.CELL_VIEW_W * 0.9f, PosUtil.CELL_W * 0.9f);
            }
        }
        get { return _icon; }
    }
}
