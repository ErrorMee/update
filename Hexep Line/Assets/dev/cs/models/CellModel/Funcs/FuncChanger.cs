
using System.Collections.Generic;

public class FuncChanger
{
    public static List<CellInfo> ChangeList()
    {
        List<List<CellInfo>> allCells = CellModel.Instance.allCells;

        List<CellInfo> changeCells = new List<CellInfo>();
        int i;
        for (i = (allCells.Count - 1); i >= 0; i--)
        {
            List<CellInfo> xCells = allCells[i];
            for (int j = (xCells.Count - 1); j >= 0; j--)
            {
                CellInfo cellInfo = xCells[j];

                if (cellInfo.isBlank == false && cellInfo.changer > 0)
                {
                    int newId = GetChangeId(cellInfo.config.id);
                    cellInfo.changer = newId + (cellInfo.changer - cellInfo.configId);
                    cellInfo.SetConfig(newId);
                    changeCells.Add(cellInfo);
					cellInfo.originalConfigId = newId;
                }
            }
        }

        return changeCells;
    }

    private static int GetChangeId(int oldId)
    {
        int newId = UnityEngine.Random.Range(10101, 10101 + 5);
        if (oldId != newId)
        {
            return newId;
        }
        else
        {
            return GetChangeId(oldId);
        }
    }
}