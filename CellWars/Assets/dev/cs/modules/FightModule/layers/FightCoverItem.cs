using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightCoverItem : FightBaseItem
{
	public CoverInfo coverInfo;

	public Text text;

    public Text tip;

	override protected void Awake()
	{
		base.Awake();
		text = transform.Find("Text").GetComponent<Text>();
		text.text = "";
        tip = transform.Find("tip").GetComponent<Text>();
        tip.text = "";
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
                iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cover.ab", "cover_" + _icon);
            }
        }
        get { return _icon; }
    }

	private int _rate;
	public int rate
	{
		set
		{
			_rate = value;

			if(_rate == 0)
			{
				text.text = "";
			}else
			{
				text.text = "" + _rate;
			}
		}
		get { return _rate; }
	}

    public void UpdateTip()
    {
        if (coverInfo.timer >= 0)
        {
            tip.text = "" + coverInfo.timer;
        }
        else
        {
            tip.text = "";
        }
    }
}
