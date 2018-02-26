using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class ActionTree:ActionNode
{
    protected List<ActionNode> nodes;

    public ActionTree()
    {
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

    public void RemoveNode(ActionNode node)
    {
        int index = nodes.IndexOf(node);
        if (-1 != index)
        {
            node.EventHandler -= OnEventHandler;
            nodes.RemoveAt(index);
        }
    }

    private void OnEventHandler(ActionNode node, ActionEventType eventType)
    {
        switch (eventType)
        {
            case ActionEventType.action_end:
                RemoveNode(node);
                node.OnDispose();
                if (IsEnd())
                {
                    OnEnd();
                }else
                {
                    OnNext();
                }
                break;
        }
    }

    private bool IsEnd()
	{
        if (nodes.Count <= 0)
        {
            return true;
        }
		return false;
	}

    abstract protected void OnNext();

    abstract protected void OnRun();

    override public void OnExecute()
    {
        if (IsEnd())
        {
            OnEnd();
        }
        else
        {
            OnRun();
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

}
