using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressMonsterActor : ActionNode
{
    private FightMonsterItem item;
    private float progressAbsorb;

    public ProgressMonsterActor(FightMonsterItem monsterItem,float progress)
        : base()
    {
        item = monsterItem;
        progressAbsorb = progress;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        item.ShowProgress(progressAbsorb);
        item.ShowBan(item.monsterInfo.banCount > 1);
        CompleteHander();
    }

    private void CompleteHander()
    {
        item = null;
        OnEnd();
    }

}