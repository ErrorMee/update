using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_module : config_base
{

    public List<config_module_item> data;

    public config_module_template template;

    override public config_item_base GetItem(int id)
    {
        for (int i = 0; i < data.Count; i++)
        {
            config_item_base item = data[i];

            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }

    override public config_item_base GetItemAt(int index)
    {
        return data[index];
    }

    public config_module_item GetItemByPrefabName(string prefab_name)
    {
        for (int i = 0; i < data.Count; i++)
        {
            config_module_item item = data[i];

            if (item.prefab_name == prefab_name)
            {
                return item;
            }
        }
        return null;
    }

    override public int GetDataCount()
    {
        return data.Count;
    }

    public List<int> GetAllLayers()
    {
        List<int> allLayers = new List<int>();

        for (int i = 0; i < data.Count; i++)
        {
            config_module_item item = data[i];
            if (allLayers.Contains(item.layer_depth) == false)
            {
                allLayers.Add(item.layer_depth);
            }
        }
        allLayers.Sort();
        return allLayers;
    }
}
