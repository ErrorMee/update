using System;
using System.Collections.Generic;

using UnityEngine;


public class GridModel : Singleton<GridModel>
{
	public int mapId = 0;

	public FightLayerType brushType = FightLayerType.none;

	public int brushId = 0;


    //view rect
    public short set_start_x = -2;
	public short set_end_x = 2;
    public short set_start_y = 2;
	public short set_end_y = -2;
    //all size
	public short set_battle_width = 5;
	public short set_battle_height = 5;

	public void ToggleChange(bool selected, FightLayerType type, int id,config_item_base itemInfo)
    {
        if (selected)
        {
            if (type == FightLayerType.map)
            {
                mapId = id;

                BattleInfo battleInfo = BattleModel.Instance.GetBattle(mapId);

                BattleModel.Instance.crtBattle = battleInfo;
				BattleModel.Instance.crtConfig = (config_map_item)itemInfo;

                GameObject EditPart = GameObject.FindGameObjectWithTag("EditPart");
                EditPart.GetComponent<EditPart>().ShowList(FightLayerType.all);
            }
            else
            {
                brushType = type;
                brushId = id;
				if(mapId > 0)
				{
					GameObject EditPart = GameObject.FindGameObjectWithTag("EditPart");
					EditPart.GetComponent<EditPart>().ShowList(type);
				}
            }
        }
        else
        {
            if (type == FightLayerType.map)
            {
                mapId = 0;
				GameObject EditPart = GameObject.FindGameObjectWithTag("EditPart");
				EditPart.GetComponent<EditPart>().ShowList(FightLayerType.none);
            }
            else
            {
                brushType = FightLayerType.none;
                brushId = 0;
            }
        }
    }
	
	public void OnItemClick(int posx,int posy,int posn)
	{
		if(mapId <= 0)
		{
			return;
		}
		if(brushType == FightLayerType.none)
		{
			return;
		}
		if(brushId <= 0)
		{
			return;
		}

		//Debug.Log ("mapId " + mapId + " brushType " + brushType + " brushId " + brushId + " posx " + posx + " posy " + posy + " posn " + posn);
		//Debug.Log (" x " + posx + " y " + posy + " n " + posn);

		if (brushType == FightLayerType.fg) {
            int fgId = BattleModel.Instance.crtBattle.fgIds[(posy + (int)PosMgr.Y_HALF_COUNT) * (int)PosMgr.X_FULL_COUNT + (posx + (int)PosMgr.X_HALF_COUNT)];
			if(fgId != brushId && !BattleModel.Instance.crtBattle.InViewRect(posx,posy))
			{
                BattleModel.Instance.crtBattle.fgIds[(posy + (int)PosMgr.Y_HALF_COUNT) * (int)PosMgr.X_FULL_COUNT + (posx + (int)PosMgr.X_HALF_COUNT)] = brushId;
				UpdateAndSaveItem(posx, posy, posn);
			}
		}else {
			int gridx = posx - BattleModel.Instance.crtBattle.start_x;
			int gridy = - posy + BattleModel.Instance.crtBattle.start_y;
			//Debug.Log (" gridx " + gridx + " gridy " + gridy);
			BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[gridy][gridx];

			if(brushType == FightLayerType.bg)
			{
				if (cell.bg_id != brushId) {
					//Debug.Log(cell.cell_id + " to " + brushId);
					cell.bg_id = brushId;
					UpdateAndSaveItem(posx, posy, posn);
				}
			}

			if (brushType == FightLayerType.monster)
			{
				if (cell.monster_id != brushId)
				{
					//Debug.Log(cell.cell_id + " to " + brushId);
					cell.monster_id = brushId;
					UpdateAndSaveItem(posx, posy, posn);
				}
			}


			if(brushType == FightLayerType.floor)
			{
				if (cell.floor_id != brushId) {
					//Debug.Log(cell.cell_id + " to " + brushId);
					cell.floor_id = brushId;
					UpdateAndSaveItem(posx, posy, posn);
				}
			}

            if (brushType == FightLayerType.cover)
            {
                if (cell.cover_id != brushId)
                {
                    //Debug.Log(cell.cell_id + " to " + brushId);
                    cell.cover_id = brushId;
                    UpdateAndSaveItem(posx, posy, posn);
                }
            }

			if(brushType == FightLayerType.cell)
			{
				if (cell.cell_id != brushId) {
					//Debug.Log(cell.cell_id + " to " + brushId);
					cell.cell_id = brushId;
					UpdateAndSaveItem(posx, posy, posn);
				}
			}

			if(brushType == FightLayerType.fence)
			{
				if (cell.walls[posn] != brushId) {
					//Debug.Log(cell.cell_id + " to " + brushId);
					cell.walls[posn] = brushId;
					UpdateAndSaveItem(posx, posy, posn);
				}
			}
		}
	}

	private void UpdateAndSaveItem(int posx,int posy,int posn)
	{
		GameObject editPart = GameObject.FindGameObjectWithTag("EditPart");
		editPart.GetComponent<EditPart>().UpdateItem( posx, posy, posn,brushId);
		//BattleModel.Instance.crtBattle.Save ();
	}
}