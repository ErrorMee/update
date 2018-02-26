using UnityEngine;
using System.Collections.Generic;


public class BallMove
{
    private static List<List<CellInfo>> allCells;
	private static List<CellInfo> blanckCells;
	private static List<CellInfo> moveCells;

    public static void Init(int width,int height)
    {
        allCells = new List<List<CellInfo>>();
        blanckCells = new List<CellInfo>();
		moveCells = new List<CellInfo>();
        for (int i = 0; i < height; i++)
        {
            List<CellInfo> xCells = new List<CellInfo>();
            allCells.Add(xCells);
            for (int j = 0; j < width; j++)
            {
                CellInfo cellInfo = new CellInfo();
                cellInfo.posX = j;
                cellInfo.posY = i;
                cellInfo.isBlank = true;
                xCells.Add(cellInfo);
                blanckCells.Add(cellInfo);
            }
        }
    }

    //移动
    public static List<CellMoveInfo> Move(List<int> newPos)
    {
        List<CellMoveInfo> moveAnims = new List<CellMoveInfo>();
        
        //fill
        int n = newPos.Count - 1;
        for (; n >= 0; n--)
        {
            int newIndex = newPos[n];
            CellInfo moveCell = blanckCells[newIndex];
			moveCell.isBlank = false;
            blanckCells.RemoveAt(newIndex);
			moveCells.Add(moveCell);
			AddMoveAnim(moveCell, moveAnims);
        }

        RecursionMove(moveAnims);

        for (n = 0; n < moveAnims.Count; n++)
        {
            CellMoveInfo moveAnim = moveAnims[n];
            moveAnim.CutTail();
        }

        return moveAnims;
    }

    private static void RecursionMove(List<CellMoveInfo> moveAnims)
    {
        bool change = false;
		int random = Random.Range(0, 2);
        int i;
		if(random > 0)
		{
			for (i = 0; i < moveCells.Count; i++)
			{
				CellInfo moveCell = moveCells[i];
				CellInfo blankCell = FindBlank(moveCell);
				if(blankCell != null && blankCell.isBlank)
				{
					allCells[blankCell.posY].RemoveAt(blankCell.posX);
					allCells[blankCell.posY].Insert(blankCell.posX, moveCell);
					allCells[moveCell.posY].RemoveAt(moveCell.posX);
					allCells[moveCell.posY].Insert(moveCell.posX, blankCell);

					blankCell.SwitchPos(moveCell);
					change = true;
				}
				AddMoveAnim(moveCell, moveAnims);
			}
		}else
		{
			for (i = moveCells.Count - 1; i >= 0; i--)
			{
				CellInfo moveCell = moveCells[i];
				CellInfo blankCell = FindBlank(moveCell);
				if(blankCell != null && blankCell.isBlank)
				{
					allCells[blankCell.posY].RemoveAt(blankCell.posX);
					allCells[blankCell.posY].Insert(blankCell.posX, moveCell);
					allCells[moveCell.posY].RemoveAt(moveCell.posX);
					allCells[moveCell.posY].Insert(moveCell.posX, blankCell);

					blankCell.SwitchPos(moveCell);
					change = true;
				}
				AddMoveAnim(moveCell, moveAnims);
			}
		}
        

        if (change)
        {
            RecursionMove(moveAnims);
        }
    }

	private static CellInfo FindBlank(CellInfo mover)
	{
		CellInfo findCell = null;

		int humpOff = mover.IsHump() ? 0 : 1;

		int random = Random.Range(0, 2);
		int slidOff = random > 0 ? 1 : -1;

		findCell = GetCellByPos(mover.posX + slidOff, mover.posY + humpOff);
		if (findCell == null || !findCell.isBlank)
		{
			findCell = GetCellByPos(mover.posX - slidOff, mover.posY + humpOff);
			return findCell;
		}
		return findCell;
	}

    private static void AddMoveAnim(CellInfo cellInfo, List<CellMoveInfo> moveAnims)
    {
        int i;
        for (i = 0; i < moveAnims.Count; i++)
        {
            CellMoveInfo cellMoveInfo = moveAnims[i];
            if (cellMoveInfo.cellInfo.runId == cellInfo.runId)
            {
				cellMoveInfo.AddFixPath(cellInfo.posX, cellInfo.posY);
                return;
            }
        }
        CellMoveInfo addMoveInfo = new CellMoveInfo();
        addMoveInfo.cellInfo = cellInfo;
		addMoveInfo.AddFixPath(cellInfo.posX, cellInfo.posY);
        moveAnims.Add(addMoveInfo);
    }

    private static CellInfo GetCellByPos(int posX, int posY)
    {
        if (posX < allCells[0].Count && posX >= 0 && posY < allCells.Count && posY >= 0)
        {
            return allCells[posY][posX];
        }
        return null;
    }
}