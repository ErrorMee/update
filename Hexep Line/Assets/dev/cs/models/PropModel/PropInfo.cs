using System;
using UnityEngine;
using System.Collections.Generic;

public class PropInfo:IComparable
{
	private static int LAST_RUN_ID = 0;

	public int runId = 0;

	public config_prop_item config;

	public int count;
    public int countUsed = 0;

    public bool isForbid;

	public PropInfo ()
	{
		runId = ++LAST_RUN_ID;
	}

	public int CompareTo(object obj)
	{
		PropInfo target = obj as PropInfo;
		if (target == null) {
			throw new NotImplementedException();
		}
		return runId.CompareTo(target.runId);
	}


	public void Use(int useCount = 1)
	{
		count = count - useCount;
        countUsed++;
		WealthInfo coinInfo = PlayerModel.Instance.GetWealth ((int)WealthTypeEnum.Coin);

        coinInfo.count -= config.GetCost(countUsed);

		PlayerModel.Instance.SaveWealths();
	}
}


