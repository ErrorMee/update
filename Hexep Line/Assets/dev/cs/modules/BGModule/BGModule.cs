using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BGModule : BaseModule {

	void Start()
	{
		HiveBG hiveBG = GetComponentInChildren<HiveBG>();

		hiveBG.ShowList (-5, 5, -7, 7, 0.95f);
	}
		
}
