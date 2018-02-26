
using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class ClockModel : Singleton<ClockModel>
{

	public Action<List<ClockInfo>> clockEvent;

	public List<ClockInfo> clocks = new List<ClockInfo>();

	public void StartUp()
	{
		LeanTween.delayedCall (1, Tick);
	}

	private void Tick()
	{
		int currentSecond = ConvertDateTimeInt(DateTime.Now);
		for (int i = 0; i < clocks.Count; i++) 
		{
			ClockInfo clock = clocks[i];
			clock.Correct (currentSecond);
		}

		if(clockEvent != null && clocks.Count > 0)
		{
			clockEvent (clocks);
		}

		LeanTween.delayedCall (1, Tick);
	}

	public void SetClock(ClockInfo clockInfo)
	{
		for (int i = 0; i < clocks.Count; i++) 
		{
			ClockInfo clock = clocks[i];
			if(clock.id == clockInfo.id)
			{
				clocks [i] = clockInfo;
				return;
			}
		}
		clocks.Add (clockInfo);
	}

	public void RemoveClock(int id)
	{
		for (int i = 0; i < clocks.Count; i++) 
		{
			ClockInfo clock = clocks[i];
			if(clock.id == id)
			{
				clocks.Remove (clock);
				return;
			}
		}
	}

	public ClockInfo GetClock(int id)
	{
		for (int i = 0; i < clocks.Count; i++) 
		{
			ClockInfo clock = clocks[i];
			if(clock.id == id)
			{
				return clock;
			}
		}
		return null;
	}

	public static int ConvertDateTimeInt(System.DateTime time)
	{
		System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
		return (int)(time - startTime).TotalSeconds;
	}
}