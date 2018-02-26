using System;
using System.Collections;
using System.Collections.Generic;

public class CollectInfo
{
	public Dictionary<int, int> dic = new Dictionary<int, int>();

	public int allCount = 0;

	public void AddCount(int id,int count = 1)
	{
        if(id <= 0)
        {
            return;
        }
		if (dic.ContainsKey(id))
		{
			dic[id] += count;
		}
		else
		{
			dic.Add(id, count);
		}
		allCount += count;
	}

	public int GetCount(int id)
	{
		if (dic.ContainsKey(id))
		{
			return dic[id];
		}
		else
		{
			return 0;
		}
	}
	
	public void Merger(CollectInfo collectInfo)
	{
		foreach (int key in collectInfo.dic.Keys)
		{
			AddCount(key,collectInfo.dic[key]);
		}
	}
}