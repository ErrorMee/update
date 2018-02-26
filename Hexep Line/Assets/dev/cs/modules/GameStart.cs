using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	void Awake()
	{
		Application.targetFrameRate = 30;
	}

	void Start () 
	{
		ResModel.Instance.Init ();
		ModuleModel.Instance.StartGame ();
	}

}
