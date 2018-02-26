
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_sort : config_base
{

    public List<config_sort_item> data;

    public config_sort_template template;

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

    override public int GetDataCount()
    {
        return data.Count;
    }

    public config_sort_item GetItemByTypeAndSpecial(int gtype, string special)
    {
        for (int i = 0; i < data.Count; i++)
        {
            config_sort_item item = data[i];

            if (item.gtype == gtype && item.special == special)
            {
                return item;
            }
        }
        return null;
    }

    public List<config_sort_item> GetItemsByType(int gtype)
    {
        List<config_sort_item> items = new List<config_sort_item>();
        for (int i = 0; i < data.Count; i++)
        {
            config_sort_item item = data[i];

            if (item.gtype == gtype)
            {
                items.Add(item);
            }
        }
        return items;
    }
}
