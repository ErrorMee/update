using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CellLayer : FightBaseLayer
{
    public CellInfo lastTouchCell;

	private OrderAction rootAction;

	private FightUI fightUI;

	private FloorLayer floorLayer;
	private CoverLayer coverLayer;
	private FanceLayer fancelayer;
	private FloorHideLayer floorHideLayer;
    private MonsterLayer monsterLayer;
	private EffectLayer effectLayer;
    private FightModule fightModule;

    private bool isDeductStep = false;
    private bool coverFlowInterrupt = false;

    override protected void Awake()
    {
        base.Awake();
        list.itemPrefab = ResModel.Instance.GetPrefab("prefab/fightmodule/" + "FightCellItem");
    }

	void Start()
	{
        fightUI = GameObject.Find("Canvas/layer_0/FightModule/fightUI").GetComponent<FightUI>();
        floorLayer = GameObject.Find("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/floor").GetComponent<FloorLayer>();
        coverLayer = GameObject.Find("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/cover").GetComponent<CoverLayer>();
        fancelayer = GameObject.Find("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/fence").GetComponent<FanceLayer>();
		floorHideLayer = GameObject.Find("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/floor_hide").GetComponent<FloorHideLayer>();
		monsterLayer = GameObject.Find("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/monster").GetComponent<MonsterLayer>();
		effectLayer = GameObject.Find ("Canvas/layer_0/FightModule/layers/effect").GetComponent<EffectLayer>();
        fightModule = GameObject.Find("Canvas/layer_0/FightModule").GetComponent<FightModule>();

        PropModel.Instance.PropRefreshEvent = PropRefreshEvent;
        PropModel.Instance.PropChangeCellsEvent = PropChangeCellsEvent;
	}

	void Update () {
		
		if (FightModule.crtFightStadus == FightStadus.ready_play)
		{
			PlayEliminate();
            return;
		}

        if (FightModule.crtFightStadus == FightStadus.prop_clear)
        {
            PlayPropClear();
            return;
        }

        if (BattleModel.Instance.crtBattle.need_open_fill)
        {
            if (FightModule.crtFightStadus == FightStadus.idle)
            {
                BattleModel.Instance.crtBattle.need_open_fill = false;
                CellModel.Instance.UpdateBlankList();
                Filling(FightStadus.open_fill);
				CellModel.Instance.Clear();
            }
        }
	}

    public override void ShowList()
    {
		Debug.Log ("ShowList");
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
                CreateCellItem(CellModel.Instance.allCells[i][j]);
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
			FightCellItem itemCtr = item.GetComponent<FightCellItem>();
			
			if(!itemCtr.cell_info.isBlank && itemCtr.cell_info.rotate >= 0 )
			{
				itemCtr.zrotate = itemCtr.cell_info.rotate * FightConst.ROTATE_BASE;
			}else
			{
				itemCtr.zrotate = itemCtr.cell_info.config.rotate * FightConst.ROTATE_BASE;
			}
		}
	}

    public void ClearListStadus()
    {
        int i;
        for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
        {
            for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
            {
                FightCellItem itemCtr = GetItemByRunId(CellModel.Instance.allCells[i][j].runId);
                if (itemCtr != null)
                {
                    itemCtr.stadus = CellItemStadus.normal;
                    itemCtr.index.text = "";
                }
            }
        }
    }

    private GameObject CreateCellItem(CellInfo cellInfo)
    {
		if (cellInfo.configId <= 0 || cellInfo.configId == 10001)
        {
            return null;
        }

        GameObject item = list.NewItem();
        item.name = cellInfo.posX + "_" + cellInfo.posY;
        FightCellItem itemCtr = item.AddComponent<FightCellItem>();
        itemCtr.type = type;

        UpdateCellInfo(itemCtr, cellInfo,true);
        return item;
    }

    private void UpdateCellInfo(FightCellItem itemCtr, CellInfo cellInfo, bool create)
    {
        itemCtr.cell_info = cellInfo;
        if (cellInfo.changer > 0)
        {
            itemCtr.icon = cellInfo.changer;
        }
        else
        {
            itemCtr.icon = cellInfo.config.icon;
        }
        itemCtr.UpdateTip();
        itemCtr.zrotate = cellInfo.config.rotate * FightConst.ROTATE_BASE;
        PosUtil.SetFightCellPos(itemCtr.transform, cellInfo.posX, cellInfo.posY);
		
        if (create)
        {
            itemCtr.stadus = CellItemStadus.normal;
        }
    }

    private FightCellItem GetItemByRunId(int runId)
    {
        List<GameObject> items = list.items;

        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = (GameObject)items[i];
            if (item == null)
            {
                continue;
            }
            FightCellItem cellItemCtr = item.GetComponent<FightCellItem>();

            if (cellItemCtr.cell_info.runId == runId)
            {
                return cellItemCtr;
            }
        }
        return null;
    }

	public void RollInCell(CellInfo cellInfo)
	{
		FightCellItem cellItemCtr = GetItemByRunId(cellInfo.runId);
		cellItemCtr.stadus = CellItemStadus.over;
		cellItemCtr.index.text = "" + CellModel.Instance.lineCells.Count;
	}

	public void RollBackCell(CellInfo cellInfo)
	{
		FightCellItem cellItemCtr = GetItemByRunId(cellInfo.runId);
		cellItemCtr.stadus = CellItemStadus.normal;
		cellItemCtr.index.text = "";
	}

	public void ChangeCells(List<CellInfo> cells)
    {
        if (cells != null)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                CellInfo cellInfo = cells[i];

                FightCellItem itemCtr = GetItemByRunId(cellInfo.runId);

                if (itemCtr != null)
                {
                    UpdateCellInfo(itemCtr, cellInfo,false);
                }
            }
			effectLayer.ChangeBombs(cells);
        }
    }

	private void PlayEliminate()
	{
		FuncEliminate.Eliminate();

		List<List<CellAnimInfo>> cellAnims = CellModel.Instance.anims;
		List<List<FloorAnimInfo>> floorAnims = FloorModel.Instance.anims;
		List<List<WallAnimInfo>> wallAnims = WallModel.Instance.anims;
		List<List<CoverAnimInfo>> coverAnims = CoverModel.Instance.anims;
		List<List<MonsterAnimInfo>> monsterAnims = MonsterModel.Instance.anims;

		rootAction = new OrderAction();
		for (int i = 0; i < cellAnims.Count; i++)
		{
			ParallelAction paralle = new ParallelAction();

			List<CellAnimInfo> cellAnimss = cellAnims[i];
			int j;
			for (j = 0; j < cellAnimss.Count; j++)
			{
				CellAnimInfo animInfo = cellAnimss[j];
				FightCellItem item = GetItemByRunId(animInfo.runId);
				if(item == null && animInfo.animationType != CellAnimType.nullbomb)
				{
					continue;//todo不因该有这个判断 变色技能需要再研究
				}

				OrderAction order = new OrderAction();

				switch (animInfo.animationType)
                {
                case CellAnimType.clear:
                    order.AddNode(new PlaySoundActor("clear"));
                    order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));
                    FightEffectItem effectItem = effectLayer.GetEffectItemByPos(animInfo.toInfo.posX, animInfo.toInfo.posY);
                    order.AddNode(new ShowBombActor(item,effectItem,false));
                    order.AddNode(new ShowEffectActor(item.transform, "effect_cell_clear", fightModule.transform));
                    order.AddNode(new DestroyActor(item.gameObject));
                    break;
                case CellAnimType.wreck:
                    order.AddNode(new PlaySoundActor("wreck"));
                    order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));
                    effectItem = effectLayer.GetEffectItemByPos(animInfo.toInfo.posX, animInfo.toInfo.posY);
                    order.AddNode(new ShowBombActor(item,effectItem,false));
                    order.AddNode(new ShowEffectActor(item.transform, "effect_cell_bomb", fightModule.transform));
                    order.AddNode(new ChangeCellActor(item, animInfo.toInfo));
                    if (animInfo.toInfo.isBlank)
                    {
                        order.AddNode(new DestroyActor(item.gameObject));
                    }
                    break;
                case CellAnimType.bomb:
                    order.AddNode(new PlaySoundActor("wreck"));
                    order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(1.2f, 1.2f, 1), 0.1f));
                    effectItem = effectLayer.GetEffectItemByPos(animInfo.toInfo.posX, animInfo.toInfo.posY);
                    order.AddNode(new ShowBombActor(item,effectItem,false));
                    order.AddNode(new ShowEffectActor(item.transform, "effect_cell_bomb", fightModule.transform));
                    order.AddNode(new ChangeCellActor(item, animInfo.toInfo));
                    if (animInfo.toInfo.isBlank)
                    {
                        order.AddNode(new DestroyActor(item.gameObject));
                    }
                    break;
                case CellAnimType.nullbomb:
                    effectItem = effectLayer.GetEffectItemByPos(animInfo.toInfo.posX, animInfo.toInfo.posY);
                    order.AddNode(new ShowBombActor(item,effectItem,false));   
                    break;
                }

                if (item != null && item.cell_info.isBlank)
				{
					if (item.cell_info.timer > 0)
					{
						order.AddNode(new AddStepActor(item.cell_info.addCount));
						order.AddNode(new WaitActor(200));
					}
				}

				paralle.AddNode(order);
			}

			floorLayer.PlayEliminate(floorAnims[i], paralle);
			coverLayer.PlayEliminate(coverAnims[i], paralle);
			fancelayer.PlayEliminate(wallAnims[i], paralle);
			monsterLayer.PlayEliminate(monsterAnims[i], paralle);

			rootAction.AddNode(paralle);
		}

		ExecuteAction(FightStadus.eliminate);																								
	}

    private void PlayPropClear()
    {
        List<List<CellAnimInfo>> cellAnims = CellModel.Instance.anims;
        rootAction = new OrderAction();
        for (int i = 0; i < cellAnims.Count; i++)
        {
            ParallelAction paralle = new ParallelAction();

            List<CellAnimInfo> cellAnimss = cellAnims[i];
            int j;
            for (j = 0; j < cellAnimss.Count; j++)
            {
                CellAnimInfo animInfo = cellAnimss[j];
                FightCellItem item = GetItemByRunId(animInfo.runId);

                OrderAction order = new OrderAction();

                switch (animInfo.animationType)
                {
                    case CellAnimType.clear:
                        order.AddNode(new PlaySoundActor("bomb"));
						order.AddNode(new ShowEffectActor(item.transform, "effect_cell_clear", fightModule.transform));
                        order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));
                        order.AddNode(new DestroyActor(item.gameObject));
                        break;
                }
                paralle.AddNode(order);
            }
            rootAction.AddNode(paralle);
        }

        ExecuteAction(FightStadus.prop_eliminate);	
    }

    private void DeductStep()
    {
        isDeductStep = true;
        bool isSkillDeduct = FuncDeduct.Deduct();
        fightUI.UpdateCollect(true,true);
        fightUI.UpdateCollect(false, true, isSkillDeduct);

        List<List<CellAnimInfo>> cellAnims = CellModel.Instance.anims;
        rootAction = new OrderAction();
        for (int i = 0; i < cellAnims.Count; i++)
        {
            ParallelAction paralle = new ParallelAction();

            List<CellAnimInfo> cellAnimss = cellAnims[i];
            int j;
            for (j = 0; j < cellAnimss.Count; j++)
            {
                CellAnimInfo animInfo = cellAnimss[j];
                FightCellItem item = GetItemByRunId(animInfo.runId);
				if(item != null)
				{
					OrderAction order = new OrderAction();

					FightEffectItem effectItem = effectLayer.FindEffectItemByPos(animInfo.toInfo.posX, animInfo.toInfo.posY);

					order.AddNode(new ShowBombActor(item,effectItem));
					order.AddNode(new WaitActor(300));
					order.AddNode(new ShowBombActor(item,effectItem,false));
					order.AddNode(new ChangeCellActor(item, animInfo.toInfo));
					if (animInfo.toInfo.isBlank)
					{
						order.AddNode(new ShowEffectActor(item.transform, "effect_cell_clear", fightModule.transform));
						order.AddNode(new DestroyActor(item.gameObject));
					}

					paralle.AddNode(order);
				}
            }
            rootAction.AddNode(new PlaySoundActor("wreck"));
            rootAction.AddNode(paralle);
        }
        
        ExecuteAction(FightStadus.deduct);
    }

    private void EndWin()
    {
        rootAction = new OrderAction();
        WaitActor waitActor = new WaitActor(1000);
        rootAction.AddNode(waitActor);

        //PromptModel.Instance.Pop(LanguageUtil.GetTxt(11402));
        ExecuteAction(FightStadus.end_win);
    }

	private void ExecuteAction(FightStadus fightStadus)
	{
		FightModule.crtFightStadus = fightStadus;
		rootAction.EventHandler += OnEventHandler;
		rootAction.OnExecute();
	}

	private void OnEventHandler(ActionNode node, ActionEventType eventType)
	{
		switch (eventType)
		{
		case ActionEventType.action_end:
			switch (FightModule.crtFightStadus)
			{
			case FightStadus.eliminate:
                AudioModel.Instance.ClearAllActorSound();
                SkillModel.Instance.SkillEnd();
				PlayPutAutoSkill();
				break;
            case FightStadus.prop_eliminate:
                Filling(FightStadus.prop_move);
                break;
            case FightStadus.deduct:
                Filling();
                break;
			case FightStadus.auto_skill:
				UnlockHide();
				break;
			case FightStadus.unlock_hide:
                UnlockMonster(false);
				fightUI.skillListPart.gameObject.SetActive(false);
                break;
            case FightStadus.unlock_monster:
				fightUI.CollectUnlocks(HideModel.Instance.backUpUnLocks);
				fightUI.CollectUnlocks(MonsterModel.Instance.backUpUnLocks);
				Filling();
                break;
			case FightStadus.move:
                Crawl();
                break;
            case FightStadus.crawl:
                CreateCrawl();
                UnlockMonster(true);
                break;
            case FightStadus.jet_monster:
				Invade();
				break;
            case FightStadus.invade:
                Flow();
                break;
            case FightStadus.floor_flow:
                Changer();
                break;
			case FightStadus.changer:
				CoverMove();
				break;
            case FightStadus.coverMove:
            case FightStadus.prop_move:
                AudioModel.Instance.ClearAllActorSound();
                if (FightModel.Instance.fightInfo.isWin)
                {
					SkillModel.Instance.SkillEnd();
					if (FightModel.Instance.fightInfo.stepLeft > 0 || SkillModel.Instance.fighting_entitys.Count > 0)
                    {//扣除剩余次数
                        DeductStep();
                    }
                    else
                    {//完成退出
                        EndWin();
                    }
                }
                else
                {
					CellModel.Instance.Clear();
                    if (FightModel.Instance.fightInfo.stepLeft > 0)
                    {//继续
                        FightModule.crtFightStadus = FightStadus.idle;
                        if (fightModule.CheckRoll())
                        {
                            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11403));
                        }
                        else
                        {
							CheckAutoRefresh();
                        }
                    }
                    else
                    {//购买次数还是退出
						PropInfo propInfo = PropModel.Instance.GetProp(10004);
						if(propInfo.isForbid)
						{
							ModuleModel.Instance.AddUIModule((int)ModuleEnum.REPORT);
						}else
						{
							CheckAutoRefresh();
							fightUI.stepShortPart.Show(true);
						}
                    }
                    ClearListStadus();
                }
                
				break;
            case FightStadus.prop_refresh:
				FightModule.crtFightStadus = FightStadus.idle;
				break;
            case FightStadus.open_fill:
				FightModule.crtFightStadus = FightStadus.idle;
				CheckAutoRefresh();
				break;
            case FightStadus.end_win:
                ModuleModel.Instance.AddUIModule((int)ModuleEnum.REPORT);
                break;
			}
			break;
		}
	}

	public void CheckAutoRefresh()
	{
		if (FuncCheckDead.IsDead())
		{
            PropModel.Instance.RefreshCell();
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11404));
			Refresh(500);
		}
	}

	private void PlayPutAutoSkill()
	{
		rootAction = new OrderAction();
		
		WaitActor waitActor = new WaitActor(200);
		rootAction.AddNode(waitActor);

        List<SkillEntityInfo> skillEntitys = SkillModel.Instance.GetNewSkillEntitys();
		fightUI.ShowSkill();
		
		
		if (lastTouchCell != null && skillEntitys.Count > 0)
        {
            ParallelAction parallelAction = new ParallelAction();
            int holdIndex = 0;
            bool lastIsHump = lastTouchCell.IsHump();
            for (int i = 0; i < skillEntitys.Count; i++)
            {
                OrderAction skillOrder = new OrderAction();

                SkillEntityInfo skillEntity = skillEntitys[i];

                Vector2 addPos = new Vector2();

                for (int h = holdIndex; h < 19; h++)
                {
                    Vector2 holePos = FightConst.GetHoleByLevel(h, lastIsHump);

                    Vector2 checkPos = new Vector2(lastTouchCell.posX + holePos.x, lastTouchCell.posY - holePos.y);

                    CellInfo checkCell = CellModel.Instance.GetCellByPos((int)checkPos.x, (int)checkPos.y);

                    if (checkCell != null && checkCell.isBlank)
                    {
                        addPos = checkPos;
                        holdIndex = h + 1;
                        break;
                    }
                }

                CellInfo addItem = CellModel.Instance.AddItem(skillEntity.seed.config_cell_item.id, (int)addPos.x, (int)addPos.y);

                SkillModel.Instance.ThrowSkillEntity(skillEntity, addItem);

                GameObject itemObj = CreateCellItem(addItem);
				itemObj.transform.SetParent(effectLayer.transform,false);
                Vector2 toPos = PosUtil.GetFightCellPos(addItem.posX, addItem.posY);
                PosUtil.SetCellPos(itemObj.transform, skillEntity.seed.seed_x, skillEntity.seed.seed_y,1.0f);
                
                rootAction.AddNode(new PlaySoundActor("Useskill"));

                rootAction.AddNode(new ShowEffectActor(itemObj.transform, "effect_skill_fly"));

                rootAction.AddNode(new MoveActor((RectTransform)itemObj.transform, new Vector3(toPos.x, toPos.y, 0),1200));
                
                skillOrder.AddNode(new SetLayerActor(itemObj.transform,transform));
                skillOrder.AddNode(new PlaySoundActor("PutAutoSkill"));
                skillOrder.AddNode(new ClearEffectActor(itemObj.transform, "effect_skill_fly"));
                skillOrder.AddNode(new ScaleActor((RectTransform)itemObj.transform, new Vector3(1.2f, 1.2f, 0), 0.2f));
                skillOrder.AddNode(new ScaleActor((RectTransform)itemObj.transform, new Vector3(1, 1, 0), 0.1f));

                parallelAction.AddNode(skillOrder);
            }
            rootAction.AddNode(parallelAction);
        }
        
		waitActor = new WaitActor(200);
		rootAction.AddNode(waitActor);
		ExecuteAction(FightStadus.auto_skill);
	}

	private void UnlockHide()
	{
		List<int> unLockIds = HideModel.Instance.UnLock();
		HideModel.Instance.BackUpUnLock(unLockIds);
		rootAction = new OrderAction();
		ParallelAction paralle = new ParallelAction();
		for (int j = 0; j < unLockIds.Count; j++)
		{
			int unLockId = unLockIds[j];
			FightHideItem item = floorHideLayer.GetItemByRunId(unLockId);
			OrderAction unlockOrder = new OrderAction ();
			if (item.hiderInfo.isNull)
			{
				unlockOrder.AddNode(new SetLayerActor(item.transform, effectLayer.transform));
				unlockOrder.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(1.25f, 1.25f, 0), 0.15f));
				unlockOrder.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.25f));
				unlockOrder.AddNode (new DestroyActor (item));
			}
			paralle.AddNode (unlockOrder);
		}
		rootAction.AddNode (paralle);
		ExecuteAction(FightStadus.unlock_hide);
	}

    private void UnlockMonster(bool isJet = false)
    {
        List<int> unLockIds = MonsterModel.Instance.UnLock(isJet);
        MonsterModel.Instance.BackUpUnLockMonster(unLockIds);
        rootAction = new OrderAction();
        for (int j = 0; j < unLockIds.Count; j++)
        {
            int monsterRunId = unLockIds[j];
            FightMonsterItem monsterItem = monsterLayer.GetItemByRunId(monsterRunId);
            rootAction.AddNode(new SetLayerActor(monsterItem.transform, effectLayer.transform));
            
            CellInfo monsterCell = new CellInfo();
            monsterCell.posX = monsterItem.monsterInfo.posX;
            monsterCell.posY = monsterItem.monsterInfo.posY;
            if (isJet)
            {
				if (monsterItem.monsterInfo.IsNull())
				{
					CellDirType dirType = WallModel.Instance.GetGapWallDir(monsterCell);
					int zrotate = PosUtil.GetRotateByDir(dirType);
					rootAction.AddNode(new RoatateActor((RectTransform)monsterItem.transform, new Vector3(0, 0, zrotate), 0.25f));
				}else
				{
					rootAction.AddNode(new ScaleActor((RectTransform)monsterItem.transform, new Vector3(1.15f, 1.15f, 1f), 0.2f));
				}
            }

            List<CellInfo> releaseList = MonsterModel.Instance.ReleaseList(monsterRunId);
            if (releaseList.Count > 0)
            {
                ParallelAction paralle = new ParallelAction();
                for (int i = 0; i < releaseList.Count; i++)
                {
                    CellInfo cellInfo = releaseList[i];
                    FightCellItem item = GetItemByRunId(cellInfo.runId);
                    if(item == null)
                    {
                        GameObject itemObj = CreateCellItem(cellInfo);
                        item = itemObj.GetComponent<FightCellItem>();
                    }
                    OrderAction order = new OrderAction();
                    order.AddNode(new PlaySoundActor("Refresh"));

                    order.AddNode(new ShowEffectLineActor(effectLayer, cellInfo, monsterCell, monsterItem.monsterInfo.releaseId));

                    order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.1f));

                    order.AddNode(new ChangeCellActor(item, cellInfo, monsterItem.monsterInfo.releaseId));

                    paralle.AddNode(order);
                }
                rootAction.AddNode(paralle);
            }
            if (monsterItem.monsterInfo.IsNull())
            {
				rootAction.AddNode(new ScaleActor((RectTransform)monsterItem.transform, new Vector3(1.25f, 1.25f, 0), 0.15f));
                if (j == 0)
                {
                    rootAction.AddNode(new ScaleActor((RectTransform)monsterItem.transform, new Vector3(0, 0, 0), 0.25f));
                }else
                {
                    rootAction.AddNode(new ScaleActor((RectTransform)monsterItem.transform, new Vector3(0, 0, 0), 0.15f));
                }
            }
            else
            {
                rootAction.AddNode(new ScaleActor((RectTransform)monsterItem.transform, new Vector3(1, 1, 1), 0.05f));

                CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(monsterItem.monsterInfo.posY, monsterItem.monsterInfo.posX);

                FightCoverItem coverItem = coverLayer.GetItemByRunId(coverInfo.runId);

                rootAction.AddNode(new ChangeCoverActor(coverLayer,coverItem, coverInfo));
                rootAction.AddNode(new ProgressMonsterActor(monsterItem, monsterItem.monsterInfo.progress));
                rootAction.AddNode(new SetLayerActor(monsterItem.transform, monsterLayer.transform));
            }
        }
        if (isJet)
        {
            ExecuteAction(FightStadus.jet_monster);
        }
        else
        {
            ParallelAction paralleTimer = new ParallelAction();

            List<CellInfo> timerCells = CellModel.Instance.Timing();
            for (int i = 0; i < timerCells.Count; i++)
            {
                CellInfo cellInfo = timerCells[i];
                FightCellItem item = GetItemByRunId(cellInfo.runId);
                if (item != null)
                {
                    OrderAction order = new OrderAction();
                    order.AddNode(new PlaySoundActor("Refresh"));
                    order.AddNode(new ChangeCellActor(item, cellInfo));
                    if (cellInfo.isBlank)
                    {
                        order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.2f));
                        order.AddNode(new DestroyActor(item.gameObject));
                    }
                    paralleTimer.AddNode(order);
                }
            }

            List<CoverInfo> timerCovers = CoverModel.Instance.Timing();
            for (int i = 0; i < timerCovers.Count; i++)
            {
                CoverInfo coverInfo = timerCovers[i];
                FightCoverItem item = coverLayer.GetItemByRunId(coverInfo.runId);

                OrderAction order = new OrderAction();
                order.AddNode(new PlaySoundActor("Refresh"));
                order.AddNode(new ChangeCoverActor(coverLayer,item, coverInfo));

                if (coverInfo.IsNull())
                {
                    order.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.2f));
                    order.AddNode(new DestroyActor(item.gameObject));

                    List<CoverInfo> covers = CoverModel.Instance.GetNeighbors(coverInfo);
                    for (int n = 0; n < covers.Count;n++ )
                    {
                        CoverInfo cover = covers[n];
                        if (cover != null)
                        {
                            item = coverLayer.GetItemByRunId(cover.runId);
                            order.AddNode(new ChangeCoverActor(coverLayer, item, cover));
                            if (cover.config != null)
                            {
                                coverFlowInterrupt = true;
                            }
                        }
                    }
                }
                paralleTimer.AddNode(order);
            }

            rootAction.AddNode(paralleTimer);

			if(coverFlowInterrupt)
			{
				rootAction.AddNode(new FuncActor(coverLayer.ShowList));//todo 爆后流动导致多出蜘蛛网
			}

            ExecuteAction(FightStadus.unlock_monster);
        }
    }

    private void CreateCrawl()
    {
        List<MonsterInfo> createMonsters = MonsterModel.Instance.CrawlCreate();

        for(int i = 0;i<createMonsters.Count;i++)
        {
            MonsterInfo createInfo = createMonsters[i];
            GameObject monsterItem = monsterLayer.CreateMonsterItem(createInfo.posX, createInfo.posY, createInfo);
            monsterItem.transform.localScale = new Vector3(0, 0, 1);
            LeanTween.scale(monsterItem, new Vector3(1, 1, 1), 0.2f);

            CellInfo cellInfo = CellModel.Instance.GetCellByPos(createInfo.posX, createInfo.posY);
            FightCellItem itemCtr = GetItemByRunId(cellInfo.runId);
			if(itemCtr != null)
			{
				UpdateCellInfo(itemCtr, cellInfo, false);
			}else
			{
				CreateCellItem(cellInfo);
			}
        }
    }

    private void Crawl()
    {
        rootAction = new OrderAction();
        if (!isDeductStep)
        {
            MonsterModel.Instance.Crawl();

            List<MonsterCrawlInfo> crawAnims = MonsterModel.Instance.crawAnims;

            rootAction = new OrderAction();
            ParallelAction paralle = new ParallelAction();

            for (int i = 0; i < crawAnims.Count; i++)
            {
                MonsterCrawlInfo crawAnim = crawAnims[i];

                OrderAction orderAction = new OrderAction();
                paralle.AddNode(orderAction);

                FightMonsterItem monsterItem = monsterLayer.GetItemByRunId(crawAnim.monster.runId);

                for (int j = 0; j < crawAnim.pathCells.Count; j++)
                {
                    CellInfo pathCell = crawAnim.pathCells[j];
                    Vector2 toPos = PosUtil.GetFightCellPos(pathCell.posX, pathCell.posY);

                    float zrotate = 0;
                    if (j > 0)
                    {
                        Vector2 fromPos = PosUtil.GetFightCellPos(crawAnim.pathCells[j - 1].posX, crawAnim.pathCells[j - 1].posY);
                        zrotate = PosUtil.VectorAngle(new Vector2(fromPos.x, fromPos.y), new Vector2(toPos.x, toPos.y));
                    }
                    else
                    {
                        Vector2 anchoredPos = ((RectTransform)monsterItem.transform).anchoredPosition;
                        zrotate = PosUtil.VectorAngle(new Vector2(anchoredPos.x, anchoredPos.y), new Vector2(toPos.x, toPos.y));
                    }

                    orderAction.AddNode(new RotationActor((RectTransform)monsterItem.transform, zrotate));

                    float speed = 600;
                    orderAction.AddNode(new MoveActor((RectTransform)monsterItem.transform, new Vector3(toPos.x, toPos.y, 0), speed));

                    if (pathCell.isBlank == false)
                    {
                        FightCellItem cellItem = GetItemByRunId(pathCell.runId);
                        pathCell.SetConfig((int)crawAnim.monster.releaseList[0].id);
						pathCell.changer = 0;
                        orderAction.AddNode(new ChangeCellActor(cellItem, pathCell));
                    }
                }

                if (crawAnim.roadEnd)
                {
                    orderAction.AddNode(new ScaleActor((RectTransform)monsterItem.transform, new Vector3(0, 0, 0), 0.3f));
                    orderAction.AddNode(new ChangeMonsterActor(monsterItem, crawAnim.monster));
                }
            }

            rootAction.AddNode(paralle);
        }
        
        ExecuteAction(FightStadus.crawl);
    }

    private void PropRefreshEvent()
	{
        Refresh();
	}

    private void PropChangeCellsEvent(List<CellInfo> cells)
    {
        ChangeCells(cells);
        Refresh(0, cells);
        CheckAutoRefresh();
    }

    private void Changer()
    {
        List<CellInfo> cells = new List<CellInfo>();

        if(!isDeductStep)
        {
            cells = FuncChanger.ChangeList();
        }

        ChangeCells(cells);
        Refresh(0, cells, FightStadus.changer);
    }

	private void CoverMove()
	{
        if (!isDeductStep)
        {
            coverLayer.UpdateRate();
            Filling(FightStadus.coverMove, 200);
        }
        else
        {
            Filling(FightStadus.coverMove, 100);
        }
	}

    private void Refresh(int waitmillisecond = 0, List<CellInfo> cells = null, FightStadus fightStadus = FightStadus.prop_refresh)
	{
        bool hasRefresh = false;
		rootAction = new OrderAction();
        ParallelAction scale1 = new ParallelAction();
		ParallelAction movos = new ParallelAction();
        ParallelAction scale2 = new ParallelAction();
        if (cells == null)
        {
            for (int i = 0; i < CellModel.Instance.allCells.Count; i++)
            {
                List<CellInfo> xCells = CellModel.Instance.allCells[i];
                for (int j = 0; j < xCells.Count; j++)
                {
                    CellInfo cellInfo = xCells[j];
                    CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(cellInfo.posY,cellInfo.posX);
                    if (cellInfo.isBlank == false && cellInfo.config.cell_type == (int)CellType.five && coverInfo.IsNull())
                    {
                        hasRefresh = true;

                        FightCellItem item = GetItemByRunId(cellInfo.runId);

                        if (fightStadus == FightStadus.changer)
                        {
                            item.transform.localRotation = Quaternion.identity;
                            scale1.AddNode(new RoatateActor((RectTransform)item.transform, new Vector3(0, 0, 180), 0.2f));
                        }
                        else
                        {
                            scale1.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.3f));
                        }

                        if (fightStadus == FightStadus.changer)
                        {
                            scale2.AddNode(new RoatateActor((RectTransform)item.transform, new Vector3(0, 0, 360), 0.2f));
                        }
                        else
                        {
                            Vector2 toPos = PosUtil.GetFightCellPos(cellInfo.posX, cellInfo.posY);
                            movos.AddNode(new MoveActor((RectTransform)item.transform, new Vector3(toPos.x, toPos.y, 0), 0, 0.1f));

                            scale2.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(1, 1, 1), 0.3f));
                        }
                        
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < cells.Count; j++)
            {
                CellInfo cellInfo = cells[j];
                if (cellInfo.isBlank == false && cellInfo.config.cell_type == (int)CellType.five)
                {
                    hasRefresh = true;

                    FightCellItem item = GetItemByRunId(cellInfo.runId);

                    if (fightStadus == FightStadus.changer)
                    {
                        item.transform.localRotation = Quaternion.identity;
                        scale1.AddNode(new RoatateActor((RectTransform)item.transform, new Vector3(0, 0, 180), 0.2f));
                    }
                    else
                    {
                        scale1.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(0, 0, 0), 0.3f));
                    }

                    
                    if (fightStadus == FightStadus.changer)
                    {
                        scale2.AddNode(new RoatateActor((RectTransform)item.transform, new Vector3(0, 0, 360), 0.2f));
                    }
                    else
                    {
                        Vector2 toPos = PosUtil.GetFightCellPos(cellInfo.posX, cellInfo.posY);
                        movos.AddNode(new MoveActor((RectTransform)item.transform, new Vector3(toPos.x, toPos.y, 0), 0, 0.1f));

                        scale2.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(1, 1, 1), 0.3f));
                    }
                }
            }
        }
        
		if (waitmillisecond > 0) {
			rootAction.AddNode (new WaitActor (waitmillisecond));
		}

        if (hasRefresh)
        {
            rootAction.AddNode(new PlaySoundActor("Refresh"));
        }
        
        rootAction.AddNode(scale1);
        rootAction.AddNode(movos);
        rootAction.AddNode(scale2);

        ExecuteAction(fightStadus);
	}

	private void Filling(FightStadus fightState = FightStadus.move,int waitmillisecond = 0)
	{
        CellModel.Instance.anims = new List<List<CellAnimInfo>>();
		FunMove.Move(false,isDeductStep);
        List<CellMoveInfo> moveAnims = CellModel.Instance.moveAnims;

		rootAction = new OrderAction();
        ParallelAction paralle = new ParallelAction();

        Dictionary<int, int> newStartPos = new Dictionary<int, int>();
        for (int i = 0; i < moveAnims.Count; i++)
		{
            CellMoveInfo cellMoveInfo = moveAnims[i];

            OrderAction orderAction = new OrderAction();
            paralle.AddNode(orderAction);

            FightCellItem item = GetItemByRunId(cellMoveInfo.cellInfo.runId);
            if (item == null)
            {
                item = CreateCellItem(cellMoveInfo.cellInfo).GetComponent<FightCellItem>();
                int xKey = (int)cellMoveInfo.paths[0].x;
                if (newStartPos.ContainsKey(xKey))
                {
                    int preIndex = newStartPos[xKey];
                    newStartPos[xKey] = preIndex - 1;
                }
                else
                {
                    newStartPos.Add(xKey, -1);
                }

                PosUtil.SetFightCellPos(item.transform, xKey, newStartPos[xKey]);
            }

            for (int j = 0; j < cellMoveInfo.paths.Count; j++)
            { 
                Vector2 pathPoint = cellMoveInfo.paths[j];
                Vector2 toPos = PosUtil.GetFightCellPos((int)pathPoint.x, (int)pathPoint.y);
                float speed = isDeductStep ? 1750 : 1350;
                orderAction.AddNode(new MoveActor((RectTransform)item.transform, new Vector3(toPos.x, toPos.y, 0), speed));
            }
            FightEffectItem effectItem = effectLayer.GetEffectItemByPos(cellMoveInfo.cellInfo.posX, cellMoveInfo.cellInfo.posY);
			orderAction.AddNode(new PlayCellMoveEndActor(item,effectItem,cellMoveInfo.cellInfo));
		}
		if (waitmillisecond > 0) {
			rootAction.AddNode (new WaitActor (waitmillisecond));
		}
        rootAction.AddNode(paralle);
        ExecuteAction(fightState);
	}

	private void Invade()
	{
		rootAction = new OrderAction();
		
        if(!isDeductStep)
        {
            ParallelAction paralle = new ParallelAction();
            List<CellAnimInfo> invadeCells = InvadeModel.Instance.EffectInvade();
            for (int i = 0; i < invadeCells.Count; i++)
            {
                ParallelAction paralleMove = new ParallelAction();

                CellInfo fromCell = invadeCells[i].fromInfo;
                CellInfo toCell = invadeCells[i].toInfo;
                //Vector2 fromPos = PosUtil.GetFightCellPos(fromCell.posX, fromCell.posY);
                Vector2 toPos = PosUtil.GetFightCellPos(toCell.posX, toCell.posY);
                FightCellItem item = GetItemByRunId(toCell.runId);
                PosUtil.SetFightCellPos(item.transform, fromCell.posX, fromCell.posY);
                item.icon = toCell.config.icon;
                item.transform.localScale = new Vector3(0.5f, 0.5f, 1);

                OrderAction order1 = new OrderAction();
                order1.AddNode(new PlaySoundActor("Refresh"));
                order1.AddNode(new MoveActor((RectTransform)item.transform, new Vector3(toPos.x, toPos.y, 0), 0, 0.2f));
                paralleMove.AddNode(order1);

                OrderAction order2 = new OrderAction();
                order2.AddNode(new ScaleActor((RectTransform)item.transform, new Vector3(1, 1, 1), 0.15f));
                paralleMove.AddNode(order2);

                paralle.AddNode(paralleMove);
            }
            
            rootAction.AddNode(paralle);
        }
		
		ExecuteAction(FightStadus.invade);	
	}

    private void Flow()
    {
        rootAction = new OrderAction();
        if (!isDeductStep)
        {
            ParallelAction paralle = new ParallelAction();

            floorLayer.Flow(paralle);
            if (coverFlowInterrupt == false)
            {
                coverLayer.Flow(paralle);
            }
            coverFlowInterrupt = false; 

            rootAction.AddNode(paralle);
        }
        
        ExecuteAction(FightStadus.floor_flow);	
    }
}