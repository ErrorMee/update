using System;
using System.Collections;
using System.Collections.Generic;

//管理所有的关卡地图配置数据 记录当前地图
public class BattleModel:Singleton<BattleModel>
{
    public config_map_item crtConfig;

    public List<BattleInfo> battles = new List<BattleInfo>();

	public BattleInfo crtBattle;

    public int play_mapId = 0;

    public int ready_map = 0;

    public int lose_map = 0;

	public void InitCrtBattle(config_map_item config)
	{
		crtConfig = config;
		BattleInfo battleInfo = GetBattle(crtConfig.id);
        if (battleInfo == null)
        {
            battleInfo = new BattleInfo();
            battleInfo.mapId = crtConfig.id;
            battles.Add(battleInfo);

            byte[] battleInfoBytes = ResModel.Instance.GetTextBytes("dat/map/" +  battleInfo.mapId.ToString());
            battleInfo.FillByte(battleInfoBytes);
        }
        else
        {
            if (battleInfo.clearCaching)
            {
                byte[] battleInfoBytes = ResModel.Instance.GetTextBytes("dat/map/" +  battleInfo.mapId.ToString());
                battleInfo.FillByte(battleInfoBytes);
            }
        }

        battleInfo.need_open_fill = true;//todo
		crtBattle = battleInfo;
	}

	public void InitFight()
	{
        FightModel.Instance.InitFightInfo();
		CellInfo.start_x = crtBattle.start_x;

        MonsterModel.Instance.Destroy();
		FloorModel.Instance.Destroy ();
		CellModel.Instance.Destroy ();
        WallModel.Instance.Destroy();
		CoverModel.Instance.Destroy();
		InvadeModel.Instance.Destroy ();

		int i;
		for (i = 0; i < crtBattle.battle_height; i++)
		{
			List<MonsterInfo> yMonsters = new List<MonsterInfo>();
			List<FloorInfo> yFloors = new List<FloorInfo>();
			List<CellInfo> yCells = new List<CellInfo>();
			List<List<WallInfo>> yWalls = new List<List<WallInfo>>();
			List<CoverInfo> yCovers = new List<CoverInfo>();

			MonsterModel.Instance.allMonsters.Add(yMonsters);
			FloorModel.Instance.allFloors.Add(yFloors);
			CellModel.Instance.allCells.Add (yCells);
			WallModel.Instance.allWalls.Add(yWalls);
			CoverModel.Instance.allCovers.Add(yCovers);

			for (int j = 0; j < crtBattle.battle_width; j++)
			{
				BattleCellInfo battleCellInfo = crtBattle.allCells[i][j];

				MonsterInfo monsterInfo = new MonsterInfo ();
                monsterInfo.configId = battleCellInfo.monster_id;
				monsterInfo.SetConfig(battleCellInfo.monster_id);
				monsterInfo.posX = j;
				monsterInfo.posY = i;
				yMonsters.Add(monsterInfo);

				FloorInfo floorInfo = new FloorInfo ();
				floorInfo.SetConfig(battleCellInfo.floor_id);
				floorInfo.posX = j;
				floorInfo.posY = i;
				yFloors.Add(floorInfo);

				CellInfo cellInfo = new CellInfo ();
				cellInfo.SetConfig(battleCellInfo.cell_id);
				cellInfo.posX = j;
				cellInfo.posY = i;
				yCells.Add(cellInfo);

                if (cellInfo.isBlank == false && cellInfo.config.cell_type == (int)CellType.changer)
                {
					cellInfo.changer = cellInfo.config.icon;
                    cellInfo.SetConfig(cellInfo.config.hide_id);
					cellInfo.originalConfigId = cellInfo.config.hide_id;
                }

				if(cellInfo.config != null)
				{
					if(cellInfo.config.cell_type == (int)CellType.invade || cellInfo.config.id == 10007)
					{
						InvadeModel.Instance.AddInvade(cellInfo);
					}
				}

				CoverInfo coverInfo = new CoverInfo();
				coverInfo.SetConfig(battleCellInfo.cover_id);
				coverInfo.posX = j;
				coverInfo.posY = i;
				yCovers.Add(coverInfo);

				List<WallInfo> xWalls = new List<WallInfo>();
				for (int n = 0; n < 3; n++)
				{
					WallInfo wallInfo = new WallInfo();
					wallInfo.SetConfig(battleCellInfo.walls[n]);
					wallInfo.posX = j;
					wallInfo.posY = i;
					wallInfo.posN = n;
					xWalls.Add(wallInfo);
				}
				yWalls.Add(xWalls);
			}
		}
		MonsterModel.Instance.RandomPos ();
		MonsterModel.Instance.HoldAll ();
		HideModel.Instance.LoadHider ();
	}

    public void RollCut(int cutNum)
    {
        crtBattle.RollCut(cutNum);

        int i;
        for (i = 0; i < cutNum; i++)
        {
            MonsterModel.Instance.allMonsters.RemoveAt(0);
            FloorModel.Instance.allFloors.RemoveAt(0);
            CellModel.Instance.allCells.RemoveAt(0);
            WallModel.Instance.allWalls.RemoveAt(0);
            CoverModel.Instance.allCovers.RemoveAt(0);
        }

        for (i = 0; i < crtBattle.battle_height; i++)
        {
            List<MonsterInfo> yMonsters = MonsterModel.Instance.allMonsters[i];
            List<FloorInfo> yFloors = FloorModel.Instance.allFloors[i];
            List<CellInfo> yCells = CellModel.Instance.allCells[i];
            List<List<WallInfo>> yWalls = WallModel.Instance.allWalls[i];
            List<CoverInfo> yCovers = CoverModel.Instance.allCovers[i];

            for (int j = 0; j < crtBattle.battle_width; j++)
            {
                MonsterInfo monsterInfo = yMonsters[j];
                monsterInfo.posY -= cutNum;

                FloorInfo floorInfo = yFloors[j];
                floorInfo.posY -= cutNum;

                CellInfo cellInfo = yCells[j];
                cellInfo.posY -= cutNum;

                List<WallInfo> xWalls = yWalls[j];
                for (int n = 0; n < xWalls.Count; n++)
                {
                    WallInfo wallInfo = xWalls[n];
                    wallInfo.posY -= cutNum;
                }

                CoverInfo coverInfo = yCovers[j];
                coverInfo.posY -= cutNum;
            }
        }

		SkillModel.Instance.InitFightingEntitys ();
    }

    public BattleInfo GetBattle(int mapId)
    {
        for (int i = 0; i < battles.Count; i++)
        {
            BattleInfo battleInfo = battles[i];
            if (battleInfo.mapId == mapId)
            {
                return battleInfo;
            }
        }
        return null;
    }

	public void Delbattle(int mapId)
	{
		for (int i = 0; i < battles.Count; i++)
		{
			BattleInfo battleInfo = battles[i];
			if (battleInfo.mapId == mapId)
			{
				battles.RemoveAt(i);
				return;
			}
		}
	}
}
