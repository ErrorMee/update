using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using LitJson;

public class PlayerModel : Singleton<PlayerModel>
{
    public static bool CLEAR_ALL = false;

	public List<WealthInfo> wealths = new List<WealthInfo> ();

	public ExchangeInfo exchangeInfo;

	public Action updateWealthsEvent;
	public Action<int> updateWealthEvent;

	public void LoadWeaths()
	{

        if (PlayerModel.CLEAR_ALL)
        {
            PlayerPrefsUtil.RemoveData(PlayerPrefsUtil.WEALTH);
			PlayerPrefsUtil.RemoveData (PlayerPrefsUtil.ENERGY_BUY);
        }

		string WEALTH = PlayerPrefsUtil.GetString(PlayerPrefsUtil.WEALTH);
		wealths = JsonMapper.ToObject<List<WealthInfo>>(WEALTH);
		if (wealths == null)
		{
			wealths = new List<WealthInfo>();

			for(int i = 0;i<ResModel.Instance.config_wealth.data.Count;i++)
			{
				config_wealth_item config_item = ResModel.Instance.config_wealth.data[i];

				wealths.Add(CreatWealthInfo(config_item.id, config_item.firstCount, config_item.limitCount));
			}

			SaveWealths();
		}
		CheckEnergyRecover (true);
		ClockModel.Instance.clockEvent += OnClockEvent;
	}

	private void OnClockEvent(List<ClockInfo> clocks)
	{
		for (int i = 0; i < clocks.Count; i++) 
		{
			ClockInfo clock = clocks[i];
			if(clock.id == (int)WealthTypeEnum.Energy)
			{
				WealthInfo energyInfo = GetWealth((int)WealthTypeEnum.Energy);
				if (energyInfo.count < energyInfo.limit) {
					float ENERGY_RECOVER = GameModel.Instance.GetGameConfig (1004);
					if (clock.value >= ENERGY_RECOVER) {

						int recoverCount = (int)Math.Floor (clock.value / ENERGY_RECOVER);
						if ((energyInfo.limit - energyInfo.count) > recoverCount)
						{
							energyInfo.count = energyInfo.count + (int)recoverCount;
						}
						else
						{
							energyInfo.count = energyInfo.limit;
						}
						PlayerModel.Instance.SaveWealths();
						PlayerPrefsUtil.SetInt (PlayerPrefsUtil.ENERGY_OUT_TIME,ClockModel.ConvertDateTimeInt(System.DateTime.Now));

						int leftTime = (int)(clock.value % ENERGY_RECOVER);
						ClockModel.Instance.SetClock (new ClockInfo ((int)WealthTypeEnum.Energy,leftTime));
					}
				} else {
					ClockModel.Instance.RemoveClock (clock.id);
				}
				return;
			}
		}
	}

	public void CheckEnergyRecover(bool isFirst)
	{
		WealthInfo energyInfo = GetWealth((int)WealthTypeEnum.Energy);
		if (energyInfo.count < energyInfo.limit) 
		{
			if(ClockModel.Instance.GetClock((int)WealthTypeEnum.Energy) == null)
			{
				int energyRestoreTime;
				if (isFirst) {
					int getTime = PlayerPrefsUtil.GetInt (PlayerPrefsUtil.ENERGY_OUT_TIME);
					if (getTime > 0) {
						energyRestoreTime = getTime;
					} else {
						energyRestoreTime = ClockModel.ConvertDateTimeInt(DateTime.Now);
						PlayerPrefsUtil.SetInt (PlayerPrefsUtil.ENERGY_OUT_TIME,energyRestoreTime);
					}
					ClockModel.Instance.SetClock (new ClockInfo ((int)WealthTypeEnum.Energy,0,energyRestoreTime));
				} else {
					energyRestoreTime = ClockModel.ConvertDateTimeInt(DateTime.Now);
					PlayerPrefsUtil.SetInt (PlayerPrefsUtil.ENERGY_OUT_TIME,energyRestoreTime);
					ClockModel.Instance.SetClock (new ClockInfo ((int)WealthTypeEnum.Energy,0,energyRestoreTime));
				}
			}
		}
	}



	private WealthInfo CreatWealthInfo(int wealthType,int count,int limit = 0)
	{
		WealthInfo wealthInfo = new WealthInfo();
		wealthInfo.type = wealthType;
		wealthInfo.count = count;
		wealthInfo.limit = limit;
		return wealthInfo;
	}

	public void SaveWealths(int id = 0)
	{
		string WEALTH = JsonMapper.ToJson(wealths);
		PlayerPrefsUtil.SetString(PlayerPrefsUtil.WEALTH, WEALTH);
		if(id == 0)
		{
			if(updateWealthsEvent != null)
			{
				updateWealthsEvent();
			}
		}else
		{
			if(updateWealthEvent != null)
			{
				updateWealthEvent(id);
			}
		}
	}

	public WealthInfo GetWealth(int wealthType)
	{
		for(int i = 0;i<wealths.Count;i++)
		{
			WealthInfo wealthInfo = wealths[i];
			if(wealthType == wealthInfo.type){
				return wealthInfo;
			}
		}
		return null;
	}

	public void ExchangeWealth(int type,int count,Action sucFun)
	{
		exchangeInfo = new ExchangeInfo ();
		exchangeInfo.type = type;
		exchangeInfo.count = count;
		exchangeInfo.sucFun = sucFun;
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.EXCHANGE);
	}
}