
using System;
using UnityEngine;
using System.Collections.Generic;

public class CellAnimInfo:IComparable
{
	public int startFrame = 0;

	public int runId = -1;

	public int toX = -1;

	public int toY = -1;

	public CellInfo toInfo;
    public CellInfo fromInfo;

	public CellAnimType animationType = CellAnimType.move;

	public CellAnimInfo ()
	{

	}

	//用来排序
	public int CompareTo(object obj)
	{
		CellAnimInfo target = obj as CellAnimInfo;

		return target.startFrame.CompareTo(startFrame);
	}
}