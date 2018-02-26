
using System;
using UnityEngine;
using System.Collections.Generic;

public class WallInfo:IComparable
{
	private static int LAST_RUN_ID = 0;
	public int runId = -1;
	public int configId = -1;
    public config_wall_item config;

	public int life = 0;

	public int posX;
	public int posY;
	public int posN;

	public WallInfo ()
	{
		runId = ++LAST_RUN_ID;
	}

    public WallInfo Copy()
    {
        WallInfo wallInfo = new WallInfo();
        wallInfo.runId = runId;
        wallInfo.configId = configId;
        //if (config != null)
        //{
        //    wallInfo.config = (config_wall_item)config.Copy();
        //}
        wallInfo.config = config;
        wallInfo.life = life;
        wallInfo.posX = posX;
        wallInfo.posY = posY;
        wallInfo.posN = posN;
        return wallInfo;
    }

	//用来排序
	public int CompareTo(object obj)
	{
		CellInfo target = obj as CellInfo;
		if (target == null) {
			throw new NotImplementedException();
		}
		
		int thisValue;
		if(this.posX%2 == 0)
		{
			thisValue = this.posX + this.posY*10000 + 500;
		}else
		{
			thisValue = this.posX + this.posY*10000;
		}
		
		int targetValue;
		if(target.posX%2 == 0)
		{
			targetValue = target.posX + target.posY*10000 + 500;
		}else
		{
			targetValue = target.posX + target.posY*10000;
		}
		
		return thisValue.CompareTo(targetValue);
	}

	public void SetConfig(int id)
	{
		if(id == 20000)
		{
			id = 0;
		}

		configId = id;
        config = (config_wall_item)GameMgr.resourceMgr.config_wall.GetItem(configId);
        if (config == null)
		{
			life = 0;
		}else
		{
            life = config.life;
		}
	}

	public bool CanLine()
	{
		if(IsNull())
		{
			return true;
		}
        return config.line;
	}

	public bool CanPass()
	{
		if(IsNull())
		{
			return true;
		}
		return false;
	}

	public bool CanWreck()
	{
		if(IsNull())
		{
            return false;
		}
        return (life > 0);
	}

	public void Wreck()
	{
		life --;
		if(life <= 0)
		{
            if (config.id != config.hide_id)
			{
                SetConfig(config.hide_id);
			}else
			{
				SetConfig(0);
			}
		}
	}

	public bool IsNull()
	{
		if(configId == 0)
		{
			return true;
		}
		return false;
	}
}