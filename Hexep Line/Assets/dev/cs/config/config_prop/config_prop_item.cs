using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class config_prop_item : config_item_base
{
	public int times;//次数
    public string costABC;//花费公式

    override public void Clear()
    {
        base.Clear();
		times = 0;
        costABC = "";
    }

    public int GetCost(int count)
    {
        List<float> abc = new List<float>();

        string[] strs = costABC.Split(new char[] { '|' });

        for (int i = 0; i < strs.Length; i++)
        {
            abc.Add(float.Parse(strs[i]));
        }

        int costNum = (int)(abc[0] * Math.Pow(count, 2) + abc[1] * (count) + abc[2]);

        return costNum;
    }
}
