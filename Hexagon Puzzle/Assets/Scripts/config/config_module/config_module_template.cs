using UnityEngine;
using System.Collections;

public class config_module_template : config_template_base
{
    //ab包名
    public string ab_name;
    //预设名
    public string prefab_name;

    //窗口深度
    public string layer_depth;

    //打开时需要提前关闭哪些
    public string open_clear;

    //打开时需要稍后打开哪些
    public string open_add;

    //关闭时需要提前关闭哪些
    public string close_clear;

    //永不销毁
    public string never_close;
}