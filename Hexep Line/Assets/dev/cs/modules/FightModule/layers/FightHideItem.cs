using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightHideItem : FightBaseItem
{
	public HiderInfo hiderInfo;

	override protected void Awake()
	{
		base.Awake();

	}

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
				iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/monster/" + "monster_" + _icon);

			}
		}
		get { return _icon; }
	}
}
