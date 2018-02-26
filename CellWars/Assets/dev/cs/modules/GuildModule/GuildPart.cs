using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GuildPart : MonoBehaviour
{
    private int BASE_OFFSET = 40;

    public Transform blackTrans;
    public Button close;
    public Button smallClose;

    public RectTransform chartTrans;
    public Image chatBg;
    public Text chartText;
    public RectTransform trigTrans;

    public Transform fingerTrans;

    public Animation fingerAnim;

    private config_guild_item configItem;

    private RectTransform targetTrans;
    private RectTransform targetParentTrans;
    private RectTransform holdTrans;
    private RectTransform preTargetTrans;

    private Vector3 targetWorldUIPos;

    private CycleActor cycleActor;

    public static bool ViewGuilding;

    void Start()
    {
        GuildModel.Instance.updateViewEvent = OnUpdateViewEvent;

        EventTriggerListener.Get(close.gameObject).onClick = OnClose;
        EventTriggerListener.Get(smallClose.gameObject).onClick = OnSmallClose;

        Hide();
        close.gameObject.SetActive(false);
		smallClose.gameObject.SetActive(false);
        ViewGuilding = false;
    }

    private void OnClose(GameObject go)
    {
        GuildModel.Instance.shield = true;
        GuildModel.Instance.ForceCompleteSave();
        Hide();
    }

    private void OnSmallClose(GameObject go)
    {
        GuildModel.Instance.ForceCompleteSave();
        Hide();
        GuildModel.Instance.CompleteContinue();
    }

    private void OnUpdateViewEvent(config_guild_item item)
    {
        configItem = item;
        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        ViewGuilding = false;
        blackTrans.gameObject.SetActive(false);
        chartTrans.gameObject.SetActive(false);
        fingerTrans.gameObject.SetActive(false);
        trigTrans.gameObject.SetActive(false);
        if (targetTrans != null)
        {
            EventTriggerListener.Get(targetTrans.gameObject).onUp -= OnTargetUp;
        }
        EventTriggerListener.Get(blackTrans.gameObject).onClick = null;

        ClearTargetTrans(-1);
        ClearTargetTrans(0);
        ClearTargetTrans(1);

        targetParentTrans = null;
        configItem = null;

        if (cycleActor != null)
        {
            cycleActor.OnDispose();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ViewGuilding = true;
        blackTrans.gameObject.SetActive(true);

        LeanTween.delayedCall(0.1f, ShowChat);
        LeanTween.delayedCall(0.4f, ShowFinger);
    }

    private void ShowChat()
    {
        if (configItem.guild_type == (int)GuildType.tip)
        {
            chartTrans.gameObject.SetActive(true);
            chartText.text = LanguageUtil.GetTxt(Convert.ToInt32(configItem.desc)).ToString();
            chatBg.rectTransform.sizeDelta = new Vector2(500, chartText.preferredHeight + 60);
            chartTrans.localPosition = new Vector3(0, 150, 1);
            EventTriggerListener.Get(blackTrans.gameObject).onClick = OnBlankClick;
            trigTrans.gameObject.SetActive(false);
            return;
        }

        string firstAims = configItem.GetAims();
        UpdateTargetTrans(firstAims);
        
        targetWorldUIPos = targetTrans.localPosition;
        if (configItem.desc != null && configItem.desc != "")
        {
            chartTrans.gameObject.SetActive(true);
            trigTrans.gameObject.SetActive(true);

            chartText.text = LanguageUtil.GetTxt(Convert.ToInt32(configItem.desc)).ToString();
            chatBg.rectTransform.sizeDelta = new Vector2(500, chartText.preferredHeight + 60);

            int offx = 0;
            int offy = 0;

            Vector3 chatPos = new Vector3();
            chatPos.x = targetWorldUIPos.x;
            if (chatPos.x > 260)
            {
                offx = (int)chatPos.x - 260;
                chatPos.x = 260;
            }
            if (chatPos.x < -260)
            {
                offx = (int)chatPos.x + 260;
                chatPos.x = -260;
            }
            chatPos.y = targetWorldUIPos.y;
			if (chatPos.y > 0 && configItem.guild_type != (int)GuildType.link)
            {
                chatPos.y = targetWorldUIPos.y - targetTrans.sizeDelta.y / 2 - chatBg.rectTransform.sizeDelta.y / 2 - BASE_OFFSET;
                offy = 1;
            }
            else
            {
                chatPos.y = targetWorldUIPos.y + targetTrans.sizeDelta.y / 2 + chatBg.rectTransform.sizeDelta.y / 2 + BASE_OFFSET;
                offy = -1;
            }

            trigTrans.localScale = new Vector3(1, offy, 1);
            trigTrans.localPosition = new Vector3(offx, offy * chatBg.rectTransform.sizeDelta.y / 2, 1);

            chartTrans.localPosition = new Vector3(chatPos.x, chatPos.y, chatPos.z);
        }
    }

    private void ShowFinger()
    {
        if (configItem == null || configItem.guild_type == (int)GuildType.tip || configItem.guild_type == (int)GuildType.introduction)
        {
            return;
        }

        fingerTrans.gameObject.SetActive(true);
        fingerTrans.localPosition = new Vector3(targetWorldUIPos.x, targetWorldUIPos.y, 0);
        switch (configItem.guild_type)
        {
            case (int)GuildType.click:
                fingerAnim.Play("UI_FINGER_CLICK");
                break;
            case (int)GuildType.link:

                fingerAnim.Stop();

                List<string> aimsArr = configItem.GetAimsArr();

                cycleActor = new CycleActor();

                for (int i = 1; i < aimsArr.Count; i++)
                {
                    string aims = aimsArr[i];

                    RectTransform itemTrans = (RectTransform)GameObject.Find(aims).transform;

                    MoveActor moveActor = new MoveActor((RectTransform)fingerTrans, new Vector3(itemTrans.localPosition.x, itemTrans.localPosition.y, 0), 0, 0.5f);
                    cycleActor.AddNode(moveActor);
                }

				MoveActor moveBackActor = new MoveActor((RectTransform)fingerTrans, new Vector3(targetWorldUIPos.x, targetWorldUIPos.y, 0),0,0.3f);
                cycleActor.AddNode(moveBackActor);
                cycleActor.OnExecute();

                break;
        }

    }

    private void OnTargetClick(GameObject go)
    {
		ComleteGuild();
    }

    private void OnBlankClick(GameObject go)
    {
		if(configItem.next_id == 0)
		{
			EventTriggerListener.Get(blackTrans.gameObject).onClick = null;
			LTDescr tween = LeanTween.move(chartTrans, new Vector3(0, 1000, 0), 0.4f);
			tween.setEase(LeanTweenType.easeInOutBack);
			tween.onComplete = ComleteGuild;

		}else
		{
			ComleteGuild();
		}
    }

	private void ComleteGuild()
	{
		GuildModel.Instance.CompleteSave();
		Hide();
		GuildModel.Instance.CompleteContinue();
	}

    private void OnTargetDown(GameObject go)
    {
        
        if (holdTrans != null)
        {//同时只处理一个
            return;
        }
        
        holdTrans = (RectTransform)go.transform;
        
        OpenNext();

        preTargetTrans = (RectTransform)go.transform;

        EventTriggerListener.Get(go).onUp += OnTargetUp;
        
    }

    private void OnTargetEnter(GameObject go)
    {
        if (CrossPlatformInputManager.GetButton("Fire1") && FightModule.crtFightStadus == FightStadus.line)
        {
            
            if (holdTrans == go.transform)
            {
                return;
            }
            
            holdTrans = (RectTransform)go.transform;
            OpenNext();
            preTargetTrans = (RectTransform)go.transform;
        }
    }

    private void OnTargetUp(GameObject go)
    {
        string nextAimsName = configItem.FindNextAims(holdTrans.name);
        int aimsIndex = configItem.AimsIndex(holdTrans.name);
        if (nextAimsName == "" || aimsIndex > 1)
        {
			ComleteGuild();
        }
        else
        {
            ClearTargetTrans(-1);
            ClearTargetTrans(0);
            ClearTargetTrans(1);

            string firstAims = configItem.GetAims();
            UpdateTargetTrans(firstAims);
        }

        EventTriggerListener.Get(go).onUp -= OnTargetUp;
        
    }

    private bool OpenNext()
    {
        string nextAimsName = configItem.FindNextAims(holdTrans.name);

        ClearTargetTrans(-1);

        if (nextAimsName != "")
        {
            UpdateTargetTrans(nextAimsName);

            return true;
        }

        return false;
    }

    private void ClearTargetTrans(int index = 0)//-1 pre 0 hold 1 target
    {
        RectTransform clearTrans;

        switch (index)
        {
            case -1:
                clearTrans = preTargetTrans;
                break;
            case 1:
                clearTrans = targetTrans;
                break;
            default:
                clearTrans = holdTrans;
                break;
        }

        if (clearTrans != null)
        {
            if (configItem != null)
            {
                switch (configItem.guild_type)
                {
                    case (int)GuildType.click:
                        EventTriggerListener.Get(clearTrans.gameObject).onClick -= OnTargetClick;
                        break;
                    case (int)GuildType.link:
                        EventTriggerListener.Get(clearTrans.gameObject).onDown -= OnTargetDown;
                        EventTriggerListener.Get(clearTrans.gameObject).onEnter -= OnTargetEnter;
                        break;
                }
            }

            clearTrans.SetParent(targetParentTrans, true);

            switch (index)
            {
                case -1:
                    preTargetTrans = null;
                    break;
                case 1:
                    targetTrans = null;
                    break;
                default:
                    holdTrans = null;
                    break;
            }
        }
    }

    private void UpdateTargetTrans(string aims)
    {
		GameObject aimObj = GameObject.Find (aims);
		if(aimObj == null)
		{
			Debug.Log("aimObj == null" + aims);
			return;
		}
		targetTrans = (RectTransform)aimObj.transform;
        targetParentTrans = (RectTransform)targetTrans.parent;
        targetTrans.SetParent(transform, true);
        targetTrans.SetSiblingIndex(blackTrans.GetSiblingIndex() + 1);

        switch (configItem.guild_type)
        {
            case (int)GuildType.click:
                EventTriggerListener.Get(targetTrans.gameObject).onClick += OnTargetClick;
                break;
            case (int)GuildType.link:
                EventTriggerListener.Get(targetTrans.gameObject).onDown += OnTargetDown;
                EventTriggerListener.Get(targetTrans.gameObject).onEnter += OnTargetEnter;
                break;
            case (int)GuildType.introduction:
                EventTriggerListener.Get(targetTrans.gameObject).onClick += OnTargetClick;
                EventTriggerListener.Get(blackTrans.gameObject).onClick = OnBlankClick;
                break;
        }
    }
}
