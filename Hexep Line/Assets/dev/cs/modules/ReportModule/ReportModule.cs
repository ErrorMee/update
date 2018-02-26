using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ReportModule : BaseModule
{
	public Animation winOpenAnim;

    public WinA winA;
    public WinB winB;
    public LoseA loseA;
    public LoseB loseB;

    override protected void Awake()
    {
        base.Awake();
        winA.gameObject.SetActive(false);
        winB.gameObject.SetActive(false);
        loseA.gameObject.SetActive(false);
        loseB.gameObject.SetActive(false);
    }

    void OnEnable()
    {
		winOpenAnim.Play();
        BattleModel.Instance.play_mapId = 0;
        AudioModel.Instance.PlayeSound("report");
    }

    void Start()
    {
        UpdateView();
    }

    override protected void UpdateView()
    {
        if (FightModel.Instance.fightInfo.isWin)
        {
            bool isPassed = MapModel.Instance.IsPassed(BattleModel.Instance.crtConfig.id);

            SkillTempletInfo skillTempletInfo = SkillTempletModel.Instance.GetUnlockSkill(BattleModel.Instance.crtConfig.id);
			if (skillTempletInfo == null || isPassed || skillTempletInfo.config.type != 1)
            {
                winA.gameObject.SetActive(true);
            }
            else
            {
                winB.gameObject.SetActive(true);
            }
        }
        else
        {
            bool skillModuleIsUnlockLevelUp = SkillTempletModel.Instance.SkillModuleIsUnlockLevelUp();
            if (skillModuleIsUnlockLevelUp)
            {
                loseB.gameObject.SetActive(true);
            }else
            {
                loseA.gameObject.SetActive(true);
            }
        }

        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    public void OnCloseClick(GameObject go)
    {
        BattleModel.Instance.lose_map = 0;
        ModuleModel.Instance.AddUIModule((int)ModuleEnum.MAP);
    }

    public void OnSkillClick(GameObject go)
	{
		BattleModel.Instance.lose_map = 0;
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.SKILL);
	}

    public void OnAgainClick(GameObject go)
	{
        BattleModel.Instance.lose_map = 0;
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.MAP);
		FightMap(BattleModel.Instance.crtConfig.id);
	}

    public void OnNextClick(GameObject go)
	{
        BattleModel.Instance.lose_map = 0;
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.MAP);
		FightMap(BattleModel.Instance.crtConfig.id + 1);
	}

	private void FightMap(int mapId)
	{
		config_map_item config = (config_map_item)ResModel.Instance.config_map.GetItem(mapId);
		
		if (config != null)
		{
			int endlevel = (int)GameModel.Instance.GetGameConfig(1007);
			if (config.id > endlevel)
			{
				config_item_base mapconfigitem = ResModel.Instance.config_map.GetItem(endlevel);
                PromptModel.Instance.Pop(string.Format(LanguageUtil.GetTxt(11101), Convert.ToInt32(mapconfigitem.name)));
				return;
			}
			
			BattleModel.Instance.InitCrtBattle(config);
			BattleModel.Instance.InitFight();
			
			SkillTempletModel.Instance.UpdataTemplet();
			
			FillModel.Instance.InitFillInfo();
			
			ModuleModel.Instance.AddUIModule((int)ModuleEnum.PREPARE);
		}
		else
		{
            Debug.Log(LanguageUtil.GetTxt(11102));
		}
	}
}
