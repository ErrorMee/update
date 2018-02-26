using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseList : MonoBehaviour {
	private static int CREATE_COUNT = 0;
	private GameObject _itemPrefab;
	public GameObject itemPrefab
	{
		set
		{
			if(_itemPrefab!=null){
				Debug.LogWarning("itemPrefab!=null");
				return;
			}
			items = new List<GameObject>();
			_itemPrefab = value;
		}
		get{return _itemPrefab;}
	}

    public void ForceChangeItemPrefab(GameObject prefab)
    {
        _itemPrefab = prefab;
    }

	private List<GameObject> _items;
	public List<GameObject> items
	{
		set{_items = value;}
		get{return _items;}
	} 

	public GameObject NewItem()
	{
		if(itemPrefab == null)
		{
			Debug.LogWarning("itemPrefab == null");
			return null;
		}
		GameObject item = GameObject.Instantiate(itemPrefab);
		CREATE_COUNT++;
		item.name = "cell_" + CREATE_COUNT;
		items.Add (item);

		item.transform.SetParent(transform,false);

		return item;
	}

    public void DestroyItem(int index)
    {
        if (items != null)
        {
            GameObject item = items[index];
            items.RemoveAt(index);
            Destroy(item);
        }
    }

    public void DestroyItemByName(string itemName)
    {
        int findIndex = -1;

        int i;
        int count = items.Count;
        for (i = 0; i < count; i++)
        {
            GameObject item = items[i];
            if (item.name == itemName)
            {
                findIndex = i; 
                break;
            }
        }
        if(findIndex >= 0)
        {
            DestroyItem(findIndex);
        }
    }

    public GameObject GetItemByName(string itemName)
    {
        int i;
        int count = items.Count;
        for (i = 0; i < count; i++)
        {
            GameObject item = items[i];
            if (item.name == itemName)
            {
                return item;
            }
        }
        return null;
    }

	public void ClearList()
	{
        if (items != null)
        {
            int i;
            int count = items.Count;
            for (i = 0; i < count; i++)
            {
                GameObject item = items[i];
				if(item != null)
				{
					Destroy(item);
				}
            }
        }
		
		items = new List<GameObject>();
		CREATE_COUNT = 0;
	}
}
