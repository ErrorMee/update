
using UnityEngine;
using System.Collections;

public class RotationActor : ActionNode
{
    private RectTransform targetTrans;
    private float zrotate;

    public RotationActor(RectTransform target,float _zrotate)
        : base()
    {
        targetTrans = target;
        zrotate = _zrotate;
    }

    public override void OnExecute()
    {
        base.OnExecute();

        targetTrans.localRotation = Quaternion.Euler(0, 0, zrotate);

        targetTrans = null;
        OnEnd();
    }
}