using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MapItem : MonoBehaviour {

	public Text nameText;
	public Button btn;

	public Image star1BGImage;
	public Image star2BGImage;
	public Image star3BGImage;

	public Image star1Image;
	public Image star2Image;
	public Image star3Image;

	public Image bgImage;
    public Image lockImage;
	//public Image iconImage;

	public Animation anim;

	public config_map_item config;

    void Awake()
    {
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    public void SetView(bool enable, int starNum, int oldStar = -1)
	{
		anim.Stop();
        btn.interactable = enable;
		//bgImage.gameObject.SetActive(enable);
		lockImage.gameObject.SetActive(!enable);

		star1BGImage.transform.parent.gameObject.SetActive(enable);
		star1BGImage.gameObject.SetActive(enable);
		star2BGImage.gameObject.SetActive(enable);
		star3BGImage.gameObject.SetActive(enable);

        //if (config != null && config.special != null && config.special != "")
        //{
            //iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/" + config.special.Split(new char[] { '_' })[0] + ".ab", config.special);
        //}

        star1Image.gameObject.SetActive(false);
        star2Image.gameObject.SetActive(false);
        star3Image.gameObject.SetActive(false);
        if (starNum >= 1)
        {
            star1Image.gameObject.SetActive(true);
            if (oldStar >= 0 && oldStar < 1)
            {
                Animation staranim = star1Image.GetComponent<Animation>();
                staranim.Play();
            }
        }

        if (starNum >= 2)
        {
            star2Image.gameObject.SetActive(true);
            if (oldStar >= 0 && oldStar < 2)
            {
                Animation staranim = star2Image.GetComponent<Animation>();
                staranim.Play();
            }
        }

        if (starNum >= 3)
        {
            star3Image.gameObject.SetActive(true);
            if (oldStar >= 0 && oldStar < 3)
            {
                Animation staranim = star3Image.GetComponent<Animation>();
                staranim.Play();
            }
        }

        if (config != null && config.name != null)
        {
            nameText.text = config.name;
        }
        else
        {
            nameText.text = "";
        }

        if (enable)
        {
			//iconImage.rectTransform.anchoredPosition = new Vector2(0, 2);
            if (star1Image.gameObject.activeSelf)
            {
				nameText.color = ColorMgr.GetColor(ColorMgr.MAP_PASS);
            }
            else
            {
                nameText.color = ColorMgr.GetColor(ColorMgr.MAP_OPEN);
				anim.Play();
            }
            //nameText.rectTransform.anchoredPosition = new Vector2(0, -40);
        }
        else
        {
			//iconImage.rectTransform.anchoredPosition = new Vector2(0, 20);
            nameText.color = ColorMgr.GetColor(ColorMgr.MAP_CLOSE,0.5f);
            //nameText.rectTransform.anchoredPosition = new Vector2(0, -30);
        }
	}
}
