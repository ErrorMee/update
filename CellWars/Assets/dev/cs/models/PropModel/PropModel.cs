using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PropStadus
{
	unSelect = 0,
	select = 1,
	doing = 2,
}

public class PropModel : Singleton<PropModel>
{
	public Action<PropStadus> UpdatePropStadus;
    public Action PropRefreshEvent;
    public Action<int> PropAddSetpEvent;
    public Action<List<CellInfo>> PropChangeCellsEvent;

	public List<PropInfo> allProps = new List<PropInfo> ();

	public PropStadus crtPropStadus = PropStadus.unSelect;

	public PropInfo crtProp = null;

	private int safeCount = 0;

	public void Destroy()
	{
		allProps = new List<PropInfo> ();
	}
	
	public void InitProps()
	{
		allProps = new List<PropInfo> ();

		List<config_prop_item> prop_data = GameMgr.resourceMgr.config_prop.data;
		
		for (int i = 0; i < prop_data.Count; i++)
		{
			config_prop_item prop_item = prop_data[i];
            bool isForbid = BattleModel.Instance.crtConfig.IsForbidProp(prop_item.id);

            PropInfo propInfo = new PropInfo();
            propInfo.config = prop_item;
            propInfo.count = propInfo.config.times;
            propInfo.isForbid = isForbid;
            allProps.Add(propInfo);
		}
        PropRefreshEvent = null;
        PropAddSetpEvent = null;
		UpdatePropStadus = null;
		crtPropStadus = PropStadus.unSelect;
		crtProp = null;
	}

	public PropInfo GetProp(int propId)
	{
		for (int i = 0; i < allProps.Count; i++)
		{
			PropInfo propInfo = allProps[i];

			if(propInfo.config.id == propId)
			{
				return propInfo;
			}
		}
		return null;
	}

	public void OnSelectChanged(bool select , PropInfo propInfo)
	{
		if(select)
		{
			crtProp = propInfo;
		}else
		{
			crtProp = null;
		}

		if (crtProp != null) {
			ChangePropStadus(PropStadus.select);
		} else {
			ChangePropStadus(PropStadus.unSelect);
		}
	}

	public void ChangePropStadus(PropStadus stadus)
	{
		crtPropStadus = stadus;
		if (UpdatePropStadus != null)
		{
			UpdatePropStadus(crtPropStadus);
		}
	}

	private void ExitFight()
	{
		BattleModel.Instance.play_mapId = 0;
		GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
	}

	public void RefreshCell()
	{
		safeCount ++;
		if(safeCount > 1000)
		{
			safeCount = 0;
            ConfirmInfo confirmInfo = new ConfirmInfo(LanguageUtil.GetTxt(11602), LanguageUtil.GetTxt(11601), ExitFight, null, false);
			ConfirmModel.Instance.AddConfirm(confirmInfo);
			return;
		}

		CellModel.Instance.anims = new List<List<CellAnimInfo>> ();

		List<CellInfo> allElments = new List<CellInfo> ();

		for(int i = 0;i< BattleModel.Instance.crtBattle.ShowHeight();i++)
		{
			List<CellInfo> xCells = CellModel.Instance.allCells[i];
			for(int j = 0; j<xCells.Count; j++)
			{
				CellInfo cellInfo = xCells[j];
                if (cellInfo.isBlank == false && cellInfo.isMonsterHold == false && cellInfo.config.cell_type == (int)CellType.five)
				{
                    CoverInfo coverInfo = CoverModel.Instance.GetCoverByPos(cellInfo.posY, cellInfo.posX);
                    if (coverInfo.IsNull())
                    {
                        allElments.Add(cellInfo);
                    }
				}
			}
		}

		int switchTimes = 0;
		while(allElments.Count > 1)
		{
			int randomA = UnityEngine.Random.Range(0, allElments.Count);
			CellInfo cellA = allElments[randomA];
			allElments.RemoveAt(randomA);

			int randomB = UnityEngine.Random.Range(0, allElments.Count);
			CellInfo cellB = allElments[randomB];
			allElments.RemoveAt(randomB);

			switchTimes ++;
			CellModel.Instance.SwitchPos(cellA,cellB);
			cellA.SwitchPos(cellB);
		}

        if (FuncCheckDead.IsDead ()) {
			RefreshCell ();
		} else {
			safeCount = 0;
		}
	}

}
