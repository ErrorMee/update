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
				iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + _icon);
				//iconImage.color = ColorUtil.GetColor(ColorUtil.COLOR_FG_CELL);
				iconImage.rectTransform.sizeDelta = new Vector2(PosUtil.CELL_VIEW_W * 1f, PosUtil.CELL_W * 1f);
			}
		}
		get { return _icon; }
	}
}
