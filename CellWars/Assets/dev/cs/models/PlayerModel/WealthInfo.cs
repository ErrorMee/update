using System;
using System.Collections.Generic;


public enum WealthTypeEnum
{
	
	None = 0,		//无

    Money = 10001,		//金钱

	Gem = 11101,   		//钻石

	Coin = 11102,    	//金币

	Energy = 11103,    	//体力

    Star = 11104,    	//星星
}

public class WealthInfo
{

	//
	public int type;

	public int count;

	public int limit;
}