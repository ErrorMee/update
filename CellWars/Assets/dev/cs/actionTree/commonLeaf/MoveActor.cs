using UnityEngine;
using System.Collections;

public class MoveActor : ActionNode
{
    private RectTransform targetTrans;
    private Vector3 toPos;
    private float tweenSeed;
	private float tweenFixTime;

    public MoveActor(RectTransform target, Vector3 pos, float speed = 1600.00f,float fixTime = 0)
        : base()
    {
        targetTrans = target;
        toPos = pos;
        tweenSeed = speed;
		tweenFixTime = fixTime;
    }

    public override void OnExecute()
    {
        base.OnExecute();
		float tweenTime = 0;
        
		if(tweenFixTime != 0)
		{
			tweenTime = tweenFixTime;
		}else
		{
			float len = Mathf.Sqrt(Mathf.Pow(targetTrans.anchoredPosition.x - toPos.x, 2) + Mathf.Pow(targetTrans.anchoredPosition.y - toPos.y, 2));
			tweenTime = len / tweenSeed;
		}
        LeanTween.move(targetTrans, toPos, tweenTime).onComplete = CompleteHander;
    }

    private void CompleteHander()
    {
        OnEnd();
    }

    override public void OnDispose()
    {
        base.OnDispose();
        targetTrans = null;
    }
}