using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberText : Text
{

    public float maxRollTime = 0;

    private Coroutine ctDelayRoll = null;
    private Coroutine ctRoll = null;

    public void RollNumber(int endNum, string prefixP = "",float delayTime = 0) 
    {
        List<RollNumberInfo> rollNumberInfos = new List<RollNumberInfo>();
        RollNumberInfo rollNumberInfo = new RollNumberInfo(endNum, 0, prefixP);
        rollNumberInfos.Add(rollNumberInfo);
        RollNumberList(rollNumberInfos, delayTime);
    }

	public void RollNumberStart(int endNum,int startNum, string prefixP = "",float delayTime = 0) 
	{
		List<RollNumberInfo> rollNumberInfos = new List<RollNumberInfo>();
		RollNumberInfo rollNumberInfo = new RollNumberInfo(endNum, startNum, prefixP);
		rollNumberInfos.Add(rollNumberInfo);
		RollNumberList(rollNumberInfos, delayTime);
	}

    public void RollNumberList(List<RollNumberInfo> rollNumberInfos, float delayTime = 0)
    {
        int maxRollCount = 0;
        for (int i = 0; i < rollNumberInfos.Count; i++)
        {
            RollNumberInfo rollNumberInfo = rollNumberInfos[i];
            rollNumberInfo.CalculateStep();
            if (rollNumberInfo.rollTimes > maxRollCount)
            {
                maxRollCount = rollNumberInfo.rollTimes;
            }
        }

		maxRollTime = (maxRollCount + 1) * RollNumberInfo.RollTimeGap;

        if (ctDelayRoll != null)
        {
            StopCoroutine(ctDelayRoll);
        }

        if (ctRoll != null)
        {
            StopCoroutine(ctRoll);
        }

        if (delayTime > 0)
        {
            ctDelayRoll = StartCoroutine(DelayRoll(delayTime, rollNumberInfos));
        }
        else
        {
            ctRoll = StartCoroutine(Roll(rollNumberInfos));
        }
    }

    IEnumerator DelayRoll(float delayTime, List<RollNumberInfo> rollNumberInfos)
    {
        yield return new WaitForSeconds(delayTime);
        ctRoll = StartCoroutine(Roll(rollNumberInfos));
    }

    IEnumerator Roll(List<RollNumberInfo> rollNumberInfos)
    {
        bool isEnd = true;

        for (int i = 0; i < rollNumberInfos.Count; i++)
        {
            RollNumberInfo rollNumberInfo = rollNumberInfos[i];
            bool send = rollNumberInfo.Roll();
            if (send == false)
            {
                isEnd = false;
            }
        }

        ShowNumberText(rollNumberInfos);

        if (isEnd)
        {
            yield break;
        }
        else
        {
			yield return new WaitForSeconds(RollNumberInfo.RollTimeGap);
            ctRoll = StartCoroutine(Roll(rollNumberInfos));
        }
    }

    public void ShowNumberText(List<RollNumberInfo> rollNumberInfos) 
    {
        string str = "";
        for (int i = 0; i < rollNumberInfos.Count; i++)
        {
            RollNumberInfo rollNumberInfo = rollNumberInfos[i];
            str = str + rollNumberInfo.prefix + rollNumberInfo.tempValue;
        }
        text = str;
    }
}