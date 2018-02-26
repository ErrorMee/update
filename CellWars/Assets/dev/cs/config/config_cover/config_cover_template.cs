using UnityEngine;
using System.Collections;

public class config_cover_template : config_template_base
{
	public string hide_id;
    public string show_rate;//显示回合
    public string hide_rate;//隐藏回合
    public string line;
    public string life;//生命值 -1不会被击不会被炸 0只可以被炸 1可以被炸可以被击 2只可以被击
    public string move;
}
