using System;
using UnityEngine;

public class LineLayer : FightBaseLayer
{
    override protected void Awake()
    {
        base.Awake();
        list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "FightLineItem");
    }

    public override void ShowList()
    {
        base.ShowList();
    }

    public void ShowLine(CellInfo cellA,CellInfo cellB)
    {
        GameObject item = list.NewItem();
        item.name = cellA.posX + "_" + cellA.posY + "_" + cellB.posX + "_" + cellB.posY;

        PosMgr.SetFightCellPos(item.transform, cellA.posX, cellA.posY);

        item.transform.Rotate(0, 0, PosMgr.VectorAngle(PosMgr.GetFightCellPos(cellA.posX, cellA.posY), PosMgr.GetFightCellPos(cellB.posX, cellB.posY)));
    }

    public void DestroyLine(CellInfo cellA, CellInfo cellB)
    {
        list.DestroyItemByName(cellA.posX + "_" + cellA.posY + "_" + cellB.posX + "_" + cellB.posY);
    }

}