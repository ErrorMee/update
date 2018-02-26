using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_module_item : config_item_base
{
    public string ab_name;
    public string prefab_name;

    public int layer_depth;

    //打开时需要提前关闭哪些
    public string open_clear;

    //打开时需要稍后打开哪些
    public string open_add;

    //关闭时需要提前关闭哪些
    public string close_clear;

    public int never_close;

    override public void Clear()
    {
        base.Clear();
        layer_depth = 0;
        open_clear = "";
        open_add = "";
        close_clear = "";
        never_close = 0;
    }

    public int GetOpenClearCount()
    {
        if (open_clear == null || open_clear == "null" || open_clear == "")
        {
            return 0;
        }

        if (open_clear != null && open_clear == "all")
        {
            return -1;
        }

        string[] strs = open_clear.Split(new char[] { ',' });

        return strs.Length;
    }

    public List<int> GetOpenClears()
    {
        string[] strs = open_clear.Split(new char[] { ',' });

        List<int> ints = new List<int>();

        for (int i = 0; i < strs.Length; i++)
        {
            string str = strs[i];

            int newint = int.Parse(str);

            ints.Add(newint);
        }

        return ints;
    }

    public int GetOpenAddCount()
    {
        if (open_add == null || open_add == "null" || open_add == "")
        {
            return 0;
        }

        if (open_add != null && open_add == "all")
        {
            return -1;
        }

        string[] strs = open_add.Split(new char[] { ',' });

        return strs.Length;
    }

    public List<int> GetOpenAdds()
    {
        string[] strs = open_add.Split(new char[] { ',' });

        List<int> ints = new List<int>();

        for (int i = 0; i < strs.Length; i++)
        {
            string str = strs[i];

            int newint = int.Parse(str);

            ints.Add(newint);
        }

        return ints;
    }

    public int GetCloseClearCount()
    {
        if (close_clear == null || close_clear == "null" || close_clear == "")
        {
            return 0;//无
        }

        if (close_clear != null && close_clear == "all")
        {
            return -1;//所有
        }

        string[] strs = close_clear.Split(new char[] { ',' });

        return strs.Length;
    }

    public List<int> GetCloseClears()
    {
        string[] strs = close_clear.Split(new char[] { ',' });

        List<int> ints = new List<int>();

        for (int i = 0; i < strs.Length; i++)
        {
            string str = strs[i];

            int newint = int.Parse(str);

            ints.Add(newint);
        }

        return ints;
    }

}
