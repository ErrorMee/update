using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PropDescPart : MonoBehaviour {

	public Transform closeTrans;
	public Button closeBtn;

	public Text titel;

	public Text content;

	public Image showImage;

	public Button sureBtn;
    public Text sureText;

    public Transform costTrans;
	public Text costText;

	public Text timesText;

    public Animation winOpenAnim;

	void Awake()
	{
        sureText.text = LanguageUtil.GetTxt(11613);
	}

	void OnEnable()
	{
        EventTriggerListener.Get(closeBtn.gameObject).onClick = OnCloseClick;
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnCloseClick;
        EventTriggerListener.Get(sureBtn.gameObject).onClick = OnSureClick;
        PropInfo propInfo = PropModel.Instance.crtProp;
		
		if (propInfo != null) {
            titel.text = LanguageUtil.GetTxt(Convert.ToInt32(propInfo.config.name));
            content.text = LanguageUtil.GetTxt(Convert.ToInt32(propInfo.config.desc));
			showImage.overrideSprite = ResModel.Instance.GetSprite("icon/prop/" + "prop_" + propInfo.config.icon);
		}

        winOpenAnim.Play();

		PlayerModel.Instance.updateWealthsEvent += UpdateView;
	}
	
	void OnDisable()
	{
		PlayerModel.Instance.updateWealthsEvent -= UpdateView;
	}

	private void UpdateView()
	{
		if(PropModel.Instance.crtProp != null)
		{
			WealthInfo coin = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);

            costText.text = "" + PropModel.Instance.crtProp.config.GetCost(PropModel.Instance.crtProp.countUsed + 1);
            if (PropModel.Instance.crtProp.config.GetCost(PropModel.Instance.crtProp.countUsed + 1) > coin.count)
			{
				costText.color = Color.red;
			}else
			{
				costText.color = Color.green;
			}
			
			timesText.text = PropModel.Instance.crtProp.count + "/" + PropModel.Instance.crtProp.config.times; 
			
			if(PropModel.Instance.crtProp.count > 0)
			{
				timesText.color = Color.green;
                costTrans.gameObject.SetActive(true);
			}else
			{
				timesText.color = Color.red;
                costTrans.gameObject.SetActive(false);
			}
		}
	}

	public void Show(bool isShow)
	{
		gameObject.SetActive (isShow);
		if(isShow)
		{
            FontUtil.SetAllFont(transform, FontUtil.FONT_1);

			WealthInfo coin = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);

            costText.text = "" + PropModel.Instance.crtProp.config.GetCost(PropModel.Instance.crtProp.countUsed + 1);
            if (PropModel.Instance.crtProp.config.GetCost(PropModel.Instance.crtProp.countUsed + 1) > coin.count)
			{
				costText.color = Color.red;
			}else
			{
				costText.color = Color.green;
			}
			
			timesText.text = PropModel.Instance.crtProp.count + "/" + PropModel.Instance.crtProp.config.times; 
			
			if(PropModel.Instance.crtProp.count > 0)
			{
				timesText.color = Color.green;
                costTrans.gameObject.SetActive(true);
			}else
			{
				timesText.color = Color.red;
                costTrans.gameObject.SetActive(false);
			}
		}else
		{
			
		}
	}

    private void OnCloseClick(GameObject go)
	{
        PropModel.Instance.ChangePropStadus(PropStadus.unSelect);
	}

    private void OnSureClick(GameObject go)
	{
		if(PropModel.Instance.crtProp.count <= 0)
		{
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11405));
			return;
		}

		WealthInfo coin = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
		if(PropModel.Instance.crtProp.config.GetCost(PropModel.Instance.crtProp.countUsed + 1) > coin.count)
		{
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, (int)WealthTypeEnum.Coin);
            PlayerModel.Instance.ExchangeWealth((int)WealthTypeEnum.Coin,PropModel.Instance.crtProp.config.GetCost(PropModel.Instance.crtProp.countUsed + 1) - coin.count,DoingProp);
			return;
		}
		DoingProp ();
	}

	private void DoingProp()
	{
		PropModel.Instance.ChangePropStadus(PropStadus.doing);
	}

    private void OnSelectChanged(bool select)
    {
        if (select)
        {
            sureBtn.interactable = true;
        }
        else
        {
            sureBtn.interactable = false;
        }
    }

}

