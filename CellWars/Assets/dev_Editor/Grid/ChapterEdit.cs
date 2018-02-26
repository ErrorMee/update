
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChapterEdit : MonoBehaviour {

	public BaseList list;
	
	public GameObject item;

	void Awake()
	{
		list.itemPrefab = item;
	}

	void Start () {
	
		ShowList();

	}


	private void ShowList()
	{
		int i;
		int index = 0;
		for (i = (int)PosMgr.Y_HALF_COUNT; i >= -PosMgr.Y_HALF_COUNT; i--)
		{
			for (int j = -(int)PosMgr.X_HALF_COUNT; j <= PosMgr.X_HALF_COUNT; j++)
			{
				index ++;
				if(i <= (int)PosMgr.Y_HALF_COUNT - 3 &&
				   i >= -PosMgr.Y_HALF_COUNT + 3 &&
				   j >= -(int)PosMgr.X_HALF_COUNT + 1 && 
				   j <= PosMgr.X_HALF_COUNT - 1)
				{
					CreateCellItem(j, i, index);
				}
			}
		}
	}

	private void CreateCellItem(int posX, int posY,int index)
	{
		GameObject item = list.NewItem();
		item.name = posX + "_" + posY;
		PosMgr.SetCellPos(item.transform,posX, posY,0.4f);

		Text nameText = item.GetComponentInChildren<Text>();

		nameText.text = "" + index;
	}
}
