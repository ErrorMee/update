using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHideLayer : FightBaseLayer
{
	override protected void Awake()
	{
		base.Awake();
		list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "FightHideItem");
	}

	public override void ShowList()
	{
		base.ShowList();

		int i;
		for (i = 0; i < HideModel.Instance.allHiders.Count; i++)
		{
			HiderInfo hiderInfo = HideModel.Instance.allHiders[i];
			CreateHiderItem(hiderInfo);
		}
	}

	protected GameObject CreateHiderItem(HiderInfo hiderInfo)
	{
		if (hiderInfo.isNull)
		{
			return null;
		}

		GameObject item = list.NewItem();
		item.name = hiderInfo.posX + "_" + hiderInfo.posY;

		FightHideItem itemCtr = item.AddComponent<FightHideItem>();
		itemCtr.hiderInfo = hiderInfo;
		itemCtr.type = type;
		itemCtr.icon = hiderInfo.configId;
		PosMgr.SetFightCellPos(item.transform,hiderInfo.posX, hiderInfo.posY);
		return item;
	}

	public FightHideItem GetItemByRunId(int runId)
	{
		List<GameObject> items = list.items;

		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightHideItem cellItemCtr = item.GetComponent<FightHideItem>();
			if (cellItemCtr == null)
			{
				continue;
			}
			if (cellItemCtr.hiderInfo.runId == runId)
			{
				return cellItemCtr;
			}
		}
		return null;
	}

}