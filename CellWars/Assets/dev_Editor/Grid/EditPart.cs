using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EditPart : MonoBehaviour
{

    public EditList listPrefab;

    private Dictionary<FightLayerType, EditList> listDic = new Dictionary<FightLayerType, EditList>();

    void OnEnable()
    {
        CreatList(FightLayerType.bg, 1);
        CreatList(FightLayerType.floor, 2);
        CreatList(FightLayerType.cell, 3);
		CreatList(FightLayerType.monster, 4);
        CreatList(FightLayerType.fence, 5);
        CreatList(FightLayerType.cover, 6);
        CreatList(FightLayerType.fg, 7);
        ShowList(FightLayerType.none);
    }

    private void CreatList(FightLayerType type, int index)
    {
        EditList list = GameObject.Instantiate<EditList>(listPrefab);
        list.type = type;
        list.gameObject.name = "" + type;
        listDic.Add(type, list);
        list.transform.SetParent(transform, false);
    }

    public void ShowList(FightLayerType type)
    {
        foreach (FightLayerType key in listDic.Keys)
        {
            EditList list = listDic[key];
            if (type == FightLayerType.all)
            {
                list.gameObject.SetActive(true);
            }
            else if (type == FightLayerType.none)
            {
                list.gameObject.SetActive(false);
            }
            else if (key == type)
            {
				if(key == FightLayerType.bg)
				{
					list.gameObject.SetActive(false);
				}

                list.gameObject.SetActive(true);
            }
            else
            {
				if(key != FightLayerType.bg)
				{
					list.gameObject.SetActive(false);
				}
            }
        }
    }

	public void UpdateItem(int posx,int posy,int posn,int id)
	{
		EditList list = listDic[GridModel.Instance.brushType];
		EditItem item = list.GetItemByPos( posx, posy, posn);
        if (GridModel.Instance.brushType == FightLayerType.cell)
        {
            config_cell_item config_cell = (config_cell_item)GridMain.resourceMgr.config_cell.GetItem(id);
            item.icon = config_cell.icon;
            item.zrotate = config_cell.rotate * FightConst.ROTATE_BASE;
        }
        else if (GridModel.Instance.brushType == FightLayerType.monster)
        {
            config_monster_item config_monster = (config_monster_item)GridMain.resourceMgr.config_monster.GetItem(id);
            item.icon = config_monster.icon;
            item.zrotate = config_monster.rotate * FightConst.ROTATE_BASE;
        }
		else if (GridModel.Instance.brushType == FightLayerType.cover)
		{
			config_cover_item config_cover = (config_cover_item)GridMain.resourceMgr.config_cover.GetItem(id);
			item.icon = config_cover.icon;
		}
        else
        {
            item.icon = id;
        }
	}
}
