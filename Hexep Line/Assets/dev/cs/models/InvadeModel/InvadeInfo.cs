
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvadeInfo
{
	public int invadeId;

	public List<Vector2> sourcePos = new List<Vector2>();

	public bool blocked = false;

	public InvadeInfo ()
	{
	}

    public CellAnimInfo Invade()
	{
		if(blocked == true)
		{
			blocked = false;
			return null;
		}

        List<CellAnimInfo> waitList = new List<CellAnimInfo>();

		int i;
		for(i = 0;i< CellModel.Instance.allCells.Count;i++)
		{
			List<CellInfo> xCells = CellModel.Instance.allCells[i];
			for(int j = 0; j<xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];

                if (cellInfo.isBlank == false && (cellInfo.config.id == invadeId || cellInfo.config.hide_id == invadeId))
				{
					List<CellInfo> neighbors = CellModel.Instance.GetNeighbors(cellInfo);

					for(int n = 0;n<neighbors.Count;n++)
					{
						CellInfo neighbor = neighbors[n];

						if(neighbor != null && neighbor.isBlank == false && neighbor.posY >= 0 && CoverModel.Instance.AbsorbLine(neighbor.posX,neighbor.posY) == false)
						{
							if(neighbor.config.cell_type == (int)CellType.five && neighbor.isMonsterHold == false)
							{
                                WallInfo wall = WallModel.Instance.GetWall(cellInfo, neighbor);
								if (wall.IsNull())
								{
									//Debug.Log("find");
                                    CellAnimInfo cellAnim = new CellAnimInfo();
                                    cellAnim.fromInfo = cellInfo;
                                    cellAnim.toInfo = neighbor;
                                    waitList.Add(cellAnim);
								}
							}
						}
					}
				}
			}
		}

		if(waitList.Count > 0)
		{
			int rangeIndex = Random.Range (0, waitList.Count);
            CellAnimInfo cellAnim = waitList[rangeIndex];
            cellAnim.toInfo.changer = 0;
            cellAnim.toInfo.SetConfig(invadeId);
            return cellAnim;
		}
		return null;
	}

}