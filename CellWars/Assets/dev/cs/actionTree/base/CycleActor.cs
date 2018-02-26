using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CycleActor : ActionNode
{
    protected List<ActionNode> nodes;
    private int crtNodeIndex = 0;

    private float ctrCycleTimes;
    public CycleActor(int cycleTimes = -1)
        : base()
    {
        ctrCycleTimes = cycleTimes;
        nodes = new List<ActionNode>();
    }

    public void AddNode(ActionNode node)
    {
        if (node == null)
        {
            return;
        }
        node.EventHandler += OnEventHandler;
        nodes.Add(node);
    }

    override public void OnExecute()
    {
        if (ctrCycleTimes == 0)
        {
            OnEnd();
        }
        else
        {
            OnNext();
        }
    }

    private void OnEventHandler(ActionNode node, ActionEventType eventType)
    {
        switch (eventType)
        {
            case ActionEventType.action_end:
                if (IsEnd())
                {
                    crtNodeIndex = 0;
                    OnEnd();
                }
                else
                {
                    crtNodeIndex++;
                    OnNext();
                }
                break;
        }
    }

    private bool IsEnd()
    {
        if ((crtNodeIndex + 1) >= nodes.Count)
        {
            return true;
        }
        return false;
    }

    private void OnNext()
    {
        ActionNode node = nodes[crtNodeIndex];
        node.OnExecute();
    }

    protected new void OnEnd()
    {
        if (ctrCycleTimes <= -1)//无限循环
        {
            OnNext();
        }
        else
        {
            if (ctrCycleTimes == 0)
            {
                OnEnd();
            }else
            {
                ctrCycleTimes--;
            }
        }
        
    }

    override public void OnDispose()
    {
        while (nodes.Count > 0)
        {
            ActionNode node = nodes[nodes.Count - 1];
            RemoveNode(node);
            node.OnDispose();
        }
    }

    private void RemoveNode(ActionNode node)
    {
        int index = nodes.IndexOf(node);
        if (-1 != index)
        {
            node.EventHandler -= OnEventHandler;
            nodes.RemoveAt(index);
        }
    }
}