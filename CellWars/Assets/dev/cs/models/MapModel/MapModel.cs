using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using LitJson;

//管理个人关卡进度数据

public class MapModel : Singleton<MapModel>
{
    public bool hasTestData = false;
    public int TestMapEnd = 20000;

	public List<ChapterInfo> chapters = new List<ChapterInfo>();

    public List<MapInfo> passMaps = new List<MapInfo>();

    public List<MapInfo> newMaps = new List<MapInfo>();

    public List<MapInfo> changeMaps = new List<MapInfo>();

    public StarInfo starInfo = new StarInfo();

    public config_chapter_item selectChapter;
    public Action SelectChapterEvent;
	public Action SwitchChapterEvent;
	public Action<config_chapter_item> StarRewardEvent;

    public void LoadMaps(int maxId = 0)
    {
        if (PlayerModel.CLEAR_ALL)
        {
			PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.CHAPTER);
            PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.PASS_MAP);
            PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.NEW_MAP);
        }

		string CHAPTER = PlayerPrefsUtil.GetString(PlayerPrefsUtil.CHAPTER);
		chapters = JsonMapper.ToObject<List<ChapterInfo>>(CHAPTER);
		if (chapters == null)
		{
			chapters = new List<ChapterInfo>();
		}

        string PASS_MAP = PlayerPrefsUtil.GetString(PlayerPrefsUtil.PASS_MAP);
        passMaps = JsonMapper.ToObject<List<MapInfo>>(PASS_MAP);
        if (passMaps == null)
        {
            passMaps = new List<MapInfo>();
        }

        string NEW_MAP = PlayerPrefsUtil.GetString(PlayerPrefsUtil.NEW_MAP);
        newMaps = JsonMapper.ToObject<List<MapInfo>>(NEW_MAP);

        if (newMaps == null)
        {
            newMaps = new List<MapInfo>();
            OpenLevel(20001);
        }
        else
        {
            if (newMaps.Count == 0)
            {
                MapInfo mapInfo = passMaps[passMaps.Count - 1];
                selectChapter = GameMgr.resourceMgr.config_chapter.GetChapterByMap(mapInfo);
            }
            else
            {
                MapInfo mapInfo = newMaps[newMaps.Count - 1];
                selectChapter = GameMgr.resourceMgr.config_chapter.GetChapterByMap(mapInfo);
            }
            if (selectChapter == null)
            {
                selectChapter = (config_chapter_item)GameMgr.resourceMgr.config_chapter.data[GameMgr.resourceMgr.config_chapter.data.Count - 1];
            }
        }

		if (maxId > 0)
        {
			WealthInfo coinInfo = PlayerModel.Instance.GetWealth ((int)WealthTypeEnum.Coin);
			WealthInfo gemInfo = PlayerModel.Instance.GetWealth ((int)WealthTypeEnum.Gem);

            newMaps = new List<MapInfo>();
            passMaps = new List<MapInfo>();
			int openId = 0;
            for (int j = 0; j < GameMgr.resourceMgr.config_map.data.Count; j++)
			{
                config_map_item config_map_item = GameMgr.resourceMgr.config_map.data[j];
				if(config_map_item.id <= maxId)
				{
					MapInfo mapInfo = new MapInfo();
					mapInfo.configId = config_map_item.id;
					mapInfo.star = 2;
					mapInfo.score = config_map_item.GetJudgeScores()[mapInfo.star - 1];
					passMaps.Add(mapInfo);
					
					coinInfo.count += (int)GameModel.Instance.GetGameConfig(1005);
					gemInfo.count += (int)GameModel.Instance.GetGameConfig(1006);;

					for (int i = 0; i < SkillTempletModel.Instance.skillGroups.Count; i++)
					{
						SkillGroupInfo skillTempletGroupInfo = SkillTempletModel.Instance.skillGroups[i];
						
						int groupId = skillTempletGroupInfo.GetGroupId();
						
						WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(groupId);
						bottleInfo.count += 60;
					}
					openId = config_map_item.id + 1;
				}
            }
			OpenLevel(openId);
			PlayerModel.Instance.SaveWealths();
            SaveMap();
        }

        UpdateStarInfo();
    }

	public void SelectChapter(config_chapter_item itemConfig)
	{
		MapModel.Instance.selectChapter = itemConfig;
		if (SelectChapterEvent != null)
		{
			SelectChapterEvent();
		}
	}

    private void UpdateStarInfo()
    {
        starInfo.openMapFullStar = 0;
        starInfo.crtStar = 0;
        int i;
        for (i = 0; i < passMaps.Count; i++)
        {
            starInfo.openMapFullStar += 3;

            MapInfo mapInfo = MapModel.Instance.passMaps[i];

            starInfo.crtStar += mapInfo.star;
        }

        for (i = 0; i < MapModel.Instance.newMaps.Count; i++)
        {
            starInfo.openMapFullStar += 3;
        }
    }

    private MapInfo OpenLevel(int configId)
    {
        MapInfo newMap = new MapInfo();
        newMap.configId = configId;
        newMaps.Add(newMap);
        selectChapter = GameMgr.resourceMgr.config_chapter.GetChapterByMap(newMap);
        return newMap;
    }

	public void SlideChapter(bool isLeft)
	{
		List<config_chapter_item> datas = GameMgr.resourceMgr.config_chapter.data;
		int totalChapterCount = datas.Count;
		if (isLeft)
		{
			if (selectChapter.GetIndex() < totalChapterCount)
			{
				SwitchChapter((config_chapter_item)GameMgr.resourceMgr.config_chapter.GetItemAt(selectChapter.GetIndex()));
			}
		}
		else
		{
			if (selectChapter.GetIndex() > 1)
			{
				SwitchChapter((config_chapter_item)GameMgr.resourceMgr.config_chapter.GetItemAt(selectChapter.GetIndex() - 2));
			}
		}
	}

	public void SwitchChapter(config_chapter_item config)
    {
		selectChapter = config;
		if(SwitchChapterEvent != null)
		{
			SwitchChapterEvent();
		}
    }

    public void PassLevel(MapInfo passMap)
    {
        if (passMap.configId <= TestMapEnd)
        {
            return;
        }

        int i;
        for (i = 0; i < passMaps.Count; i++)
        {
            MapInfo mapInfo = passMaps[i];
            if (mapInfo.configId == passMap.configId)
            {
                if (mapInfo.star < passMap.star)
                {
					mapInfo.oldStar = mapInfo.star;

                    mapInfo.star = passMap.star;
                }
                if (mapInfo.score < passMap.score)
                {
                    mapInfo.score = passMap.score;
                }
                changeMaps.Add(mapInfo);
                SaveMap();
                UpdateStarInfo();
                return;
            }
        }

        for (i = 0; i < newMaps.Count; i++)
        {
            MapInfo mapInfo = newMaps[i];
            if (mapInfo.configId == passMap.configId)
            {
                newMaps.RemoveAt(i);
                passMaps.Add(passMap);
                changeMaps.Add(passMap);
				passMap.oldStar = 0;
                for (int j = 0; j < GameMgr.resourceMgr.config_map.data.Count; j++)
                {
                    config_map_item config_map_item = GameMgr.resourceMgr.config_map.data[j];

					if (config_map_item.pre_id == passMap.configId && config_map_item.pre_id != config_map_item.id)
                    {
                        MapInfo newMap = OpenLevel(config_map_item.id);
                        changeMaps.Add(newMap);
						break;
                    }
                }
                SaveMap();
				UpdateStarInfo();
				return;
            }
        }
    }

	public void ChapterReward(config_chapter_item itemConfig)
	{
		for(int i =0 ;i<chapters.Count;i++)
		{
			ChapterInfo chapter = chapters[i];

			if(chapter.id == itemConfig.id)
			{
				chapter.reward = true;

				List<TIVInfo> rewards = itemConfig.GetRewardList();

				for(int j = 0;j<rewards.Count;j++)
				{
					TIVInfo reward = rewards[j];
					WealthInfo wealthInfo = PlayerModel.Instance.GetWealth(reward.id);
					wealthInfo.count += (int)reward.value;
					PromptModel.Instance.Pop("+"+reward.value,true,reward.id);
				}
				PlayerModel.Instance.SaveWealths();

				SaveChapter();
				if(StarRewardEvent != null)
				{
					StarRewardEvent(itemConfig);
				}
				return;
			}
		}
		ChapterInfo chapterNew = new ChapterInfo();
		chapterNew.id = itemConfig.id;
		chapterNew.reward = true;
		chapters.Add(chapterNew);

		List<TIVInfo> rewardsNew = itemConfig.GetRewardList();

		for(int j = 0;j<rewardsNew.Count;j++)
		{
			TIVInfo reward = rewardsNew[j];
			WealthInfo wealthInfo = PlayerModel.Instance.GetWealth(reward.id);
			wealthInfo.count += (int)reward.value;
			PromptModel.Instance.Pop("+"+reward.value,true,reward.id);
		}
		PlayerModel.Instance.SaveWealths();

		SaveChapter();
		if(StarRewardEvent != null)
		{
			StarRewardEvent(itemConfig);
		}
	}

	public ChapterInfo GetChapterInfo(int id)
	{
		for(int i =0 ;i<chapters.Count;i++)
		{
			ChapterInfo chapter = chapters[i];

			if(chapter.id == id)
			{
				return chapter;
			}
		}
		return null;
	}

    public void SaveMap()
    {
        string PASS_MAP = JsonMapper.ToJson(passMaps);
        PlayerPrefsUtil.SetString(PlayerPrefsUtil.PASS_MAP, PASS_MAP);
        string NEW_MAP = JsonMapper.ToJson(newMaps);
        PlayerPrefsUtil.SetString(PlayerPrefsUtil.NEW_MAP, NEW_MAP);
    }

	private void SaveChapter()
	{
		string CHAPTER = JsonMapper.ToJson(chapters);
		PlayerPrefsUtil.SetString(PlayerPrefsUtil.CHAPTER, CHAPTER);
	}

	public bool IsPassed(int mapId)
	{
		int i;
		for (i = 0; i < passMaps.Count; i++)
		{
			MapInfo map = passMaps[i];
			if (map.configId == mapId)
			{
				return true;
			}
		}
		return false;
	}

    public MapInfo GetMapInfo(int mapId)
    {
        int i;
        for (i = 0; i < passMaps.Count; i++)
        {
            MapInfo map = passMaps[i];
            if (map.configId == mapId)
            {
                return map;
            }
        }

        for (i = 0; i < newMaps.Count; i++)
        {
            MapInfo map = newMaps[i];
            if (map.configId == mapId)
            {
                return map;
            }
        }
        return null;
    }
}