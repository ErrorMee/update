
using System.Collections.Generic;

public class FuncEliminate
{
	//消除
	public static void Eliminate(bool exercise = false)
	{
		FloorModel.Instance.anims = new List<List<FloorAnimInfo>>();
		CellModel.Instance.anims = new List<List<CellAnimInfo>>();
		WallModel.Instance.anims = new List<List<WallAnimInfo>>();
		CoverModel.Instance.anims = new List<List<CoverAnimInfo>>();
		MonsterModel.Instance.anims = new List<List<MonsterAnimInfo>>();

		CollectModel.Instance.tempCollect = new CollectInfo ();
		int i;
		for (i = 0; i < CellModel.Instance.lineCells.Count; i++)
		{
			CellInfo cellInfo = CellModel.Instance.lineCells[i];

			List<FloorAnimInfo> floorAnims = new List<FloorAnimInfo>();
			FloorModel.Instance.anims.Add(floorAnims);

			List<CellAnimInfo> stepMoves = new List<CellAnimInfo>();
			CellModel.Instance.anims.Add(stepMoves);

			List<WallAnimInfo> stepWallMoves = new List<WallAnimInfo>();
			WallModel.Instance.anims.Add(stepWallMoves);

			List<CoverAnimInfo> coverAnims = new List<CoverAnimInfo>();
			CoverModel.Instance.anims.Add(coverAnims);

			List<MonsterAnimInfo> monsterAnims = new List<MonsterAnimInfo>();
			MonsterModel.Instance.anims.Add(monsterAnims);

			//解除盖子
			if (CoverModel.Instance.AbsorbLine(cellInfo.posX, cellInfo.posY))
			{
				CoverInfo clearCover = CoverModel.Instance.GetCoverByPos(cellInfo.posY, cellInfo.posX);
				if (exercise)
				{
					CollectModel.Instance.tempCollect.AddCount(clearCover.config.id, 1);
				}
				clearCover = CoverModel.Instance.ClearCover(clearCover);
				CoverModel.Instance.AddAnim(clearCover, CellAnimType.clear);
			}
			else
			{
				//消除连线
				if (cellInfo.bombMark == false && cellInfo.isBlank == false)
				{
					cellInfo.isBlank = true;

					if (exercise)
					{
						CollectModel.Instance.tempCollect.AddCount(cellInfo.configId, 1);
						CollectModel.Instance.scoreLen++;
					}
					else
					{
						MonsterModel.Instance.UpdateBan(cellInfo);
					}
					CellModel.Instance.AddAnim(cellInfo, CellAnimType.clear);
				}

				//消除地板
				FloorInfo floorInfo = FloorModel.Instance.GetFloorByPos(cellInfo.posY,cellInfo.posX);
				if(!floorInfo.IsNull())
				{
					if (exercise)
					{
						CollectModel.Instance.tempCollect.AddCount(floorInfo.config.id, 1);
					}

					floorInfo = FloorModel.Instance.ClearFloor(floorInfo);
					FloorModel.Instance.AddAnim(floorInfo);
				}

				//击毁
				List<CellInfo> neighbors = CellModel.Instance.GetNeighbors(cellInfo);
				for (int j = 0; j < neighbors.Count; j++)
				{
					CellInfo neighbor = neighbors[j];

					if (neighbor != null)
					{
						WallInfo wall = WallModel.Instance.GetWall(cellInfo, neighbor);
						if (wall.IsNull())
						{

							CoverInfo wreckCover = CoverModel.Instance.GetCoverByPos(neighbor.posY, neighbor.posX);
							bool coverCanWreck = CoverModel.Instance.CanWreck(wreckCover);

							if (coverCanWreck)
							{//击毁盖子
								if (exercise)
								{
									CollectModel.Instance.tempCollect.AddCount(wreckCover.config.id, 1);
								}
								CoverModel.Instance.Wreck(wreckCover);
								if (wreckCover.IsNull())
								{
									CoverModel.Instance.AddAnim(wreckCover, CellAnimType.clear);
								}
								else
								{
									CoverModel.Instance.AddAnim(wreckCover, CellAnimType.wreck);
								}
							}
							else
							{   //击毁邻居
								if (neighbor.CanWreck())
								{
									if (exercise)
									{
										CollectModel.Instance.tempCollect.AddCount(neighbor.configId, 1);
										if(neighbor.config.id == 10014)
										{
											CollectModel.Instance.scoreLen += 10;
										}

									}else
									{
										InvadeModel.Instance.InvadeBlocked(neighbor);
									}

									neighbor.Wreck();
									CellModel.Instance.AddAnim(neighbor, CellAnimType.wreck);
								}
								//击毁怪物
								MonsterInfo monster = MonsterModel.Instance.GetMonsterByPos(neighbor.posY, neighbor.posX);
								if (monster != null && monster.CanWreck())
								{
									if (exercise)
									{
										CollectModel.Instance.tempCollect.AddCount(monster.config.id, 1);
									}
									monster.Wreck(false);
									MonsterModel.Instance.AddAnim(monster, CellAnimType.wreck);
								}
							}
						}
					}
				}

				//击毁墙体
				List<WallInfo> neighborWalls = WallModel.Instance.GetNeighborWalls(cellInfo);
				for (int j = 0; j < neighborWalls.Count; j++)
				{
					WallInfo neighborWall = neighborWalls[j];
					if (neighborWall != null && neighborWall.CanWreck())
					{
						neighborWall.Wreck();
						if (exercise == false)
						{
							WallModel.Instance.AddAnim(neighborWall, CellAnimType.wreck);
						}
					}
				}
			}

			//炸弹
			List<CellInfo> bombCells = SkillModel.Instance.GetBombCells(cellInfo);
			for (int j = 0; j < bombCells.Count; j++)
			{
				CellInfo bombCell = bombCells[j];
				if (bombCell != null)
				{
					bombCell.bombMark = false;
                    
					//炸毁盖子
                    if (CoverModel.Instance.AbsorbLine(bombCell.posX, bombCell.posY))
                    {
                        CoverInfo clearCover = CoverModel.Instance.GetCoverByPos(bombCell.posY, bombCell.posX);
                        if (exercise)
                        {
                            CollectModel.Instance.tempCollect.AddCount(clearCover.config.id, 1);
                        }
                        clearCover = CoverModel.Instance.ClearCover(clearCover);
                        CoverModel.Instance.AddAnim(clearCover, CellAnimType.clear);
                        CellModel.Instance.AddAnim(bombCell, CellAnimType.nullbomb);
                    }
                    else {

                        if (!MonsterModel.Instance.StopSkill(bombCell.posX, bombCell.posY))
                        {
                            if (bombCell.CanBomb())
                            {
                                //炸毁格子
                                if (exercise)
                                {
                                    CollectModel.Instance.tempCollect.AddCount(bombCell.configId, 1);
                                    if (bombCell.config.cell_type == (int)CellType.terrain)
                                    {
                                        List<int> hides = bombCell.config.GetHides();
                                        for (int h = 0; h < hides.Count; h++)
                                        {
                                            CollectModel.Instance.tempCollect.AddCount(hides[h], 1);
											if (hides[h] == 10014)
											{
												CollectModel.Instance.scoreLen += 10;
											}
                                        }

                                    }
                                    if (bombCell.config.cell_type == (int)CellType.five)
                                    {
                                        CollectModel.Instance.scoreLen++;
                                    }
                                }
                                bombCell.Bomb();
                                CellModel.Instance.AddAnim(bombCell, CellAnimType.bomb);
                            }
                            else
                            {
                                CellModel.Instance.AddAnim(bombCell, CellAnimType.nullbomb);
                            }

                            //炸毁地板
                            FloorInfo floorInfo = FloorModel.Instance.GetFloorByPos(bombCell.posY, bombCell.posX);
                            if (!floorInfo.IsNull())
                            {
                                if (exercise)
                                {
                                    CollectModel.Instance.tempCollect.AddCount(floorInfo.config.id, 1);
                                }
                                floorInfo = FloorModel.Instance.ClearFloor(floorInfo);
                                FloorModel.Instance.AddAnim(floorInfo);
                            }
                        }
                        else
                        {
                            MonsterInfo monster = MonsterModel.Instance.GetMonsterByPos(bombCell.posY, bombCell.posX);
                            if (monster != null && monster.CanWreck())//炸毁怪物
                            {
                                if (exercise)
                                {
                                    CollectModel.Instance.tempCollect.AddCount(monster.config.id, 1);
                                }
                                monster.Wreck(true);
                                MonsterModel.Instance.AddAnim(monster, CellAnimType.wreck);
                            }
                            CellModel.Instance.AddAnim(bombCell, CellAnimType.nullbomb);
                        }
                    }

                    //炸毁墙体
                    List<WallInfo> bomNeighborWalls = WallModel.Instance.GetNeighborWalls(bombCell);
                    for (int n = 0; n < bomNeighborWalls.Count; n++)
                    {
                        WallInfo neighborWall = bomNeighborWalls[n];
                        if (neighborWall != null && neighborWall.CanWreck())
                        {
                            neighborWall.Wreck();
                            WallModel.Instance.AddAnim(neighborWall, CellAnimType.wreck);
                        }
                    }
				}
			}
		}

		CellModel.Instance.lineCells = new List<CellInfo>();
		for (i = 0; i < CellModel.Instance.allCells.Count; i++)
		{
			List<CellInfo> xCells = CellModel.Instance.allCells[i];
			for (int j = 0; j < xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];
				if (cellInfo.isBlank)
				{
					CellModel.Instance.lineCells.Add(cellInfo);
				}
			}
		}
	}
}