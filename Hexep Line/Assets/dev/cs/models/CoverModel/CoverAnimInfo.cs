
using System;
using UnityEngine;
using System.Collections.Generic;

public class CoverAnimInfo:IComparable
{
	public int startFrame = 0;

	public CoverInfo coverInfo;

    public CellAnimType animationType = CellAnimType.move;

	public CoverAnimInfo ()
	{
		
	}
	
	//用来排序
	public int CompareTo(object obj)
	{
		CoverAnimInfo target = obj as CoverAnimInfo;
		
		return target.startFrame.CompareTo(startFrame);
	}
}