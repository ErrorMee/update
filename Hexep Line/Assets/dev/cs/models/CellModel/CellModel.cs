using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellModel:Singleton<CellModel>
{
	//战场所有格子
	public List<List<CellInfo>> allCells = new List<List<CellInfo>> ();
	//已经连接的
	public List<CellInfo> lineCells = new List<CellInfo> ();
	//退出连接的
	public CellInfo rollbackCell = null;
	//动画记录
	public List<List<CellAnimInfo>> anims = new List<List<CellAnimInfo>> ();
    //移动动画记录
    public List<CellMoveInfo> moveAnims = new List<CellMoveInfo>();

	public void Destroy()
	{
		Clear ();
		allCells = new List<List<CellInfo>> ();
	}

	public void Clear()
	{
		for(int i = 0;i< allCells.Count;i++)
		{
			List<CellInfo> xCells = allCells[i];
			for(int j = 0; j<xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];
				cellInfo.isLink = false;
			}
		}

		lineCells = new List<CellInfo> ();
		rollbackCell = null;
        anims = new List<List<CellAnimInfo>>();
	}

	public void UndoLineCells()
	{
		int i;
		for(i =0;i<lineCells.Count;i++)
		{
			CellInfo tempInfo = lineCells[i];
			tempInfo.isLink = false;
			MonsterModel.Instance.UnAbsorb(tempInfo.config.id);
			UnAbsorb(tempInfo.config.id);
		}
		lineCells = new List<CellInfo> (); 
	}

    public void UpdateBlankList()
    {
        lineCells = new List<CellInfo>();
        int i;
        for (i = 0; i < allCells.Count; i++)
        {
            List<CellInfo> xCells = allCells[i];
            for (int j = 0; j < xCells.Count; j++)
            {
                CellInfo cellInfo = xCells[j];
                if (cellInfo.isBlank)
                {
                    lineCells.Add(cellInfo);
                }
            }
        }
    }

    public CellInfo AddItem(int configId, int posX, int posY)
    {
        CellInfo addCell = CreateItem(configId, posX, posY);
        RemoveFromLines(posX, posY);

        return addCell;
    }

    public void RemoveFromLines(int posX, int posY)
    {
        for (int j = 0; j < lineCells.Count; j++)
        {
            CellInfo blankCell = lineCells[j];
            if (blankCell.posX == posX && blankCell.posY == posY)
            {
                lineCells.RemoveAt(j);
            }
        }
    }

    private CellInfo CreateItem(int configId, int posX, int posY)
    {
        CellInfo addCell = new CellInfo();
        addCell.SetConfig(configId);
        addCell.posX = posX;
        addCell.posY = posY;
        allCells[addCell.posY][addCell.posX] = addCell;
        return addCell;
    }

	public CellInfo FillNewItem(CellInfo blankCell, int blankIndex,bool isDeductStep = false)
    {
		CellInfo addCell = CreateItem(FillModel.Instance.crtFillInfo.GetFillId(isDeductStep), blankCell.posX, blankCell.posY);
        if (addCell.isBlank == false && addCell.config.cell_type == (int)CellType.changer)
        {
            addCell.changer = addCell.config.icon;
            addCell.SetConfig(addCell.config.hide_id);
			addCell.originalConfigId = addCell.config.hide_id;
        }
        lineCells.RemoveAt(blankIndex);
        return addCell;
    }

	public void SwitchPos(CellInfo cellA,CellInfo cellB)
	{
		allCells [cellA.posY].RemoveAt (cellA.posX);
		allCells [cellA.posY].Insert (cellA.posX, cellB);
		allCells [cellB.posY].RemoveAt (cellB.posX);
		allCells [cellB.posY].Insert (cellB.posX, cellA);
	}

	public List<CellInfo> GetNeighbors(CellInfo centerCell)
	{
		List<CellInfo> neighbors = new List<CellInfo> ();
		neighbors.Add (GetCellByPos(centerCell.posX - 1,centerCell.posY));
		neighbors.Add (GetCellByPos(centerCell.posX + 1,centerCell.posY));
		neighbors.Add (GetCellByPos(centerCell.posX,centerCell.posY + 1));
		neighbors.Add (GetCellByPos(centerCell.posX,centerCell.posY - 1));
        if (centerCell.IsHump())
		{
			neighbors.Add (GetCellByPos(centerCell.posX - 1,centerCell.posY - 1));
			neighbors.Add (GetCellByPos(centerCell.posX + 1,centerCell.posY - 1));
		}else{
			neighbors.Add (GetCellByPos(centerCell.posX - 1,centerCell.posY + 1));
			neighbors.Add (GetCellByPos(centerCell.posX + 1,centerCell.posY + 1));
		}
		return neighbors;
	}

    public CellInfo GetDirCell(CellInfo centerCell, CellDirType dirType)
    {
        switch (dirType)
        {
            case CellDirType.down:
                return GetCellByDir(centerCell, 0, 1);
            case CellDirType.right_up:
                return GetCellByDir(centerCell, 1, -1);
            case CellDirType.right_down:
                return GetCellByDir(centerCell, 1, 1);
            case CellDirType.left_up:
                return GetCellByDir(centerCell, -1, -1);
            case CellDirType.left_down:
                return GetCellByDir(centerCell, -1, 1);
            default:
                return GetCellByDir(centerCell, 0, -1);
        }
    }

    private CellInfo GetCellByDir(CellInfo centerCell, int dirX, int dirY)
    {
        bool isHump = centerCell.IsHump();
        CellInfo findCell;
        if (dirX == 0)
        {
            findCell = GetCellByPos(centerCell.posX + dirX, centerCell.posY + dirY);
        }
        else
        {
            int humpFlag = isHump ? 1 : -1;
            findCell = GetCellByPos(centerCell.posX + dirX, centerCell.posY + (dirY - humpFlag) / 2);
        }
        return findCell;
    }

    public List<CellInfo> GetDirCells(CellInfo centerCell, CellDirType dirType,int count = 0)
    {
        List<CellInfo> dirCells = new List<CellInfo>();
        switch(dirType)
        {
            case CellDirType.down:
			RecursionGetDirCells(dirCells, centerCell, 0, 1,count);
                break;
            case CellDirType.right_up:
			RecursionGetDirCells(dirCells, centerCell, 1, -1,count);
                break;
            case CellDirType.right_down:
			RecursionGetDirCells(dirCells, centerCell, 1, 1,count);
                break;
            case CellDirType.left_up:
			RecursionGetDirCells(dirCells, centerCell, -1, -1,count);
                break;
            case CellDirType.left_down:
			RecursionGetDirCells(dirCells, centerCell, -1, 1,count);
                break;
            default:
			RecursionGetDirCells(dirCells, centerCell, 0, -1,count);
                break;
        }

        return dirCells;
    }

	private void RecursionGetDirCells(List<CellInfo> dirCells, CellInfo centerCell, int dirX, int dirY,int count = 0)
    {
        bool isHump = centerCell.IsHump();
        CellInfo findCell;
        if (dirX == 0)
        {
            findCell = GetCellByPos(centerCell.posX + dirX, centerCell.posY + dirY);
        }
        else
        {
            int humpFlag = isHump ? 1 : -1;
            findCell = GetCellByPos(centerCell.posX + dirX, centerCell.posY + (dirY - humpFlag) / 2);
        }
        
        if (findCell != null)
        {
            dirCells.Add(findCell);
			if(count > 0)
			{
				if(dirCells.Count >= count)
				{
					return;
				}
			}
			RecursionGetDirCells(dirCells, findCell, dirX, dirY,count);
        }
    }

    public void AddAnim(CellInfo cellInfo, CellAnimType animationType = CellAnimType.move)
	{
        List<CellAnimInfo> stepAnims = anims[anims.Count - 1];
		CellAnimInfo animInfo = new CellAnimInfo();
        animInfo.startFrame = anims.Count;
        animInfo.runId = cellInfo.runId;
        animInfo.toX = cellInfo.posX;
        animInfo.toY = cellInfo.posY;
        
        animInfo.toInfo = cellInfo.Copy();
        animInfo.animationType = animationType;
        stepAnims.Add(animInfo);
	}

    public void AddMoveAnim(CellInfo cellInfo,bool isSlider)
    {
        int i;
        for (i = 0; i < moveAnims.Count;i++ )
        {
            CellMoveInfo cellMoveInfo = moveAnims[i];
            if (cellMoveInfo.cellInfo.runId == cellInfo.runId)
            {
                cellMoveInfo.AddPath(cellInfo.posX, cellInfo.posY, isSlider);
                return;
            }
        }
        CellMoveInfo addMoveInfo = new CellMoveInfo();
        addMoveInfo.cellInfo = cellInfo;
        addMoveInfo.AddPath(cellInfo.posX, cellInfo.posY, isSlider);
        moveAnims.Add(addMoveInfo);
    }

	public CellInfo GetCellByPos(int posX,int posY)
	{
        if (posX < BattleModel.Instance.crtBattle.battle_width && posX >= 0 && posY < BattleModel.Instance.crtBattle.battle_height && posY >= 0)
		{
			return allCells [posY] [posX];
		}
		return null;
	}

    public List<CellInfo> Timing()
    {
        List<CellInfo> timerCells = new List<CellInfo>();
        for (int i = 0; i < allCells.Count; i++)
        {
            List<CellInfo> xCells = allCells[i];
            for (int j = 0; j < xCells.Count; j++)
            {
                CellInfo cellInfo = xCells[j];
                
                if (cellInfo.isBlank)
                {
                    cellInfo.timer = -1;
                }
                else if (cellInfo.timer > 0)
                {
                    cellInfo.timer--;
                    timerCells.Add(cellInfo);

                    if (cellInfo.timer == 0)
                    {
                        cellInfo.isBlank = true;
                        cellInfo.isLink = true;
                        lineCells.Add(cellInfo);
                    }
                }
            }
        }
        return timerCells;
    }

	public void Absorb(int id)
	{
		for(int i = 0;i< allCells.Count;i++)
		{
			List<CellInfo> xCells = allCells[i];
			for(int j = 0; j<xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];
				cellInfo.Absorb(id);
			}
		}
	}

	public void UnAbsorb(int id)
	{
		for(int i = 0;i< allCells.Count;i++)
		{
			List<CellInfo> xCells = allCells[i];
			for(int j = 0; j<xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];
				cellInfo.UnAbsorb(id);
			}
		}
	}
}
