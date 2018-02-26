using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SkillItem : MonoBehaviour {

	public Image circleTemp;

	public Image circleProgress;

    public Image lightImage;
    public Animation lightAnim;

	public Image iconImage;

	public Image lockImage;

	public Text textProgress;

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
				iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);
            }
        }
        get { return _icon; }
    }

	private float _progress;
	public float progress
	{
		set
		{
			_progress = value;
            circleProgress.fillAmount = _progress;
			textProgress.text = "";//Mathf.Round(progress * 100) + "%";
		}
		get { return _progress; }
	}

    private float _progressTemp;
    public float progressTemp
    {
        set
        {
            _progressTemp = value;
            circleTemp.fillAmount = _progressTemp;
            if (circleTemp.fillAmount >= 0.99f)
            {
                lightImage.gameObject.SetActive(true);
                lightAnim.enabled = true;
            }
            else
            {
                lightImage.gameObject.SetActive(false);
                lightAnim.enabled = false;
				iconImage.rectTransform.localScale = new Vector3(1,1,1);
            }
        }
        get { return _progressTemp; }
    }

	public void ShowLock(bool isForbid)
	{
		lockImage.enabled = isForbid;
		iconImage.enabled = !isForbid;
	}

	public void SetHideId(int hideId)
	{
		lockImage.color = ColorMgr.GetColor(ColorMgr.GetCellColorValue(hideId),0.8f);
	}

	void Awake () 
	{
		progress = 0;
        progressTemp = 0;
	}
}
