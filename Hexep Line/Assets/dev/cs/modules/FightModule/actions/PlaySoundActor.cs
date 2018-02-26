using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaySoundActor : ActionNode
{
    private string sound;

    public PlaySoundActor(string soundName)
        : base()
    {
        sound = soundName;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        AudioModel.Instance.PlayeActorSound(sound);
        OnEnd();
    }
}