using System;
using UnityEngine;

public class FgLayer : FightBaseLayer
{
    override protected void Awake()
    {
        base.Awake();
        list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "FightBaseItem");
    }
    public override void ShowList()
    {
        base.ShowList();

        if (BattleModel.Instance.crtBattle == null)
        {
            return;
        }

		int index = 0;
        for (int i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
		{
            for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
			{
				if(i <= PosMgr.Y_SATRT_FG && i >= PosMgr.Y_END_FG && j <= PosMgr.X_END_FG && j >= PosMgr.X_SATRT_FG)
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
        PosMgr.SetCellPos(item.transform, posX, posY);
        return item;
    }

}