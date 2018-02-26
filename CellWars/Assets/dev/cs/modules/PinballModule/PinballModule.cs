using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PinballModule : BaseModule
{
    public Transform operatePart;
    public Button closeBtn;
	public Button ordinaryBtn;
    public Button advanceBtn;
    public Button refreshBtn;

    public BallShowPart showPart;

    override protected void Awake()
    {
        base.Awake();
		ordinaryBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(13001);
		advanceBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(13002);
    }

    void Start()
    {
        showPart.Init();
    }

    void OnEnable()
    {
        EventTriggerListener.Get(closeBtn.gameObject).onClick = OnCloseClick;
		EventTriggerListener.Get(ordinaryBtn.gameObject).onClick = OnOrdinaryClick;
        EventTriggerListener.Get(advanceBtn.gameObject).onClick = OnAdvanceClick;
        EventTriggerListener.Get(refreshBtn.gameObject).onClick = OnRefreshClick;
    }

    private void OnCloseClick(GameObject go)
    {
        GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
    }

	private void OnOrdinaryClick(GameObject go)
    {
        List<int> newPos = new List<int>();
        newPos.Add(2);
        showPart.Play(newPos);
		operatePart.gameObject.SetActive(false);
    }

    private void OnAdvanceClick(GameObject go)
    {
        List<int> newPos = new List<int>();
        newPos.Add(1);
        newPos.Add(3);
        showPart.Play(newPos);
		operatePart.gameObject.SetActive(false);
    }

    private void OnRefreshClick(GameObject go)
    {
        
    }
}