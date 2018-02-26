
using System;
using UnityEngine;
using System.Collections.Generic;

public class MonsterInfo:IComparable
{
	private static int LAST_RUN_ID = 0;

	public int runId = -1;

    public int configId = 0;

	public config_monster_item config;
	
	public int posX;
	public int posY;

    public List<TIVInfo> releaseList = new List<TIVInfo>();

    public int releaseId = 0;
    public int special0;
    public int special1;

    public float progress;
    public int banCount = -1;

    public float rotate = 0;

    private List<Vector2> crawlPassPoints = new List<Vector2>();
    public int crawlCount = 0;

    private CellDirType randomCrawlDir = CellDirType.up;

	public MonsterInfo ()
	{
		runId = ++LAST_RUN_ID;
	}

	//复制
	public MonsterInfo Copy()
	{
		MonsterInfo monsterInfo = new MonsterInfo ();
		monsterInfo.runId = runId;
		monsterInfo.configId = configId;
		monsterInfo.config = config;
		monsterInfo.posX = posX;
		monsterInfo.posY = posY;
		return monsterInfo;
	}
	
	//用来排序
	public int CompareTo(object obj)
	{
		CoverInfo target = obj as CoverInfo;
		return runId.CompareTo(target.runId);
	}
	
	public void SetConfig(int id)
	{
		if(id == 40000)
		{
			id = 0;
		}
        special0 = 0;
        special1 = 0;
		config = (config_monster_item)GameMgr.resourceMgr.config_monster.GetItem(id);
        if (config != null)
        {
            special0 = config.GetSpecialValue(0);
            special1 = config.GetSpecialValue(1);
            if(config.monster_type == (int)MonsterType.fall)
            {
                banCount = 0;
            }
        }
	}

    public void Hold(bool hold = true)
    {
        if (IsNull())
        {
            return;
        }
        CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);

        if (config.monster_type == (int)MonsterType.volcano || config.monster_type == (int)MonsterType.flashlight
                || config.monster_type == (int)MonsterType.bulb || config.monster_type == (int)MonsterType.fall
                || config.monster_type == (int)MonsterType.crawl)
        {
            centerCell.isMonsterHold = hold;
            return;
        }

