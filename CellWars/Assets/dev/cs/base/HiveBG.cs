using UnityEngine;
using System.Collections;

public class HiveBG : MonoBehaviour
{

    private BaseList list;

    private int halfWidth;
    private int halfHeight;

    private void Awake()
    {
        list = GetComponent<BaseList>();
        list.itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/base.ab", "hiveItem");

    }

    public void ShowList(int halfY, int halfX)
    {
        halfWidth = halfX;
        halfHeight = halfY;
        list.ClearList();

        int i;
        for (i = halfY; i >= -halfY; i--)
        {
            for (int j = -halfX; j <= halfX; j++)
            {
				CreateHiveItem(j, i);
            }
        }
    }

    private void CreateHiveItem(int posX, int posY,bool isScale = false)
    {
        GameObject item = list.NewItem();
        item.name = posX + "_" + posY;
        PosMgr.SetCellPos(item.transform, posX, posY);
        if (isScale)
        {
            item.transform.localScale = new Vector3(1.12f,1.12f,1);
        }
    }

    public void DestroyList(int startX, int endX, int startY, int endY)
    {
        int index = (halfHeight * 2 + 1) * (halfWidth * 2 + 1);
        int i;
        for (i = halfHeight; i >= -halfHeight; i--)
        {
            for (int j = -halfWidth; j <= halfWidth; j++)
            {
                index--;
                if (i <= startY && i >= endY && j >= startX && j <= endX)
                {
                    list.DestroyItem(index);
                }
            }
        }
    }

	public void DestroyListItem(int posX, int posY)
	{
		list.DestroyItemByName(posX + "_" + posY);
	}
}
