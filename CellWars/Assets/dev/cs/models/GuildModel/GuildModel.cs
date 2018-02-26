using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class GuildModel:Singleton<GuildModel>
{
	public bool shield = false;

    private config_guild_item guidingItem;

    private GuildInfo guildInfo;

    private List<config_guild_item> guildRoots;

    public Action<config_guild_item> updateViewEvent;

    public void InitGuild()
    {

        if (PlayerModel.CLEAR_ALL)
        {
            PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.GUILD);
            guidingItem = null;
            shield = true;
        }

        string guildInfoStr = PlayerPrefsUtil.GetString(PlayerPrefsUtil.GUILD);
        guildInfo = JsonMapper.ToObject<GuildInfo>(guildInfoStr);
        if (guildInfo == null)
        {
            guildInfo = new GuildInfo();
        }
        if (guildInfo.completeRoots == null)
        {
            guildInfo.completeRoots = new List<int>();
        }
        if (guildInfo.completeKeys == null)
        {
            guildInfo.completeKeys = new List<int>();
        }

        guildRoots = new List<config_guild_item>();
        List<config_guild_item> configdata = GameMgr.resourceMgr.config_guild.data;
        for (int i = 0; i < configdata.Count; i++)
        {
            config_guild_item item = (config_guild_item)configdata[i];

            if (item.id == item.root_id)
            {
                guildRoots.Add(item);
                if (PlayerModel.CLEAR_ALL)
                {
                    item.SetCompleteTemp(false);
                }
            }
        }

        //uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        
    }

    public void CheckGuild()
    {
        int startId = 0;
        if (shield)
        {
            return;
        }

        //Debug.Log("guidingItem == null " + (guidingItem == null));
        if (guidingItem == null)
        {

            if (guildInfo.completeKeys.Count > 0)//todo 需要断线检查
            {
                int completeKey = guildInfo.completeKeys[0];
                config_guild_item completeItem = (config_guild_item)GameMgr.resourceMgr.config_guild.GetItem(completeKey);
                //Debug.Log("out line continue");
                StartGuild(completeItem);
                return;
            }

            for (int i = 0; i < guildRoots.Count; i++)
            {
                config_guild_item guildRoot = guildRoots[i];

                bool isComplete = CheckRootComplete(guildRoot);

                if (guildRoot.GetCompleteTemp() == false && isComplete == false && guildRoot.id > startId)
                {
                    bool needGuild = ConditionCheck(guildRoot);
                    if (needGuild)
                    {
                        //Debug.Log("find new guild");
                        StartGuild(guildRoot);
                        return;
                    }
                }
            }
            //Debug.Log("no needGuild");
        }
        else
        {
            GuildView();
        }
    }

    private void StartGuild(config_guild_item item)
    {
        //Debug.Log("StartGuild " + item.id);

        guidingItem = item;

        GuildView();
    }

    private bool CheckRootComplete(config_guild_item item)
    {
        if(shield)
        {
            return true;
        }

        for (int i = 0; i < guildInfo.completeRoots.Count; i++)
        {
            if (guildInfo.completeRoots[i] == item.root_id)
            {
                return true;
            }
        }

        return false;
    }

    private bool ConditionCheck(config_guild_item item)
    {
        List<TIVInfo> conditions = item.GetConditions();

        bool isReach = true;
        for (int i = 0; i < conditions.Count;i++ )
        {
            TIVInfo condition = conditions[i];
            switch ((int)condition.id)
            {
                case (int)GuildIDType.crt_star_num:

                    int gameValue = MapModel.Instance.starInfo.crtStar;

                    bool conditionCompare = ConditionCompare(condition,gameValue);

                    if (conditionCompare == false)
                    {
                        return false;
                    }

                    break;
                case (int)GuildIDType.guild_complete:

                    if (guildInfo.completeRoots.IndexOf((int)condition.value) == -1)
                    {
                        return false;
                    }

                    break;
			    case (int)GuildIDType.pass_map:

				MapInfo map = MapModel.Instance.GetMapInfo((int)condition.value);

				    if(map == null)
				    {
					    return false;
				    }

				    if(map.star <= 0)
				    {
					    return false;
				    }
                    break;
                case (int)GuildIDType.play_map:

				if (BattleModel.Instance.play_mapId != (int)condition.value)
                    {
                        return false;
                    }

                break;
                case (int)GuildIDType.ready_map:

                if (BattleModel.Instance.ready_map != (int)condition.value)
                {
                    return false;
                }

                break;

                case (int)GuildIDType.lose_map:

                if (BattleModel.Instance.lose_map != (int)condition.value)
                {
                    return false;
                }

                break;
            }

        }
        return isReach;
    }

    private bool ConditionCompare(TIVInfo condition, int gameValue)
    {
        switch (condition.type)
        {
            case (int)GuildConditionType.equal:

			return (int)condition.value == gameValue;

            case (int)GuildConditionType.more_then:

			return (int)condition.value > gameValue;

            case (int)GuildConditionType.less_then:

			return (int)condition.value < gameValue;
        }
        return false;
    }

    private void GuildView()
    {
        if (guidingItem.guild_type == (int)GuildType.tip)
        {
            if (updateViewEvent != null)
            {
                updateViewEvent(guidingItem);
            }
            return;
        }
        GameObject target = GameObject.Find(guidingItem.GetAims());
        if (target != null && target.activeSelf == true)
        {
            if (updateViewEvent != null)
            {
                updateViewEvent(guidingItem);
            }
        }
        else
        {
            //Debug.Log("target == null " + guidingItem.GetAims());
        }
    }

    public void CompleteSave()
    {
        config_guild_item nextItem = FindNextItem(guidingItem);

        if (nextItem == null)
        {
            if (!guildInfo.completeRoots.Contains(guidingItem.root_id))
            {
                if (guidingItem.complete_tip != null && guidingItem.complete_tip != "")
                {
                    PromptModel.Instance.Pop(LanguageUtil.GetTxt(Convert.ToInt32(guidingItem.complete_tip)));
                }

                List<TIVInfo> conditions = guidingItem.GetConditions();
                if (conditions != null)
                {
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        TIVInfo condition = conditions[i];

                        if (condition.id == (int)GuildIDType.play_map)
                        {
                            guidingItem.SetCompleteTemp(true);
                            return;
                        }
                    }
                }

                guildInfo.completeRoots.Add(guidingItem.root_id);
                Save();
            }
            
        }else
        {

        }
    }

    public void ForceCompleteSave()
    {
        if (guidingItem != null)
        {
            if (!guildInfo.completeRoots.Contains(guidingItem.root_id))
            {
                guildInfo.completeRoots.Add(guidingItem.root_id);
                Save();
            }
        }
        guidingItem = null;
    }

    public void CompleteContinue()
    {
        guidingItem = FindNextItem(guidingItem);

        if (guidingItem == null)
        {
            CheckGuild();
        }
        else
        {
			if(guidingItem.wait_time > 0)
			{
				LeanTween.delayedCall(guidingItem.wait_time/1000.00f,CheckGuild);
			}else
			{
				CheckGuild();
			}
        }
    }

    private config_guild_item FindNextItem(config_guild_item item)
    {
        if (item == null)
        {
            return null;
        }
        if (item.next_id == 0)
        {
            return null;
        }else
        {
            config_guild_item nextItem = (config_guild_item)GameMgr.resourceMgr.config_guild.GetItem(item.next_id);
            return nextItem;
        }
    }

    private void Save()
    {
        string guildInfoStr = JsonMapper.ToJson(guildInfo);
        PlayerPrefsUtil.SetString(PlayerPrefsUtil.GUILD, guildInfoStr);
    }

}