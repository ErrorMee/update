using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLayer : FightBaseLayer
{

    private FightModule fightModule;

    override protected void Awake()
	{
		base.Awake();
		list.itemPrefab = ResModel.Instance.GetPrefab("prefab/fightmodule/" + "FightMonsterItem");
    }

    void Start()
    {
        fightModule = GameObject.Find("Canvas/layer_0/FightModule").GetComponent<FightModule>();
    }

	public override void ShowList()
	{
		base.ShowList();

		if (BattleModel.Instance.crtBattle == null)
		{
			return;
		}

		int i;
		for (i = 0; i < MonsterModel.Instance.allMonsters.Count; i++)
		{
			for (int j = 0; j < MonsterModel.Instance.allMonsters[0].Count; j++)
			{
				MonsterInfo monsterInfo = MonsterModel.Instance.allMonsters[i][j];
				CreateMonsterItem(j, i, monsterInfo);
			}
		}
	}

	public void UpdateList()
	{
		List<GameObject> items = list.items;

		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightMonsterItem itemCtr = item.GetComponent<FightMonsterItem>();

			if(!itemCtr.monsterInfo.IsNull())
			{
				UpdateMonsterItem(itemCtr, itemCtr.monsterInfo);
			}
		}
	}

	public GameObject CreateMonsterItem(int posX, int posY, MonsterInfo monsterInfo)
	{
		if (monsterInfo.IsNull())
		{
			return null;
		}
		FightMonsterItem itemCtr = GetItemByRunId(monsterInfo.runId);
		if (itemCtr == null)
		{
			GameObject item = list.NewItem();
			item.name = posX + "_" + posY;
			itemCtr = item.AddComponent<FightMonsterItem>();
			PosUtil.SetFightCellPos(item.transform, posX, posY);
		}
		UpdateMonsterItem(itemCtr, monsterInfo);
		return itemCtr.gameObject;
	}

	public void UpdateMonsterItem(FightMonsterItem itemCtr, MonsterInfo monsterInfo)
	{
		itemCtr.type = type;
		itemCtr.monsterInfo = monsterInfo;
		itemCtr.icon = monsterInfo.config.icon;

		if (monsterInfo.rotate != 0)
		{
			itemCtr.zrotate = monsterInfo.rotate * FightConst.ROTATE_BASE;
		}
		else
		{
			itemCtr.zrotate = monsterInfo.config.rotate * FightConst.ROTATE_BASE;
		}
		itemCtr.ShowProgress(monsterInfo.progress);
		itemCtr.ShowBan(monsterInfo.banCount > 1);
	}

	public FightMonsterItem GetItemByRunId(int runId)
	{
		List<GameObject> items = list.items;

		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightMonsterItem cellItemCtr = item.GetComponent<FightMonsterItem>();

			if (cellItemCtr.monsterInfo.runId == runId)
			{
				return cellItemCtr;
			}
		}
		return null;
	}

	public FightMonsterItem GetItemByPos(int posx,int posy)
	{
		List<GameObject> items = list.items;

		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightMonsterItem cellItemCtr = item.GetComponent<FightMonsterItem>();

			if (cellItemCtr.monsterInfo.posX == posx && cellItemCtr.monsterInfo.posX == posy)
			{
				return cellItemCtr;
			}
		}
		return null;
	}

	public void PlayEliminate(List<MonsterAnimInfo> monsterAnimss, ActionTree actionTree)
	{
		
		for (int j = 0; j < monsterAnimss.Count; j++)
		{
			OrderAction order = new OrderAction();
			MonsterAnimInfo monsterAnimInfo = monsterAnimss[j];

			FightMonsterItem item = GetItemByRunId(monsterAnimInfo.monsterInfo.runId);

			switch (monsterAnimInfo.animationType)
			{
			case CellAnimType.clear:
                order.AddNode(new ShowEffectActor(item.transform, "effect_cell_bomb", fightModule.transform));
				order.AddNode(new PlaySoundActor("remove"));
				order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));
				order.AddNode(new DestroyActor(item.gameObject));
				break;
			case CellAnimType.wreck:
                order.AddNode(new ShowEffectActor(item.transform, "effect_cell_bomb", fightModule.transform));
                order.AddNode(new PlaySoundActor("remove"));
				order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));
				//if (monsterAnimInfo.monsterInfo.IsNull())
				//{
				//    order.AddNode(new DestroyActor(item.gameObject));
				//}else
				//{
				order.AddNode(new ChangeMonsterActor(item, monsterAnimInfo.monsterInfo));
				//}
				break;
			}
			actionTree.AddNode(order);
		}

	}
}