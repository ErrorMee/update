
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_wealth : config_base
{

    public List<config_wealth_item> data;

    public config_wealth_template template;

    override public config_item_base GetItem(int id)
    {
        for (int i = 0; i < data.Count; i++)
        {
            config_item_base item = data[i];

            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }

    override public config_item_base GetItemAt(int index)
    {
        return data[index];
    }

    override public int GetDataCount()
    {
        return data.Count;
    }

	public config_wealth_item GetItemByType(int id)
	{
		for (int i = 0; i < data.Count; i++)
		{
			config_wealth_item item = data[i];
			
			if (item.id == id)
			{
				return item;
			}
		}
		return null;
	}
}
