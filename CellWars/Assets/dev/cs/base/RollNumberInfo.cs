using System;
using System.Collections.Generic;
using UnityEngine;

public class RollNumberInfo
{
	//参照列表
	private List<int> referenceList = new List<int>() { 1, 5, 50, 500, 5000, 5000, 50000, 500000 };

	//滚动间隔
	public const float RollTimeGap = 0.01f;

    public string prefix = "";

    public int startNum = 0;

    public int endNum = 0;

    public int rollStep;

    public int rollTimes;

    public int tempValue = 0;

    public RollNumberInfo(int endNumP, int startNumP = 0, string prefixP = "")
	{
        prefix = prefixP;
        startNum = startNumP;
        endNum = endNumP;
		tempValue = startNum;
        if (startNum == endNum)
        {
            if (endNum >= 0)
            {
                startNum--;
            }
            else
            {
                startNum++;
            }
        }

	}

    public void CalculateStep()
    {
        int off = endNum - startNum;
        int absOff = Mathf.Abs(off);
        rollTimes = 0;
        RecursionTimes(absOff, 1, referenceList);

        rollStep = off / rollTimes;
    }

    private void RecursionTimes(int absOff, int depth,List<int> referenceList)
    {
        int pow = (int)Mathf.Pow(10, depth);
        int index = depth - 1;
        if (index >= referenceList.Count)
        {
            index = referenceList.Count - 1;
        }
        if (absOff <= pow)
        {
            rollTimes += (int)Mathf.Ceil(absOff / (referenceList[index] + 0.00000f));
        }
        else
        {
            rollTimes += (int)Mathf.Ceil(pow / (referenceList[index] + 0.00000f));
            absOff -= pow;
            depth++;
            RecursionTimes(absOff, depth, referenceList);
        }
    }

    public bool Roll()
    {
        if (tempValue >= endNum)
        {
            tempValue = endNum;
            return true;
        }

        tempValue += rollStep;
        if (tempValue >= endNum)
        {
            tempValue = endNum;
            return true;
        }else
        {
            return false;
        }
    }
}
