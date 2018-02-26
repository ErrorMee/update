using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapLockModule : BaseModule {

	public Transform closeTrans;
	public Transform closeBtn;

	public Text title;
	public Text text1;
	public Text text2;
	public Text text3;
	public Text text4;
	public Text text5;

	public Transform moreBtn;
	public Transform buyBtn;

	override protected void Awake()
	{
		base.Awake();
		InitView();
		InitEvents();
	}

	private void InitView()
	{
		title.text = LanguageUtil.GetTxt(16001);
		text2.text = LanguageUtil.GetTxt(16003);
		text3.text = LanguageUtil.GetTxt(16004);
		text4.text = LanguageUtil.GetTxt(16005);

		UpdateView (0);
	}

	private void UpdateView(int type)
	{
		config_map_item mapConfig = BattleModel.Instance.crtConfig;
		IVInfo starLimit = mapConfig.GetStarLimit();
		StarInfo starInfo = MapModel.Instance.starInfo;

		text1.text = LanguageUtil.GetTxt(16002) + " " + ColorMgr.GetColorStr (ColorMgr.COLOR_RED, "" + starInfo.crtStar) + "/" + starLimit.id;

		WealthInfo infoGem = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);
		if (infoGem.count < starLimit.value) {
			text5.color = Color.red;
		} else {
			text5.color = Color.green;
		}
		text5.text = "" + starLimit.value;
	}

	private void InitEvents()
	{
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClickClose;
		EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClickClose;
		EventTriggerListener.Get(moreBtn.gameObject).onClick = OnClickMore;
		EventTriggerListener.Get(buyBtn.gameObject).onClick = OnClickBuy;
		PlayerModel.Instance.updateWealthEvent += UpdateView;
	}

	void OnDisable()
	{
		PlayerModel.Instance.updateWealthEvent -= UpdateView;
	}

	private void OnClickClose(GameObject go)
	{
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.MAPLOCK);
	}

	private void OnClickMore(GameObject go)
	{
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.MAPLOCK);
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.PREPARE);
		GameMgr.moduleMgr.AddUIModule (ModuleEnum.STAR);
	}

	private void OnClickBuy(GameObject go)
	{
		config_map_item mapConfig = BattleModel.Instance.crtConfig;
		IVInfo starLimit = mapConfig.GetStarLimit();
		WealthInfo infoGem = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);
		if (infoGem.count < starLimit.value) {
			PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, (int)WealthTypeEnum.Gem);
			GameMgr.moduleMgr.AddUIModule(ModuleEnum.WEALTH);
		} else {
			OnBuy ();
		}
	}

	private void OnBuy()
	{
		config_map_item mapConfig = BattleModel.Instance.crtConfig;
		IVInfo starLimit = mapConfig.GetStarLimit();

		WealthInfo infoGem = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);
		infoGem.count -= (int)starLimit.value;
		PlayerModel.Instance.SaveWealths();
		
		MapInfo mapInfo = MapModel.Instance.GetMapInfo(mapConfig.id);
		mapInfo.buyPassed = true;
		MapModel.Instance.SaveMap ();

		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.MAPLOCK);
	}
}
