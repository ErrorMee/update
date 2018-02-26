
using System;using UnityEngine;using System.Collections.Generic;

public class MonsterCrawlInfo
{
    public MonsterInfo monster;

    public List<CellInfo> pathCells = new List<CellInfo>();

    public bool roadEnd = false;

    public void AddPath(CellInfo cellInfo)
    {
        pathCells.Add(cellInfo);

        if (monster.RoadEnd())
        {
            roadEnd = true;
            monster.Hold(false);
            MonsterModel.Instance.ClearCrawlCreator(monster);
            monster.SetConfig(0);
        }
        else
        {
            roadEnd = false;
        }
    }
}