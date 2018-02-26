using UnityEngine;
using System.Collections;

public enum ActionEventType
{
    action_end = 0,
    action_continue = 1,
}

public delegate void ActionEventHandler(ActionNode node, ActionEventType eventType);

public class ActionNode
{

    public event ActionEventHandler EventHandler;

    public ActionNode()
    {
    }

    virtual public void OnExecute()
    {
    }

    virtual public void OnDispose()
    {
    }

    protected void OnEnd()
    {
        if (EventHandler != null)
        {
            EventHandler(this, ActionEventType.action_end);
        }
    }
}
