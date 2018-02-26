using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeFanceActor : ActionNode
{
	private FightFanceItem item;
	private WallInfo info;
	
	public ChangeFanceActor(FightFanceItem fanceItem, WallInfo wallInfo)
		: base()
	{
		item = fanceItem;
		info = wallInfo;
	}
	
	public override void OnExecute()
	{
		base.OnExecute();
        if (item != null)
        {
            item.transform.localScale = new Vector3(1, 1, 1);
            item.wall_info = info;
            item.icon = info.configId;
            if (item.wall_info.IsNull())
            {
				item.gameObject.SetActive(false);
                //GameObject.Destroy(item.gameObject);
            }
        }
		CompleteHander ();
	}
	
	private void CompleteHander()
	{
		item = null;
		info = null;
		OnEnd();
	}
	
}