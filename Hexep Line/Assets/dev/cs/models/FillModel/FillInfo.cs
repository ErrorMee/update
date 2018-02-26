using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillInfo
{
    public static int FILL_COUNT = 0;

    public int id;
    //默认
    public IFSInfo defaultFill;
    //组
    public List<IntervalInfo> intervals = new List<IntervalInfo>();
    //特殊步
    public Dictionary<int, IFSInfo> specifics = new Dictionary<int,IFSInfo>();
	//重复组
	public List<CopyFillInfo> copys = new List<CopyFillInfo>();

	//上一个
	private int lastFillId = 0;

    public void FillTxt(string content)
    {
        if (id <= 10000)
        {
            return;
        }
        if (content != null)
        {
            string[] txtArr = content.Split('\n');

            foreach (string txt in txtArr)
            {
                string[] txts = txt.Split('|');
                switch (txts[0].ToLower())
                {
                    case "d":
                        defaultFill = new IFSInfo();
                        defaultFill.FillTxt(txts[2]);
                        break;
                    case "g":
                        IntervalInfo intervalInfo = new IntervalInfo();
                        intervalInfo.FillTxt(txts[1], txts[2]);
                        intervals.Add(intervalInfo);
                        break;
                    case "s":
                        IFSInfo iFSInfo = new IFSInfo();
                        int step = Convert.ToInt32(txts[1]);
                        iFSInfo.FillTxt(txts[2]);
                        specifics.Add(step, iFSInfo);
                        break;
					case "c":
						CopyFillInfo copyInfo = new CopyFillInfo();
						copyInfo.FillTxt(txts[1], txts[2]);
						copys.Add(copyInfo);
						break;
                }
            }
        }
    }

	public int GetFillId(bool isDeductStep = false)
    {
        FILL_COUNT++;

		if (id <= 10000 || isDeductStep)
        {
			lastFillId = GetRandomFillId();
			return lastFillId;
        }

		for (int i = 0; i < copys.Count; i++)
		{
			CopyFillInfo copyFillInfo = copys[i];
			int inRange = copyFillInfo.InRange (FILL_COUNT);
			if (inRange > 0)
			{
				if(inRange == 2)
				{
					//copys.RemoveAt (i);
				}
				return lastFillId;
			}
		}

        if (specifics.ContainsKey(FILL_COUNT))
        {
            IFSInfo specific = specifics[FILL_COUNT];
            lastFillId = specific.GetFillId();
			return lastFillId;
        }

        for (int i = 0; i < intervals.Count; i++)
        {
            IntervalInfo intervalInfo = intervals[i];
			int inRange = intervalInfo.InRange(FILL_COUNT);
			if (inRange > 0)
            {
				lastFillId = intervalInfo.GetFillId();
				if(inRange == 2)
				{
					//intervals.RemoveAt (i);
				}
				return lastFillId;
            }
        }

        if (defaultFill == null)
        {
			lastFillId = GetRandomFillId();
			return lastFillId;
        }

		lastFillId = defaultFill.GetFillId();
		return lastFillId;
    }

    private int GetRandomFillId()
    {
        return UnityEngine.Random.Range(10101, 10101 + 5);
    }
}