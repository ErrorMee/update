using UnityEngine;
using System.Collections;

public class RoatateActor : ActionNode
{
    private RectTransform targetTrans;
    private Vector3 toRotate;
    private float tweenTime;

    public RoatateActor(RectTransform target, Vector3 ratates, float time)
        : base()
    {
        targetTrans = target;
        toRotate = ratates;
        tweenTime = time;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        
        LeanTween.rotate(targetTrans.gameObject, toRotate, tweenTime).onComplete = CompleteHander;
    }

    private void CompleteHander()
    {
        targetTrans = null;
        OnEnd();
    }
}