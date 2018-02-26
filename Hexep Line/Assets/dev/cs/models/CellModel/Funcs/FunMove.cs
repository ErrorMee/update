
using System.Collections.Generic;

public class FunMove
{
    //移动
	public static void Move(bool single = false,bool isDeductStep = false)
    {
        SetBlindBlank();
        CellModel.Instance.moveAnims = new List<CellMoveInfo>();
		SingleMove(single,isDeductStep);
		SkillModel.Instance.InitFightingEntitys();
    }

	private static void SingleMove(bool single = false,bool isDeductStep = false)
    {
        bool change = false;

        CellModel.Instance.lineCells.Sort();
        //fill
        int n = CellModel.Instance.lineCells.Count - 1;
        for (; n >= 0; n--)
        {
            CellInfo blankTopCell = CellModel.Instance.lineCells[n];
			bool isCoverOpen = CoverModel.Instance.IsOpen(blankTopCell.posX, blankTopCell.posY);
			if (blankTopCell.isMonsterHold == false && blankTopCell.posY == 0 && isCoverOpen)
            {
                WallInfo topWall = WallModel.Instance.GetWallByPos(blankTopCell.posY, blankTopCell.posX, (int)CellDirType.up);
                if (topWall.CanPass())
                {
					CellInfo addCell = CellModel.Instance.FillNewItem(blankTopCell, n,isDeductStep);
                    CellModel.Instance.AddMoveAnim(addCell,false);
                }
            }
        }

        int i;
        for (i = 0; i < CellModel.Instance.lineCells.Count; i++)
        {
            CellInfo blankCell = CellModel.Instance.lineCells[i];
			bool isCoverOpen = CoverModel.Instance.CanMoveIn(blankCell.posX, blankCell.posY);
            bool hasFall = false;
			if (blankCell.isMonsterHold == false && isCoverOpen)
            {
				if(blankCell.posY > 0 || (blankCell.posY == 0 && blankCell.IsHump() == false))
				{
					//fall
					WallInfo topWall = WallModel.Instance.GetWallByPos(blankCell.posY, blankCell.posX, (int)CellDirType.up);

					CellInfo topCell = CellModel.Instance.GetCellByPos(blankCell.posX, blankCell.posY - 1);
					if (topWall.CanPass())
					{
						if (topCell != null && topCell.CanMove())
						{
							CellModel.Instance.SwitchPos(blankCell, topCell);
							blankCell.SwitchPos(topCell);
							CellModel.Instance.AddMoveAnim(topCell, false);
							hasFall = true;
							change = true;
							break;
						}
					}
					//Slid
					if (hasFall == false)
					{
						if (topCell == null && blankCell.posY != 0)
						{
							continue;
						}
						if (topCell != null && topCell.isBlank)
						{
							//isCoverOpen = CoverModel.Instance.IsOpen(topCell.posX, topCell.posY);
							if (topCell.isBlindBlank == false && topCell.isMonsterHold == false)
							{
								continue;
							}
						}

						CellInfo slidCell = FindSlider(blankCell);
						if (slidCell != null && slidCell.CanMove())
						{
							int posn = (int)CellDirType.left_up;
							if (slidCell.posX > blankCell.posX)
							{
								posn = (int)CellDirType.right_up;
							}
							WallInfo slidWall = WallModel.Instance.GetWallByPos(blankCell.posY, blankCell.posX, posn);
							if (slidWall.CanPass())
							{
								CellModel.Instance.SwitchPos(slidCell, blankCell);
								slidCell.SwitchPos(blankCell);
								CellModel.Instance.AddMoveAnim(slidCell,true);
								change = true;
								break;
							}
						}
					}
				}
            }
        }
        if (change && single == false)
        {
			SingleMove(false,isDeductStep);
        }
    }

    private static CellInfo FindSlider(CellInfo blank)
    {
        CellInfo findCell = null;

		int humpOff = blank.IsHump () ? -1 : 0;
		int slidOff = CellInfo.SORT_FLAG;

		findCell = CellModel.Instance.GetCellByPos(blank.posX + slidOff, blank.posY + humpOff);
		WallInfo slidWall = WallModel.Instance.GetWallByPos(blank.posY, blank.posX, 1 + slidOff);
		if (findCell == null || !findCell.CanMove() || !slidWall.CanPass())
		{
			findCell = CellModel.Instance.GetCellByPos(blank.posX - slidOff, blank.posY + humpOff);
			return findCell;
		}
		return findCell;
    }

    private static void SetBlindBlank()
    {
        List<List<CellInfo>> allCells = CellModel.Instance.allCells;
        int i;
        for (i = 0; i < allCells.Count; i++)
        {
            List<CellInfo> xCells = allCells[i];

            List<CellInfo> copyCells = new List<CellInfo>();
            for (int j = 0; j < xCells.Count; j++)
            {
                copyCells.Add(xCells[j]);
            }
            copyCells.Sort();

            for (int j = copyCells.Count - 1; j >= 0; j--)
            {
                CellInfo cellInfo = copyCells[j];
                if (cellInfo.isBlank)
                {
					bool isCoverOpen = CoverModel.Instance.IsOpen(cellInfo.posX, cellInfo.posY);
					if(!isCoverOpen)
					{
						cellInfo.isBlindBlank = true;
					}else
					{
						if (HasBlindBarrier(cellInfo, CellDirType.up) && HasBlindBarrier(cellInfo, CellDirType.left_up) && HasBlindBarrier(cellInfo, CellDirType.right_up))
						{
							cellInfo.isBlindBlank = true;
						}
						else
						{
							cellInfo.isBlindBlank = false;
						}
					}
                }
            }
        }
    }

    private static bool HasBlindBarrier(CellInfo blankCell, CellDirType dir)
    {
        int cellX, cellY, wallX, wallY,wallN;

        bool isHump = blankCell.IsHump();

        switch (dir)
        {
            case CellDirType.left_up:
                cellX = blankCell.posX - 1;
                if (isHump)
                {
                    cellY = blankCell.posY - 1;
                }else
                {
                    cellY = blankCell.posY;
                }
                break;
            case CellDirType.right_up:
                cellX = blankCell.posX + 1;
                if (isHump)
                {
                    cellY = blankCell.posY - 1;
                }
                else
                {
                    cellY = blankCell.posY;
                }
                break;
            default:
                cellX = blankCell.posX;
                cellY = blankCell.posY - 1;
                break;
        }

        wallX = blankCell.posX;
        wallY = blankCell.posY;
        wallN = (int)dir;

        CellInfo cellInfo = CellModel.Instance.GetCellByPos(cellX,cellY);
        WallInfo wallInfo = WallModel.Instance.GetWallByPos(wallY,wallX,wallN);

        if (cellY < 0 && wallInfo.CanPass())
        {
            return false;
        }

        if (cellX < 0 || cellX >= BattleModel.Instance.crtBattle.battle_width)
        {
            return true;
        }

		if (cellInfo == null)
		{
			return true;
		}

        if (cellInfo.isBlindBlank)
        {
            return true;
        }

        if (cellInfo.isMonsterHold)
        {
            return true;
        }

		bool isCoverOpen = CoverModel.Instance.IsOpen(cellInfo.posX, cellInfo.posY);
		if (!isCoverOpen)
		{
			return true;
		}

        if (cellInfo.isBlank == false && !cellInfo.CanMove())
        {
            return true;
        }

        if (!wallInfo.CanPass())
        {
            return true;
        }

        return false;
    }
}