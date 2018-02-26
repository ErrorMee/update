using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillSmallItem : MonoBehaviour 
{

    public Action<SkillGroupInfo> selectTempletEvent;

    public Toggle toggle;
	public Text nameText;
	public Image iconImage;
    
    private SkillGroupInfo _skillTempletGroupInfo;
    public SkillGroupInfo skillTempletGroupInfo
    {
        set
        {
            _skillTempletGroupInfo = value;
            toggle.onValueChanged.AddListener(OnToggleChange);
        }
        get { return _skillTempletGroupInfo; }
    }

	private int _icon;
	public int icon
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
			}
		}
		get { return _icon; }
	}

	public void ShowName(string namestr)
	{
		nameText.text = namestr;
	}

    private void OnToggleChange(bool select)
    {
        if (select)
        {
            if (selectTempletEvent != null)
            {
                selectTempletEvent(skillTempletGroupInfo);
            }
        }
    }

}
