using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeMonsterActor : ActionNode
{

    private FightMonsterItem item;
    private MonsterInfo info;

    public ChangeMonsterActor(FightMonsterItem monsterItem, MonsterInfo monsterInfo)
        : base()
    {
        item = monsterItem;
        info = monsterInfo;
    }

    public override void OnExecute()
    {
        base.OnExecute();

        if (item != null)
        {
            item.transform.localScale = new Vector3(1, 1, 1);
            item.monsterInfo = info;
            if (info.config != null)
            {
                item.icon = info.config.icon;
            }
            else
            {
                item.icon = 0;
            }
        }

        CompleteHander();
    }

    private void CompleteHander()
    {
        item = null;
        info = null;
        OnEnd();
    }

}