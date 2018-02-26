
using System;
using UnityEngine;
using System.Collections.Generic;

public class CoverInfo:IComparable
{
	private static int LAST_RUN_ID = 0;

	public int runId = -1;

	public config_cover_item config;
	
	public int posX;
	public int posY;

	public CoverShowType show_type;
	public int rate;

    public int timer = -1;

	public CoverInfo ()
	{
		runId = ++LAST_RUN_ID;
	}

    public CoverInfo Copy()
    {
        CoverInfo coverInfo = new CoverInfo();
        coverInfo.runId = runId;
        //if (config != null)
        //{
        //    coverInfo.config = (config_cover_item)config.Copy();
        //}
        coverInfo.config = config;
        coverInfo.posX = posX;
        coverInfo.posY = posY;
        coverInfo.show_type = show_type;
        coverInfo.rate = rate;
        return coverInfo;
    }
	
	//用来排序
	public int CompareTo(object obj)
	{
		CoverInfo target = obj as CoverInfo;
		return runId.CompareTo(target.runId);
	}
	
	public void SetConfig(int id)
	{
		if(id == 30000)
		{
			id = 0;
		}
		config = (config_cover_item)ResModel.Instance.config_cover.GetItem(id);

		if(config != null)
		{
			if(config.show_rate == 0)
			{
				show_type = CoverShowType.aways;
			}else
			{
				if(config.hide_rate > 0)
				{
					show_type = CoverShowType.show;
					rate = config.show_rate;
                }
                else if (config.hide_rate < 0)
                {
                    show_type = CoverShowType.hide;

                    rate = (int)Math.Abs(config.hide_rate);
                }
                else
                {
                    show_type = CoverShowType.aways;
                }
			}
            if (timer < 0 )
            {
                if (config.GetSpecialValue(1) > 0)
                {
                    timer = config.GetSpecialValue(1);
                }
            }
		}
	}

	public bool CanLine()
	{
		if(IsNull() || show_type == CoverShowType.hide || config.line)
		{
            return true;
		}
		
		return false;
	}
	
	public bool CanMove()
	{
		if(IsNull() == false && show_type != CoverShowType.hide)
		{
			return false;
		}
		
		return true;
	}

    public bool CanAbsorbLine()
    {
        if (IsNull())
        {
            return false;
        }

        if (show_type == CoverShowType.hide)
        {
            return false;
        }

        return (config.life >= 0);
    }

    public bool CanWreck()
    {
        if (IsNull())
        {
            return false;
        }

        if (show_type == CoverShowType.hide)
        {
            return false;
        }

        return (config.life == 1);
    }

    public void Wreck()
    {
        if (config.hide_id != config.id)
        {
            SetConfig(config.hide_id);
        }
        else
        {
            SetConfig(0);
        }
    }

	public bool IsNull()
	{
		return config == null;
	}

	public bool IsOpen()
	{
        if (IsNull())
        {
            return true;
        }
        return show_type == CoverShowType.hide;
	}

    public bool StopSkill()
    {
        if (IsNull())
        {
            return false;
        }
        if (show_type == CoverShowType.show)
        {
            return true;
        }

        if (show_type == CoverShowType.aways)
        {
            if (!config.line)
            {
                return true;
            }
        }
        return false;
    }

	public bool CanMoveIn()
	{
		if (IsNull())
		{
			return true;
		}
		return show_type != CoverShowType.show;
	}

    public bool IsHump(int pos = -1)
    {
        if (pos == -1)
        {
            pos = posX;
        }
        return (pos - CellInfo.start_x) % 2 != 0;
    }

    public void SwitchPos(CoverInfo coverInfo)
    {
        CoverInfo coverCopy = coverInfo.Copy();
        coverInfo.posX = posX;
        coverInfo.posY = posY;
        posX = coverCopy.posX;
        posY = coverCopy.posY;
    }
}