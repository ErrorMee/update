using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using LitJson;

public class SkillTempletModel : Singleton<SkillTempletModel>
{
	public List<SkillGroupInfo> skillGroups = new List<SkillGroupInfo> ();

    public List<int> skillSelects = new List<int>();

	public const short OPEN_LEVEL = 7;
    public static short MAX_LEVEL;

	private List<List<Vector2>> holeHump;
	private List<List<Vector2>> holeValley;

    public Action<SkillTempletInfo> updateOperateEvent;
    public Action updateSelectEvent;

    public int selectGroupIndex = 0;

    public void LoadSkillTemplets()
	{
        
        if (PlayerModel.CLEAR_ALL)
        {
            PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.SKILL_LV);
            PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.SKILL_SELECT);
        }

        string SKILL_SELECTStr = PlayerPrefsUtil.GetString(PlayerPrefsUtil.SKILL_SELECT);
        skillSelects = JsonMapper.ToObject<List<int>>(SKILL_SELECTStr);

        string SKILL_LVStr = PlayerPrefsUtil.GetString(PlayerPrefsUtil.SKILL_LV);
        List<int> SKILL_LVs = JsonMapper.ToObject<List<int>>(SKILL_LVStr);

        skillGroups = new List<SkillGroupInfo>();

        List<config_skill_item> skills = GameMgr.resourceMgr.config_skill.data;
        for (int i = 0; i < skills.Count; i++)
        {
            config_skill_item skill_item = skills[i];

            SkillTempletInfo skillTempletInfo = new SkillTempletInfo();
            skillTempletInfo.config = skill_item;
            if (SKILL_LVs != null)
            {
                skillTempletInfo.lv = SKILL_LVs[i];
            }

            SkillGroupInfo findSkillGroupInfo = null;

            for (int n = 0; n < skillGroups.Count; n++)
            {
                SkillGroupInfo checker = skillGroups[n];
                if (checker.GetGroupId() == skill_item.groupId && findSkillGroupInfo == null)
                {
                    findSkillGroupInfo = checker;
                }
            }

            if (findSkillGroupInfo == null)
            {
                SkillGroupInfo skillTempletGroupInfo = new SkillGroupInfo();
                skillGroups.Add(skillTempletGroupInfo);
                skillTempletGroupInfo.skillTemplets.Add(skillTempletInfo);
            }
            else
            {
                findSkillGroupInfo.skillTemplets.Add(skillTempletInfo);
            }
        }

        if (skillSelects == null)
        {
            skillSelects = new List<int>();
            for (int n = 0; n < skillGroups.Count; n++)
			{
				List<SkillTempletInfo> skillTemplets = skillGroups[n].skillTemplets;
				for(int m = 0;m<skillTemplets.Count;m++)
				{
					SkillTempletInfo skillTemplet = skillTemplets[m];
					if(skillTemplet.config.type == 1)
					{
						skillSelects.Add(skillTemplet.config.cellId);
					}
				}
			}
        }

        InitHole();
        UpdateStarInfo();
	}

    public List<SkillTempletInfo> GetSkillTemplets(int type)
    {
        List<SkillTempletInfo> skills = new List<SkillTempletInfo>();
        for (int i = 0; i < skillGroups.Count;i++ )
        {
            SkillGroupInfo group = skillGroups[i];
            for (int j = 0; j < group.skillTemplets.Count;j++ )
            {
                SkillTempletInfo t = group.skillTemplets[j];

                if(t.config.type == type)
                {
                    skills.Add(t);
                }
            }
        }

        return skills;
    }

    private void UpdateStarInfo()
    {
        StarInfo startInfo = MapModel.Instance.starInfo;
        startInfo.skillFullStar = 0;
        startInfo.skillUsedStar = 0;

        for (int i = 0; i < skillGroups.Count; i++)
        {
            SkillGroupInfo skillTempletGroupInfo = skillGroups[i];

            for (int j = 0; j < skillTempletGroupInfo.skillTemplets.Count; j++)
            {
                SkillTempletInfo skillTempletInfo = skillTempletGroupInfo.skillTemplets[j];
                startInfo.skillFullStar += skillTempletInfo.GetCostStar(SkillTempletModel.MAX_LEVEL);
                startInfo.skillUsedStar += skillTempletInfo.GetCostStar();
            }
        }
    }

    private void InitHole()
    {
		holeHump = new List<List<Vector2>>();
		holeValley = new List<List<Vector2>>();

        List<List<int>> indexss = new List<List<int>>() { new List<int>() { 0, 1, 2, 3, 4, 5, 6 }, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 } };
		List<List<int>> indexs2 = new List<List<int>>() { new List<int>() { 0, 1, 2, 3, 4, 5, 6 }, new List<int>() { 0, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 } };
		List<List<int>> indexs3 = new List<List<int>>() { new List<int>() { 0, 1, 2, 3, 4, 5, 6 }, new List<int>() { 6, 5, 4, 3, 2, 1, 0, 11, 10, 9, 8, 7 } };
		List<List<int>> indexs4 = new List<List<int>>() { new List<int>() { 0, 1, 2, 3, 4, 5, 6 }, new List<int>() { 6, 7, 8, 9, 10, 11, 0, 1, 2, 3, 4, 5 } };
		List<List<List<int>>> bigIndex = new List<List<List<int>>>();
		bigIndex.Add(indexss);
		bigIndex.Add(indexs2);
		bigIndex.Add(indexs3);
		bigIndex.Add(indexs4);

		for(int n = 0; n < bigIndex.Count; n++)
		{
			List<Vector2> smallHump = new List<Vector2>();
			holeHump.Add(smallHump);
			List<Vector2> smallValley = new List<Vector2>();
			holeValley.Add(smallValley);

			List<List<int>> indexsSmall = bigIndex[n];

			for (int i = 0; i < indexsSmall.Count;i++ )
			{
				List<int> indexs = indexsSmall[i];
				for (int j = 0; j < indexs.Count; j++)
				{
					int index = indexs[j];
					smallHump.Add(FightConst.RING_HUMP[i][index]);
					smallValley.Add(FightConst.RING_VALLEY[i][index]);
				}
			}
		}

        SkillTempletModel.MAX_LEVEL = (short)holeValley[0].Count;
    }

    public SkillTempletInfo GetTemplet(int id)
    {

        for (int i = 0; i < skillGroups.Count; i++)
		{
            SkillGroupInfo skillTempletGroupInfo = skillGroups[i];

			for(int j = 0;j<skillTempletGroupInfo.skillTemplets.Count;j++)
			{
				SkillTempletInfo skillTempletInfo = skillTempletGroupInfo.skillTemplets[j];
                if (skillTempletInfo.config.cellId == id)
				{
					return skillTempletInfo;
				}
			}
		}

        return null;
    }

    public SkillGroupInfo GetGroup(int group_id)
    {
        for (int i = 0; i < skillGroups.Count; i++)
		{
            SkillGroupInfo skillTempletGroupInfo = skillGroups[i];

            if (skillTempletGroupInfo.GetGroupId() == group_id)
			{
				return skillTempletGroupInfo;
			}
		}
        return null;
    }

	public Vector2 GetHolePos(int lv,bool isHump,int dir = 0)
    {
        if (isHump)
        {
			return holeHump[dir][lv - 1];
        }
        else
        {
			return holeValley[dir][lv - 1];
        }
    }

    public void UpLevel(SkillTempletInfo operatingTemplet,int offsetLv)
    {
        operatingTemplet.lv += offsetLv;
        if (operatingTemplet.lv >= SkillTempletModel.MAX_LEVEL)
        {
            operatingTemplet.lv = SkillTempletModel.MAX_LEVEL;
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11306));
        }
        SaveTemplet();

        config_sort_item config_sort_item = GameMgr.resourceMgr.config_sort.GetItemByTypeAndSpecial(4, "" + operatingTemplet.config.groupId);

		float sortProgress = operatingTemplet.GetShowLv() / (config_sort_item.refer + 0.00f);

        SocialModel.Instance.ReportProgress(config_sort_item.gid, sortProgress);

        if (updateOperateEvent != null)
        {
            updateOperateEvent(operatingTemplet);
        }
    }

    public void ClearLv(SkillTempletInfo operatingTemplet)
    {
        operatingTemplet.lv = SkillTempletModel.OPEN_LEVEL;
        SaveTemplet();
        if (updateOperateEvent != null)
        {
            updateOperateEvent(operatingTemplet);
        }
    }

	public void UpdataTemplet()
	{
        for (int i = 0; i < skillGroups.Count; i++)
		{
            SkillGroupInfo skillTempletGroupInfo = skillGroups[i];
			
			for(int j = 0;j<skillTempletGroupInfo.skillTemplets.Count;j++)
			{
				SkillTempletInfo skillTempletInfo = skillTempletGroupInfo.skillTemplets[j];
				if (skillTempletInfo.IsOpen())
				{
                    if (BattleModel.Instance.crtConfig.IsForbidSkill(skillTempletInfo.config.cellId))
					{
						skillTempletInfo.fobid = true;
					}else{
						skillTempletInfo.fobid = false;
					}
				}else
				{
					skillTempletInfo.fobid = true;
				}
			}
		}
	}

    public bool SkillModuleIsUnlock()
    {
        for (int i = 0; i < skillGroups.Count; i++)
        {
            SkillGroupInfo skillGroupInfo = skillGroups[i];

            for (int j = 0; j < skillGroupInfo.skillTemplets.Count; j++)
            {
                SkillTempletInfo skillTempletInfo = skillGroupInfo.skillTemplets[j];
                if (skillTempletInfo.IsUnlock())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool SkillModuleIsUnlockLevelUp()
    {
        SkillTempletInfo skillTempletInfo = skillGroups[0].skillTemplets[1];
        if (skillTempletInfo.IsUnlock())
        {
            return true;
        }

        return false;
    }

    public SkillTempletInfo GetUnlockSkill(int mapId)
    {
        for (int i = 0; i < skillGroups.Count; i++)
        {
            SkillGroupInfo skillGroupInfo = skillGroups[i];

            for (int j = 0; j < skillGroupInfo.skillTemplets.Count; j++)
            {
                SkillTempletInfo skillTempletInfo = skillGroupInfo.skillTemplets[j];
                if (skillTempletInfo.config.unlockId == mapId)
                {
                    return skillTempletInfo;
                }
            }
        }
        return null;
    }

    public void SelectTemplet(int id)
    {
        config_cell_item cellTarget = (config_cell_item)GameMgr.resourceMgr.config_cell.GetItem(id);
        int i;
        for (i = 0; i < skillSelects.Count; i++)
        {
            int skillId = SkillTempletModel.Instance.skillSelects[i];
            config_cell_item cell = (config_cell_item)GameMgr.resourceMgr.config_cell.GetItem(skillId);

            if (cellTarget.hide_id == cell.hide_id)
            {
                SkillTempletModel.Instance.skillSelects[i] = id;
                SaveSelect();
                if (updateSelectEvent != null)
                {
                    updateSelectEvent();
                }
                return;
            }
        }
    }

    private void SaveTemplet()
    {
        UpdateStarInfo();

        List<int> SKILL_LVs = new List<int>();
        for (int i = 0; i < skillGroups.Count;i++ )
        {
            List<SkillTempletInfo> skillTemplets = skillGroups[i].skillTemplets;

            for (int j = 0; j < skillTemplets.Count;j++ )
            {
                SKILL_LVs.Add(skillTemplets[j].lv);
            }
        }

        string SKILL_LVStr = JsonMapper.ToJson(SKILL_LVs);
        PlayerPrefsUtil.SetString(PlayerPrefsUtil.SKILL_LV, SKILL_LVStr);
    }

    private void SaveSelect()
    {
        string SKILL_SELECTStr = JsonMapper.ToJson(skillSelects);
        PlayerPrefsUtil.SetString(PlayerPrefsUtil.SKILL_SELECT, SKILL_SELECTStr);
    }
}
