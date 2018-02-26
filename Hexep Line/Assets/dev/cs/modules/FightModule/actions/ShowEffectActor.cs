using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowEffectActor : ActionNode
{
    private Transform parentTrans;
    private string effectName;
    private Transform parentOff;

    public ShowEffectActor(Transform parent_trans, string effect_name, Transform parent_off = null)
        : base()
    {
        parentTrans = parent_trans;
        effectName = effect_name;
        parentOff = parent_off;
    }

    public override void OnExecute()
    {
        base.OnExecute();

        GameObject itemPrefab = ResModel.Instance.GetPrefab("effect/" +  effectName);
        GameObject item = GameObject.Instantiate(itemPrefab);
        item.name = effectName;
        item.transform.SetParent(parentTrans, false);

        if (parentOff != null)
        {
            item.transform.SetParent(parentOff, true);
        }
        
        CompleteHander();
    }

    private void CompleteHander()
    {
        parentTrans = null;
        parentOff = null;
        OnEnd();
    }
}