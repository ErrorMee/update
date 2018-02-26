using System;
using UnityEngine;

public class BgLayer : FightBaseLayer
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

        int i;
        for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
        {
            for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
            {
                BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[i][j];
				GameObject bgItem = CreateBaseItem(j, i, cell.bg_id);
				FightBaseItem itemCtr = bgItem.GetComponent<FightBaseItem>();

                if (cell.bg_id == 10202)
                {
					//DestroyObject(bgItem);
					itemCtr.iconImage.color = ColorMgr.GetColor(ColorMgr.COLOR_BLACK,0.3f);
					itemCtr.iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W * 0.95f, PosMgr.CELL_W * 0.94f);
				}else
				{
					itemCtr.iconImage.rectTransform.sizeDelta = new Vector2(PosMgr.CELL_VIEW_W * 0.9f, PosMgr.CELL_W * 0.9f);
				}
            }
        }
    }

}