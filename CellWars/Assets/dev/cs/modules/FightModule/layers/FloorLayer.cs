using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLayer : FightBaseLayer
{
    override protected void Awake()
    {
        base.Awake();
        list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "FightBaseItem");
    }
    public override void ShowList()
    {
        base.ShowList();
	
        int i;
        for (i = 0; i < FloorModel.Instance.allFloors.Count; i++)
        {
			for (int j = 0; j < FloorModel.Instance.allFloors[0].Count; j++)
            {
				FloorInfo floorInfo = FloorModel.Instance.allFloors[i][j];
                CreateFloorItem(j, i, floorInfo);
            }
        }
    }

	protected GameObject CreateFloorItem(int posX, int posY, FloorInfo floorInfo)
    {
		if (floorInfo.IsNull())
        {
            return null;
        }

        GameObject item = list.NewItem();
        item.name = posX + "_" + posY;

        FightFloorItem itemCtr = item.AddComponent<FightFloorItem>();
		itemCtr.floorInfo = floorInfo;
        itemCtr.type = type;
		itemCtr.icon = floorInfo.config.icon;
		PosMgr.SetFightCellPos(item.transform,posX, posY);
        return item;
    }

	public FightFloorItem GetItemByRunId(int runId)
	{
		List<GameObject> items = list.items;
		
		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightFloorItem cellItemCtr = item.GetComponent<FightFloorItem>();
			
			if (cellItemCtr.floorInfo.runId == runId)
			{
				return cellItemCtr;
			}
		}
		return null;
	}

    public void PlayEliminate(List<FloorAnimInfo> floorAnimss, ActionTree actionTree)
    {
        for (int j = 0; j < floorAnimss.Count; j++)
        {
			OrderAction order = new OrderAction();
            FloorAnimInfo floorAnimInfo = floorAnimss[j];
            FightFloorItem item = GetItemByRunId(floorAnimInfo.floorInfo.runId);
            order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));
            order.AddNode(new DestroyActor(item.gameObject));
			actionTree.AddNode(order);
        }
    }

    public void Flow(ActionTree actionTree)
    {
        FloorModel.Instance.Flow();

        List<FloorAnimInfo> anims = FloorModel.Instance.anims[0];

        for (int i = 0; i < anims.Count; i++)
        {
            FloorAnimInfo animInfo = anims[i];

            FightFloorItem item = GetItemByRunId(animInfo.floorInfo.runId);
            if (item != null)
            {
                ParallelAction paralle = new ParallelAction();

                Vector2 toPos = PosMgr.GetFightCellPos(animInfo.floorInfo.posX, animInfo.floorInfo.posY);
                paralle.AddNode(new MoveActor((RectTransform)item.transform, new Vector3(toPos.x, toPos.y, 0), 0, 0.3f));

                OrderAction orderAction = new OrderAction();
                
                orderAction.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0.5f, 0.5f, 1), 0.1f));
                orderAction.AddNode(new WaitActor(100));
                orderAction.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(1, 1, 1), 0.1f));
                paralle.AddNode(orderAction);

                actionTree.AddNode(paralle);
            }
        }
    }
}