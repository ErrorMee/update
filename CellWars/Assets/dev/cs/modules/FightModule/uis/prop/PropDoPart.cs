using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PropDoPart : MonoBehaviour {

    public Button close;
    public Image frameImage;
    public Image blackDown;

	public Transform skillPart;
	public Transform suspendPart;
	public Transform stepPart;
	public Transform progressPart;
	public Transform taskPart;

    public Image tipBg;
    public Text tipText;

    private CellInfo depth0Cell;

    private int selectDepth = 0;

    private int crtPropId;

	public BaseList list;

	private void Awake()
	{
		list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "PropTouchItem");

        EventTriggerListener.Get(close.gameObject).onClick = OnCloseHander;
        EventTriggerListener.Get(blackDown.gameObject).onClick = OnCloseHander;
	}

    private void OnCloseHander(GameObject go)
    {
        Show(0);
		PropModel.Instance.ChangePropStadus(PropStadus.unSelect);
        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11408));
    }

	public void Show(int propId)
	{
        depth0Cell = null;
        selectDepth = 0;
        crtPropId = propId;

        bool isShow = crtPropId > 0;
		skillPart.gameObject.SetActive (!isShow);
		suspendPart.gameObject.SetActive (!isShow);
		stepPart.gameObject.SetActive (!isShow);
		progressPart.gameObject.SetActive (!isShow);
		taskPart.gameObject.SetActive (!isShow);

		gameObject.SetActive (isShow);
		if(isShow)
		{
			float showWidth = BattleModel.Instance.crtBattle.ShowWidth() * PosMgr.CELL_W * PosMgr.CELL_W_MOD;
			float showOffWidth = BattleModel.Instance.crtBattle.ShowOffsetWidth()*0.5f* PosMgr.CELL_W * PosMgr.CELL_W_MOD;

			float showHeight = BattleModel.Instance.crtBattle.ShowHeight() * PosMgr.CELL_H;
			float showOffHeight = BattleModel.Instance.crtBattle.ShowOffsetHeight()*0.5f* PosMgr.CELL_H;

			frameImage.rectTransform.sizeDelta = new Vector2(showWidth + 100,showHeight + 140);

			frameImage.rectTransform.anchoredPosition = new Vector2(showOffWidth,showOffHeight + 40);

            switch (crtPropId)
            {
                case 10002:
                    tipText.text = LanguageUtil.GetTxt(11409);
                    break;
                case 10003:
                    tipText.text = LanguageUtil.GetTxt(11409);
                    break;
                case 10005:
                    tipText.text = LanguageUtil.GetTxt(11409);
                    break;
            }
            tipBg.rectTransform.sizeDelta = new Vector2(tipText.preferredWidth + 80, tipBg.rectTransform.sizeDelta.y);
            ShowList();
		}else
		{

		}
	}

	public void ShowList()
	{
        selectDepth = 0;

		list.ClearList();

		int i;

        for (i = 0; i < CellModel.Instance.allCells.Count; i++)
		{
			List<CellInfo> cellXs = CellModel.Instance.allCells[i];
			for (int j = 0; j < cellXs.Count; j++)
			{
				CellInfo cellInfo = cellXs[j];
				bool isCoverOpen = CoverModel.Instance.IsOpen(cellInfo.posX,cellInfo.posY);
                if (cellInfo.isBlank == false && cellInfo.isMonsterHold == false && cellInfo.config.cell_type == (int)CellType.five && isCoverOpen)
                {
                    CreateSelectItem(j, i, true);
                }else
				{
					CreateSelectItem(j, i, false);
				}
			}
		}
	}

	private PropTouchItem CreateSelectItem(int posX, int posY, bool valid)
	{
		GameObject item = list.NewItem();
		item.name = posX + "_" + posY;

        PropTouchItem itemCtr = item.GetComponent<PropTouchItem>();

        PosMgr.SetFightCellPos(item.transform, posX, posY);

        itemCtr.valid = valid;

        itemCtr.control_x = posX;
        itemCtr.control_y = posY;

		if (itemCtr.valid) {

			EventTriggerListener.Get (item).onClick = OnClickSelectHander;

		}
		return itemCtr;
	}

    private void OnClickSelectHander(GameObject go)
    {
        PropTouchItem itemCtr = go.GetComponent<PropTouchItem>();

        if (itemCtr.valid)
        {
            CellInfo cellInfo = CellModel.Instance.GetCellByPos(itemCtr.control_x, itemCtr.control_y);
            switch (crtPropId)
            {
                case 10002:

                    CellModel.Instance.anims = new List<List<CellAnimInfo>>();
                    List<CellAnimInfo> stepMoves = new List<CellAnimInfo>();
                    CellModel.Instance.anims.Add(stepMoves);
                    
                    if (cellInfo != null)
                    {
                        cellInfo.isLink = true;
                        CellModel.Instance.lineCells.Add(cellInfo);

                        cellInfo.isBlank = true;
                        CellModel.Instance.AddAnim(cellInfo, CellAnimType.clear);

                        FightModule.crtFightStadus = FightStadus.prop_clear;
                        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11410));
                    }
                    Show(0);
					PropModel.Instance.crtProp.Use();
					PropModel.Instance.ChangePropStadus(PropStadus.unSelect);
                    break;
                case 10003:
                    
                    if (selectDepth == 1)
                    {
                        //change
                        depth0Cell.SetConfig(itemCtr.control_id);
						depth0Cell.changer = 0;
                        List<CellInfo> changeCells = new List<CellInfo>();
                        changeCells.Add(depth0Cell);
                        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11411));
                        PropModel.Instance.PropChangeCellsEvent(changeCells);

                        Show(0);
						PropModel.Instance.crtProp.Use();
						PropModel.Instance.ChangePropStadus(PropStadus.unSelect);
                        return;
                    }

                    if (selectDepth == 0)
                    {
                        depth0Cell = cellInfo;
                        tipText.text = LanguageUtil.GetTxt(11412);
                        tipBg.rectTransform.sizeDelta = new Vector2(tipText.preferredWidth + 80, tipBg.rectTransform.sizeDelta.y);
                        CreatBrushs(cellInfo);
                    }

                    break;
                case 10005:

                    if (selectDepth == 1)
                    {
                        CellModel.Instance.anims = new List<List<CellAnimInfo>>();
                        stepMoves = new List<CellAnimInfo>();
                        CellModel.Instance.anims.Add(stepMoves);

                        CellModel.Instance.SwitchPos(depth0Cell, cellInfo);
                        depth0Cell.SwitchPos(cellInfo);

                        List<CellInfo> changeCells = new List<CellInfo>();
                        changeCells.Add(depth0Cell);
                        changeCells.Add(cellInfo);

                        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11413));
                        PropModel.Instance.PropChangeCellsEvent(changeCells);

                        Show(0);
						PropModel.Instance.crtProp.Use();
						PropModel.Instance.ChangePropStadus(PropStadus.unSelect);
                        return;
                    }

                    if (selectDepth == 0)
                    {

                        List<CellInfo> neighbors = CellModel.Instance.GetNeighbors(cellInfo);

                        bool hasSwitch = ShowSwitchList(neighbors, cellInfo);

                        if (hasSwitch == true)
                        {
                            tipText.text = LanguageUtil.GetTxt(11414);
                            tipBg.rectTransform.sizeDelta = new Vector2(tipText.preferredWidth + 80, tipBg.rectTransform.sizeDelta.y);
                            depth0Cell = cellInfo;
                        }
                        else
                        {
							PromptModel.Instance.Pop(LanguageUtil.GetTxt(11415));
                            ShowList();
                        }
                    }

                    break;
            }
        }
        else
        {
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11415));
        }
    }

    private void CreatBrushs(CellInfo centerCell)
    {
        selectDepth = 1;

        list.ClearList();

        List<int> posIndexs = new List<int>() { 5, 6, 2, 3 };

        if (centerCell.posX < 1)
        {
            posIndexs = new List<int>() { 1, 2, 3, 4 };
            if (centerCell.posY < 1)
            {
                posIndexs = new List<int>() { 3, 4, 12, 13 };
            }
            if (centerCell.posY >= (BattleModel.Instance.crtBattle.ShowHeight() - 1))
            {
                posIndexs = new List<int>() { 1, 2, 7, 8 };
            }
        } else if (centerCell.posX >= (BattleModel.Instance.crtBattle.ShowWidth() - 1))
        {
            posIndexs = new List<int>() { 1, 6, 5, 4 };
            if (centerCell.posY < 1)
            {
                posIndexs = new List<int>() { 14, 13, 5, 4 };
            }
            if (centerCell.posY >= (BattleModel.Instance.crtBattle.ShowHeight() - 1))
            {
                posIndexs = new List<int>() { 1, 6, 7, 18 };
            }
        }
        else {
            if (centerCell.posY < 1)
            {
                posIndexs = new List<int>() { 14, 12, 5, 3 };
            }if (centerCell.posY >= (BattleModel.Instance.crtBattle.ShowHeight() - 1))
            {
                posIndexs = new List<int>() { 2, 6, 8, 18 };
            }
        }

        List<int> controlIds = new List<int>() { 10101, 10102, 10103, 10104, 10105 };
		List<Vector2> brushPoss = new List<Vector2> ();
        for (int i = 0; i < posIndexs.Count; i++)
        {
            int posIndex = posIndexs[i];

            Vector2 offsetPos;
            if (centerCell.IsHump())
            {
                if (posIndex >= FightConst.RING_HUMP[0].Count)
                {
                    offsetPos = FightConst.RING_HUMP[1][posIndex - FightConst.RING_HUMP[0].Count];
                }
                else {
                    offsetPos = FightConst.RING_HUMP[0][posIndex];
                }
                
            }
            else {
                if (posIndex >= FightConst.RING_VALLEY[0].Count)
                {
                    offsetPos = FightConst.RING_VALLEY[1][posIndex - FightConst.RING_VALLEY[0].Count];
                }
                else
                {
                    offsetPos = FightConst.RING_VALLEY[0][posIndex];
                }
            }

            int controlId = 0;
            for (int j = 0; j < controlIds.Count; j++)
            {
                int tempId = controlIds[j];

                if (tempId != centerCell.configId)
                {
                    controlId = tempId;
                    controlIds.RemoveAt(j);
                    break;
                }
             }

			brushPoss.Add(CreateBrushItem(centerCell.posX, centerCell.posY, (int)offsetPos.x, (int)offsetPos.y, controlId));
        }

		for (int i = 0; i < CellModel.Instance.allCells.Count; i++)
		{
			List<CellInfo> cellXs = CellModel.Instance.allCells[i];
			for (int j = 0; j < cellXs.Count; j++)
			{
				bool has = false;
				CellInfo cellInfo = cellXs[j];
				for(int n = 0;n<brushPoss.Count;n++)
				{
					Vector2 brushPos = brushPoss[n];
					if((int)brushPos.x == cellInfo.posX && cellInfo.posY == (int)brushPos.y )
					{
						has = true;
						break;
					}
				}
				if(has == false)
				{
					PropTouchItem itemCtr = CreateSelectItem(j, i, false);

					if(j == centerCell.posX && i == centerCell.posY)
					{
						itemCtr.Select();
					}
				}

			}
		}
    }


	private Vector2 CreateBrushItem(int cX, int cY, int oX, int oY,int brushId)
    {
        GameObject item = list.NewItem();
        item.name = oX + "_" + oY;

        PropTouchItem itemCtr = item.GetComponent<PropTouchItem>();

		PosMgr.SetFightCellPos(item.transform, cX + oX, cY - oY);

        itemCtr.control_x = cX + oX;
        itemCtr.control_y = cY + oY;
        itemCtr.control_id = brushId;
        itemCtr.icon = brushId;
        itemCtr.valid = true;
        itemCtr.brushImage.gameObject.SetActive(true);
        EventTriggerListener.Get(item).onClick = OnClickSelectHander;

		return new Vector2 (cX + oX,cY - oY);
    }


    private bool ShowSwitchList(List<CellInfo> cells, CellInfo centerCell)
    {
        selectDepth = 1;

        list.ClearList();

        bool hasSwitch = false;

        int i;
		List<Vector2> switchPoss = new List<Vector2> ();
        for (i = 0; i < cells.Count; i++)
        {
            CellInfo cellInfo = cells[i];
			if (cellInfo != null)
			{
				bool isCoverOpen = CoverModel.Instance.IsOpen(cellInfo.posX,cellInfo.posY);
                if (cellInfo.isBlank == false && cellInfo.isMonsterHold == false && cellInfo.config.cell_type == (int)CellType.five && cellInfo.configId != centerCell.configId && isCoverOpen)
				{
					CreateSelectItem(cellInfo.posX, cellInfo.posY, true);
					hasSwitch = true;
					switchPoss.Add(new Vector2(cellInfo.posX, cellInfo.posY));
				}
			}
        }

		for (i = 0; i < CellModel.Instance.allCells.Count; i++)
		{
			List<CellInfo> cellXs = CellModel.Instance.allCells[i];
			for (int j = 0; j < cellXs.Count; j++)
			{
				bool has = false;
				CellInfo cellInfo = cellXs[j];
				for(int n = 0;n<switchPoss.Count;n++)
				{
					Vector2 switchPos = switchPoss[n];
					if((int)switchPos.x == cellInfo.posX && cellInfo.posY == (int)switchPos.y )
					{
						has = true;
						break;
					}
				}
				if(has == false)
				{
					PropTouchItem itemCtr = CreateSelectItem(j, i, false);

					if(j == centerCell.posX && i == centerCell.posY)
					{
						itemCtr.Select();
					}
				}
			}
		}

        return hasSwitch;
    }

}
