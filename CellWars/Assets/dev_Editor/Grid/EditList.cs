using UnityEngine;
using System.Collections;

public class EditList : MonoBehaviour {

    public BaseList list;

    public GameObject item;

    public FightLayerType type;

    void Awake()
    {
        list.itemPrefab = item;
    }

    void OnEnable()
    {
        if (BattleModel.Instance.crtBattle != null)
        {
            switch (type)
            {
                case FightLayerType.bg:
                    int i;
                    for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
                    {
                        for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
                        {
                            BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[i][j];
                            CreateCellItem(j + BattleModel.Instance.crtBattle.start_x, - i + BattleModel.Instance.crtBattle.start_y, cell.bg_id);
                        }
                    }
					break;
				case FightLayerType.monster:
                    for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
				{
                    for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
					{
                        BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[i][j];

                        config_monster_item config_monster = (config_monster_item)GridMain.resourceMgr.config_monster.GetItem(cell.monster_id);
						if(config_monster != null)
						{
							CreateCellItem(j + BattleModel.Instance.crtBattle.start_x, -i + BattleModel.Instance.crtBattle.start_y, config_monster.icon, config_monster.rotate);
						}else{
							CreateCellItem(j + BattleModel.Instance.crtBattle.start_x, -i + BattleModel.Instance.crtBattle.start_y, cell.monster_id);
						}
					}
				}
				break;

                case FightLayerType.floor:
                for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
				{
                    for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
					{
                        BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[i][j];
                        CreateCellItem(j + BattleModel.Instance.crtBattle.start_x, - i + BattleModel.Instance.crtBattle.start_y, cell.floor_id);
					}
				}
                    break;
                case FightLayerType.cell:
				for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
				{
					for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
					{
						BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[i][j];
                        config_cell_item config_cell = (config_cell_item)GridMain.resourceMgr.config_cell.GetItem(cell.cell_id);

                        CreateCellItem(j + BattleModel.Instance.crtBattle.start_x, -i + BattleModel.Instance.crtBattle.start_y, config_cell.icon, config_cell.rotate);
					}
				}
                    break;
                case FightLayerType.fence:
					for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
					{
						for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
						{
							BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[i][j];
						CreateWallItem(j + BattleModel.Instance.crtBattle.start_x, - i + BattleModel.Instance.crtBattle.start_y, 0,cell.walls[0]);
						CreateWallItem(j + BattleModel.Instance.crtBattle.start_x, - i + BattleModel.Instance.crtBattle.start_y, 1,cell.walls[1]);
						CreateWallItem(j + BattleModel.Instance.crtBattle.start_x, - i + BattleModel.Instance.crtBattle.start_y, 2,cell.walls[2]);
						}
					}
                    break;
                case FightLayerType.cover:
                    for (i = 0; i < BattleModel.Instance.crtBattle.battle_height; i++)
                    {
                        for (int j = 0; j < BattleModel.Instance.crtBattle.battle_width; j++)
                        {
                            BattleCellInfo cell = BattleModel.Instance.crtBattle.allCells[i][j];
						config_cover_item config_cover = (config_cover_item)GridMain.resourceMgr.config_cover.GetItem(cell.cover_id);

						if(config_cover != null)
						{
							CreateCoverItem(j + BattleModel.Instance.crtBattle.start_x, - i + BattleModel.Instance.crtBattle.start_y, config_cover.icon);
						}else{
							CreateCoverItem(j + BattleModel.Instance.crtBattle.start_x, - i + BattleModel.Instance.crtBattle.start_y, cell.cover_id);
						}    
					}
                    }
                    break;
                case FightLayerType.fg:
					int index = 0;
                    for (i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
					{
                        for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
						{
							int id = BattleModel.Instance.crtBattle.fgIds[index];
							CreateCellItem(j, i, id);
							index ++;
						}
					}
                    break;
            }
        }
    }

    protected virtual void CreateCellItem(int posX, int posY, int id, int rotate = 0)
    {
        GameObject item = list.NewItem();
        item.name = posX + "_" + posY;

        EditItem itemCtr = item.GetComponent<EditItem>();
		itemCtr.type = type;
		itemCtr.gridPos = new Vector3 (posX, posY, 0);
        itemCtr.icon = id;
        itemCtr.zrotate = rotate * FightConst.ROTATE_BASE;
		PosMgr.SetCellPos(item.transform,posX, posY,0.4f);

		EventTriggerListener.Get(item).onClick = itemCtr.OnCellClick;
    }

    protected virtual void CreateCoverItem(int posX, int posY, int id)
    {
        GameObject item = list.NewItem();
        item.name = posX + "_" + posY;

        EditItem itemCtr = item.GetComponent<EditItem>();
        itemCtr.type = type;
        itemCtr.gridPos = new Vector3(posX, posY, 0);
        itemCtr.icon = id;
		PosMgr.SetCellPos(item.transform,posX, posY,0.4f);
        EventTriggerListener.Get(item).onClick = itemCtr.OnCellClick;
    }

	protected virtual void CreateWallItem(int posX, int posY,int posN,int id)
	{
		GameObject item = list.NewItem();
		item.name = posX + "_" + posY + "_" + posN;
		
		EditItem itemCtr = item.GetComponent<EditItem>();
		itemCtr.type = type;
		itemCtr.gridPos = new Vector3 (posX, posY, posN);
		itemCtr.icon = id;
		PosMgr.SetWallPos(item.transform,posX, posY,posN,0.4f);
		itemCtr.zrotate = GetZRotate(posN);
		EventTriggerListener.Get(item).onClick = itemCtr.OnCellClick;
	}

	public EditItem GetItemByPos(int posx,int posy,int posn)
	{
		for(int i = 0;i<list.items.Count;i++)
		{
			GameObject item = list.items[i];
			EditItem itemCtr = item.GetComponent<EditItem>();

			if(itemCtr.gridPos.x == posx && itemCtr.gridPos.y == posy && itemCtr.gridPos.z == posn)
			{
				return itemCtr;
			}
		}
		return null;
	}

	private float GetZRotate(int pn)
	{
		switch(pn)
		{
		case 0:
			return -120f;
		case 1:
			return 0f;
		case 2:
			return -60f;
		}
		return 0f;
	}

    void OnDisable()
    {
        list.ClearList();
    }
}
