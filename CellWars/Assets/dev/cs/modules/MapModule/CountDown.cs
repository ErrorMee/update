using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{

    public Text coutDownText;

    private WealthInfo energyInfo;

    void Awake()
    {
		energyInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Energy);
		ClockModel.Instance.clockEvent += OnClockEvent;
    }

	void OnDestroy()
	{
		ClockModel.Instance.clockEvent -= OnClockEvent;
	}

	private void OnClockEvent(List<ClockInfo> clocks)
	{
		if (energyInfo.count >= energyInfo.limit) {
			coutDownText.text = "";
		} else {

			ClockInfo energyClock = ClockModel.Instance.GetClock ((int)WealthTypeEnum.Energy);

			if (energyClock != null) {
				coutDownText.text = "" + ((int)GameModel.Instance.GetGameConfig(1004) - energyClock.value) + "s +1";
			} else 
			{
				coutDownText.text = "";
			}
		}
	}
}
