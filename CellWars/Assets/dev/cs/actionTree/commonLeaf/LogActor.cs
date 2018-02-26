using UnityEngine;
using System.Collections;

public class LogActor : ActionNode
{
    private string log = "";
    public LogActor(string data)
        : base()
    {
        log = data;
    }

    public override void OnExecute()
    {
        Debug.Log(log);
        log = null;
        OnEnd();
    }
}