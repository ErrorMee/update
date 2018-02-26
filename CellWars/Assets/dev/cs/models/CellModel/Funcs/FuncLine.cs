
public class FuncLine
{
	public static CellLineType Line(CellInfo cellInfo,bool isHead = false)
	{
		if(cellInfo.isBlank)
		{
			return CellLineType.failure;
		}

        if (cellInfo.isMonsterHold)
        {
            return CellLineType.failure;
        }

		if(isHead)
		{
			if(CellModel.Instance.lineCells.Count > 0)
			{
				return CellLineType.failure;
			}
		}

		if(CellModel.Instance.lineCells.Count == 0)
		{
            if (cellInfo.CanLineTo() == false)
			{
				return CellLineType.failure;
			}
			if(CoverModel.Instance.CanLine(cellInfo.posX,cellInfo.posY))
			{
				LineAdd(cellInfo);
                MonsterModel.Instance.Absorb(cellInfo.config.id);
				CellModel.Instance.Absorb(cellInfo.config.id);
				return CellLineType.success;
			}else
			{
				return CellLineType.failure;
			}
		}
		CellInfo lastInfo = CellModel.Instance.lineCells [CellModel.Instance.lineCells.Count - 1];//lastInfo倒数第一个
		
		if(lastInfo == cellInfo)
		{
			return CellLineType.failure;
		}

        if (lastInfo.CanLineTo(cellInfo) == false)
		{
			return CellLineType.failure;
		}

		if(cellInfo.IsNeighbor (lastInfo))
		{
			if(CellModel.Instance.lineCells.Count > 1)
			{
				CellInfo beforeInfo = CellModel.Instance.lineCells [CellModel.Instance.lineCells.Count - 2];//beforeInfo倒数第二个
				if(beforeInfo == cellInfo)
				{
                    CellModel.Instance.rollbackCell = lastInfo;
                    CellModel.Instance.rollbackCell.isLink = false;
					CellModel.Instance.lineCells.RemoveAt(CellModel.Instance.lineCells.Count - 1);
					MonsterModel.Instance.UnAbsorb(lastInfo.config.id);
					CellModel.Instance.UnAbsorb(lastInfo.config.id);
					return CellLineType.rollback;
				}else
				{
					if(cellInfo.isLink == false)
					{
                        if (WallModel.Instance.CanLine(lastInfo, cellInfo))
						{
							if(CoverModel.Instance.CanLine(cellInfo.posX,cellInfo.posY))
							{
								LineAdd(cellInfo);
                                MonsterModel.Instance.Absorb(cellInfo.config.id);
								CellModel.Instance.Absorb(cellInfo.config.id);
								return CellLineType.success;
							}else{
								return CellLineType.failure;
							}

						}
						return CellLineType.failure;
					}else
					{
						return CellLineType.failure;
					}
				}
			}else
			{
				if(cellInfo.isLink == false)
				{
                    if (WallModel.Instance.CanLine(lastInfo, cellInfo))
					{
						if(CoverModel.Instance.CanLine(cellInfo.posX,cellInfo.posY))
						{
							LineAdd(cellInfo);
                            MonsterModel.Instance.Absorb(cellInfo.config.id);
							CellModel.Instance.Absorb(cellInfo.config.id);
							return CellLineType.success;
						}else{
							return CellLineType.failure;
						}
					}
					return CellLineType.failure;
				}else
				{
					return CellLineType.failure;
				}
			}
		}
		return CellLineType.failure;
	}

    private static void LineAdd(CellInfo cellInfo)
	{
		cellInfo.isLink = true;
		CellModel.Instance.lineCells.Add(cellInfo);
	}
}