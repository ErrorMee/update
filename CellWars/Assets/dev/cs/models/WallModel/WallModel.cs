using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallModel : Singleton<WallModel>
{
	public List<List<List<WallInfo>>> allWalls = new List<List<List<WallInfo>>>();

	//移动记录
    public List<List<WallAnimInfo>> anims = new List<List<WallAnimInfo>>();

	public void Destroy()
	{
		Clear ();
		allWalls = new List<List<List<WallInfo>>> ();
	}
	
	public void Clear()
	{
        anims = new List<List<WallAnimInfo>>();
	}

	public WallInfo GetWallInfByRunId(int runId)
	{
		for(int i = 0;i<allWalls.Count;i++)
		{
			for(int j = 0;j<allWalls[i].Count;j++)
			{
				for(int n = 0;n<allWalls[i][j].Count;n++)
				{
					WallInfo wallInfo = allWalls[i][j][n];

					if(wallInfo.runId == runId)
					{
						return wallInfo;
					}
				}
			}
		}
		return null;
	}

	public bool CanLine(CellInfo fromCell,CellInfo toCell)
	{
        WallInfo wall = GetWall(fromCell, toCell);
        if (wall == null)
        {
            return false;
        }
        return wall.CanLine();
	}

	public WallInfo GetWallByPos(int posy ,int posx,int posn)
	{
		if (posx < BattleModel.Instance.crtBattle.battle_width && posx >= 0 && posy < BattleModel.Instance.crtBattle.battle_height && posy >= 0)
		{
			return allWalls[posy][posx][posn];
		}
		return null;
	}

    public WallInfo GetWall(CellInfo fromCell, CellInfo toCell)
    {
        if (fromCell.posX == toCell.posX && fromCell.posY == toCell.posY)
        {//xiang tong
            return null;
        }
        if (Mathf.Abs(fromCell.posX - toCell.posX) > 1 || Mathf.Abs(fromCell.posY - toCell.posY) > 1)
        {// tai yuan
            return null;
        }
        CellInfo targetCell;
        int targetN = 1;
        if (fromCell.posX == toCell.posX)//up dowm
        {
            targetCell = fromCell.posY > toCell.posY ? fromCell : toCell;
        }
        else
        {
            bool isLeft = fromCell.posX > toCell.posX;
            bool isUp = fromCell.posY > toCell.posY;

            if (fromCell.posY == toCell.posY)
            {
                if (toCell.IsHump())
                {
                    isUp = true;
                }
            }
            targetN = (isLeft ? 1 : -1)*((isLeft ? 1 : -1) + (isUp ? -1 : 1));
            targetCell = isUp ? fromCell : toCell;
        }

		WallInfo wall = GetWallByPos(targetCell.posY,targetCell.posX,targetN);
        return wall;
    }

	public List<WallInfo> GetNeighborWalls(CellInfo cellInfo)
	{
		List<WallInfo> walls = new List<WallInfo> ();
		walls.Add(GetNeighborWallByDir(cellInfo,CellDirType.up));
        walls.Add(GetNeighborWallByDir(cellInfo, CellDirType.left_up));
        walls.Add(GetNeighborWallByDir(cellInfo, CellDirType.left_down));
		walls.Add(GetNeighborWallByDir(cellInfo,CellDirType.down));
		walls.Add(GetNeighborWallByDir(cellInfo,CellDirType.right_down));
        walls.Add(GetNeighborWallByDir(cellInfo, CellDirType.right_up));
		return walls;
	}

    public WallInfo GetGapWall(CellInfo cellInfo)
    {
        WallInfo gapWall = GetNeighborWallByDir(cellInfo, CellDirType.up);
		if (gapWall != null && gapWall.IsNull())
        {
            return gapWall;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.right_up);
		if (gapWall != null && gapWall.IsNull())
        {
            return gapWall;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.right_down);
		if (gapWall != null && gapWall.IsNull())
        {
            return gapWall;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.down);
		if (gapWall != null && gapWall.IsNull())
        {
            return gapWall;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.left_down);
		if (gapWall != null && gapWall.IsNull())
        {
            return gapWall;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.left_up);
		if (gapWall != null && gapWall.IsNull())
        {
            return gapWall;
        }
        
        return null;
    }

    public CellDirType GetGapWallDir(CellInfo cellInfo)
    {
        WallInfo gapWall = GetNeighborWallByDir(cellInfo, CellDirType.up);
		if (gapWall != null && gapWall.IsNull())
        {
            return CellDirType.up;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.right_up);
		if (gapWall != null && gapWall.IsNull())
        {
            return CellDirType.right_up;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.right_down);
		if (gapWall != null && gapWall.IsNull())
        {
            return CellDirType.right_down;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.down);
		if (gapWall != null && gapWall.IsNull())
        {
            return CellDirType.down;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.left_down);
		if (gapWall != null && gapWall.IsNull())
        {
            return CellDirType.left_down;
        }
        gapWall = GetNeighborWallByDir(cellInfo, CellDirType.left_up);
		if (gapWall != null && gapWall.IsNull())
        {
            return CellDirType.left_up;
        }
        return CellDirType.no;
    }

	public WallInfo GetNeighborWallByDir(CellInfo cellInfo,CellDirType dirType)
	{
		bool isHump = cellInfo.IsHump();

		switch(dirType)
		{
		case CellDirType.left_up:
		case CellDirType.up:
		case CellDirType.right_up:
			return GetWallByPos(cellInfo.posY,cellInfo.posX,(int)dirType);
		case CellDirType.left_down:
			if(cellInfo.posX > 0)
			{
				if(isHump)
				{
					return GetWallByPos(cellInfo.posY,cellInfo.posX - 1,2);
				}else
				{
                    if (cellInfo.posY < (BattleModel.Instance.crtBattle.battle_height - 1))
					{
						return GetWallByPos(cellInfo.posY + 1,cellInfo.posX - 1,2);
					}
				}
			}
			break;
		case CellDirType.down:
            if (cellInfo.posY < (BattleModel.Instance.crtBattle.battle_height - 1))
			{
				return GetWallByPos(cellInfo.posY + 1,cellInfo.posX,1);
			}
			break;
		case CellDirType.right_down:
            if (cellInfo.posX < (BattleModel.Instance.crtBattle.battle_width - 1))
			{
				if(isHump)
				{
					return GetWallByPos(cellInfo.posY,cellInfo.posX + 1,0);
				}else
				{
                    if (cellInfo.posY < (BattleModel.Instance.crtBattle.battle_height - 1))
					{
						return GetWallByPos(cellInfo.posY + 1,cellInfo.posX + 1,0);
					}
				}
			}
			break;
		}
		return null;
	}

    public void AddAnim(WallInfo wallInfo, CellAnimType animationType = CellAnimType.move)
	{
        List<WallAnimInfo> stepMoves = anims[anims.Count - 1];
		WallAnimInfo moveInfo = new WallAnimInfo();
        moveInfo.startFrame = anims.Count;
		moveInfo.toInfo = wallInfo;
		stepMoves.Add(moveInfo);
	}

}
