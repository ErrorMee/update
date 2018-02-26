
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_skill_item : config_item_base, IComparable
{
    public int cellId;
    public int groupId;
    public int unlockId;
    public int type;
	public int dir;
    public string bottleABC;
    public string starABC;

    override public void Clear()
    {
        base.Clear();
        cellId = 0;
        groupId = 0;
        unlockId = 0;
		type = 0;
		dir = 0;
        bottleABC = "";
        starABC = "";
    }

    public List<float> GetBottleABC()
    {
        List<float> abc = new List<float>();

        string[] strs = bottleABC.Split(new char[] { '|' });

        for (int i = 0; i < strs.Length;i++ )
        {
            abc.Add(float.Parse(strs[i]));
        }
        return abc;
    }

    public List<float> GetStarABC()
    {
        List<float> abc = new List<float>();

        string[] strs = starABC.Split(new char[] { '|' });

        for (int i = 0; i < strs.Length; i++)
        {
            abc.Add(float.Parse(strs[i]));
        }
        return abc;
    }
}