using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
//using UnityEditor;

public class SetPart : MonoBehaviour
{
	public Transform close;
	public Transform del;
	public Transform save;

	public Transform clear;
	public Transform random;
	public Transform fill;

	public GameObject toggleItem;
	
	public BaseList list;

	void Awake()
	{
		list.itemPrefab = toggleItem;

		EventTriggerListener.Get(close.gameObject).onClick = OnClose;
		EventTriggerListener.Get(save.gameObject).onClick = OnSave;
		EventTriggerListener.Get(del.gameObject).onClick = OnDel;
		EventTriggerListener.Get(clear.gameObject).onClick = OnClear;
		EventTriggerListener.Get(random.gameObject).onClick = OnRandom;
		EventTriggerListener.Get(fill.gameObject).onClick = OnFill;
	}

	private void OnClose(GameObject go)
	{
		gameObject.SetActive(false);
	}

	private void OnDel(GameObject go)
	{
		int delId = GetItemValueint ("id");
		GridMain.resourceMgr.config_map.DelItem (delId);
		GridMain.resourceMgr.config_map.data.Sort();
		FileUtil.Instance().WriteFile("config/" + GridMain.resourceMgr.config_map.name, JsonMapper.ToJson(GridMain.resourceMgr.config_map), true);

		BattleModel.Instance.crtBattle = null;
		BattleModel.Instance.Delbattle (delId);
		string filePath = FileUtil.Instance().FullName("dat/map/" + delId);
        File.Delete(filePath + ".bytes");

		gameObject.SetActive(false);

		GameObject gridPanel = GameObject.FindGameObjectWithTag("GridMain");
		GridMain gridMain = gridPanel.GetComponent<GridMain> ();
		gridMain.togglePart.UpDateList (FightLayerType.map);

		GameObject EditPart = GameObject.FindGameObjectWithTag("EditPart");
		EditPart.GetComponent<EditPart>().ShowList(FightLayerType.none);

		//AssetDatabase.Refresh();
	}

	private void OnSave(GameObject go)
	{
		Save();
	}

	private void OnClear(GameObject go)
	{
		Save(0);
	}

	private void OnRandom(GameObject go)
	{
		Save(-1);
	}

	private void OnFill(GameObject go)
	{
		Save(UnityEngine.Random.Range(10101, 10101 + 5));
	}

	private void Save(int cellId = -2)
	{
		GridModel.Instance.set_start_x = GetItemValueshort ("start_x");
		GridModel.Instance.set_start_y = GetItemValueshort ("start_y");
		GridModel.Instance.set_end_x = GetItemValueshort ("end_x");
		GridModel.Instance.set_end_y = GetItemValueshort ("end_y");
		GridModel.Instance.set_battle_width = GetItemValueshort ("battle_width");
		GridModel.Instance.set_battle_height = GetItemValueshort ("battle_height");

		bool needBuild = false;

		if(BattleModel.Instance.crtBattle.start_x != GridModel.Instance.set_start_x)
		{
			needBuild = true;
		}
		BattleModel.Instance.crtBattle.start_x = GridModel.Instance.set_start_x;

		if(BattleModel.Instance.crtBattle.start_y != GridModel.Instance.set_start_y)
		{
			needBuild = true;
		}
		BattleModel.Instance.crtBattle.start_y = GridModel.Instance.set_start_y;

		if(BattleModel.Instance.crtBattle.end_x != GridModel.Instance.set_end_x)
		{
			needBuild = true;
		}
		BattleModel.Instance.crtBattle.end_x = GridModel.Instance.set_end_x;

		if(BattleModel.Instance.crtBattle.end_y != GridModel.Instance.set_end_y)
		{
			needBuild = true;
		}
		BattleModel.Instance.crtBattle.end_y = GridModel.Instance.set_end_y;

		if(BattleModel.Instance.crtBattle.battle_width != GridModel.Instance.set_battle_width)
		{
			needBuild = true;
		}
		BattleModel.Instance.crtBattle.battle_width = GridModel.Instance.set_battle_width;

		if(BattleModel.Instance.crtBattle.battle_height != GridModel.Instance.set_battle_height)
		{
			needBuild = true;
		}
		BattleModel.Instance.crtBattle.battle_height = GridModel.Instance.set_battle_height;

		BattleModel.Instance.crtConfig.id = GetItemValueint ("id");
		BattleModel.Instance.crtConfig.icon = GetItemValueint ("icon");
		BattleModel.Instance.crtConfig.name = GetItemValuestring ("name");
		BattleModel.Instance.crtConfig.desc = GetItemValuestring ("desc");
		BattleModel.Instance.crtConfig.pre_id = GetItemValueint ("pre_id");
		BattleModel.Instance.crtConfig.task = GetItemValuestring ("task");
		BattleModel.Instance.crtConfig.step = GetItemValueint ("step");
		BattleModel.Instance.crtConfig.build = GetItemValueint ("build");
		BattleModel.Instance.crtConfig.fill = GetItemValueint ("fill");
		BattleModel.Instance.crtConfig.judge = GetItemValuestring ("judge");
		BattleModel.Instance.crtConfig.forbid_skill = GetItemValuestring("forbid_skill");
		BattleModel.Instance.crtConfig.forbid_prop = GetItemValuestring("forbid_prop");

		gameObject.SetActive(false);

		Debug.Log("needBuild " + needBuild);
		if(needBuild)
		{
			BattleModel.Instance.crtBattle.FillNew(GridModel.Instance.set_start_x,GridModel.Instance.set_start_y,GridModel.Instance.set_end_x,GridModel.Instance.set_end_y,GridModel.Instance.set_battle_width,GridModel.Instance.set_battle_height);
			GameObject EditPart = GameObject.FindGameObjectWithTag("EditPart");
			EditPart.GetComponent<EditPart>().ShowList(FightLayerType.none);
			EditPart.GetComponent<EditPart>().ShowList(FightLayerType.all);
		}else
		{
			List<List<BattleCellInfo>> allCells = BattleModel.Instance.crtBattle.allCells;
			for (int i = 0; i < allCells.Count; i++)
			{
				List<BattleCellInfo> xcells = allCells[i];
				for (int j = 0; j < xcells.Count; j++)
				{
					BattleCellInfo battleCellInfo = xcells[j];

					if(battleCellInfo.cell_id >= 10101 && battleCellInfo.cell_id <= 10105)
					{
						if(cellId == -1)
						{
							battleCellInfo.cell_id = UnityEngine.Random.Range(10101, 10101 + 5);
						}

						if(cellId == 0)
						{
							battleCellInfo.cell_id = 10000;
						}

						if(cellId > 0)
						{
							battleCellInfo.cell_id = cellId;
						}
					}
				}
			}

			GameObject EditPart = GameObject.FindGameObjectWithTag("EditPart");
			EditPart.GetComponent<EditPart>().ShowList(FightLayerType.none);
			EditPart.GetComponent<EditPart>().ShowList(FightLayerType.all);
		}
	}

