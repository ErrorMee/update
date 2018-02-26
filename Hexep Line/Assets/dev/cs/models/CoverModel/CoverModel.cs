using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoverModel : Singleton<CoverModel>
{
	public List<List<CoverInfo>> allCovers = new List<List<CoverInfo>>();
	
	//动画记录
	public List<List<CoverAnimInfo>> anims = new List<List<CoverAnimInfo>> ();
	
	public void Destroy()
	{
		Clear ();
		allCovers = new List<List<CoverInfo>> ();
	}
	
	public void Clear()
	{
		anims = new List<List<CoverAnimInfo>> ();
	}
	
	public CoverInfo GetCoverInfByRunId(int runId)
	{
		for(int i = 0;i<allCovers.Count;i++)
		{
			for(int j = 0;j<allCovers[i].Count;j++)
			{
				CoverInfo coverInfo = allCovers[i][j];
				
				if(coverInfo.runId == runId)
				{
					return coverInfo;
				}
			}
		}
		return null;
	}
	
	public bool CanLine(int posx,int posy)
	{
		CoverInfo cover = GetCoverByPos(posy,posx);
		if (cover == null)
		{
			return true;
		}
		return cover.CanLine();
	}

	public bool AbsorbLine(int posx,int posy)
	{
		CoverInfo cover = GetCoverByPos(posy,posx);
		if (cover == null)
		{
			return false;
		}
        return cover.CanAbsorbLine();
	}

    public CoverInfo ClearCover(CoverInfo cover)
	{
		if (cover != null)
		{
			cover.SetConfig(0);
		}
		return cover;
	}

	public bool CanMove(int posx,int posy)
	{
		CoverInfo cover = GetCoverByPos(posy,posx);
		if (cover == null)
		{
			return true;
		}
		return cover.CanMove();
	}

    public bool CanWreck(CoverInfo cover)
    {
        if (cover == null)
        {
            return false;
        }
        return cover.CanWreck();
    }

    public void Wreck(CoverInfo cover)
    {
        cover.Wreck();
    }

	public bool IsOpen(int posx,int posy)
	{
		CoverInfo cover = GetCoverByPos(posy,posx);
		if (cover == null)
		{
			return true;
		}
		return cover.IsOpen ();
	}

    public bool StopSkill(int posx, int posy)
    {
        CoverInfo cover = GetCoverByPos(posy, posx);
        if (cover == null)
        {
            return false;
        }
        return cover.StopSkill();
    }

	public bool CanMoveIn(int posx,int posy)
	{
		CoverInfo cover = GetCoverByPos(posy,posx);
		if (cover == null)
		{
			return true;
		}
		return cover.CanMoveIn ();
	}

	public CoverInfo GetCoverByPos(int posy ,int posx)
	{
		if (posx < BattleModel.Instance.crtBattle.battle_width && posx >= 0 && posy < BattleModel.Instance.crtBattle.battle_height && posy >= 0)
		{
			return allCovers [posy] [posx];
		}
		return null;
	}
	
	public void AddAnim(CoverInfo coverInfo,CellAnimType animationType = CellAnimType.clear)
	{
        List<CoverAnimInfo> stepAnims = anims[anims.Count - 1];
		CoverAnimInfo animInfo = new CoverAnimInfo();
        animInfo.animationType = animationType;
		animInfo.startFrame = anims.Count;
		animInfo.coverInfo = coverInfo;
        stepAnims.Add(animInfo);
	}

    public int RollCount()
    {
        int leftHeight = allCovers.Count;

        int viewCount = BattleModel.Instance.crtBattle.ShowHeight();

        if (leftHeight > viewCount)
        {
            int firstHeight = -1;

            for (int i = 0; i < allCovers.Count; i++)
            {
                if (firstHeight >= 0)
                {
                    break;
                }
                for (int j = 0; j < allCovers[i].Count; j++)
                {
                    CoverInfo coverInfo = allCovers[i][j];
                    if (coverInfo.IsNull() == false && coverInfo.config.life == 1 && coverInfo.config.line == false)
                    {
                        firstHeight = i;
                        break;
                    }

                    CellInfo cellInfo = CellModel.Instance.GetCellByPos(j, i);
                    if (cellInfo.isBlank == false && cellInfo.config.id == 10014)
                    {
                        firstHeight = i;
                        break;
                    }
                }
            }

            if (firstHeight == -1)
            {
                firstHeight = allCovers.Count;
            }

            if (firstHeight > BattleModel.Instance.crtBattle.ShowWidth() / 2)
            {
                int hideHeight = leftHeight - viewCount;

                if (hideHeight > 4)
                {
                    return 4;
                }
                else
                {
                    return hideHeight;
                }
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }


    public void Flow()
    {
        anims = new List<List<CoverAnimInfo>>();
        List<CoverAnimInfo> stepAnim = new List<CoverAnimInfo>();
        anims.Add(stepAnim);

        List<CoverInfo> allFlowCover = new List<CoverInfo>();
        int i;
        for (i = 0; i < allCovers.Count; i++)
        {
            for (int j = 0; j < allCovers[i].Count; j++)
            {
                CoverInfo coverInfo = allCovers[i][j];

                if (coverInfo.IsNull() == false && coverInfo.config.move)
                {
                    allFlowCover.Add(coverInfo);
                }
            }
        }

        for (i = 0; i < allFlowCover.Count; i++)
        {
            CoverInfo coverInfo = allFlowCover[i];

            List<CoverInfo> neighborNulls = new List<CoverInfo>();

            List<CoverInfo> neighbors = GetNeighbors(coverInfo);
            for (int j = 0; j < neighbors.Count; j++)
            {
                CoverInfo neighbor = neighbors[j];
                if (neighbor != null && neighbor.IsNull())
                {
                    CellInfo neighborCell = CellModel.Instance.GetCellByPos(neighbor.posX, neighbor.posY);
					if(neighborCell.isBlank)
					{
						MonsterInfo monsterInfo = MonsterModel.Instance.GetMonsterByPos(neighbor.posX, neighbor.posY);
						if(monsterInfo.IsNull())
						{
							neighborNulls.Add(neighbor);
						}
					}else
					{
						if (neighborCell.config.cell_type != (int)CellType.terrain)
						{
							neighborNulls.Add(neighbor);
						}
					}
                    
                }
            }

            int nullCount = neighborNulls.Count;
            if (nullCount > 0)
            {
                CoverInfo selectNeighbor = neighborNulls[Random.Range(0, nullCount)];

                SwitchPos(coverInfo, selectNeighbor);
                coverInfo.SwitchPos(selectNeighbor);

                AddAnim(coverInfo, CellAnimType.move);
            }
        }

    }

    public List<CoverInfo> GetNeighbors(CoverInfo centerCover)
    {
        List<CoverInfo> neighbors = new List<CoverInfo>();
        neighbors.Add(GetCoverByPos(centerCover.posY, centerCover.posX - 1));
        neighbors.Add(GetCoverByPos(centerCover.posY, centerCover.posX + 1));
        neighbors.Add(GetCoverByPos(centerCover.posY + 1, centerCover.posX));
        neighbors.Add(GetCoverByPos(centerCover.posY - 1, centerCover.posX));
        if (centerCover.IsHump())
        {
            neighbors.Add(GetCoverByPos(centerCover.posY - 1, centerCover.posX - 1));
            neighbors.Add(GetCoverByPos(centerCover.posY - 1, centerCover.posX + 1));
        }
        else
        {
            neighbors.Add(GetCoverByPos(centerCover.posY + 1, centerCover.posX - 1));
            neighbors.Add(GetCoverByPos(centerCover.posY + 1, centerCover.posX + 1));
        }
        return neighbors;
    }

    public void SwitchPos(CoverInfo cellA, CoverInfo cellB)
    {
        allCovers[cellA.posY].RemoveAt(cellA.posX);
        allCovers[cellA.posY].Insert(cellA.posX, cellB);
        allCovers[cellB.posY].RemoveAt(cellB.posX);
        allCovers[cellB.posY].Insert(cellB.posX, cellA);
    }

    public List<CoverInfo> Timing()
    {
        List<CoverInfo> timerCovers = new List<CoverInfo>();
        for (int i = 0; i < allCovers.Count; i++)
        {
            List<CoverInfo> xCovers = allCovers[i];
            for (int j = 0; j < xCovers.Count; j++)
            {
                CoverInfo coverInfo = xCovers[j];
                
                if (coverInfo.IsNull())
                {
                    coverInfo.timer = -1;
                }
                else if (coverInfo.timer > 0)
                {
                    coverInfo.timer--;
                    timerCovers.Add(coverInfo);

                    if (coverInfo.timer == 0)
                    {
                        List<CoverInfo> covers = CoverModel.Instance.GetNeighbors(coverInfo);
                        for (int n = 0; n < covers.Count;n++ )
                        {
                            CoverInfo cover = covers[n];

							if (cover != null)
							{
								if (cover.timer == -1)
								{
									CellInfo cellInfo = CellModel.Instance.GetCellByPos(cover.posX,cover.posY);
									if(cellInfo.isBlank == false && cellInfo.config.id == 10001)
									{
									}else{
										cover.SetConfig(coverInfo.config.GetSpecialValue(0));
									}
								}
							}

                        }
                        coverInfo.SetConfig(0);
                    }
                }
            }
        }
        return timerCovers;
    }
}
