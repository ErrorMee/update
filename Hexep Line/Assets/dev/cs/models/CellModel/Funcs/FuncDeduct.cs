
using System.Collections.Generic;

public class FuncDeduct
{
    //步数扣除
    public static bool Deduct()
    {
        List<List<CellInfo>> allCells = CellModel.Instance.allCells;

        CellModel.Instance.anims = new List<List<CellAnimInfo>>();
        List<CellAnimInfo> stepMoves = new List<CellAnimInfo>();
        CellModel.Instance.anims.Add(stepMoves);
        CellModel.Instance.lineCells = new List<CellInfo>();

        List<CellInfo> deductSkillCells = SkillModel.Instance.DeductSkill();

        if (deductSkillCells.Count > 0)
        {
            for (int j = (deductSkillCells.Count - 1); j >= 0; j--)
            {
                CellInfo cellInfo = deductSkillCells[j];
				if (cellInfo.isMonsterHold == false && cellInfo.config != null)
                {
                    cellInfo.isLink = true;
                    cellInfo.isBlank = true;
                    CellModel.Instance.lineCells.Add(cellInfo);
                    CellModel.Instance.AddAnim(cellInfo, CellAnimType.clear);
					CollectModel.Instance.tempCollect.AddCount(cellInfo.config.id, 1);
                }
            }
            return true;
        }
        else
        {
            CellInfo firstCell = null;
            int i;
            for (i = (allCells.Count - 1); i >= 0; i--)
            {
                List<CellInfo> xCells = allCells[i];
                for (int j = (xCells.Count - 1); j >= 0; j--)
                {
                    CellInfo cellInfo = xCells[j];
                    bool coverCanMove = CoverModel.Instance.CanMove(cellInfo.posX, cellInfo.posY);
                    if (cellInfo.isBlank == false && cellInfo.config.cell_type == (int)CellType.five && coverCanMove && cellInfo.isMonsterHold == false)
                    {
                        if (firstCell == null)
                        {
                            firstCell = cellInfo;
                        }

                        if (firstCell.configId == cellInfo.configId)
                        {
                            cellInfo.isLink = true;
                            cellInfo.isBlank = true;
                            CellModel.Instance.lineCells.Add(cellInfo);
                            CellModel.Instance.AddAnim(cellInfo, CellAnimType.clear);
							CollectModel.Instance.tempCollect.AddCount(cellInfo.config.id, 1);
                        }
                    }
                }
            }
        }
        return false;
    }
}