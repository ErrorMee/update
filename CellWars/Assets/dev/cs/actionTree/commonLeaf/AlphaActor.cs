using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlphaActor : ActionNode
{
    private RectTransform targetTrans;
    private float toAlpha;
    private float tweenTime;

    public AlphaActor(RectTransform trans, float alpha, float millisecond)
        : base()
    {
        targetTrans = trans;
        toAlpha = alpha;
        tweenTime = millisecond / 1000.00f;
    }

    public override void OnExecute()
    {
        base.OnExecute();

        LeanTween.alpha(targetTrans, toAlpha, tweenTime).onComplete = CompleteHander;
    }

    private void CompleteHander()
    {
        targetTrans = null;
        OnEnd();
    }

}