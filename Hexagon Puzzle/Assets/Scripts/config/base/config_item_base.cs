using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class config_item_base : IComparable
{
    public int id;
    public int icon;
    public string name;
    public string desc;
    public string special;
    public virtual void Clear()
    {
        id = 0;
		icon = 0;
        name = "";
        desc = "";
        special = "";
    }

	public virtual config_item_base Copy()
	{
		config_item_base item = new config_item_base();
		item.id = id;
		item.icon = icon;
		item.name = name;
		item.desc = desc;
        item.special = special;
		return item;
	}

    //用来排序
    virtual public int CompareTo(object obj)
    {
        config_item_base target = obj as config_item_base;
        return id.CompareTo(target.id);
    }

    public int GetSpecialValue(int index)
    {
		if(special == null || special == "")
		{
			return 0;
		}
        string[] strs = special.Split(new char[] { '|' });

        if (strs.Length > index)
        {
            return int.Parse(strs[index]);
        }
        return 0;
    }
}
