using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillHoleItem : MonoBehaviour
{
	public SkillGroupInfo skillGroupInfo;
    public Text nameText;
    public Image iconImage;
	public Image bombImage;
    public Image lockImage;
    public Animation waitAnim;

    private int _icon;
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
                iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);
            }
        }
        get { return _icon; }
    }

    private bool _islock;
    public bool islock
    {
        set
        {
            _islock = value;
            lockImage.gameObject.SetActive(_islock);
            bombImage.gameObject.SetActive(!_islock);
            if (_islock)
			{
                bombImage.color = ColorMgr.GetColor(ColorMgr.COLOR_BLACK,0.3f);
			}else
			{
                bombImage.color = ColorMgr.GetColor(ColorMgr.CELLS[skillGroupInfo.index],0.8f);
            }
        }
        get { return _islock; }
    }

    public void ShowName(string namestr)
    {
        nameText.text = namestr;
    }

}
