using System;
using UnityEngine;

public class FgLayer : FightBaseLayer
{
    override protected void Awake()
    {
        base.Awake();
        list.itemPrefab = ResModel.Instance.GetPrefab("prefab/fightmodule/" + "FightBaseItem");
    }
    public override void ShowList()
    {
        base.ShowList();

        if (BattleModel.Instance.crtBattle == null)
        {
            return;
        }

		int index = 0;
        for (int i = (int)PosUtil.Y_HALF_COUNT; i >= -PosUtil.Y_HALF_COUNT; i--)
		{
            for (int j = -(int)PosUtil.X_HALF_COUNT; j <= PosUtil.X_HALF_COUNT; j++)
			{
				if(i <= PosUtil.Y_SATRT_FG && i >= PosUtil.Y_END_FG && j <= PosUtil.X_END_FG && j >= PosUtil.X_SATRT_FG)
				{
					int id = BattleModel.Instance.crtBattle.fgIds[index];
					CreateFgItem(j, i, id);
				}
				index ++;
			}
		}
    }

    private GameObject CreateFgItem(int posX, int posY, int icon)
    {
        if (icon <= 0)
        {
			CellInfo cell = CellModel.Instance.GetCellByPos(posX - BattleModel.Instance.crtBattle.start_x, - posY + BattleModel.Instance.crtBattle.start_y);
			if(cell != null && cell.config != null)
			{
				if(cell.config.id != 10001)
				{
					return null;
				}else
				{
					icon = 10302;
				}
			}else
			{
				return null;
			}
        }

        GameObject item = list.NewItem();
        item.name = posX + "_" + posY;

		FightFgItem itemCtr = item.AddComponent<FightFgItem>();
        itemCtr.type = type;
        itemCtr.icon = icon;
        PosUtil.SetCellPos(item.transform, posX, posY);
        return item;
    }

}