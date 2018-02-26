using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class EditItem : MonoBehaviour
{
    public Image iconImage;

	public FightLayerType type;

	public Vector3 gridPos;

    protected int _icon;
    public int icon
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
				if(type == FightLayerType.fence)
				{
					iconImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/wall.ab", "wall_" + _icon);
					iconImage.rectTransform.sizeDelta = new Vector2(40,10);
				}else
				{
                    iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W / 2.5f, PosMgr.CELL_H / 2.5f);
					iconImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);

                    if (type == FightLayerType.cover)
                    {
                        iconImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/cover.ab", "cover_" + _icon);
                    }

					if (type == FightLayerType.monster)
					{
						iconImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/monster.ab", "monster_" + _icon);
						config_monster_item monsterConfig = (config_monster_item)GridMain.resourceMgr.config_monster.GetItem(_icon);
						if(monsterConfig.size == 2)
						{
							iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W, PosMgr.CELL_H);
						}
					}

				}
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
                itemRect.Rotate(0, 0, _zrotate);
            }
		}
		get{return _zrotate;}
	}

	public void OnCellClick(GameObject go)
	{
        GridModel.Instance.OnItemClick((int)gridPos.x, (int)gridPos.y, (int)gridPos.z);
	}

    void Awake()
    {
        iconImage = transform.Find("icon").GetComponent<Image>();
    }
}
