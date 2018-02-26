using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightFgItem : FightBaseItem
{
	public new int icon
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
				//iconImage.color = ColorMgr.GetColor(ColorMgr.COLOR_FG_CELL);
				iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W * 1f, PosMgr.CELL_W * 1f);
			}
		}
		get { return _icon; }
	}
}
