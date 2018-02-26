using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TaskItem : MonoBehaviour {

	public Image iconImage;
	public Text progressText;
	public Image yesImage;
	
	private Vector2 _pos;
	public Vector2 pos
	{
		set
		{
			_pos = value;

			RectTransform itemRect = GetComponent<RectTransform> ();
			itemRect.anchoredPosition = _pos;
		}
		get{return _pos;}
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
                    iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/monster/" + "monster_" + _icon);
                }
                else
                {
                    iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + _icon);
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
			if(_count <= 0)
			{
				yesImage.gameObject.SetActive(true);
				progressText.text = "";
			}else
			{
				yesImage.gameObject.SetActive(false);
				progressText.text = "" + _count;
			}
		}
		get { return _count; }
	}

	void Awake () 
	{
		count = 0;
		FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
	}
}
