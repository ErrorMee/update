using UnityEngine;
using System.Collections;

public class ParallelAction:ActionTree
{
    public ParallelAction():base()
    {
    }

    override protected void OnNext()
    {
        //Debug.Log("ParallelAction left:" + nodes.Count);
    }

    override protected void OnRun()
    {
        int count = nodes.Count;
        int i;
        for (i = count - 1; i >= 0; i--)
        {
            ActionNode node = nodes[i];
            node.OnExecute();
        }

    }
}
