using UnityEngine;
using System.Collections;

public class config_cell_template : config_template_base
{

    //隐藏的格子
    public string hide_id;
    //格子的类型
    public string cell_type;
    //连接类型 0 无类型不可连接 1-5可连类型 -1可连接任何可连类型
    public string line_type;
    //生命值 -2(永驻炸弹)-1不会被击不会被炸 0只可以被炸 1可以被炸可以被击 2只可以被击
    public string life;
    //是否可移动
    public string move;
    //连接消除后可是否攻击周围
    public string atk;
    //方向旋转0-5
    public string rotate;

    
}
