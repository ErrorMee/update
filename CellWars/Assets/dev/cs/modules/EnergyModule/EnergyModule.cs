using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnergyModule : BaseModule
{
	public Transform closeTrans;
	public Button closeBtn;

	public Text title;

	public Text getText;

	public Transform buyBtn;
	public Text buyTitle;

	public Text costText;

	public Text enoughText;

	private int gemCost;
	private bool gemEnough = false;

	override protected void Awake()
	{
		base.Awake();
		title.text = LanguageUtil.GetTxt(11908);
		buyTitle.text = LanguageUtil.GetTxt(11608);
		enoughText.text = LanguageUtil.GetTxt(11909);
		PlayerModel.Instance.updateWealthEvent += InitWealthView;
		PlayerModel.Instance.updateWealthsEvent += InitView;
	}

	void Start()
	{
		InitEvents();
		InitView();
	}

	void OnDisable()
	{
		PlayerModel.Instance.updateWealthEvent -= InitWealthView;
		PlayerModel.Instance.updateWealthsEvent -= InitView;
	}

	private void InitEvents()
	{
		EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClickClose;
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClickClose;
		EventTriggerListener.Get(buyBtn.gameObject).onClick = OnBuyClick;
	}

	private void InitView()
	{
		InitWealthView(0);
	}

	private void InitWealthView(int type = 0)
	{
		int buyTimes = PlayerPrefsUtil.GetInt (PlayerPrefsUtil.ENERGY_BUY, 0);

		config_wealth_item config = (config_wealth_item)GameMgr.resourceMgr.config_wealth.GetItem ((int)WealthTypeEnum.Energy);

		string[] strs = config.special.Split(new char[] { '|' });

		int max = int.Parse(strs[0]);
		int a = int.Parse(strs[1]);
		int b = int.Parse(strs[2]);
		int c = int.Parse(strs[3]);

		gemCost = (int)(a * Math.Pow(buyTimes, 2) + b * buyTimes + c);

		int canBuyTimes = max - config.limitCount;

		WealthInfo infoEnergy = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Energy);
		WealthInfo infoGem = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);


		costText.text = "" + gemCost;
		if (infoGem.count >= gemCost) {
			costText.color = Color.green;
			gemEnough = true;
		} else {
			costText.color = Color.red;
			gemEnough = false;
		}

		if (buyTimes < canBuyTimes) {
			buyBtn.gameObject.SetActive (true);
			enoughText.gameObject.SetActive (false);

			getText.text = "<size=64>" + infoEnergy.count + "/</size><color=#00ff00>" + infoEnergy.limit + "+1" + "</color>";

		} else {
			buyBtn.gameObject.SetActive (false);
			enoughText.gameObject.SetActive (true);

			getText.text = "<size=64>" + infoEnergy.count + "/</size><color=#00ff00>" + infoEnergy.limit + "" 
				+ "</color><size=48> " + LanguageUtil.GetTxt(11911) + "</size>";
		}
	}

	private void OnClickClose(GameObject go)
	{
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.ENERGY);
		ScreenSlider.OpenSlid ();
	}

	private void OnBuyClick(GameObject go)
	{
		if (gemEnough) {
			
			PlayerPrefsUtil.SetInt (PlayerPrefsUtil.ENERGY_BUY, PlayerPrefsUtil.GetInt (PlayerPrefsUtil.ENERGY_BUY, 0) + 1);

			WealthInfo infoEnergy = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Energy);
			infoEnergy.limit += 1;
			WealthInfo infoGem = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);
			infoGem.count -= gemCost;
			PlayerModel.Instance.SaveWealths();

			PromptModel.Instance.Pop (LanguageUtil.GetTxt (11910) + " +1", true, (int)WealthTypeEnum.Energy);
			PlayerModel.Instance.CheckEnergyRecover (false);
		} else {
			PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, (int)WealthTypeEnum.Gem);
			GameMgr.moduleMgr.AddUIModule(ModuleEnum.WEALTH);
		}
	}
}