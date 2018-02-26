using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FuncActor : ActionNode
{
	private Action callFunc;

	public FuncActor(Action func)
		: base()
	{
		callFunc = func;
	}
	
	override public void OnExecute()
	{
		if(callFunc != null)
		{
			callFunc();
		}
		callFunc = null;
		OnEnd();
	}
}