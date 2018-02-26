using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillSelectItem : MonoBehaviour {

    public Image iconImage;

    public Image lockImage;

    public Text lvText;

    public Transform switchTip;

    private SkillTempletInfo nextTempletInfo;

    private SkillTempletInfo _skillTempletInfo;
    public SkillTempletInfo skillTempletInfo
    {
        set
        {
            _skillTempletInfo = value;

            if (_skillTempletInfo.IsOpen())
            {
				if (_skillTempletInfo.fobid)
				{
					lvText.text = "";
					lockImage.gameObject.SetActive(true);
				}else{
					lvText.text = "";
					lockImage.gameObject.SetActive(false);
				}
            }
            else
            {
				lvText.text = "";
                lockImage.gameObject.SetActive(true);
            }
            FindSwicthNext();
            EventTriggerListener.Get(gameObject).onClick = OnItemClick;
        }
        get { return _skillTempletInfo; }
    }

    void Awake()
    {
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
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

    private void FindSwicthNext()
    {
        nextTempletInfo = null;

        config_cell_item cell = (config_cell_item)ResModel.Instance.config_cell.GetItem(skillTempletInfo.config.cellId);

        List<SkillTempletInfo> skillTemplets = SkillTempletModel.Instance.GetGroup(cell.hide_id).skillTemplets;

        int i;
        for (i = 0; i < skillTemplets.Count; i++)
        {
            SkillTempletInfo skillTemplet = skillTemplets[i];

            if (skillTemplet.config.cellId == skillTempletInfo.config.cellId)
            {
                break;
            }
        }

        if (i == (skillTemplets.Count - 1))
        {
            nextTempletInfo = skillTemplets[0];
        }
        else
        {
            nextTempletInfo = skillTemplets[i + 1];
        }

        if (nextTempletInfo.IsUnlock() == false)
        {
            nextTempletInfo = null;
            switchTip.gameObject.SetActive(false);
        }
        else
        {
            switchTip.gameObject.SetActive(true);
        }
    }

    private void OnItemClick(GameObject go)
    {
        if (nextTempletInfo != null)
        {
            SkillTempletModel.Instance.SelectTemplet(nextTempletInfo.config.cellId);
        }
    }
}
