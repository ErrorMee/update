using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallBaseLayer : MonoBehaviour
{

    public BaseList list;

    protected virtual void Awake()
    {
        list = GetComponent<BaseList>();
    }

    public virtual void ShowList()
    {
        list.ClearList();
    }

    public GameObject CreateBaseItem(int posX, int posY)
    {
        GameObject item = list.NewItem();
        item.name = posX + "_" + posY;
		item.AddComponent<BallBaseItem>();
        PosMgr.SetFightCellPos(item.transform, posX, posY);
        return item;
    }

    public GameObject GetItemByPos(int x,int y)
    {
        List<GameObject> items = list.items;
        string nametemp = x + "_" + y;
        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = (GameObject)items[i];
            if (item == null)
            {
                continue;
            }
            if (item.name == nametemp)
            {
                return item;
            }
        }
        return null;
    }

    public float GetZRotate(int pn)
    {
        switch (pn)
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
