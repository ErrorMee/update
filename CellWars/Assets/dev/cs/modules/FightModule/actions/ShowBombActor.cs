using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowBombActor : ActionNode
{
    private FightCellItem itemCell;
	private FightEffectItem itemEffect;
	private bool isShowBomb;

	public ShowBombActor(FightCellItem cellItem,FightEffectItem effectItem,bool isShow = true)
        : base()
    {
		itemCell = cellItem;
		itemEffect = effectItem;
		isShowBomb = isShow;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        if (itemCell != null)
        {
            itemCell.transform.localScale = new Vector3(1, 1, 1);
        }
		
        if (itemEffect != null)
        {
            itemEffect.bomb_select.gameObject.SetActive(isShowBomb);
        }

        CompleteHander();
    }

    private void CompleteHander()
    {
		itemCell = null;
		itemEffect = null;
        OnEnd();
    }

}