using UnityEngine;
using System.Collections;

public class DestroyActor : ActionNode
{
    private Object targetObj;
    public DestroyActor(Object target)
        : base()
    {
        targetObj = target;
    }

    public override void OnExecute()
    {
        GameObject.Destroy(targetObj);
        OnEnd();
    }
}