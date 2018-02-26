using System;

public class HiderInfo
{
	private static int LAST_RUN_ID = 0;
	public int runId = -1;
	public int configId = 0;
	public int posX;
	public int posY;
	public bool isNull = false;

	public HiderInfo ()
	{
		runId = ++LAST_RUN_ID;
	}
		
	public bool Expose()
	{
		if (isNull)
		{
			return false;
		}

		FloorInfo floorInfo = FloorModel.Instance.GetFloorByPos(posY, posX);

		if (floorInfo == null || floorInfo.IsNull())
		{
			isNull = true;
			return true;
		}
		else
		{
			return false;
		}
	}

}