using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetLayerActor : ActionNode
{
	private Transform itemT;
	private Transform layerT;
	
	public SetLayerActor(Transform cellTrans,Transform layerTrans)
		: base()
	{
		itemT = cellTrans;
		layerT = layerTrans;
	}
	
	public override void OnExecute()
	{
		base.OnExecute();
		itemT.SetParent (layerT, false);
		CompleteHander();
	}
	
	private void CompleteHander()
	{
		itemT = null;
		itemT = null;
		OnEnd();
	}
	
}