using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddStepActor : ActionNode
{
    private int addCount;
    private Transform layerT;

    public AddStepActor(int count)
        : base()
    {
        addCount = count;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        PropModel.Instance.PropAddSetpEvent(addCount);
        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11512) + "+" + addCount);
        CompleteHander();
    }

    private void CompleteHander()
    {
        addCount = 0;
        OnEnd();
    }

}