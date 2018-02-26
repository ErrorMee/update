
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_chapter_item : config_item_base, IComparable
{
    public string maps;
    public string map_pos;
	public string reward;

    override public void Clear()
    {
        base.Clear();
        maps = "";
        map_pos = "";
		reward = "";
    }

    public List<int> GetMapIds()
    {
        string[] mapsArr = maps.Split(',');
		if(mapsArr.Length < 2)
		{
			return new List<int>();
		}
        int startId = Convert.ToInt32(mapsArr[0]);
        int endId = Convert.ToInt32(mapsArr[1]);

        List<int> mapIds = new List<int>();

        for (int i = startId; i <= endId;i++ )
        {
            mapIds.Add(i);
        }
        return mapIds;
    }

	public List<TIVInfo> GetRewardList()
	{
		List<TIVInfo> tasks = new List<TIVInfo>();

		string[] strs = reward.Split(new char[] { ',' });

		for (int i = 0; i < strs.Length; i++)
		{
			string str = strs[i];

			string[] str2s = str.Split(new char[] { '|' });

			TIVInfo tiv = new TIVInfo();

			tiv.id = int.Parse(str2s[0]);
			tiv.value = float.Parse(str2s[1]);

			tasks.Add(tiv);
		}

		return tasks;
	}

    public bool ContainMap(int mapId)
    {
        List<int> mapIds = GetMapIds();

        return mapIds.Contains(mapId);
    }

    public string[] GetMapIndexs()
    {
        return map_pos.Split(',');
    }

    public int GetIndex()
    {
        return (Convert.ToInt32(desc) / 10);
    }

}