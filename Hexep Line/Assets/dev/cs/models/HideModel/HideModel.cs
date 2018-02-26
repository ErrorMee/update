using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HideModel : Singleton<HideModel>
{
	public List<HiderInfo> allHiders = new List<HiderInfo>();

	public List<int> backUpUnLocks = new List<int>();

	public void LoadHider()
	{
		allHiders = new List<HiderInfo> ();
		List<TIVInfo> tasks = FightModel.Instance.fightInfo.task;
		int hiderCount = 0;
		int i;
		for(i = 0;i<tasks.Count;i++)
		{
			TIVInfo task = tasks[i];
			if(task.id == 40106)
			{
				hiderCount = (int)task.value;
				break;
			}
		}

		List<Vector2> allFloorPos = FloorModel.Instance.GetAllFloorPos ();
		for (i = 0; i < hiderCount; i++) 
		{
			if(allFloorPos.Count > 0)
			{
				int randomIndex = UnityEngine.Random.Range(0, allFloorPos.Count);
				Vector2 floorPos = allFloorPos [randomIndex];
				allFloorPos.RemoveAt (randomIndex);

				HiderInfo hider = new HiderInfo ();
				hider.configId = 40106;
				hider.posX = (int)floorPos.x;
				hider.posY = (int)floorPos.y;
				allHiders.Add (hider);
			}
		}
	}

	public List<int> UnLock()
	{
		List<int> unLockIds = new List<int>();
		for (int i = 0; i < allHiders.Count; i++)
		{
			HiderInfo hiderInfo = allHiders[i];
			bool isExpose = hiderInfo.Expose();
			if (isExpose)
			{
				unLockIds.Add(hiderInfo.runId);
			}
		}
		return unLockIds;
	}

	public void BackUpUnLock(List<int> unLockIds)
	{
		backUpUnLocks = new List<int>();

		for (int i = 0; i < unLockIds.Count;i++ )
		{
			HiderInfo hiderInfo = GetInfoByRunId(unLockIds[i]);
			backUpUnLocks.Add(hiderInfo.configId);
		}
	}

	public HiderInfo GetInfoByRunId(int runId)
	{
		for(int i = 0;i<allHiders.Count;i++)
		{
			HiderInfo hiderInfo = allHiders[i];

			if(hiderInfo.runId == runId)
			{
				return hiderInfo;
			}
		}
		return null;
	}
}
