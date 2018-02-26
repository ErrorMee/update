using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PropItem : MonoBehaviour
{
	public PropInfo propInfo;

    public Toggle toggle;

    public Image iconImage;

    public Image selectImage;

    public Text numText;

	public Image lockImage;

    private int _icon;
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
                iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/prop/" + "prop_" + _icon);
                selectImage.overrideSprite = ResModel.Instance.GetSprite("icon/prop/" + "prop_" + _icon);
            }
        }
        get { return _icon; }
    }
	
    void Awake()
    {
		numText.text = "";
    }

	public void OnSelectChanged(GameObject go)
	{
		if(propInfo.isForbid)
		{
			PromptModel.Instance.Pop(LanguageUtil.GetTxt(11511));
			return;
		}
        PropModel.Instance.OnSelectChanged(!toggle.isOn, propInfo);
	}
}
