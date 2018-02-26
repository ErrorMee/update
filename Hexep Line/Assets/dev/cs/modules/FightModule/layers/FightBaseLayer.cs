using UnityEngine;
using System.Collections;

public class FightBaseLayer : MonoBehaviour {

	public BaseList list;

	public FightLayerType type;

    protected virtual void Awake()
	{
        list = GetComponent<BaseList>();

	}

	public virtual void ShowList()
	{
		list.ClearList();
	}

    protected GameObject CreateBaseItem(int posX, int posY, int icon)
	{
        if (icon <= 0)
        {
            return null;
        }

		GameObject item = list.NewItem();
		item.name = posX + "_" + posY;

        FightBaseItem itemCtr = item.AddComponent<FightBaseItem>();
		itemCtr.type = type;
        itemCtr.icon = icon;
        PosUtil.SetFightCellPos(item.transform, posX, posY);
        return item;
	}

    protected float GetZRotate(int pn)
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
}
