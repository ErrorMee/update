using System;
using UnityEngine;
using System.Collections.Generic;

public class FloorInfo
{
	private static int LAST_RUN_ID = 0;
	
	public int runId = -1;
	
	public config_cell_item config;
	
	public int posX;
	public int posY;

	public FloorInfo ()
	{
		runId = ++LAST_RUN_ID;
	}

	public void SetConfig(int id)
	{
		if(id == 10400)
		{
			id = 0;
		}
		config = (config_cell_item)ResModel.Instance.config_cell.GetItem(id);
	}

	public bool IsNull()
	{
		return config == null;
	}

    //¸´ÖÆ
    public FloorInfo Copy()
    {
        FloorInfo floorInfo = new FloorInfo();
        floorInfo.runId = runId;
        //if (config != null)
        //{
        //    floorInfo.config = (config_cell_item)config.Copy();
        //}
        floorInfo.config = config;
        floorInfo.posX = posX;
        floorInfo.posY = posY;

        return floorInfo;
    }

    public void SwitchPos(FloorInfo cellInfo)
    {
        FloorInfo cellCopy = cellInfo.Copy();
        cellInfo.posX = posX;
        cellInfo.posY = posY;
        posX = cellCopy.posX;
        posY = cellCopy.posY;
    }

    public bool IsHump(int pos = -1)
    {
        if (pos == -1)
        {
            pos = posX;
        }
        return (pos - CellInfo.start_x) % 2 != 0;
    }
}


