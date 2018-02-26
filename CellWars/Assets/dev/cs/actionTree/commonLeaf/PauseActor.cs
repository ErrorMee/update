using UnityEngine;
using System.Collections;

public class PauseActor : ActionNode
{
    public int pauseId;
    public PauseActor(int id = 0):base()
    {
        pauseId = id;
    }

    public void OnContinue()
    {
        OnEnd();
    }
}
