using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TaskTipItem : MonoBehaviour
{

    public Image iconImage;
    public Text progressText;

    private Vector2 _pos;
    public Vector2 pos
    {
        set
        {
            _pos = value;

            RectTransform itemRect = GetComponent<RectTransform>();
            itemRect.anchoredPosition = _pos;
        }
        get { return _pos; }
    }

    protected int _icon;
    public int icon
    {
        set
        {
            _icon = value;

            if (_icon == 0)
            {
                iconImage.overrideSprite = null;
            }
            else
            {
                if (_icon > 40000)
                {
                    iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/monster.ab", "monster_" + _icon);
                }
                else
                {
                    iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);
                }
            }
        }
        get { return _icon; }
    }

    private int _count;
    public int count
    {
        set
        {
            _count = value;
            if (_count <= 0)
            {
                progressText.text = "";
            }
            else
            {
                progressText.text = "" + _count;
            }
        }
        get { return _count; }
    }
}
