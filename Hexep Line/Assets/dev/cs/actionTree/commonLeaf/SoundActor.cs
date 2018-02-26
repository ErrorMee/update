using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SoundActor : ActionNode
{
	private string soundName;

	public SoundActor(string soundP)
		: base()
	{
		soundName = soundP;
	}

	override public void OnExecute()
	{
		AudioModel.Instance.PlayeSound(soundName);
		OnEnd();
	}
}