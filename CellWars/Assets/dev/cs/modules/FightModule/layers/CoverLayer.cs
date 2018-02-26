using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverLayer : FightBaseLayer
{
    private FightModule fightModule;

    override protected void Awake()
    {
        base.Awake();
        list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "FightCoverItem");
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
		for (i = 0; i < CoverModel.Instance.allCovers.Count; i++)
        {
			for (int j = 0; j < CoverModel.Instance.allCovers[0].Count; j++)
            {
				CoverInfo coverInfo = CoverModel.Instance.allCovers[i][j];
                CreateCoverItem(coverInfo);
            }
        }
    }

	public void UpdateRate()
	{
		List<GameObject> items = list.items;
		
		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightCoverItem cellItemCtr = item.GetComponent<FightCoverItem>();

			CoverInfo coverInfo = cellItemCtr.coverInfo;

			if(coverInfo.show_type != CoverShowType.aways)
			{
				coverInfo.rate --;

				if(coverInfo.rate == 0)
				{
					if(coverInfo.show_type == CoverShowType.show)
					{
						coverInfo.show_type = CoverShowType.hide;
						coverInfo.rate = (int)Math.Abs(coverInfo.config.hide_rate);

						LeanTween.scaleX(cellItemCtr.gameObject,0,0.2f);
						//LeanTween.alpha((RectTransform)cellItemCtr.iconImage.transform, 0.1f, 0.2f);

					}else{
						coverInfo.show_type = CoverShowType.show;
						coverInfo.rate = coverInfo.config.show_rate;
						LeanTween.scaleX(cellItemCtr.gameObject,1,0.2f);
						//LeanTween.alpha((RectTransform)cellItemCtr.iconImage.transform, 1, 0.2f);
					}
				}

				cellItemCtr.rate = coverInfo.rate;
			}
		}
	}

    public FightCoverItem CreateCoverItem(CoverInfo coverInfo)
    {
		if (coverInfo.IsNull())
        {
            return null;
        }

        GameObject item = list.NewItem();
        item.name = coverInfo.posX + "_" + coverInfo.posY;

        FightCoverItem itemCtr = item.AddComponent<FightCoverItem>();
        itemCtr.type = type;
		itemCtr.coverInfo = coverInfo;
		itemCtr.icon = coverInfo.config.icon;
		itemCtr.rate = coverInfo.rate;
        itemCtr.UpdateTip();
        PosMgr.SetFightCellPos(item.transform, coverInfo.posX, coverInfo.posY);

		if(coverInfo.show_type == CoverShowType.hide)
		{
            itemCtr.gameObject.transform.localScale = new Vector3(0, 1, 1);
		}

        return itemCtr;
    }

	public FightCoverItem GetItemByRunId(int runId)
	{
		List<GameObject> items = list.items;
		
		for (int i = 0; i < items.Count; i++)
		{
			GameObject item = (GameObject)items[i];
			if (item == null)
			{
				continue;
			}
			FightCoverItem cellItemCtr = item.GetComponent<FightCoverItem>();
			
			if (cellItemCtr.coverInfo.runId == runId)
			{
				return cellItemCtr;
			}
		}
		return null;
	}

    public void PlayEliminate(List<CoverAnimInfo> coverAnimss, ActionTree actionTree)
    {
		
        for (int j = 0; j < coverAnimss.Count; j++)
        {
			OrderAction order = new OrderAction();
            CoverAnimInfo coverAnimInfo = coverAnimss[j];

            FightCoverItem item = GetItemByRunId(coverAnimInfo.coverInfo.runId);

            switch (coverAnimInfo.animationType)
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
                    order.AddNode(new ChangeCoverActor(this,item, coverAnimInfo.coverInfo));
                    break;
            }
			actionTree.AddNode(order);
        }

    }

    public void Flow(ActionTree actionTree)
    {
        CoverModel.Instance.Flow();

        List<CoverAnimInfo> anims = CoverModel.Instance.anims[0];

        for (int i = 0; i < anims.Count; i++)
        {
            CoverAnimInfo animInfo = anims[i];

            FightCoverItem item = GetItemByRunId(animInfo.coverInfo.runId);
            if (item != null)
            {
                ParallelAction paralle = new ParallelAction();

                Vector2 toPos = PosMgr.GetFightCellPos(animInfo.coverInfo.posX, animInfo.coverInfo.posY);
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