	public void UpdateMap()
	{
		list.ClearList();

		CreatItem ("start_x",ref BattleModel.Instance.crtBattle.start_x);
		CreatItem ("end_x",ref BattleModel.Instance.crtBattle.end_x);
		CreatItem ("battle_width",ref BattleModel.Instance.crtBattle.battle_width);

		CreatItem ("start_y",ref BattleModel.Instance.crtBattle.start_y);
		CreatItem ("end_y",ref BattleModel.Instance.crtBattle.end_y);
		CreatItem ("battle_height",ref BattleModel.Instance.crtBattle.battle_height);

		CreatItem ("id",ref BattleModel.Instance.crtConfig.id);
		CreatItem ("icon",ref BattleModel.Instance.crtConfig.icon);
		CreatItem ("name",ref BattleModel.Instance.crtConfig.name);
		CreatItem ("desc",ref BattleModel.Instance.crtConfig.desc);

		CreatItem ("pre_id",ref BattleModel.Instance.crtConfig.pre_id);
		CreatItem ("task",ref BattleModel.Instance.crtConfig.task);
		CreatItem ("step",ref BattleModel.Instance.crtConfig.step);
		CreatItem ("build",ref BattleModel.Instance.crtConfig.build);
		CreatItem ("fill",ref BattleModel.Instance.crtConfig.fill);
		CreatItem ("judge",ref BattleModel.Instance.crtConfig.judge);
        CreatItem("forbid_skill", ref BattleModel.Instance.crtConfig.forbid_skill);
        CreatItem("forbid_prop", ref BattleModel.Instance.crtConfig.forbid_prop);
	}

	public void CreatItem(string str,ref int value)
	{
		GameObject item = list.NewItem();
		item.name = str;
		SetItem setItem = item.GetComponent<SetItem> ();
		setItem.SetLabel(str);
		setItem.SetVaule (ref value);
	}

	public void CreatItem(string str,ref short value)
	{
		GameObject item = list.NewItem();
		item.name = str;
		SetItem setItem = item.GetComponent<SetItem> ();
		setItem.SetLabel(str);
		setItem.SetVaule (ref value);
	}

	public void CreatItem(string str,ref string value)
	{
		GameObject item = list.NewItem ();
		item.name = str;
		SetItem setItem = item.GetComponent<SetItem> ();
		setItem.SetLabel (str);
		setItem.SetVaule (ref value);
	}

	public int GetItemValueint(string str)
	{
		GameObject item = list.GetItemByName (str);

		SetItem setItem = item.GetComponent<SetItem> ();

		return int.Parse (setItem.GetValueStr ());
	}

	public short GetItemValueshort(string str)
	{
		GameObject item = list.GetItemByName (str);
		SetItem setItem = item.GetComponent<SetItem> ();
		
		return short.Parse (setItem.GetValueStr ());
	}

	public string GetItemValuestring(string str)
	{
		GameObject item = list.GetItemByName (str);
		
		SetItem setItem = item.GetComponent<SetItem> ();
		
		return setItem.GetValueStr ();
	}
}
