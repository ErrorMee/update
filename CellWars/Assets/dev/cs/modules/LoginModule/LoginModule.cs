using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;

public class LoginModule : BaseModule
{
    public Transform logoTrans;

    public Text studioTitle;
    public Image studioLogo;
    public Animation studioAnim;

	void Start ()
	{
		//LeanTween.delayedCall(0.3f, Login);

        StartGame();
    }

    private void Login()
    {
		studioTitle.text = ColorMgr.GetColorStr(ColorMgr.CELLS[0],"Gema Game");
		studioAnim.Play("STUDIO_IN");
        LeanTween.delayedCall(1f, OnLogin);
    }

    private void OnLogin()
    {
        LeanTween.delayedCall(1.2f, StartGame);
        LeanTween.alpha(studioLogo.rectTransform, 0, 0.4f);
        LeanTween.textAlpha(studioTitle.rectTransform, 0, 0.4f).delay = 0.4f;
        studioAnim.Play("STUDIO_OUT");
    }

    private void StartGame()
    {
        logoTrans.gameObject.SetActive(false);
        studioTitle.gameObject.SetActive(false);

        PlayerModel.Instance.LoadWeaths();
        MapModel.Instance.LoadMaps();
        SkillTempletModel.Instance.LoadSkillTemplets();
        GuildModel.Instance.InitGuild();
		ADModel.Instance.LoadADData ();

        GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);

        string mailStr = "device:" + SystemInfo.deviceUniqueIdentifier + "<br>\n\r";

        mailStr += "deviceModel:" + SystemInfo.deviceModel + "<br>\n\r";
        mailStr += "deviceName:" + SystemInfo.deviceName + "<br>\n\r";
        mailStr += "deviceType:" + SystemInfo.deviceType + "<br>\n\r";
        mailStr += "systemMemorySize:" + SystemInfo.systemMemorySize + "<br>\n\r";
        mailStr += "operatingSystem:" + SystemInfo.operatingSystem + "<br>\n\r";
        mailStr += "graphicsDeviceName:" + SystemInfo.graphicsDeviceName + "<br>\n\r";
        mailStr += "graphicsDeviceType:" + SystemInfo.graphicsDeviceType + "<br>\n\r";
        mailStr += "graphicsMemorySize:" + SystemInfo.graphicsMemorySize + "<br>\n\r";

        mailStr += "<br>\n\r player data <br>\n\r";
        mailStr += "-----wealths-----<br>\n\r" + JsonMapper.ToJson(PlayerModel.Instance.wealths) + "<br>\n\r";
        mailStr += "-----passMaps-----<br>\n\r" + JsonMapper.ToJson(MapModel.Instance.passMaps) + "<br>\n\r";
        mailStr += "-----skills-----<br>\n\r" + JsonMapper.ToJson(SkillTempletModel.Instance.GetSkillTemplets(1)) + "<br>\n\r";
        MailUtil.SendGameLog(MailUtil.GameLogType.login, mailStr);
    }
}
