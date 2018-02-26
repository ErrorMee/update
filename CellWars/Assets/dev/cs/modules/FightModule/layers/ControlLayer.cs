using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ControlLayer : FightBaseLayer
{
	private CellLayer cellLayer;
    private MonsterLayer monsterLayer;
    private LineLayer lineLayer;
	private FightUI fightUI;

	override protected void Awake()
	{
		base.Awake();
		list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "FightControlItem");
	}

	void Start()
	{
		cellLayer = GameObject.Find ("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/cell").GetComponent<CellLayer>();
        lineLayer = GameObject.Find("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/line").GetComponent<LineLayer>();
        monsterLayer = GameObject.Find("Canvas/layer_0/FightModule/layers/fight_mask/animLayer/monster").GetComponent<MonsterLayer>();
		fightUI = GameObject.Find ("Canvas/layer_0/FightModule/fightUI").GetComponent<FightUI>();
	}

	public override void ShowList()
	{
        base.ShowList();

		int index = 0;
        for (int i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
		{
            for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
			{
				int id = BattleModel.Instance.crtBattle.fgIds[index];
				if (id == 0)
				{
					CreateControlItem(j, i);
				}
				index ++;
			}
		}
	}

	private void CreateControlItem(int posX, int posY)
	{
		GameObject item = list.NewItem();
		item.name = posX + "_" + posY;
		
		FightControlItem itemCtr = item.AddComponent<FightControlItem>();
		PosMgr.SetCellPos(item.transform,posX, posY);
        itemCtr.control_x = posX - BattleModel.Instance.crtBattle.start_x;
        itemCtr.control_y = - posY + BattleModel.Instance.crtBattle.start_y;

		EventTriggerListener.Get(item).onDown += ItemDownHander;
		EventTriggerListener.Get(item).onEnter += ItemEnterHander;
		//EventTriggerListener.Get(item).onUp += ItemUpHander;
	}

	private void ItemDownHander(GameObject go)
	{
		if (FightModule.crtFightStadus == FightStadus.idle)
		{
			FightControlItem itemCtr = go.GetComponent<FightControlItem>();
			CellInfo cellInfo = CellModel.Instance.GetCellByPos(itemCtr.control_x,itemCtr.control_y);
            CellLineType lineType = FuncLine.Line(cellInfo,true);
			if (lineType == CellLineType.success)
			{
				EventTriggerListener.Get(go).onUp += ItemUpHander;

                GameMgr.audioMgr.PlayeSound("ItemEnter");
				FightModule.crtFightStadus = FightStadus.line;
				cellLayer.RollInCell(cellInfo);
				List<CellInfo> skillCells = SkillModel.Instance.EnterCell(cellInfo);
				cellLayer.ChangeCells(skillCells);
                fightUI.UpdateCollect(true);
				//fightUI.propsPart.gameObject.SetActive(false);
                monsterLayer.UpdateList();
				cellLayer.UpdateList();
				fightUI.skillListPart.gameObject.SetActive(true);
			}
		}
	}

	private void ItemEnterHander(GameObject go)
	{
		if (CrossPlatformInputManager.GetButton("Fire1") && FightModule.crtFightStadus == FightStadus.line)
		{
			FightControlItem itemCtr = go.GetComponent<FightControlItem>();
			CellInfo cellInfo = CellModel.Instance.GetCellByPos(itemCtr.control_x,itemCtr.control_y);
            CellLineType lineType = FuncLine.Line(cellInfo);
            int lineCount = CellModel.Instance.lineCells.Count;

			if (lineType == CellLineType.success)
			{
                GameMgr.audioMgr.PlayeSound("ItemEnter");
				cellLayer.RollInCell(cellInfo);
				List<CellInfo> skillCells = SkillModel.Instance.OutCell(CellModel.Instance.lineCells[lineCount - 2]);
				cellLayer.ChangeCells(skillCells);
				skillCells = SkillModel.Instance.EnterCell(cellInfo);
				cellLayer.ChangeCells(skillCells);
                lineLayer.ShowLine(CellModel.Instance.lineCells[lineCount - 2], cellInfo);
			}
			
			if (lineType == CellLineType.rollback)
			{
                GameMgr.audioMgr.PlayeSound("rollback");
				cellLayer.RollBackCell(CellModel.Instance.rollbackCell);
				List<CellInfo> skillCells = SkillModel.Instance.OutCell(CellModel.Instance.rollbackCell);
				cellLayer.ChangeCells(skillCells);
				skillCells = SkillModel.Instance.EnterCell(cellInfo);
				cellLayer.ChangeCells(skillCells);
                
                lineLayer.DestroyLine(cellInfo, CellModel.Instance.rollbackCell);
			}
            fightUI.UpdateCollect(true);
            monsterLayer.UpdateList();
			cellLayer.UpdateList();
		}
	}
	
	private void ItemUpHander(GameObject go)
    {
        EventTriggerListener.Get(go).onUp -= ItemUpHander;
        lineLayer.ShowList();
		if (FightModule.crtFightStadus == FightStadus.line)
		{
			List<CellInfo> lineCells = CellModel.Instance.lineCells;
			if (lineCells.Count > 0)
			{
				cellLayer.lastTouchCell = lineCells[lineCells.Count - 1].Copy();
			}
			if (lineCells.Count < GameModel.Instance.GetGameConfig(1003))
			{
				List<CellInfo> lineCellsCopy = new List<CellInfo>();
				for (int i = 0; i < lineCells.Count; i++)
				{
					lineCellsCopy.Add(lineCells[i]);
				}
				
				CellModel.Instance.UndoLineCells();
				
				for (int i = 0; i < lineCellsCopy.Count; i++)
				{
					List<CellInfo> skillCells = SkillModel.Instance.QuitCell(lineCellsCopy[i]);
					cellLayer.ChangeCells(skillCells);
				}
                cellLayer.ClearListStadus();
				FightModule.crtFightStadus = FightStadus.idle;
                SkillModel.Instance.SkillEnd();
                fightUI.UpdateCollect(true);
				monsterLayer.UpdateList();
				cellLayer.UpdateList();
				fightUI.skillListPart.gameObject.SetActive(false);
			}
			else
			{
                fightUI.UpdateCollect(false);
				FightModule.crtFightStadus = FightStadus.ready_play;
			}
			//fightUI.propsPart.gameObject.SetActive(true);
		}
	}
}