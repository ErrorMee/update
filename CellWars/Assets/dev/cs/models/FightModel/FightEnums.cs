
public enum CellLineType
{ 
	failure = 0,    //失败
	success = 1,    //成功
	rollback = 2,   //回退
}

public enum FightStadus
{
    task_tip = -2,			    //任务介绍
	roll = -1,				    //滚动地图
	idle = 0,			        //等待连线
	line = 1,			        //进入连线
	ready_play = 2,		        //准备播放
	eliminate = 3,		        //消除
	unlock_hide = 4,            //解锁影藏
    auto_skill = 5,             //投放自动技能
    unlock_monster = 6,         //释放怪物
    jet_monster = 7,            //喷射怪物
	move = 8,			        //移动
	invade = 9,			        //入侵
	changer = 10,			    //转变
	coverMove = 11,			    //盖子变化
    floor_flow = 12,            //地板流动
    crawl = 13,                 //爬行
    crawl_add = 14,             //爬行新增

    open_fill = 40,             //入场
    deduct = 41,		        //扣除次数
    end_win = 42,               //胜利结束

    prop_eliminate = 101,	    //消除(道具)
    prop_move = 102,	        //移动(道具)
    prop_refresh = 103,         //刷新(道具)
    prop_clear = 104,           //清除(道具)
    ready_prop_switch = 105,    //交换(道具)
    prop_switch = 106,          //交换(道具)
    ready_prop_brush = 107,     //变色(道具)
    prop_brush = 108,           //变色(道具)

}

public enum FightLayerType
{
	all = -2,
	none = -1,
	map = 0,

	bg = 1,     		//背景层
	floor_hide = 2, 	//地板隐藏层
	floor = 3,  		//地板层
    line = 4,   		//特效下层
	cell = 5,   		//格子层
	monster = 6, 		//怪物层
	fence = 7,  		//栅栏层
    cover = 8,  		//盖子层
	fg = 9,     		//前景层

	effect = 99,		//特效层
	control = 100,		//控制层
    cell_add = 101, 	//格子扩展层
}

public enum MonsterType
{
    blank = 0,
    blockade = 1,       //封锁怪物
    hide = 2,           //躲藏怪物
    volcano = 3,        //火山
    flashlight = 4,     //手电筒
    bulb = 5,           //灯泡
    fall = 6,           //坠落

    crawl = 20,         //爬行
}

public enum CellAnimType
{ 
	move = 0,       //移动
	clear = 1,  	//消除
	wreck = 2,      //击毁
	bomb = 3,       //炸毁
    nullbomb = 4,   //空炸毁
	open = 5,       //开门
	close = 6,  	//关门
}

public enum SkillFightingType
{
    wait = 0,   //等待
    idel = 1,   //待机
    active = 2, //当前作用中
    pass = 3,   //作用但是非当前
}

public enum CellItemStadus
{
    normal = 0,
    over = 1,
}

public enum CellType
{
    other = 0,          //其他
    five = 1,           //五兄弟
    radiate = 2,        //辐射
    bomb = 3,           //炸弹
    terrain = 4,        //地形
    floor = 5,          //地板
	invade = 6,			//入侵
    changer = 7,        //改变者
    bg = 11,            //背景
    fg = 12,            //前景
    timer = 15,         //倒计时
	line_bomb = 16,     //线性炸弹
	line_bomb_r = 17,   //线性旋转炸弹
	three_bomb = 18,    //三向炸弹
	three_bomb_r = 19,  //三向旋转炸弹
}

public enum CellDirType
{
    no = -1,

	up = 1,
	right_up = 2,
	left_up = 0,

	down = 4,
	left_down = 5,
	right_down = 3,
}

public enum CoverShowType
{
	aways = -1,
	show = 0,
	hide = 1,
}