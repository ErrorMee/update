using System;
using System.Collections;
using System.Collections.Generic;

public class CopyFillInfo
{
	public int start;
	public int end;

	public void FillTxt(string range,string offset)
	{
		string[] rangeArr = range.Split('-');

		start = Convert.ToInt32(rangeArr[0]);
		end = Convert.ToInt32(rangeArr[1]);

		int offInt = Convert.ToInt32(offset);
		if(offInt != 0)
		{
			int off = UnityEngine.Random.Range(0, 2);
			if(off > 0)
			{
				start = start + offInt;
				end = end + offInt;
			}
		}
	}

	public int InRange(int step)
	{
		if (step > end) {
			return -1;//超出
		} else if (step == end) {
			return 2;//max
		} else if (step >= start) {
			return 1;
		} else {
			return 0;
		}
	}
}