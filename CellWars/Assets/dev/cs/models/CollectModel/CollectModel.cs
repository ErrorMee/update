using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectModel:Singleton<CollectModel>
{
	public CollectInfo tempCollect;
	public CollectInfo profileCollect = new CollectInfo();

	private const int SCORE_BASE = 30;
	private const int LEN_LINE = 5;
	public const float SCORE_CRIT = 1.2f;

    public int scoreLen = 0;
	public int tempScore;
	public bool isCrit = false;
	public int profileScore;

    public void Clear()
    {
        tempCollect = new CollectInfo();
        profileCollect = new CollectInfo();
        tempScore = 0;
        isCrit = false;
        profileScore = 0;
    }

	//临时收集
	public void TempCollect(bool isDeductStep = false)
	{
        //test
        //isDeductStep = true;

		if(isDeductStep)
		{
			//tempCollect = new CollectInfo();
			scoreLen = CellModel.Instance.lineCells.Count;
			tempScore = scoreLen * SCORE_BASE;
			isCrit = false;
		}else
		{
            Backup();

			tempCollect = new CollectInfo();
			scoreLen = 0;
			
			FuncEliminate.Eliminate(true);

            Recovery();
			
			tempScore = scoreLen * SCORE_BASE;
			if (scoreLen > LEN_LINE)
			{
				tempScore = (int)(tempScore * SCORE_CRIT);
				isCrit = true;
			} else
			{
				isCrit = false;
			}
		}
	}

    public void TempCollectList(List<int> listIds)
    {
        tempCollect = new CollectInfo();
        scoreLen = 0;
        tempScore = 0;
        isCrit = false;
        for (int i = 0; i < listIds.Count;i++ )
        {
            tempCollect.AddCount(listIds[i], 1);
        }
    }

	//建立正式收集
	public void ProfileCollect(bool isDeductStep)
	{
		if (isDeductStep == false) {
			int exceedNum = tempCollect.allCount - LEN_LINE;

			switch (exceedNum) {
			case 1:
                    PromptModel.Instance.Pop(LanguageUtil.GetTxt(11701));
				break;
			case 2:
                PromptModel.Instance.Pop(LanguageUtil.GetTxt(11702));
				break;
			case 3:
                PromptModel.Instance.Pop(LanguageUtil.GetTxt(11703));
				break;
			case 4:
                PromptModel.Instance.Pop(LanguageUtil.GetTxt(11704));
				break;
			}

			if (exceedNum >= 5) {
                PromptModel.Instance.Pop(LanguageUtil.GetTxt(11705));
			}
		} else {
				
			PromptModel.Instance.Pop("<color=#00ff00>+" + tempScore + "</color>",true);
		}

		profileCollect.Merger (tempCollect);

		profileScore += tempScore;
	}




    //备份所有地板
    private List<List<FloorInfo>> allFloorsBackup;
    //备份所有格子
    private List<List<CellInfo>> allCellsBackup;
    //备份所有墙
    private List<List<List<WallInfo>>> allWallsBackup;
    //备份所有盖子
    private List<List<CoverInfo>> allCoversBackup;
    //备份所有怪物
    private List<List<MonsterInfo>> allMonstersBackup;

    //备份连接的格子
    private List<CellInfo> lineCellsBackup;
    //备份退出连接的
    private CellInfo rollbackCellBackup;

    //备份技能
    private List<SkillEntityInfo> fighting_entitysBackup;

    private void Recovery()
    {
        FloorModel.Instance.allFloors = allFloorsBackup;
        CellModel.Instance.allCells = allCellsBackup;
        WallModel.Instance.allWalls = allWallsBackup;
        CoverModel.Instance.allCovers = allCoversBackup;
        MonsterModel.Instance.allMonsters = allMonstersBackup;

        CellModel.Instance.lineCells = lineCellsBackup;
        CellModel.Instance.rollbackCell = rollbackCellBackup;

        SkillModel.Instance.fighting_entitys = fighting_entitysBackup;
    }

    private void Backup()
    {
        allFloorsBackup = FloorModel.Instance.allFloors;
        allCellsBackup = CellModel.Instance.allCells;
        allWallsBackup = WallModel.Instance.allWalls;
        allCoversBackup = CoverModel.Instance.allCovers;
        allMonstersBackup = MonsterModel.Instance.allMonsters;
        lineCellsBackup = CellModel.Instance.lineCells;
        rollbackCellBackup = CellModel.Instance.rollbackCell;
        fighting_entitysBackup = SkillModel.Instance.fighting_entitys;

        FloorModel.Instance.allFloors = new List<List<FloorInfo>>();
        CellModel.Instance.allCells = new List<List<CellInfo>>();
        WallModel.Instance.allWalls = new List<List<List<WallInfo>>>();
        CoverModel.Instance.allCovers = new List<List<CoverInfo>>();
        MonsterModel.Instance.allMonsters = new List<List<MonsterInfo>>();
        SkillModel.Instance.fighting_entitys = new List<SkillEntityInfo>();

        List<List<CellInfo>> allCells = allCellsBackup;
        int i;
        for (i = 0; i < allCells.Count; i++)
        {
            List<FloorInfo> yFloors = new List<FloorInfo>();
            List<CellInfo> yCells = new List<CellInfo>();
            List<List<WallInfo>> yWalls = new List<List<WallInfo>>();
            List<CoverInfo> yCovers = new List<CoverInfo>();
            List<MonsterInfo> yMonsters = new List<MonsterInfo>();

            FloorModel.Instance.allFloors.Add(yFloors);
            CellModel.Instance.allCells.Add(yCells);
            WallModel.Instance.allWalls.Add(yWalls);
            CoverModel.Instance.allCovers.Add(yCovers);
            MonsterModel.Instance.allMonsters.Add(yMonsters);

            List<CellInfo> xCells = allCells[i];
            for (int j = 0; j < xCells.Count; j++)
            {
                CellInfo cellInfo = xCells[j];
                yCells.Add(cellInfo.Copy());
                FloorInfo floorInfo = allFloorsBackup[cellInfo.posY][cellInfo.posX];
                yFloors.Add(floorInfo.Copy());
                CoverInfo coverInfo = allCoversBackup[cellInfo.posY][cellInfo.posX];
                yCovers.Add(coverInfo.Copy());
                MonsterInfo monsterInfo = allMonstersBackup[cellInfo.posY][cellInfo.posX];
                yMonsters.Add(monsterInfo.Copy());

                List<WallInfo> xWalls = new List<WallInfo>();
                yWalls.Add(xWalls);
                for (int n = 0; n < 3; n++)
                {
                    WallInfo wallInfo = allWallsBackup[cellInfo.posY][cellInfo.posX][n];
                    xWalls.Add(wallInfo.Copy());
                }
            }
        }

        CellModel.Instance.lineCells = new List<CellInfo>();
        for (i = 0; i < lineCellsBackup.Count; i++)
        {
            CellInfo cellInfo = lineCellsBackup[i];
            CellModel.Instance.lineCells.Add(CellModel.Instance.allCells[cellInfo.posY][cellInfo.posX]);
        }

        if (rollbackCellBackup == null)
        {
            CellModel.Instance.rollbackCell = null;
        }
        else
        {
            CellModel.Instance.rollbackCell = CellModel.Instance.allCells[rollbackCellBackup.posY][rollbackCellBackup.posX];
        }

        for (i = 0; i < fighting_entitysBackup.Count; i++)
        {
            SkillEntityInfo fighting_entity = fighting_entitysBackup[i];

            SkillEntityInfo fighting_entityCopy = fighting_entity.Copy();

            fighting_entityCopy.cell = CellModel.Instance.allCells[fighting_entityCopy.cell.posY][fighting_entityCopy.cell.posX];
            for (int j = 0; j < fighting_entityCopy.controlCells.Count; j++)
            {
                CellInfo controlCell = fighting_entityCopy.controlCells[j];
                fighting_entityCopy.controlCells[j] = CellModel.Instance.allCells[controlCell.posY][controlCell.posX];
            }

            SkillModel.Instance.fighting_entitys.Add(fighting_entityCopy);

        }
    }
}
