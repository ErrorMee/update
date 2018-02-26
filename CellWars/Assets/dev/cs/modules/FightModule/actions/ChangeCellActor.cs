using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeCellActor : ActionNode
{
	private FightCellItem item;
	private CellInfo info;
    private int forceId;
    public ChangeCellActor(FightCellItem cellItem, CellInfo cellInfo, int force_id = 0)
		: base()
	{
		item = cellItem;
		info = cellInfo;
        forceId = force_id;
	}
	
	public override void OnExecute()
	{
		base.OnExecute();
		item.transform.localScale = new Vector3 (1,1,1);
		item.cell_info = info;
		if(info.config == null)
		{
			item.icon = 0;
		}else
		{
            if (info.changer > 0)
            {
                item.icon = info.changer;
            }
            else
            {
                item.icon = info.config.icon;
            }
            if (forceId > 0)
            {
                item.icon = forceId;
            }
            item.zrotate = info.config.rotate * FightConst.ROTATE_BASE;
		}
        item.UpdateTip();
		CompleteHander ();
	}
	
	private void CompleteHander()
	{
		item = null;
		info = null;
		OnEnd();
	}
	
}