using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_base
{
    public string name;

    public virtual config_item_base GetItem(int id)
    {
        return null;
    }

    public virtual config_item_base GetItemAt(int index)
    {
        return null;
    }

    public virtual int GetDataCount()
    {
        return 0;
    }
}