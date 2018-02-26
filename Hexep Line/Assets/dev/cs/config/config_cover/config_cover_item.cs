using UnityEngine;
using System.Collections;

public class config_cover_item : config_item_base
{
	public int hide_id;

    public int show_rate;
    public int hide_rate;

    public bool line;
    public int life;
    public bool move;

    override public void Clear()
    {
        base.Clear();
		hide_id = 0;
        show_rate = 0;
        hide_rate = 0;
        line = false;
        life = 0;
        move = false;
    }

	public override config_item_base Copy()
	{
		config_cover_item item = new config_cover_item();
		item.id = id;
		item.icon = icon;
		item.name = name;
		item.desc = desc;
		
		item.hide_id = hide_id;
		item.show_rate = show_rate;
		item.hide_rate = hide_rate;

		item.line = line;
		item.life = life;
		item.move = move;

		return item;
	}
}
