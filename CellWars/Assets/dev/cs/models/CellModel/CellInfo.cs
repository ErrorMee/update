
using System;
using UnityEngine;
using System.Collections.Generic;

public class CellInfo:IComparable
{
	private static int LAST_RUN_ID = 0;

    public static int start_x;

	public static int SORT_FLAG = 1;

	public int runId = -1;

	public int configId = -1;

    public int originalConfigId = 0;

    public config_cell_item config;

	public int posX = -1;
	public int posY = -1;

    public bool bombMark = false;

	public bool isLink = false;

	public bool isBlank = false;

    public bool isBlindBlank = false;

    public bool isMonsterHold = false;

    public int changer = 0;

    public int timer = -1;
    public int addCount = 0;

	public int special0;
	public int special1;
	public int rotate = -1;

	public CellInfo ()
	{
		runId = ++LAST_RUN_ID;
	}

	//用来排序
	public int CompareTo(object obj)
	{
		CellInfo target = obj as CellInfo;
		if (target == null) {
			throw new NotImplementedException();
		}

		int thisValue;
        if (this.IsHump())
		{
			thisValue = this.posX*SORT_FLAG - this.posY*200 + 100;
		}else
		{
			thisValue = this.posX*SORT_FLAG - this.posY*200;
		}

		int targetValue;
        if (target.IsHump())
		{
			targetValue = target.posX*SORT_FLAG - target.posY*200 + 100;
		}else
		{
			targetValue = target.posX*SORT_FLAG - target.posY*200;
		}

		return thisValue.CompareTo(targetValue);
	}

	//复制
	public CellInfo Copy()
	{
		CellInfo cellInfo = new CellInfo ();
		cellInfo.runId = runId;
		cellInfo.configId = configId;
        cellInfo.originalConfigId = originalConfigId;

		//if(config != null)
		//{
		//	cellInfo.config = (config_cell_item)config.Copy();
		//}
        cellInfo.config = config;
		cellInfo.posX = posX;
		cellInfo.posY = posY;
        cellInfo.bombMark = bombMark;
		cellInfo.isLink = isLink;
		cellInfo.isBlank = isBlank;
        cellInfo.isBlindBlank = isBlindBlank;
        cellInfo.isMonsterHold = isMonsterHold;
        cellInfo.changer = changer;
		return cellInfo;
	}

    public int GetRootId()
    {
		if(config == null)
		{
			return 0;
		}
        return config.hide_id;
    }

	//交换
	public void SwitchPos(CellInfo cellInfo)
	{
		CellInfo cellCopy = cellInfo.Copy ();
		cellInfo.posX = posX;
		cellInfo.posY = posY;
		posX = cellCopy.posX;
		posY = cellCopy.posY;
	}

	public bool SetConfig(int id)
	{
		special0 = 0;
		special1 = 0;
		if(id == 10000)
		{
			id = 0;
		}
        if (originalConfigId <= 0)
        {
            originalConfigId = id;
        }

        if (configId == id)
		{
			return false;
		}

        configId = id;
		config = (config_cell_item)GameMgr.resourceMgr.config_cell.GetItem(configId);

		if(changer > 0)
		{
			changer = configId + 15;
		}

        if (configId <= 0)
        {
            isBlank = true;
        }
        else
        {
            isBlank = false;
        }

        if (timer < 0 && config != null)
        {
            if(config.cell_type == (int)CellType.timer)
            {
                addCount = config.GetSpecialValue(0);
                timer = config.GetSpecialValue(1);
            }
        }

		if (config != null)
		{
			special0 = config.GetSpecialValue(0);
			special1 = config.GetSpecialValue(1);
		}

		return true;
	}

    //是否可连接 空白和非连接格子不可连接
    public bool CanLineTo(CellInfo toCell = null) 
    {
        if (toCell == null)
        {
            if (isBlank || isMonsterHold)
            {
                return false;
            }
            return config.line_type != 0;
        }
        else
        {
            if (toCell.isBlank)
            {
                return false;
            }
            if (toCell.config.line_type == -1 || toCell.config.line_type == config.line_type)
            {
                return true;
            }
            return false;
        }
    }

	//是否可移动 空白和非移动格子不可移动
	public bool CanMove()
	{
        if (isBlank == false && config.move && CoverModel.Instance.CanMove(posX, posY) && isMonsterHold == false)
		{
			return true;
		}
		return false;
	}

	public bool CanCrawl()
	{
		if(isMonsterHold == true)
		{
			return false;
		}

		if(isBlank == false && config.cell_type == (int)CellType.terrain)
		{
			return false;
		}
		return true;
	}

	//是否可击毁
	public bool CanWreck()
	{
		if(config == null)
		{
			return false;
		}
        if ((config.life == 1 || config.life == 2) && isBlank == false)
		{
			return true;
		}
		return false;
	}

	public void Wreck()
	{
		if(config.IsRoot())
		{
			SetConfig(0);
		}else
		{
            SetConfig(config.hide_id);
		}
	}

    //是否可炸毁
    public bool CanBomb()
    {
		if(config == null)
		{
			return false;
		}
        if ((config.life == 1 || config.life == 0) && isBlank == false)
        {
            return true;
        }
		if ((config.cell_type == (int)CellType.bomb || config.cell_type == (int)CellType.line_bomb_r 
		      || config.cell_type == (int)CellType.three_bomb_r) && isBlank == false)
        {
            return true;
        }
        return false;
    }

	public void Bomb()
	{
		if(config.IsRoot())
		{
			SetConfig(0);
		}else
		{
            SetConfig(0);
            //SetConfig(config.hide_id);
		}
	}

	//是否物理相邻
	public bool IsNeighbor(CellInfo cellInfo)
	{
		bool isNeighbor = false;
		if(Mathf.Abs(cellInfo.posX - posX)<=1 && Mathf.Abs(cellInfo.posY - posY)<=1)
		{
			isNeighbor = true;
			if(IsHump())
			{
				if(cellInfo.posY > posY && cellInfo.posX != posX)
				{
					isNeighbor = false;
				}
			}else
			{
				if(cellInfo.posY < posY && cellInfo.posX != posX)
				{
					isNeighbor = false;
				}
			}
		}
		return isNeighbor;
	}

	public bool IsHump(int pos = -1)
	{
		if(pos == -1)
		{
			pos = posX;
		}
		return (pos - start_x) % 2 != 0;
	}

	public void Absorb(int id)
	{
		if (special0 != id || isBlank) 
		{
			return;
		}

		if (config.cell_type == (int)CellType.line_bomb_r)
		{
			rotate += 1;
		}

        if (config.cell_type == (int)CellType.three_bomb_r)
        {
            rotate += 1;
        }
    }

	public void UnAbsorb(int id)
	{
		if (special0 != id || isBlank)
		{
			return;
		}

		if (config.cell_type == (int)CellType.line_bomb_r&& rotate >= 0)
		{
			rotate -= 1;
		}

        if (config.cell_type == (int)CellType.three_bomb_r && rotate >= 0)
        {
            rotate -= 1;
        }
    }
}