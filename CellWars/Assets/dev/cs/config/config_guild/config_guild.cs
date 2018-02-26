using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_guild:config_base
{
	
	public List<config_guild_item> data;
	
	public config_guild_template template;
	
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
	
	public void DelItem (int id)
	{
		if(id > 0)
		{
			for (int i = 0; i < data.Count; i++)
			{
				config_item_base item = data[i];
				
				if (item.id == id)
				{
					data.RemoveAt(i);
					return;
				}
			}
		}
	}
	
	override public config_item_base GetItemAt(int index)
	{
		return data[index];
	}
	
	override public int GetDataCount()
	{
		return data.Count;
	}
}
