using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetModule : BaseModule {

    public Text titel;

    public Transform closeBG;
    public Button closeBtn;

    public Toggle musicMute;

    public Toggle soundMute;

    public Button reviewBtn;

    public Button shareBtn;

    public Button cmdBtn;

    public Button sortBtn;

    public Button languageBtn;

    public Animation winOpenAnim;

    public GameObject cmd;
    public InputField cmdInput;
    public Button cmdDo;
	private List<string> cmds = new List<string>() { "clearall", "openall", "openat", "addgem", "addcoin"};
    private int cmdParam = 0;

    override protected void Awake()
    {
        base.Awake();
        UpdateView();

		cmdBtn.gameObject.SetActive(GameModel.Instance.DebugDev);

        cmd.SetActive(false);
    }

    void Start()
    {
        InitView();

        InitEvents();

        winOpenAnim.Play();
    }

    override protected void UpdateView()
    {
        titel.text = LanguageUtil.GetTxt(11001);
        languageBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetDefaultLanguageName();
    }

    private void InitView()
    {
        musicMute.isOn = GameMgr.audioMgr.GetIsMute(false);
        soundMute.isOn = GameMgr.audioMgr.GetIsMute(true);
    }

    private void InitEvents()
    {
        EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClickClose;
        EventTriggerListener.Get(closeBG.gameObject).onClick = OnClickClose;

        EventTriggerListener.Get(musicMute.gameObject).onClick = OnClickMusic;
        EventTriggerListener.Get(soundMute.gameObject).onClick = OnClickSound;
        EventTriggerListener.Get(sortBtn.gameObject).onClick = OnSort;
        EventTriggerListener.Get(reviewBtn.gameObject).onClick = OnReview;
        EventTriggerListener.Get(shareBtn.gameObject).onClick = OnShare;
        
        EventTriggerListener.Get(languageBtn.gameObject).onClick = OnSwitchLanguage;

        EventTriggerListener.Get(cmdBtn.gameObject).onClick = OnCmd;
        EventTriggerListener.Get(cmdDo.gameObject).onClick = OnCmdDo;
    }

    private void OnClickClose(GameObject go)
    {
        GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
    }

    private void OnClickMusic(GameObject go)
    {
        GameMgr.audioMgr.SetMute(false, !GameMgr.audioMgr.GetIsMute(false));
    }

    private void OnClickSound(GameObject go)
    {
        GameMgr.audioMgr.SetMute(true, !GameMgr.audioMgr.GetIsMute(true));
    }

    private void OnSort(GameObject go)
    {
		SocialModel.Instance.ShowLeaderboard();
    }

    private void OnReview(GameObject go)
    {
		SocialModel.Instance.ShowReview();
    }

    private void OnShare(GameObject go)
    {
        PromptModel.Instance.Pop(LanguageUtil.GetTxt(12002));
    }

    private void OnCmd(GameObject go)
    {
        cmd.SetActive(!cmd.activeSelf);
    }

    private void OnSwitchLanguage(GameObject go)
    {
        LanguageUtil.SwitchLanguage();
        UpdateView();
    }

    private void OnCmdDo(GameObject go)
    {
        string cmdStr = cmdInput.text;
		string[] strs = cmdStr.Split(new char[] { ',' });
		string cmdFun = strs[0].ToLower();
        for (int i = 0; i < cmds.Count; i++)
        {
			if (cmdFun == cmds[i].ToLower())
            {
				if(strs.Length > 1)
				{
					cmdParam = int.Parse (strs [1]);
				}

				Invoke(cmdFun,0.1f);
                return;
            }
        }
        Debug.Log("error cmd");
    }

    private void clearall()
    {
        PlayerModel.CLEAR_ALL = true;

        PlayerModel.Instance.LoadWeaths();
        MapModel.Instance.LoadMaps();
        SkillTempletModel.Instance.LoadSkillTemplets();
        GuildModel.Instance.InitGuild();
		ADModel.Instance.LoadADData ();

        PlayerModel.CLEAR_ALL = false;

        GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
    }

    private void openall()
    {
        PlayerModel.Instance.LoadWeaths();
		MapModel.Instance.LoadMaps((int)GameModel.Instance.GetGameConfig(1007));
        SkillTempletModel.Instance.LoadSkillTemplets();
        GuildModel.Instance.InitGuild();
		ADModel.Instance.LoadADData ();

        GuildModel.Instance.shield = true;
        GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
    }

    private void openat()
    {
        //int mapid = 20052;
        PlayerModel.CLEAR_ALL = true;
        PlayerModel.Instance.LoadWeaths();
        MapModel.Instance.LoadMaps();
        SkillTempletModel.Instance.LoadSkillTemplets();
		ADModel.Instance.LoadADData ();

        PlayerModel.CLEAR_ALL = false;

        PlayerModel.Instance.LoadWeaths();
		MapModel.Instance.LoadMaps(cmdParam);
        SkillTempletModel.Instance.LoadSkillTemplets();
        GuildModel.Instance.InitGuild();
        GuildModel.Instance.shield = true;
        GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
    }

	private void addgem()
	{
		//int winGem = 200;
		WealthInfo gemInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);
		gemInfo.count += cmdParam;
		PlayerModel.Instance.SaveWealths();
	}


	private void addcoin()
	{
		//int winGem = 200;
		WealthInfo gemInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
		gemInfo.count += cmdParam;
		PlayerModel.Instance.SaveWealths();
	}
}
