
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FuncCheckDead
{
	public static List<int> checkList = new List<int> { 1, 2, 3, 4, 5, 6 };

	//是否僵局
	public static bool IsDead()
	{
		List<List<CellInfo>> allCells = CellModel.Instance.allCells;
		List<List<CellInfo>> tempAllCells = new List<List<CellInfo>>();

		int i;
		for (i = 0; i < BattleModel.Instance.crtBattle.ShowHeight(); i++)
		{
			List<CellInfo> xCells = allCells[i];
			tempAllCells.Add(new List<CellInfo>());
			for (int j = 0; j < xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];

				if (cellInfo.isBlank == false && cellInfo.isMonsterHold == false && cellInfo.config.line_type > 0)
				{
					CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(cellInfo.posY,cellInfo.posX);
					if (coverInfo.CanLine())
					{
						tempAllCells[i].Add(cellInfo.Copy());
					}
					else
					{
						tempAllCells[i].Add(null);
					}
				}
				else
				{
					tempAllCells[i].Add(null);
				}

				if (cellInfo.isBlank == false && cellInfo.isMonsterHold == false && cellInfo.config.cell_type == (int)CellType.radiate)
				{
					if (!RadiateCheck (allCells, cellInfo)) 
					{
						return false;
					}
				}
			}
		}
		return RecursiveCheck(tempAllCells);
	}

	private static bool RadiateCheck(List<List<CellInfo>> allCells, CellInfo radiateCell)
	{
		CellInfo checkCenterInfo = radiateCell;
		//一轮查找
		List<CellInfo> firstRingCells = new List<CellInfo>();
		int i;
		for (i = 0; i < checkList.Count; i++)
		{
			Vector2 indexPos;

			indexPos = FightConst.GetHoleByLevel(checkList[i], checkCenterInfo.IsHump());

			Vector2 offsetPos = new Vector2(checkCenterInfo.posX + indexPos.x, checkCenterInfo.posY - indexPos.y);

			CellInfo checkCell = GetCellByPos(allCells, (int)offsetPos.x, (int)offsetPos.y);

			if (checkCell != null && checkCell.isBlank == false && checkCell.isMonsterHold == false && 
				(checkCell.config.line_type > 0 || checkCell.config.cell_type == (int)CellType.radiate ))
			{
				CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(checkCell.posY,checkCell.posX);
				if (coverInfo.CanLine())
				{
					if (WallModel.Instance.CanLine(checkCenterInfo, checkCell))
					{
						firstRingCells.Add(checkCell);
					}
				}
			}
			if(firstRingCells.Count > 1)
			{
				for (int j = 0; j < (firstRingCells.Count - 1); j++) {
					CellInfo checkA = firstRingCells [j];
					for (int n = (j + 1); n < firstRingCells.Count; n++) {
						CellInfo checkB = firstRingCells [n];
						if(checkA.IsNeighbor(checkB))
						{
							if (WallModel.Instance.CanLine(checkA, checkB))
							{
								return false;
							}
						}
					}
				}
			}
		}

		if(firstRingCells.Count == 0)
		{
			return true;
		}else
		{
			for (int b = 0; b < firstRingCells.Count; b++) {
				CellInfo checkCenterInfo2 = firstRingCells[b];
				//二轮查找
				List<CellInfo> secondRingCells = new List<CellInfo>();
				for (i = 0; i < checkList.Count; i++)
				{
					Vector2 indexPos = FightConst.GetHoleByLevel(checkList[i], checkCenterInfo2.IsHump());

					Vector2 offsetPos = new Vector2(checkCenterInfo2.posX + indexPos.x, checkCenterInfo2.posY - indexPos.y);

					CellInfo checkCell = GetCellByPos(allCells, (int)offsetPos.x, (int)offsetPos.y);

					if (checkCell != null && checkCell.isBlank == false && checkCell.isMonsterHold == false && 
						checkCenterInfo != checkCell && checkCell.config.line_type == checkCenterInfo.config.line_type)
					{
						CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(checkCell.posY,checkCell.posX);
						if (coverInfo.CanLine())
						{
							if (WallModel.Instance.CanLine(checkCenterInfo2, checkCell))
							{
								secondRingCells.Add(checkCell);
							}
						}
					}
					if (secondRingCells.Count > 1) {
						return false;
					}
				}
			}

			return true;
		}
	}

	private static bool RecursiveCheck(List<List<CellInfo>> tempAllCells)
	{
		CellInfo checkCenterInfo = null;
		int i;
		for (i = 0; i < tempAllCells.Count; i++)
		{
			List<CellInfo> xCells = tempAllCells[i];
			for (int j = 0; j < xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];
				if (cellInfo != null && checkCenterInfo == null)
				{
					checkCenterInfo = cellInfo;
					ClearCellByPos(tempAllCells,checkCenterInfo.posX,checkCenterInfo.posY);
					break;
				}
			}
		}

		if (checkCenterInfo == null)
		{
			return true;
		}
		else
		{
			//一轮查找
			List<CellInfo> firstRingCells = new List<CellInfo>();
			for (i = 0; i < checkList.Count; i++)
			{
				Vector2 indexPos;

				indexPos = FightConst.GetHoleByLevel(checkList[i], checkCenterInfo.IsHump());

				Vector2 offsetPos = new Vector2(checkCenterInfo.posX + indexPos.x, checkCenterInfo.posY - indexPos.y);

				CellInfo checkCell = GetCellByPos(tempAllCells, (int)offsetPos.x, (int)offsetPos.y);

				if (checkCell != null && checkCell.config.line_type == checkCenterInfo.config.line_type)
				{
					if (WallModel.Instance.CanLine(checkCenterInfo, checkCell))
					{
						ClearCellByPos(tempAllCells, checkCell.posX, checkCell.posY);
						firstRingCells.Add(checkCell);
					}
				}
				if(firstRingCells.Count > 1)
				{
					return false;
				}
			}

			if(firstRingCells.Count == 0)
			{
				return RecursiveCheck(tempAllCells);
			}else
			{
				checkCenterInfo = firstRingCells[0];
				//二轮查找
				for (i = 0; i < checkList.Count; i++)
				{
					Vector2 indexPos = FightConst.GetHoleByLevel(checkList[i], checkCenterInfo.IsHump());

					Vector2 offsetPos = new Vector2(checkCenterInfo.posX + indexPos.x, checkCenterInfo.posY - indexPos.y);

					CellInfo checkCell = GetCellByPos(tempAllCells, (int)offsetPos.x, (int)offsetPos.y);

					if (checkCell != null && checkCell.config.line_type == checkCenterInfo.config.line_type)
					{
						if (WallModel.Instance.CanLine(checkCenterInfo, checkCell))
						{
							return false;
						}
					}
				}
				return RecursiveCheck(tempAllCells);
			}
		}
	}

	private static CellInfo GetCellByPos(List<List<CellInfo>> tempAllCells, int posX, int posY)
	{
		if (posX < BattleModel.Instance.crtBattle.battle_width && posX >= 0 && posY < BattleModel.Instance.crtBattle.ShowHeight() && posY >= 0)
		{
			return tempAllCells[posY][posX];
		}
		return null;
	}

	private static void ClearCellByPos(List<List<CellInfo>> tempAllCells, int posX, int posY)
	{
		if (posX < BattleModel.Instance.crtBattle.battle_width && posX >= 0 && posY < BattleModel.Instance.crtBattle.ShowHeight() && posY >= 0)
		{
			tempAllCells[posY][posX] = null;
		}
	}
}