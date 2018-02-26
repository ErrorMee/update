using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseModule : BaseModule {

	public Transform closeTrans;
	public Button closeBtn;

    public Text title;

	public Text content;

	public Toggle musicMute;

	public Toggle soundMute;

	public Button addMoveBtn;

	public Button restartBtn;

	public Button quitBtn;

    public Animation winOpenAnim;

    override protected void Awake()
    {
        base.Awake();
        title.text = LanguageUtil.GetTxt(11610);
		content.text = LanguageUtil.GetTxt(11104) + ": " + BattleModel.Instance.crtConfig.name;
		//addMoveBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11407);
        restartBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11611);
        quitBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11612);
    }

	void Start()
	{
		InitView();

		InitEvents();

        winOpenAnim.Play();

        GameMgr.audioMgr.ClearAllActorSound();

		FontUtil.FixCN();
	}

	private void InitView()
	{
		PropInfo propInfo = PropModel.Instance.GetProp(10004);
		addMoveBtn.gameObject.SetActive (!propInfo.isForbid);

		musicMute.isOn = GameMgr.audioMgr.GetIsMute(false);
		soundMute.isOn = GameMgr.audioMgr.GetIsMute(true);
	}

	private void InitEvents()
	{
		EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClickClose;
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClickClose;
		EventTriggerListener.Get (addMoveBtn.gameObject).onClick = OnClickAddMove;
		EventTriggerListener.Get(restartBtn.gameObject).onClick = OnClickRestart;
		EventTriggerListener.Get (quitBtn.gameObject).onClick = OnClickQuit;
		EventTriggerListener.Get(musicMute.gameObject).onClick = OnClickMusic;
		EventTriggerListener.Get(soundMute.gameObject).onClick = OnClickSound;
	}

	private void OnClickMusic(GameObject go)
	{
		GameMgr.audioMgr.SetMute(false, !GameMgr.audioMgr.GetIsMute(false));
	}

	private void OnClickSound(GameObject go)
	{
		GameMgr.audioMgr.SetMute(true, !GameMgr.audioMgr.GetIsMute(true));
	}

	private void OnClickClose(GameObject go)
	{
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.PAUSE);
	}

	private void OnClickRestart(GameObject go)
	{

		BattleModel.Instance.lose_map = 0;
		GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);

		int mapId = BattleModel.Instance.crtConfig.id;

		config_map_item config = (config_map_item)GameMgr.resourceMgr.config_map.GetItem(mapId);
		
		BattleModel.Instance.InitCrtBattle(config);
		BattleModel.Instance.InitFight();
		
		SkillTempletModel.Instance.UpdataTemplet();
		
		FillModel.Instance.InitFillInfo();
		
		GameMgr.moduleMgr.AddUIModule(ModuleEnum.PREPARE);
	}

	private void OnClickQuit(GameObject go)
	{
		BattleModel.Instance.play_mapId = 0;
		GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
	}

	private void OnClickAddMove(GameObject go)
	{
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.PAUSE);

		PropModel.Instance.OnSelectChanged(true,PropModel.Instance.GetProp(10004));
	}
}
