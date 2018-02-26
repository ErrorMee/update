using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class config_cell_item : config_item_base
{
    public int hide_id;

    public int cell_type;

    public int line_type;

	public int life;

    public bool move;

    public bool atk;

    public int rotate;
    

    override public void Clear()
    {
        base.Clear();
        hide_id = 0;
        cell_type = 0;
        icon = 0;
        life = -1;
        move = false;
        atk = false;
        rotate = 0;
    }

	public override config_item_base Copy()
	{
		config_cell_item item = new config_cell_item();
		item.id = id;
		item.icon = icon;
		item.name = name;
		item.desc = desc;

		item.hide_id = hide_id;
		item.cell_type = cell_type;
		item.line_type = line_type;
		item.life = life;
		item.move = move;
		item.atk = atk;
        item.rotate = rotate;
		return item;
	}
	
	public bool IsRoot()
	{
        return id == hide_id;
	}

}
