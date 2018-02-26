
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_sort_item : config_item_base, IComparable
{
    public string gid;
    public int gtype;
    public int refer;

    override public void Clear()
    {
        base.Clear();
        gid = "";
        gtype = 0;
        refer = 0;
    }
}