        if (config.monster_type == (int)MonsterType.blockade)
        {
            List<CellInfo> holdCells = CellModel.Instance.GetNeighbors(centerCell);
            holdCells.Add(centerCell);

            for (int i = 0; i < holdCells.Count; i++)
            {
                CellInfo cell = holdCells[i];
                if (cell == null)
                {
                    continue;
                }
                cell.isMonsterHold = hold;
            }
        }
    }

    public bool UnLock(bool isJet = false)
    {
        if (IsNull())
        {
            return false;
        }
        if (config.monster_type == (int)MonsterType.blockade && !isJet)
        {
            CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);
            List<CellInfo> neighborCells = CellModel.Instance.GetNeighbors(centerCell);

            for (int i = 0; i < neighborCells.Count; i++)
            {
                CellInfo cell = neighborCells[i];
                if (cell.isBlank == false)
                {
                    return false;
                }
            }

            Hold(false);

            releaseList = config.GetReleaseList();
            if (releaseList.Count > 0)
            {
                releaseId = (int)releaseList[0].id;
            }
            else
            {
                releaseId = 0;
            }
            
            SetConfig(0);
            return true;
        }

        if (config.monster_type == (int)MonsterType.bulb && !isJet)
        {
            if (progress > 0)
            {
                if (progress >= 1)
                {
                    releaseList = config.GetReleaseList();
                    releaseId = (int)releaseList[0].id;
                    if (!config.forever)
                    {
                        Hold(false);
                        SetConfig(0);
                    }
                }
                return true;
            }
            return false;
        }

        if (config.monster_type == (int)MonsterType.fall && !isJet)
        {
            if (banCount > 0)
            {
                banCount--;
            }
            releaseList = config.GetReleaseList();
            releaseId = (int)releaseList[0].id;
            return true;
        }

        if (config.monster_type == (int)MonsterType.flashlight && isJet)
        {
            CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);
            WallInfo gapWall = WallModel.Instance.GetGapWall(centerCell);

            if (gapWall != null)
            {
                releaseList = config.GetReleaseList();
                if (releaseList.Count > 0)
                {
                    releaseId = (int)releaseList[0].id;
                }
                else
                {
                    releaseId = 0;
                }
                Hold(false);
                SetConfig(0);
                return true;
            }
            return false;
        }

        if (config.monster_type == (int)MonsterType.volcano && isJet)
        {
            bool coverIsOpen = CoverModel.Instance.IsOpen(posX, posY);
            if (coverIsOpen)
            {
                releaseList = config.GetReleaseList();
                if (releaseList.Count > 0)
                {
                    releaseId = (int)releaseList[0].id;
                }
                else
                {
                    releaseId = 0;
                }
                if (config.forever)
                {
                    CoverInfo cover = CoverModel.Instance.GetCoverByPos(posY, posX);
                    cover.SetConfig(30008);
                }
                else
                {
                    Hold(false);
                    SetConfig(0);
                }

                return true;
            }
            return false;
        }

        return false;
    }

    public bool Expose()
    {
        if (IsNull())
        {
            return false;
        }

        if (config.monster_type != (int)MonsterType.hide)
        {
            return false;
        }

        CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);
        FloorInfo floorInfo = FloorModel.Instance.GetFloorByPos(posY, posX);
        if (config.size == 1)
        {
            if (floorInfo == null || floorInfo.IsNull())
            {
                SetConfig(0);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            List<CellInfo> holdCells = CellModel.Instance.GetNeighbors(centerCell);
            holdCells.Add(centerCell);

            for (int i = 0; i < holdCells.Count;i++ )
            {
                CellInfo holdCell = holdCells[i];
                FloorInfo holdFloor = FloorModel.Instance.GetFloorByPos(holdCell.posY, holdCell.posX);

                if (holdFloor != null && !holdFloor.IsNull())
                {
                    return false;
                }
            }

            SetConfig(0);
            return true;
        }
    }

	public bool IsNull()
	{
		return config == null;
	}

	public void SwitchPos(MonsterInfo monsterInfo)
	{
		MonsterInfo monsterInfoCopy = monsterInfo.Copy ();
		monsterInfo.posX = posX;
		monsterInfo.posY = posY;
		posX = monsterInfoCopy.posX;
		posY = monsterInfoCopy.posY;
	}

    public void Absorb(int id)
    {
        if (special0 == id)
        {
            if (config.monster_type == (int)MonsterType.bulb)
            {
                progress += 1.00f / special1;
				if (progress > 0.99f && progress < 1.01f)
                {
                    progress = 1;
                }
            }
        }
    }

    public void UnAbsorb(int id)
    {
        if (special0 == id)
        {
            if (config.monster_type == (int)MonsterType.bulb)
            {
                progress -= 1.00f / special1;
                if (progress <= 0.01f)
                {
                    progress = 0;
                }

				if (progress > 0.99f && progress < 1.01f)
				{
					progress = 1;
				}
            }
        }
    }

    public bool CanAbsorb(int id)
    {
        if(IsNull())
        {
            return false;
        }

        if (special0 != id)
        {
            return false;
        }

        if (config.monster_type == (int)MonsterType.bulb)
        {
            return true;
        }

        return false;
    }

    public bool CanUnAbsorb(int id)
    {
        if (IsNull())
        {
            return false;
        }

        if (special0 != id)
        {
            return false;
        }

        if (config.monster_type == (int)MonsterType.bulb)
        {
            if (progress <= 0)
            {
                return false;
            }
            return true;
        }

        return false;
    }

	//是否可击毁
	public bool CanWreck()
	{
		if (IsNull())
		{
			return false;
		}
		if (config.hide_id > 40000)
		{
			return true;
		}
		return false;
	}

	public void Wreck(bool bomb)
	{
		if (!IsNull())
		{
			if (bomb)
			{
				Hold(false);
				SetConfig(0);
			}
			else
			{
				if (config.id == config.hide_id)
				{
					Hold(false);
					SetConfig(0);
				}
				else
				{
					SetConfig(config.hide_id);
				}
			}
		}
	}

    public bool IsHump(int pos = -1)
    {
        if (pos == -1)
        {
            pos = posX;
        }
        return (pos - CellInfo.start_x) % 2 != 0;
    }

    public bool IsRoadCrawl()
    {
        if (IsNull() || config.monster_type != (int)MonsterType.crawl)
        {
            return false;
        }
        if(special0 == 1)
        {
            return true;
        }
        return false;
    }

    public void InitRoadCrawl()
    {
        CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);
        List<CellInfo> cellNeighbors = CellModel.Instance.GetNeighbors(centerCell);
        CellInfo findCellPoint = null;
        for (int i = 0; i < cellNeighbors.Count; i++)
        {
            CellInfo cellNeighbor = cellNeighbors[i];

            if (cellNeighbor == null)
            {
                continue;
            }

            BattleCellInfo bcellInfo = BattleModel.Instance.crtBattle.GetBattleCellByPos(cellNeighbor.posX, cellNeighbor.posY);
            if (bcellInfo.bg_id != special1)
            {
                continue;
            }

            findCellPoint = cellNeighbor;

            Vector2 fromPos = PosMgr.GetFightCellPos(posX, posY);
            Vector2 toPos = PosMgr.GetFightCellPos(findCellPoint.posX, findCellPoint.posY);

            rotate = PosMgr.VectorAngle(new Vector2(fromPos.x, fromPos.y), new Vector2(toPos.x, toPos.y)) / FightConst.ROTATE_BASE;

            break;
        }
    }

    public bool RoadEnd()
    {
        if(!IsRoadCrawl())
        {
            return false;
        }

        CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);
        List<CellInfo> cellNeighbors = CellModel.Instance.GetNeighbors(centerCell);
        for (int i = 0; i < cellNeighbors.Count; i++)
        {
            CellInfo cellNeighbor = cellNeighbors[i];
            if (cellNeighbor == null)
            {
                continue;
            }

            BattleCellInfo bcellInfo = BattleModel.Instance.crtBattle.GetBattleCellByPos(cellNeighbor.posX, cellNeighbor.posY);
            if (bcellInfo.bg_id == special1)
            {
                bool isPassed = false;
                for (int j = 0; j < crawlPassPoints.Count; j++)
                {
                    Vector2 passPoint = crawlPassPoints[j];
                    if (passPoint.x == cellNeighbor.posX && passPoint.y == cellNeighbor.posY)
                    {
                        isPassed = true;
                        break;
                    }
                }

                if (isPassed == false)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void InitReleaseList()
    {
        releaseList = config.GetReleaseList();
    }

    public void Crawl(bool road = false)
    {
        InitReleaseList();
        crawlCount = (int)releaseList[0].value;

        if (road)
        {
            if (crawlPassPoints.Count == 0)
            {
                crawlPassPoints.Add(new Vector2(posX, posY));
            }
            RecursionRoadCrawl();
        }
        else {
            //RecursionRandomCrawl();
        }
    }

    public void RecursionRandomCrawl()
    {
        if (crawlCount > 0)
        {
            CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);
            CellInfo findCellPoint = null;

            List<CellDirType> priorityDirList = PosMgr.PriorityDirList((int)randomCrawlDir,true);

            for (int i = 0; i < priorityDirList.Count;i++ )
            {
                CellDirType dir = priorityDirList[i];

                CellInfo dirCell = CellModel.Instance.GetDirCell(centerCell, dir);

                if (dirCell == null)
                {
                    continue;
                }

				if (dirCell.CanCrawl() == false)
                {
                    continue;
                }

                CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(dirCell.posY, dirCell.posY);
				if (coverInfo != null && coverInfo.IsOpen() == false)
                {
                    continue;
                }

                WallInfo wall = WallModel.Instance.GetWall(centerCell, dirCell);
                if (wall.IsNull() == false)
                {
                    continue;
                }
                randomCrawlDir = dir;
                findCellPoint = dirCell;
                break;
            }

            if (findCellPoint != null)
            {
                Hold(false);
                MonsterInfo toMonsterInfo = MonsterModel.Instance.GetMonsterByPos(findCellPoint.posY, findCellPoint.posX);
                MonsterModel.Instance.SwitchPos(this, toMonsterInfo);
                this.SwitchPos(toMonsterInfo);
                Hold();

                crawlCount--;
                MonsterModel.Instance.AddCrawAnim(this, findCellPoint);

                if (crawlCount > 0)
                {
                    //RecursionRandomCrawl();
                }
            }
            else
            {
                crawlCount--;
                MonsterModel.Instance.AddCrawAnim(this, centerCell);
            }
        }
    }


    private void RecursionRoadCrawl()
    {
        if (crawlCount > 0)
        {
            CellInfo centerCell = CellModel.Instance.GetCellByPos(posX, posY);
            List<CellInfo> cellNeighbors = CellModel.Instance.GetNeighbors(centerCell);
            CellInfo findCellPoint = null;

            for (int i = 0; i < cellNeighbors.Count; i++)
            {
                CellInfo cellNeighbor = cellNeighbors[i];

                if (cellNeighbor == null)
                {
                    continue;
                }

				if (cellNeighbor.CanCrawl() == false)
				{
					continue;
				}

                CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(cellNeighbor.posY, cellNeighbor.posY);
				if (coverInfo != null && coverInfo.IsOpen() == false)
                {
                    continue;
                }

                WallInfo wall = WallModel.Instance.GetWall(centerCell, cellNeighbor);
                if (wall.IsNull() == false)
                {
                    continue;
                }
                BattleCellInfo bcellInfo = BattleModel.Instance.crtBattle.GetBattleCellByPos(cellNeighbor.posX, cellNeighbor.posY);
                if (bcellInfo.bg_id != special1)
                {
                    continue;
                }

                bool isPassed = false;
                for (int j = 0; j < crawlPassPoints.Count; j++)
                {
                    Vector2 passPoint = crawlPassPoints[j];
                    if (passPoint.x == cellNeighbor.posX && passPoint.y == cellNeighbor.posY)
                    {
                        isPassed = true;
                        break;
                    }
                }

                if (isPassed == false)
                {
                    findCellPoint = cellNeighbor;
                    break;
                }
            }

            if (findCellPoint != null)
            {
                Hold(false);
                MonsterInfo toMonsterInfo = MonsterModel.Instance.GetMonsterByPos(findCellPoint.posY, findCellPoint.posX);
                MonsterModel.Instance.SwitchPos(this, toMonsterInfo);
                this.SwitchPos(toMonsterInfo);
                Hold();

                crawlCount--;
                crawlPassPoints.Add(new Vector2(findCellPoint.posX, findCellPoint.posY));

                MonsterModel.Instance.AddCrawAnim(this, findCellPoint);

                if (crawlCount > 0)
                {
                    RecursionRoadCrawl();
                }
            }
        }
    }

    public bool StopSkill()
    {
        if (IsNull())
        {
            return false;
        }
        if (config.monster_type == (int)MonsterType.crawl)
        {
            return true;
        }
        return false;
    }
}