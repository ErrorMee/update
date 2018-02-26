using UnityEngine;
using System.Collections;

public class WaitActor : ActionNode
{
    private float time;
    public WaitActor(int millisecond)
        : base()
    {
        time = millisecond / 1000.00f;
    }

    override public void OnExecute()
    {
        LeanTween.delayedCall(time, CompleteHander);
    }

    private void CompleteHander()
    {
        OnEnd();
    }
}