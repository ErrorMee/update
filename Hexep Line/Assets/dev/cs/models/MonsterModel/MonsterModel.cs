using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterModel : Singleton<MonsterModel>
{
	public List<List<MonsterInfo>> allMonsters = new List<List<MonsterInfo>>();
	
	//动画记录
	public List<List<MonsterAnimInfo>> anims = new List<List<MonsterAnimInfo>> ();
    //爬行记录
    public List<MonsterCrawlInfo> crawAnims = new List<MonsterCrawlInfo>();

    //爬行创造者列表
    public List<MonsterCrawlCreator> crawlCreators = new List<MonsterCrawlCreator>();

    public List<int> backUpUnLocks = new List<int>();

    private int absorbIndex = 0;

	public void Destroy()
	{
		Clear ();
		allMonsters = new List<List<MonsterInfo>> ();
	}
	
	public void Clear()
	{
        absorbIndex = 0;
		anims = new List<List<MonsterAnimInfo>> ();
        crawAnims = new List<MonsterCrawlInfo>();
	}
	
	public MonsterInfo GetMonsterInfoByRunId(int runId)
	{
		for(int i = 0;i<allMonsters.Count;i++)
		{
			for(int j = 0;j<allMonsters[i].Count;j++)
			{
				MonsterInfo monsterInfo = allMonsters[i][j];
				
				if(monsterInfo.runId == runId)
				{
					return monsterInfo;
				}
			}
		}
		return null;
	}

    public void HoldAll()
    {
        for (int i = 0; i < allMonsters.Count; i++)
        {
            for (int j = 0; j < allMonsters[i].Count; j++)
            {
                MonsterInfo monsterInfo = allMonsters[i][j];
                monsterInfo.Hold();
            }
        }
    }

    public void InitCrawlCreators()
    {
        crawlCreators = new List<MonsterCrawlCreator>();

        int i;
        for (i = 0; i < allMonsters.Count; i++)
        {
            for (int j = 0; j < allMonsters[i].Count; j++)
            {
                MonsterInfo monsterInfo = allMonsters[i][j];
                if (monsterInfo.IsNull() == false && monsterInfo.config.monster_type == (int)MonsterType.crawl)
                {
                    if (monsterInfo.IsRoadCrawl())
                    {
                        MonsterCrawlCreator creator = new MonsterCrawlCreator();
                        creator.runId = monsterInfo.runId;
                        creator.posX = monsterInfo.posX;
                        creator.posY = monsterInfo.posY;
                        creator.createId = monsterInfo.config.id;
                        creator.monster = monsterInfo;
                        crawlCreators.Add(creator);
                    }
                }
            }
        }
    }

    public void ClearCrawlCreator(MonsterInfo monsterInfo)
    {
        for (int i = 0; i < crawlCreators.Count; i++)
        {
            MonsterCrawlCreator creator = crawlCreators[i];
            if (creator.monster != null && creator.monster == monsterInfo)
            {
                creator.monster = null;
                break;
            }
        }
    }

    public List<MonsterInfo> CrawlCreate()
    {
        List<MonsterInfo> newMonster = new List<MonsterInfo>();

        for (int i = 0; i < crawlCreators.Count; i++)
        {
            MonsterCrawlCreator creator = crawlCreators[i];
            if (creator.monster == null)
            {
				CellInfo startCell = CellModel.Instance.GetCellByPos(creator.posX, creator.posY);

				if(!startCell.isBlank && startCell.config.cell_type == (int)CellType.terrain)
				{
					continue;
				}

                MonsterInfo monster = new MonsterInfo();
                monster.configId = creator.createId;
                monster.SetConfig(monster.configId);
                monster.posX = creator.posX;
                monster.posY = creator.posY;
                allMonsters[creator.posY][creator.posX] = monster;
                creator.monster = monster;

                monster.InitReleaseList();
                startCell.SetConfig((int)monster.releaseList[0].id);
                monster.Hold();
                monster.InitRoadCrawl();

                newMonster.Add(monster);
            }
        }
        return newMonster;
    }

    public void InitCrawl()
    {

        List<MonsterInfo> allCrawlMonsters = new List<MonsterInfo>();
        List<MonsterInfo> roadCrawlMonsters = new List<MonsterInfo>();
        List<MonsterInfo> randomCrawlMonsters = new List<MonsterInfo>();
        int i;
        for (i = 0; i < allMonsters.Count; i++)
        {
            for (int j = 0; j < allMonsters[i].Count; j++)
            {
                MonsterInfo monsterInfo = allMonsters[i][j];
                if (monsterInfo.IsNull() == false && monsterInfo.config.monster_type == (int)MonsterType.crawl)
                {
                    allCrawlMonsters.Add(monsterInfo);
                    if (monsterInfo.IsRoadCrawl())
                    {
                        roadCrawlMonsters.Add(monsterInfo);
                    }
                    else
                    {
                        randomCrawlMonsters.Add(monsterInfo);
                    }
                }
            }
        }

        for (i = 0; i < allCrawlMonsters.Count; i++)
        {
            MonsterInfo crawlInfo = allCrawlMonsters[i];
            CellInfo startCell = CellModel.Instance.GetCellByPos(crawlInfo.posX, crawlInfo.posY);
            crawlInfo.InitReleaseList();
            startCell.SetConfig((int)crawlInfo.releaseList[0].id);
        }

        for (i = 0; i < roadCrawlMonsters.Count; i++)
        {
            MonsterInfo monsterInfo = roadCrawlMonsters[i];
            monsterInfo.InitRoadCrawl();
        }
    }

    public void Crawl()
    {
        crawAnims = new List<MonsterCrawlInfo>();

        List<MonsterInfo> allCrawlMonsters = new List<MonsterInfo>();
        List<MonsterInfo> roadCrawlMonsters = new List<MonsterInfo>();
        List<MonsterInfo> randomCrawlMonsters = new List<MonsterInfo>();
        int i;
        for (i = 0; i < allMonsters.Count; i++)
        {
            for (int j = 0; j < allMonsters[i].Count; j++)
            {
                MonsterInfo monsterInfo = allMonsters[i][j];
                if (monsterInfo.IsNull() == false && monsterInfo.config.monster_type == (int)MonsterType.crawl)
                {
                    allCrawlMonsters.Add(monsterInfo);
                    if (monsterInfo.IsRoadCrawl())
                    {
                        roadCrawlMonsters.Add(monsterInfo);
                    }else
                    {
                        randomCrawlMonsters.Add(monsterInfo);
                    }
                }
            }
        }

        for (i = 0; i < roadCrawlMonsters.Count; i++)
        {
            MonsterInfo monsterInfo = roadCrawlMonsters[i];
            monsterInfo.Crawl(true);
        }

        for (i = 0; i < randomCrawlMonsters.Count; i++)
        {
            MonsterInfo monsterInfo = randomCrawlMonsters[i];
            monsterInfo.Crawl();
        }

        RecursionRandomCrawl(randomCrawlMonsters);
    }

    private void RecursionRandomCrawl(List<MonsterInfo> randomCrawlMonsters)
    {
        int i;
        for (i = 0; i < randomCrawlMonsters.Count; i++)
        {
            MonsterInfo monsterInfo = randomCrawlMonsters[i];

            monsterInfo.RecursionRandomCrawl();
        }

        for (i = 0; i < randomCrawlMonsters.Count; i++)
        {
            MonsterInfo monsterInfo = randomCrawlMonsters[i];
            if (monsterInfo.crawlCount > 0)
            {
                RecursionRandomCrawl(randomCrawlMonsters);
                break;
            }
        }
    }

    public List<int> UnLock(bool isJet = false)
    {
        List<int> unLockIds = new List<int>();
        for (int i = 0; i < allMonsters.Count; i++)
        {
            for (int j = 0; j < allMonsters[i].Count; j++)
            {
                MonsterInfo monsterInfo = allMonsters[i][j];
                bool isUnlock = monsterInfo.UnLock(isJet);

                if (isUnlock)
                {
                    unLockIds.Add(monsterInfo.runId);
                }
                else
                {
                    bool isExpose = monsterInfo.Expose();
                    if (isExpose)
                    {
                        unLockIds.Add(monsterInfo.runId);
                    }
                }
            }
        }
        return unLockIds;
    }

    public void BackUpUnLockMonster(List<int> unLockIds)
    {
        backUpUnLocks = new List<int>();

        for (int i = 0; i < unLockIds.Count;i++ )
        {
            MonsterInfo monsterInfo = GetMonsterInfoByRunId(unLockIds[i]);
            backUpUnLocks.Add(monsterInfo.configId);
        }
    }
	
	public MonsterInfo GetMonsterByPos(int posy ,int posx)
	{
        if (posx < BattleModel.Instance.crtBattle.battle_width && posx >= 0 && posy < BattleModel.Instance.crtBattle.battle_height && posy >= 0)
        {
            return allMonsters[posy][posx];
        }
        return null;
	}
	
	public void AddAnim(MonsterInfo monsterInfo,CellAnimType animationType = CellAnimType.clear)
	{
		List<MonsterAnimInfo> stepMoves = anims[anims.Count - 1];
		MonsterAnimInfo animInfo = new MonsterAnimInfo();
		animInfo.animationType = animationType;
		animInfo.startFrame = anims.Count;
		animInfo.monsterInfo = monsterInfo;
		stepMoves.Add(animInfo);
	}

    public void AddCrawAnim(MonsterInfo monster, CellInfo cellInfo)
    {
        int i;
        for (i = 0; i < crawAnims.Count; i++)
        {
            MonsterCrawlInfo monsterCrawlInfo = crawAnims[i];
            if (monsterCrawlInfo.monster.runId == monster.runId)
            {
                monsterCrawlInfo.AddPath(cellInfo);
                return;
            }
        }
        MonsterCrawlInfo addCrawlInfo = new MonsterCrawlInfo();
        addCrawlInfo.monster = monster;
        addCrawlInfo.AddPath(cellInfo);
        crawAnims.Add(addCrawlInfo);
    }

    public List<CellInfo> ReleaseList(int monsterId)
    {
        List<CellInfo> releaseList = new List<CellInfo>();

        MonsterInfo monster = GetMonsterInfoByRunId(monsterId);
        if (monster != null && monster.releaseList.Count> 0)
        {
            TIVInfo tiv = monster.releaseList[0];

            CellModel.Instance.anims = new List<List<CellAnimInfo>>();

            List<CellInfo> waitList = new List<CellInfo>();

            if ((int)tiv.value <= 0)
            {
                CellInfo centerCell = CellModel.Instance.GetCellByPos(monster.posX, monster.posY);
                CellDirType dirType = WallModel.Instance.GetGapWallDir(centerCell);
                if (dirType != CellDirType.no)
                {
                    waitList = CellModel.Instance.GetDirCells(centerCell, dirType);
                }

                for (int i = 0; i < waitList.Count; i++)
                {
                    CellInfo cellInfo = waitList[i];
                    if (cellInfo != null && cellInfo.isBlank == false && cellInfo.config.cell_type == (int)CellType.five)
                    {
                        cellInfo.SetConfig((int)tiv.id);
                        releaseList.Add(cellInfo);
                    }
                }
            }
            else
            {
                if (monster.progress > 0)
                {
                    if (monster.progress >= 1)
                    {
                        monster.progress = 0;
                        CellInfo centerCell = CellModel.Instance.GetCellByPos(monster.posX, monster.posY);
                        waitList = CellModel.Instance.GetNeighbors(centerCell);
                        for (int i = 0; i < waitList.Count; i++)
                        {
                            CellInfo cellInfo = waitList[i];
                            if (cellInfo != null && cellInfo.isBlank == false && cellInfo.config.cell_type == (int)CellType.five)
                            {
                                cellInfo.SetConfig((int)tiv.id);
                                releaseList.Add(cellInfo);
                            }
                        }
                    }
                }
                else if (monster.banCount >= 0)
                {
                    if (monster.banCount == 0)
                    {
                        CellInfo domnCell = CellModel.Instance.GetCellByPos(monster.posX, monster.posY + 1);
                        if (domnCell != null)
                        {
                            if (domnCell.isBlank)
                            {
                                domnCell.SetConfig(monster.releaseId);
                                CellModel.Instance.RemoveFromLines(domnCell.posX, domnCell.posY);
                                releaseList.Add(domnCell);
                            }
                            else
                            {
								if (domnCell.config.id != monster.releaseId && domnCell.config.cell_type != (int)CellType.terrain)
                                {
									config_cell_item config_cell = (config_cell_item)ResModel.Instance.config_cell.GetItem (monster.releaseId);

									bool inHide = false;
									List<int> hides = config_cell.GetHides();
									for (int h = 0; h < hides.Count; h++)
									{
										if(domnCell.config.id == hides[h])
										{
											inHide = true;
										}
									}

									if(inHide == false)
									{
										domnCell.SetConfig(monster.releaseId);
										CellModel.Instance.RemoveFromLines(domnCell.posX, domnCell.posY);
										releaseList.Add(domnCell);
									}
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < BattleModel.Instance.crtBattle.ShowHeight(); i++)
                    {
                        List<CellInfo> xCells = CellModel.Instance.allCells[i];
                        for (int j = 0; j < xCells.Count; j++)
                        {
                            CellInfo cellInfo = xCells[j];
                            if (cellInfo.isBlank == false && cellInfo.config.cell_type == (int)CellType.five && cellInfo.changer == 0)
                            {
                                if (tiv.id != cellInfo.config.id)
                                {
                                    waitList.Add(cellInfo);
                                }
                            }
                        }
                    }
					int minCount = Mathf.Min((int)tiv.value,waitList.Count);
					for (int i = 0; i < minCount; i++)
                    {
                        int rangeIndex = Random.Range(0, waitList.Count);

                        CellInfo cellInfo = waitList[rangeIndex];
                        cellInfo.SetConfig((int)tiv.id);

                        waitList.RemoveAt(rangeIndex);

                        releaseList.Add(cellInfo);
                    }
                }
            }
        }
        return releaseList;
    }


	public void RandomPos()
	{
		List<Vector2> allFloorPos = FloorModel.Instance.GetAllFloorPos ();
		for(int i = 0;i<allFloorPos.Count;i++)
		{
			int randomIndexA = UnityEngine.Random.Range(0, allFloorPos.Count);
			Vector2 randomPosA = allFloorPos[randomIndexA];
			MonsterInfo monsterInfoA = GetMonsterByPos((int)randomPosA.y,(int)randomPosA.x);
			if(monsterInfoA.IsNull() == false && monsterInfoA.config.monster_type != (int)MonsterType.hide)
			{
				continue;
			}

			int randomIndexB = UnityEngine.Random.Range(0, allFloorPos.Count);
			Vector2 randomPosB = allFloorPos[randomIndexB];
			MonsterInfo monsterInfoB = GetMonsterByPos((int)randomPosB.y,(int)randomPosB.x);
			if(monsterInfoB.IsNull() == false && monsterInfoB.config.monster_type != (int)MonsterType.hide)
			{
				continue;
			}

			SwitchPos(monsterInfoA,monsterInfoB);
			monsterInfoA.SwitchPos(monsterInfoB);
		}
	}

	public void SwitchPos(MonsterInfo monsterA,MonsterInfo monsterB)
	{
		allMonsters [monsterA.posY].RemoveAt (monsterA.posX);
		allMonsters [monsterA.posY].Insert (monsterA.posX, monsterB);
		allMonsters [monsterB.posY].RemoveAt (monsterB.posX);
		allMonsters [monsterB.posY].Insert (monsterB.posX, monsterA);
	}

    public void Absorb(int id)
    {
        List<MonsterInfo> absorbList = new List<MonsterInfo>();
        for (int i = 0; i < allMonsters.Count; i++)
        {
            for (int j = 0; j < allMonsters[i].Count; j++)
            {
                MonsterInfo monsterInfo = allMonsters[i][j];

                if (monsterInfo.CanAbsorb(id))
                {
                    absorbList.Add(monsterInfo);
                }
            }
        }

        if (absorbList.Count > 0)
        {
            if (absorbIndex >= absorbList.Count)
            {
                absorbIndex = 0;
            }
            MonsterInfo monsterCheck = absorbList[absorbIndex];
            monsterCheck.Absorb(id);
            absorbIndex++;
        }
    }

    public void UnAbsorb(int id)
    {
        List<MonsterInfo> absorbList = new List<MonsterInfo>();
        for (int i = 0; i < allMonsters.Count; i++)
        {
            for (int j = 0; j < allMonsters[i].Count; j++)
            {
                MonsterInfo monsterInfo = allMonsters[i][j];

                if (monsterInfo.CanUnAbsorb(id))
                {
                    absorbList.Add(monsterInfo);
                }
            }
        }

        if (absorbList.Count > 0)
        {
            absorbIndex--;
            if (absorbIndex < 0)
            {
                absorbIndex = (absorbList.Count - 1);
            }
            MonsterInfo monsterCheck = absorbList[absorbIndex];
            monsterCheck.UnAbsorb(id);
        }
    }

    public void UpdateBan(CellInfo cellInfo)
    {
        MonsterInfo centerMonster = GetMonsterByPos(cellInfo.posY, cellInfo.posX);
        List<MonsterInfo> neighbors = GetNeighbors(centerMonster);
        for (int i = 0; i < neighbors.Count; i++)
        {
            MonsterInfo neighbor = neighbors[i];
            if (neighbor != null && !neighbor.IsNull())
            {
                if(neighbor.config.monster_type == (int)MonsterType.fall)
                {
                    //neighbor.banCount = 4;
                }
            }
        }
    }

    public List<MonsterInfo> GetNeighbors(MonsterInfo centerMonster)
    {
        List<MonsterInfo> neighbors = new List<MonsterInfo>();
        neighbors.Add(GetMonsterByPos(centerMonster.posY, centerMonster.posX - 1));
        neighbors.Add(GetMonsterByPos(centerMonster.posY, centerMonster.posX + 1));
        neighbors.Add(GetMonsterByPos(centerMonster.posY + 1, centerMonster.posX));
        neighbors.Add(GetMonsterByPos(centerMonster.posY - 1, centerMonster.posX));
        if (centerMonster.IsHump())
        {
            neighbors.Add(GetMonsterByPos(centerMonster.posY - 1, centerMonster.posX - 1));
            neighbors.Add(GetMonsterByPos(centerMonster.posY - 1, centerMonster.posX + 1));
        }
        else
        {
            neighbors.Add(GetMonsterByPos(centerMonster.posY + 1, centerMonster.posX - 1));
            neighbors.Add(GetMonsterByPos(centerMonster.posY + 1, centerMonster.posX + 1));
        }
        return neighbors;
    }

    public bool StopSkill(int posx, int posy)
    {
        MonsterInfo monster = GetMonsterByPos(posy, posx);
        if (monster == null)
        {
            return false;
        }
        return monster.StopSkill();
    }
}
