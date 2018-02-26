using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class ToggleItem : MonoBehaviour
{
    private Toggle _toggle;
    private Image bgImage;
    private Text text;

    public Toggle toggle
    {
        get
        {
            if (_toggle == null)
            {
                _toggle = GetComponent<Toggle>();
            }

            return _toggle;
        }
    }

	public config_item_base itemInfo;

    private int _id;
    public int id
    {
        set
        {
            _id = value;

            text.text = "" + _id;
        }
        get { return _id; }
    }

    public FightLayerType type;

    private int _icon;
    public int icon
    {
        set
        {
            _icon = value;

            if (_icon <= 0)
            {
                bgImage.overrideSprite = null;
            }
            else
            {
                bgImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W / 2.7f, PosMgr.CELL_H / 2.7f);
                switch (type)
                {
                    case FightLayerType.map:
                        bgImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/cell.ab", "cell_" + "radiate");
                        break;
                    case FightLayerType.fence:
                        bgImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/wall.ab", "wall_" + _icon);
                        bgImage.rectTransform.sizeDelta = new Vector2(60, 15);
                        break;
                    case FightLayerType.cover:
                        bgImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/cover.ab", "cover_" + _icon);
                        break;
					case FightLayerType.monster:
						bgImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/monster.ab", "monster_" + _icon);
						break;
                    default:
                        bgImage.overrideSprite = GridMain.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);
                        break;
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
        get { return _zrotate; }
    }

    void Awake()
    {
        _toggle = GetComponent<Toggle>();

        bgImage = transform.FindChild("Background").GetComponent<Image>();

        text = transform.FindChild("Label").GetComponent<Text>();
    }

    public void OnSelectChange(bool selected)
    {
        GridModel.Instance.ToggleChange(selected, type, id,itemInfo);
    }
}
