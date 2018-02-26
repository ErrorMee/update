using System;

public enum GuildConditionType
{
    equal = 1,//等于
    more_then = 2,//大于
    less_then = 3,//小于
}

public enum GuildIDType
{
    crt_star_num = 1,	//获得星星数量
    guild_complete = 2,	//引导完成
	pass_map = 3,		//通过地图
    play_map = 4,       //在玩地图
    ready_map = 5,      //准备地图
    lose_map = 6,       //过关失败
}


public enum GuildType
{
    click = 0,//点击
    link = 1,//连线
    introduction = 2,//介绍
    tip = 3,//提示
}
