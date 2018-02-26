using System;
using UnityEngine;
using UnityEngine.UI;

public class ADModule : BaseModule
{
	public Text title;
	public Text count;
	public Text get;

	public Transform closeTrans;
	public Button playBtn;


	override protected void Awake()
	{
		base.Awake();
		title.text = LanguageUtil.GetTxt(17001);

		ADModel.Instance.CheckResetDay ();
	}

	void Start()
	{
		InitEvents();
		InitView();
	}


	void OnDisable()
	{
		ADModel.Instance.showADEvent -= OnShowADEvent;
	}

	private void InitView()
	{
		int todayTimes = PlayerPrefsUtil.GetInt (PlayerPrefsUtil.AD_TODAY_TIMES);

		int leftCount = ADModel.REWARD_AD_TIMES - todayTimes;

		if (leftCount <= 0) {
			leftCount = 0;
			get.text = "+" + 0;
			get.color = Color.white;
			count.color = Color.white;
		} else {
			get.text = "+" + ADModel.REWARD_AD_GEM;
			get.color = Color.green;
			count.color = Color.green;
		}

		count.text = leftCount + "/" + ADModel.REWARD_AD_TIMES;
	}

	private void InitEvents()
	{
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClickClose;
		EventTriggerListener.Get(playBtn.gameObject).onClick = OnPlayClick;

		ADModel.Instance.showADEvent += OnShowADEvent;
	}

	private void OnClickClose(GameObject go)
	{
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.ADREWARD);
		ScreenSlider.OpenSlid ();
	}

	private void OnPlayClick(GameObject go)
	{
		if (ADModel.Instance.ADIsReady ()) {
			ADModel.Instance.ShowAD ();
		} else {
			PromptModel.Instance.Pop(LanguageUtil.GetTxt(17002));
		}
	}

	private void OnShowADEvent()
	{
		InitView();
	}
}