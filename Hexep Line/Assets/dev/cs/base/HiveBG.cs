using UnityEngine;
using System.Collections;

public class HiveBG : MonoBehaviour
{
    private BaseList list;

    private void Awake()
    {
        list = GetComponent<BaseList>();
        list.itemPrefab = ResModel.Instance.GetPrefab("prefab/base/" + "hiveItem");

    }

	public void ShowList(int startX,int endX,int startY, int endY,float scale = 1)
    {
        list.ClearList();

        int i;
		for (i = startY; i <= endY; i++)
        {
			for (int j = startX; j <= endX; j++)
            {
				CreateHiveItem (j, i, scale);
            }
        }
    }

	private void CreateHiveItem(int posX, int posY,float scale = 1)
    {
        GameObject item = list.NewItem();
        item.name = posX + "_" + posY;
        PosUtil.SetCellPos(item.transform, posX, posY);
		item.transform.localScale = new Vector3(scale,scale,1);
    }

	public void DestroyListItem(int posX, int posY)
	{
		list.DestroyItemByName(posX + "_" + posY);
	}
}
