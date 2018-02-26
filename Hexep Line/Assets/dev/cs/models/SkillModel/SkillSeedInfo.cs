using System;
using System.Collections.Generic;

public class SkillSeedInfo
{
    public static int LAST_RUN_ID = 0;

    public int runId;

	public bool fobid = false;

    public config_cell_item config_cell_item;

    public int seed_x;

    public int seed_y;

	public int dir = 0;

    public List<int> open_holes;

    public float progressTemp;
    public float progress;

    public SkillSeedInfo()
	{
		runId = ++LAST_RUN_ID;
	}

    public int GetHideCellId()
    {
        return config_cell_item.hide_id;
    }

    public int GetCellIcon()
    {
        return config_cell_item.icon;
    }
}