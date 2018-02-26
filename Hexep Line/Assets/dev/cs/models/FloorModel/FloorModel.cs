using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorModel : Singleton<FloorModel>
{
	public List<List<FloorInfo>> allFloors = new List<List<FloorInfo>>();

	//动画记录
	public List<List<FloorAnimInfo>> anims = new List<List<FloorAnimInfo>> ();

	public FloorModel ()
	{
	}

	public void Destroy()
	{
		Clear ();
		allFloors = new List<List<FloorInfo>> ();
	}
	
	public void Clear()
	{
		anims = new List<List<FloorAnimInfo>> ();
	}
	
	public FloorInfo GetFloorByRunId(int runId)
	{
		for(int i = 0;i<allFloors.Count;i++)
		{
			for(int j = 0;j<allFloors[i].Count;j++)
			{
				FloorInfo floorInfo = allFloors[i][j];
				
				if(floorInfo.runId == runId)
				{
					return floorInfo;
				}
			}
		}
		return null;
	}

	public FloorInfo GetFloorByPos(int posy ,int posx)
	{
        if (posx < BattleModel.Instance.crtBattle.battle_width && posx >= 0 && posy < BattleModel.Instance.crtBattle.battle_height && posy >= 0)
        {
            return allFloors[posy][posx];
        }
        return null;
	}

    public FloorInfo ClearFloor(FloorInfo floorInfo)
	{
		if(floorInfo.IsNull() == false)
		{
			floorInfo.SetConfig(0);
		}
		return floorInfo;
	}

	public void AddAnim(FloorInfo floorInfo,CellAnimType animationType = CellAnimType.clear)
	{
		List<FloorAnimInfo> stepMoves = anims[anims.Count - 1];
		FloorAnimInfo animInfo = new FloorAnimInfo();
		animInfo.startFrame = anims.Count;
		animInfo.floorInfo = floorInfo;
		stepMoves.Add(animInfo);
	}

	public List<Vector2> GetAllFloorPos()
	{
		List<Vector2> allFloorPos = new List<Vector2> ();
		for(int i = 0;i<allFloors.Count;i++)
		{
			for(int j = 0;j<allFloors[i].Count;j++)
			{
				FloorInfo floorInfo = allFloors[i][j];
				
				if(floorInfo.IsNull() == false)
				{
					allFloorPos.Add(new Vector2(floorInfo.posX,floorInfo.posY));
				}
			}
		}
		return allFloorPos;
	}

    public List<FloorInfo> GetNeighbors(FloorInfo centerFloor)
    {
        List<FloorInfo> neighbors = new List<FloorInfo>();
        neighbors.Add(GetFloorByPos(centerFloor.posY, centerFloor.posX - 1));
        neighbors.Add(GetFloorByPos(centerFloor.posY, centerFloor.posX + 1));
        neighbors.Add(GetFloorByPos(centerFloor.posY + 1, centerFloor.posX));
        neighbors.Add(GetFloorByPos(centerFloor.posY - 1, centerFloor.posX));
        if (centerFloor.IsHump())
        {
            neighbors.Add(GetFloorByPos(centerFloor.posY - 1, centerFloor.posX - 1));
            neighbors.Add(GetFloorByPos(centerFloor.posY - 1, centerFloor.posX + 1));
        }
        else
        {
            neighbors.Add(GetFloorByPos(centerFloor.posY + 1, centerFloor.posX - 1));
            neighbors.Add(GetFloorByPos(centerFloor.posY + 1, centerFloor.posX + 1));
        }
        return neighbors;
    }


    public void Flow()
    {
        anims = new List<List<FloorAnimInfo>>();
        List<FloorAnimInfo> stepAnim = new List<FloorAnimInfo>();
        anims.Add(stepAnim);

        List<FloorInfo> allFlowFloor = new List<FloorInfo>();
        int i;
        for (i = 0; i < allFloors.Count; i++)
        {
            for (int j = 0; j < allFloors[i].Count; j++)
            {
                FloorInfo floorInfo = allFloors[i][j];

                if (floorInfo.IsNull() == false && floorInfo.config.move)
                {
					//CellInfo centerCell = CellModel.Instance.GetCellByPos(floorInfo.posX, floorInfo.posY);
					//if (centerCell.isBlank) {
					//	allFlowFloor.Add (floorInfo);
					//} else if(centerCell.config.cell_type != (int)CellType.terrain){
						allFlowFloor.Add(floorInfo);
					//}
                }
            }
        }

        for (i = 0; i < allFlowFloor.Count; i++)
        {
            FloorInfo floorInfo = allFlowFloor[i];
			CellInfo centerCell = CellModel.Instance.GetCellByPos(floorInfo.posX, floorInfo.posY);

            List<FloorInfo> neighborNulls = new List<FloorInfo>();

            List<FloorInfo> neighbors = GetNeighbors(floorInfo);
            for (int j = 0; j < neighbors.Count; j++)
            {
                FloorInfo neighbor = neighbors[j];
                if (neighbor != null)
                {
                    CellInfo neighborCell = CellModel.Instance.GetCellByPos(neighbor.posX, neighbor.posY);
                    if (neighbor.IsNull())
                    {
						WallInfo wallInfo = WallModel.Instance.GetWall (centerCell, neighborCell);

						if(wallInfo.IsNull())
						{
							if (neighborCell.isBlank) {
								neighborNulls.Add (neighbor);
							} else {

								if(neighborCell.config.cell_type == (int)CellType.five || neighborCell.config.cell_type == (int)CellType.changer)
								{
									neighborNulls.Add (neighbor);
								}
								if(neighborCell.config.cell_type == (int)CellType.terrain && neighborCell.config.life >= 0)
								{
									neighborNulls.Add (neighbor);
								}
							}
						}
                    }
                }
            }

            int nullCount = neighborNulls.Count;
            if (nullCount > 0)
            {
                FloorInfo selectNeighbor = neighborNulls[Random.Range(0, nullCount)];

                SwitchPos(floorInfo, selectNeighbor);
                floorInfo.SwitchPos(selectNeighbor);

                AddAnim(floorInfo, CellAnimType.move);
            }
        }

    }

    public void SwitchPos(FloorInfo cellA, FloorInfo cellB)
    {
        allFloors[cellA.posY].RemoveAt(cellA.posX);
        allFloors[cellA.posY].Insert(cellA.posX, cellB);
        allFloors[cellB.posY].RemoveAt(cellB.posX);
        allFloors[cellB.posY].Insert(cellB.posX, cellA);
    }

}


