using System;
using System.Collections;
using System.Collections.Generic;

public class IntervalInfo
{
    public int start;
    public int end;

    public IFSInfo iFSInfo = new IFSInfo();

    public void FillTxt(string range, string content)
    {
        string[] rangeArr = range.Split('-');

        start = Convert.ToInt32(rangeArr[0]);
        end = Convert.ToInt32(rangeArr[1]);

        iFSInfo.FillTxt(content);
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

    public int GetFillId()
    {
        return iFSInfo.GetFillId();
    }
}