using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_guide_item :config_item_base, IComparable
{
    public string complete_tip;//
    public int root_id;
    public int next_id;
    public string condition;
    public string aims;

    public int Guide_type;

	public int wait_time;//

    private bool complete_temp = false;

	override public void Clear()
	{
		base.Clear();
		
	}

    public List<TIVInfo> GetConditions()
    {
        if (condition == null || condition == "")
        {
            return null;
        }
        string[] items = condition.Split(',');
        List<TIVInfo> tIVInfos = new List<TIVInfo>();
        for (int i = 0; i < items.Length;i++ )
        {
            string item = items[i];

            string[] tivs = item.Split('|');

            TIVInfo tIVInfo = new TIVInfo();

            tIVInfo.type = Convert.ToInt32(tivs[0]);

            
            if (tivs.Length > 1)
            {
                tIVInfo.id = Convert.ToInt32(tivs[1]);
            }

            if (tivs.Length > 2)
            {
                tIVInfo.value = float.Parse(tivs[2]);
            }

            tIVInfos.Add(tIVInfo);
        }

        return tIVInfos;
    }

    public List<string> GetAimsArr()
    {
        string[] items = aims.Split(',');

        List<string> aimsArr = new List<string>();

        if (items.Length > 1)
        {
            string parentAims = items[0];
            for (int i = 1; i < items.Length;i++ )
            {
                aimsArr.Add(parentAims + items[i]);
            }
        }

        return aimsArr;
    }

    public string GetAims(int index = 1)
    {
        string[] items = aims.Split(',');

        string aimsStr = "";

        if (items.Length > 1)
        {
            return items[0] + items[index];
        }

        return aimsStr;
    }

    public string FindNextAims(string targetName)
    {
        string aimsStr = "";
        bool findTarget = false;
        string[] items = aims.Split(',');
        for (int i = 1; i < items.Length; i++)
        {

            if (findTarget == false && items[i] == targetName)
            {
                findTarget = true;
                continue;
            }

            if (findTarget)
            {
                return items[0] + items[i];
            }
        }
        return aimsStr;
    }

    public int AimsIndex(string targetName)
    {
        string[] items = aims.Split(',');
        for (int i = 1; i < items.Length; i++)
        {
            if (items[i] == targetName)
            {
                return i - 1;
            }
        }
        return 0;
    }

    public void SetCompleteTemp(bool isComplete)
    {
        complete_temp = isComplete;
    }

    public bool GetCompleteTemp()
    {
        return complete_temp;
    }
}