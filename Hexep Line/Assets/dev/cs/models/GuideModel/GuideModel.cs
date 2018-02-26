using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class GuideModel:Singleton<GuideModel>
{
	public bool shield = false;

    private config_guide_item guidingItem;

    private GuideInfo GuideInfo;

    private List<config_guide_item> GuideRoots;

    public Action<config_guide_item> updateViewEvent;

    public void InitGuide()
    {

        if (PlayerModel.CLEAR_ALL)
        {
            PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.GUIDE);
            guidingItem = null;
            shield = true;
        }

        string GuideInfoStr = PlayerPrefsUtil.GetString(PlayerPrefsUtil.GUIDE);
        GuideInfo = JsonMapper.ToObject<GuideInfo>(GuideInfoStr);
        if (GuideInfo == null)
        {
            GuideInfo = new GuideInfo();
        }
        if (GuideInfo.completeRoots == null)
        {
            GuideInfo.completeRoots = new List<int>();
        }
        if (GuideInfo.completeKeys == null)
        {
            GuideInfo.completeKeys = new List<int>();
        }

        GuideRoots = new List<config_guide_item>();
        List<config_guide_item> configdata = ResModel.Instance.config_guide.data;
        for (int i = 0; i < configdata.Count; i++)
        {
            config_guide_item item = (config_guide_item)configdata[i];

            if (item.id == item.root_id)
            {
                GuideRoots.Add(item);
                if (PlayerModel.CLEAR_ALL)
                {
                    item.SetCompleteTemp(false);
                }
            }
        }

        //uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        
    }

    public void CheckGuide()
    {
        int startId = 0;
        if (shield)
        {
            return;
        }

        //Debug.Log("guidingItem == null " + (guidingItem == null));
        if (guidingItem == null)
        {

            if (GuideInfo.completeKeys.Count > 0)//todo 需要断线检查
            {
                int completeKey = GuideInfo.completeKeys[0];
                config_guide_item completeItem = (config_guide_item)ResModel.Instance.config_guide.GetItem(completeKey);
                //Debug.Log("out line continue");
                StartGuide(completeItem);
                return;
            }

            for (int i = 0; i < GuideRoots.Count; i++)
            {
                config_guide_item GuideRoot = GuideRoots[i];

                bool isComplete = CheckRootComplete(GuideRoot);

                if (GuideRoot.GetCompleteTemp() == false && isComplete == false && GuideRoot.id > startId)
                {
                    bool needGuide = ConditionCheck(GuideRoot);
                    if (needGuide)
                    {
                        //Debug.Log("find new Guide");
                        StartGuide(GuideRoot);
                        return;
                    }
                }
            }
            //Debug.Log("no needGuide");
        }
        else
        {
            GuideView();
        }
    }

    private void StartGuide(config_guide_item item)
    {
        //Debug.Log("StartGuide " + item.id);

        guidingItem = item;

        GuideView();
    }

    private bool CheckRootComplete(config_guide_item item)
    {
        if(shield)
        {
            return true;
        }

        for (int i = 0; i < GuideInfo.completeRoots.Count; i++)
        {
            if (GuideInfo.completeRoots[i] == item.root_id)
            {
                return true;
            }
        }

        return false;
    }

    private bool ConditionCheck(config_guide_item item)
    {
        List<TIVInfo> conditions = item.GetConditions();

        bool isReach = true;
        for (int i = 0; i < conditions.Count;i++ )
        {
            TIVInfo condition = conditions[i];
            switch ((int)condition.id)
            {
                case (int)GuideIDType.crt_star_num:

                    int gameValue = MapModel.Instance.starInfo.crtStar;

                    bool conditionCompare = ConditionCompare(condition,gameValue);

                    if (conditionCompare == false)
                    {
                        return false;
                    }

                    break;
                case (int)GuideIDType.Guide_complete:

                    if (GuideInfo.completeRoots.IndexOf((int)condition.value) == -1)
                    {
                        return false;
                    }

                    break;
			    case (int)GuideIDType.pass_map:

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
                case (int)GuideIDType.play_map:

				if (BattleModel.Instance.play_mapId != (int)condition.value)
                    {
                        return false;
                    }

                break;
                case (int)GuideIDType.ready_map:

                if (BattleModel.Instance.ready_map != (int)condition.value)
                {
                    return false;
                }

                break;

                case (int)GuideIDType.lose_map:

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
            case (int)GuideConditionType.equal:

			return (int)condition.value == gameValue;

            case (int)GuideConditionType.more_then:

			return (int)condition.value > gameValue;

            case (int)GuideConditionType.less_then:

			return (int)condition.value < gameValue;
        }
        return false;
    }

    private void GuideView()
    {
        if (guidingItem.Guide_type == (int)GuideType.tip)
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
        config_guide_item nextItem = FindNextItem(guidingItem);

        if (nextItem == null)
        {
            if (!GuideInfo.completeRoots.Contains(guidingItem.root_id))
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

                        if (condition.id == (int)GuideIDType.play_map)
                        {
                            guidingItem.SetCompleteTemp(true);
                            return;
                        }
                    }
                }

                GuideInfo.completeRoots.Add(guidingItem.root_id);
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
            if (!GuideInfo.completeRoots.Contains(guidingItem.root_id))
            {
                GuideInfo.completeRoots.Add(guidingItem.root_id);
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
            CheckGuide();
        }
        else
        {
			if(guidingItem.wait_time > 0)
			{
				LeanTween.delayedCall(guidingItem.wait_time/1000.00f,CheckGuide);
			}else
			{
				CheckGuide();
			}
        }
    }

    private config_guide_item FindNextItem(config_guide_item item)
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
            config_guide_item nextItem = (config_guide_item)ResModel.Instance.config_guide.GetItem(item.next_id);
            return nextItem;
        }
    }

    private void Save()
    {
        string GuideInfoStr = JsonMapper.ToJson(GuideInfo);
        PlayerPrefsUtil.SetString(PlayerPrefsUtil.GUIDE, GuideInfoStr);
    }

}