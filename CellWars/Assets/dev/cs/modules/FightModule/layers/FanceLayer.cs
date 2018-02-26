using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanceLayer : FightBaseLayer
{
    private FightModule fightModule;

    override protected void Awake()
    {
        base.Awake();
        list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "FightFanceItem");
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
        for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
        {
            for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
            {
                if (j > 0)
                {
                    CreateFance(WallModel.Instance.GetWallByPos(i, j, 0));
                }
                
                CreateFance(WallModel.Instance.GetWallByPos(i, j, 1));
                if (j < (BattleModel.Instance.crtBattle.battle_width - 1))
                {
                    CreateFance(WallModel.Instance.GetWallByPos(i, j, 2));
                }
            }
        }
    }

	private GameObject CreateFance(WallInfo wallInfo)
    {
		if(wallInfo.IsNull())
		{
			return null;
		}
        GameObject item = list.NewItem();
		item.name = wallInfo.posY + "_" + wallInfo.posX + "_" + wallInfo.posN;

        FightFanceItem itemCtr = item.AddComponent<FightFanceItem>();
		UpdateFance (itemCtr,wallInfo);

        PosMgr.SetFightWallPos(item.transform, wallInfo.posX, wallInfo.posY, wallInfo.posN);
		itemCtr.zrotate = GetZRotate(wallInfo.posN);
        return item;
    }

	public void UpdateFance(FightFanceItem itemCtr,WallInfo wallInfo)
	{
		itemCtr.wall_info = wallInfo;
		itemCtr.type = type;
		itemCtr.icon = wallInfo.configId;
	}

	public FightFanceItem GetItemByRunId(int runId)
	{
		List<GameObject> items = list.items;
		
		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightFanceItem cellItemCtr = item.GetComponent<FightFanceItem>();
			
			if (cellItemCtr.wall_info.runId == runId)
			{
				return cellItemCtr;
			}
		}
		return null;
	}

    public void PlayEliminate(List<WallAnimInfo> wallAnimss, ActionTree actionTree)
    {
        for (int j = 0; j < wallAnimss.Count; j++)
        {
            WallAnimInfo moveInfo = wallAnimss[j];

            FightFanceItem item = GetItemByRunId(moveInfo.toInfo.runId);
            if (item != null)
            {
                OrderAction order = new OrderAction();

                order.AddNode(new ShowEffectActor(item.transform, "effect_wall_wreck", fightModule.transform));

                order.AddNode(new PlaySoundActor("wreck"));
                order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));
                order.AddNode(new ChangeFanceActor(item, moveInfo.toInfo));

                actionTree.AddNode(order);
            }
        }
    }
}