using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayBallMoveEndActor : ActionNode
{
	private BallBaseItem itemBall;

	public PlayBallMoveEndActor(BallBaseItem ballItem)
		: base()
	{
		itemBall = ballItem;
	}

	public override void OnExecute()
	{
		base.OnExecute();
		Animation anim = itemBall.GetComponentInChildren<Animation>();
		anim.Play ();
		CompleteHander();
	}

	private void CompleteHander()
	{
		itemBall = null;
		OnEnd();
	}

}