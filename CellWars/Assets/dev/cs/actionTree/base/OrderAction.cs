using UnityEngine;
using System.Collections;

public class OrderAction:ActionTree
{
    public OrderAction():base()
    {
    }

    override protected void OnNext()
    {
        if (nodes.Count == 0)
        {

        }
        else
        {
            ActionNode node = nodes[0];
            node.OnExecute();
        }
    }

    override protected void OnRun()
    {
        OnNext();
    }
}
