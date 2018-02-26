using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightMonsterItem : FightBaseItem
{
	public MonsterInfo monsterInfo;
	
	public Text text;

    public Image progress;
    public Image ban;

	override protected void Awake()
	{
		base.Awake();
		text = transform.Find("Text").GetComponent<Text>();
		text.text = "";
        progress = transform.Find("progress").GetComponent<Image>();
        progress.fillAmount = 0;
        ban = transform.Find("ban").GetComponent<Image>();
        ban.gameObject.SetActive(false);
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
				iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/monster.ab", "monster_" + _icon);

				if(monsterInfo.config.size == 2)
				{
					iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W * 3, PosMgr.CELL_H * 3);
				}else
				{
                    iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W * 0.9f, PosMgr.CELL_W * 0.9f);
				}
			}
		}
		get { return _icon; }
	}

    public void ShowBan(bool show)
    {
        ban.gameObject.SetActive(show);
    }

    public void ShowProgress(float value)
    {
        progress.fillAmount = value;
    }
}
