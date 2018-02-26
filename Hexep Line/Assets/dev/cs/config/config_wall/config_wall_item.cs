using UnityEngine;
using System.Collections;

public class config_wall_item : config_item_base
{
	public int hide_id;

    public int life;

    public bool line;

    override public void Clear()
    {
        base.Clear();
		hide_id = 0;
        life = 0;
        line = false;
    }

	public override config_item_base Copy()
	{
		config_wall_item item = new config_wall_item();
		item.id = id;
		item.icon = icon;
		item.name = name;
		item.desc = desc;
		
		item.hide_id = hide_id;
		item.life = life;
		item.line = line;

		return item;
	}
}
