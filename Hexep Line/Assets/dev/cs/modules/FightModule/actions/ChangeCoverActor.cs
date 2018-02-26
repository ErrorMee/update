using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeCoverActor : ActionNode
{
    private CoverLayer layer;
    private FightCoverItem item;
    private CoverInfo info;

    public ChangeCoverActor(CoverLayer coverLayer,FightCoverItem coverItem, CoverInfo coverInfo)
        : base()
    {
        layer = coverLayer;
        item = coverItem;
        info = coverInfo;
    }

    public override void OnExecute()
    {
        base.OnExecute();

        if (item == null)
        {
            item = layer.CreateCoverItem(info);
        }

        if (item != null)
        {
            item.transform.localScale = new Vector3(1, 1, 1);
            item.coverInfo = info;
            if (info.config != null)
            {
                item.icon = info.config.icon;
            }
            item.UpdateTip();
        }
        
        CompleteHander();
    }

    private void CompleteHander()
    {
        layer = null;
        item = null;
        info = null;
        OnEnd();
    }

}