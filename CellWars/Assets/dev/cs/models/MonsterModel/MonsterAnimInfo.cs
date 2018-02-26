
using System;
using UnityEngine;
using System.Collections.Generic;

public class MonsterAnimInfo:IComparable
{
	public int startFrame = 0;

	public MonsterInfo monsterInfo;

	public CellAnimType animationType = CellAnimType.move;

	public MonsterAnimInfo ()
	{

	}

	//用来排序
	public int CompareTo(object obj)
	{
		MonsterAnimInfo target = obj as MonsterAnimInfo;

		return target.startFrame.CompareTo(startFrame);
	}
}