
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_wealth_item : config_item_base, IComparable
{

	public int firstCount;

	public int limitCount;

    public string accs;
    public string price;

    override public void Clear()
    {
        base.Clear();
        accs = "";
        price = "";
    }

	public List<TIVInfo> GetPriceList()
	{
		List<TIVInfo> tasks = new List<TIVInfo>();
		
		string[] strs = price.Split(new char[] { ',' });
		
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
}