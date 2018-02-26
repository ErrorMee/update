using UnityEngine;
using System.Collections;



public class TogglePart : MonoBehaviour {

    public ToggleList listPrefab;

    void OnEnable()
    {
        CreatList(FightLayerType.map, 0);
        CreatList(FightLayerType.bg, 1);
        CreatList(FightLayerType.floor, 2);
        CreatList(FightLayerType.cell, 3);
		CreatList(FightLayerType.monster, 4);
        CreatList(FightLayerType.cell_add, 5);
        CreatList(FightLayerType.fence, 6);
        CreatList(FightLayerType.cover, 7);
        CreatList(FightLayerType.fg, 8);
    }

    private void CreatList(FightLayerType type, int index)
    {
        ToggleList listItem = GameObject.Instantiate<ToggleList>(listPrefab);
        listItem.gameObject.name = "" + type;
        listItem.transform.SetParent(transform, false);
        listItem.transform.localPosition = new Vector3(0, -130 * index, 0);
        listItem.InitList(type);
    }

	public ToggleList UpDateList(FightLayerType type)
	{
		Transform childTrans = transform.FindChild("" + type);
		if(childTrans)
		{
			ToggleList listItem = childTrans.GetComponent<ToggleList>();
			listItem.InitList(type);
			return listItem;
		}
		return null;
	}
}
