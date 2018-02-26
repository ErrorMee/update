using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClearEffectActor : ActionNode
{
    private Transform parentTrans;
    private string effectName;

    public ClearEffectActor(Transform parent_trans, string effect_name)
        : base()
    {
        parentTrans = parent_trans;
        effectName = effect_name;
    }

    public override void OnExecute()
    {
        base.OnExecute();

        Transform item = parentTrans.Find(effectName);

        if (item == null)
        {
            parentTrans = null;
            OnEnd();
        }
        else
        {
            LeanTween.scale((RectTransform)item, new Vector3(0,0,0), 0.2f).onComplete = CompleteHander;
        }
    }

    private void CompleteHander()
    {
        Transform item = parentTrans.Find(effectName);
        GameObject.Destroy(item.gameObject);
        parentTrans = null;
        OnEnd();
    }
}