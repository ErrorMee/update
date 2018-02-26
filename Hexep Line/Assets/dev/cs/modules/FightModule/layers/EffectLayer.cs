using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class EffectLayer : FightBaseLayer
{
	private int winkWaitTime = 120;
	private int winkTime = 0;
	private float winkGNum = 6.0f;

    public GameObject effectItem;
    public GameObject lineItem;

	override protected void Awake()
	{
		base.Awake();
        effectItem = ResModel.Instance.GetPrefab("prefab/fightmodule/" + "FightEffectItem");
        lineItem = ResModel.Instance.GetPrefab("prefab/fightmodule/" + "FightLineItem");
        list.itemPrefab = effectItem;
	}

	void Update()
	{
		winkTime++;
		if (winkTime > winkWaitTime)
		{
			winkTime = 0;
			winkWaitTime = UnityEngine.Random.Range(90, 150);
			if (FightModule.crtFightStadus == FightStadus.idle)
			{
				PlayWink();
			}
		}
	}

	private void PlayWink()
	{
		GameObject[] eyes = GameObject.FindGameObjectsWithTag("Eyes");

		int groupCount = (int)Mathf.Floor(eyes.Length / winkGNum);

		for (int i = 0; i < groupCount; i++)
		{
			int groupStart = i * (int)winkGNum;
			int groupEnd = (i + 1) * (int)winkGNum - 1;
			if (i == (groupCount - 1))
			{
				groupEnd = eyes.Length - 1;
			}
			int randomIndex = UnityEngine.Random.Range(groupStart, groupEnd + 1);

			GameObject eye = eyes[randomIndex];
			Animation[] anims = eye.GetComponentsInChildren<Animation>();
			int winkType = UnityEngine.Random.Range(0, 3);//0 ¶¼Õ£ÑÛ 1×ó 2ÓÒ

			switch (winkType)
			{
			case 1:
				anims[0].Play();
				break;
			case 2:
				anims[1].Play();
				break;
			default:
				for (int j = 0; j < anims.Length; j++)
				{
					anims[j].Play();
				}
				break;
			}
		}
	}

    private FightEffectItem CreateEffectItem(int posX, int posY)
	{
        list.ForceChangeItemPrefab(effectItem);
		GameObject item = list.NewItem();

		FightEffectItem itemCtr = item.AddComponent<FightEffectItem>();
        PosUtil.SetFightCellPos(item.transform, posX, posY);
        itemCtr.control_x = posX;
        itemCtr.control_y = posY;
		item.name = itemCtr.control_x + "_" + itemCtr.control_y;

        return itemCtr;
	}

	public void ChangeBombs(List<CellInfo> cells)
	{
		if (cells != null)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				CellInfo cellInfo = cells[i];

                FightEffectItem item = GetEffectItemByPos(cellInfo.posX, cellInfo.posY);
                if (item == null)
                {
                    item = CreateEffectItem(cellInfo.posX, cellInfo.posY);
                }
				item.SetBombSelect(cellInfo.bombMark);
			}
		}
	}

    public FightEffectItem GetEffectItemByPos(int posX, int posY)
	{
		GameObject item = list.GetItemByName(posX + "_" + posY);
		if(item == null)
		{
			return null;
		}
		return item.GetComponent<FightEffectItem>();
	}

	public FightEffectItem FindEffectItemByPos(int posX, int posY)
	{
		GameObject item = list.GetItemByName(posX + "_" + posY);
		if(item == null)
		{
			return CreateEffectItem(posX, posY);
		}
		return item.GetComponent<FightEffectItem>();
	}


    public GameObject ShowLine(CellInfo cellA, CellInfo cellB)
    {
        list.ForceChangeItemPrefab(lineItem);
        GameObject item = list.NewItem();
        item.name = cellA.posX + "_" + cellA.posY + "_" + cellB.posX + "_" + cellB.posY;

        PosUtil.SetFightCellPos(item.transform, cellB.posX, cellB.posY);

        item.transform.Rotate(0, 0, PosUtil.VectorAngle(PosUtil.GetFightCellPos(cellB.posX, cellB.posY), PosUtil.GetFightCellPos(cellA.posX, cellA.posY)));

        return item;
    }

    public void DestroyLine(CellInfo cellA, CellInfo cellB)
    {
        list.DestroyItemByName(cellA.posX + "_" + cellA.posY + "_" + cellB.posX + "_" + cellB.posY);
    }
}