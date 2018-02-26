using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HarvestActor : ActionNode
{
	private int posx;
	private int posy;
	private int wealthId;
	private int addCount;
	private BallBaseLayer effectLayer;
	private GameObject item;

	public HarvestActor(int px,int py,int id,int count,BallBaseLayer layer)
		: base()
	{
		posx = px;
		posy = py;
		wealthId = id;
		addCount = count;
		effectLayer = layer;
	}

	public override void OnExecute()
	{
		base.OnExecute();
		item = effectLayer.CreateBaseItem(posx, posy);
		GiftEffectItem effectCtr = item.GetComponent<GiftEffectItem>();
		effectCtr.Play(wealthId,addCount);
		CompleteHander();
	}

	private void CompleteHander()
	{
		OnEnd();
		effectLayer = null;
		item = null;
	}

}