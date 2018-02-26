
using System;
using UnityEngine;
using System.Collections.Generic;

public class WallAnimInfo:IComparable
{
	public int startFrame = 0;
	
	public WallInfo toInfo;
	
	public WallAnimInfo ()
	{
		
	}
	
	//用来排序
	public int CompareTo(object obj)
	{
		WallAnimInfo target = obj as WallAnimInfo;
		
		return target.startFrame.CompareTo(startFrame);
	}
}