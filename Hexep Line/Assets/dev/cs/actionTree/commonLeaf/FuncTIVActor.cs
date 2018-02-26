using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FuncTIVActor : ActionNode
{
	private Action<List<TIVInfo>> callFunc;
	private List<TIVInfo> funcParams;

	public FuncTIVActor(Action<List<TIVInfo>> func,List<TIVInfo> p = null)
		: base()
	{
		callFunc = func;
		funcParams = p;
	}

	override public void OnExecute()
	{
		if(callFunc != null)
		{
			if(funcParams != null)
			{
				callFunc(funcParams);
			}
		}
		callFunc = null;
		funcParams = null;
		OnEnd();
	}
}