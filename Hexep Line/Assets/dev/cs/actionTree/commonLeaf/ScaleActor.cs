using UnityEngine;
using System.Collections;

public class ScaleActor : ActionNode
{
    private RectTransform targetTrans;
    private Vector3 toScale;
    private float tweenTime;

    public ScaleActor(RectTransform target, Vector3 scale, float time)
        : base()
    {
        targetTrans = target;
        toScale = scale;
        tweenTime = time;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        if (targetTrans == null)
        {
            CompleteHander();
            return;
        }
        LeanTween.scale(targetTrans, toScale, tweenTime).onComplete = CompleteHander;
    }

    private void CompleteHander()
    {
        targetTrans = null;
        OnEnd();
    }
}