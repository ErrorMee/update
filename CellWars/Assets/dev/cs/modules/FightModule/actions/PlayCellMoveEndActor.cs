using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayCellMoveEndActor : ActionNode
{
	private FightCellItem itemCell;
	private FightEffectItem itemEffect;
	private CellInfo info;
	
	public PlayCellMoveEndActor(FightCellItem cellItem, FightEffectItem effectItem, CellInfo cellInfo)
		: base()
	{
		itemCell = cellItem;
		itemEffect = effectItem;
		info = cellInfo;
	}
	
	public override void OnExecute()
	{
		base.OnExecute();
		itemCell.moveEndAnim.Play ();
		if(itemEffect != null)
		{
			itemEffect.SetBombSelect (info.bombMark);
		}

		CompleteHander ();
	}
	
	private void CompleteHander()
	{
		itemCell = null;
		itemEffect = null;
		info = null;
		OnEnd();
	}
	
}