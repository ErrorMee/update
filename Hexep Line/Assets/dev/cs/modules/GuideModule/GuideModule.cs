using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuideModule : BaseModule 
{
	
	public Text fpsText;

    override protected void Awake()
    {
        base.Awake();
    }

	void Start () 
	{
        fpsText.text = "";
		ShowFPS ();
	}

	private void ShowFPS()
	{
		ShowFPS showFPS = gameObject.AddComponent<ShowFPS> ();
		showFPS.fpsText = fpsText;
	}
}
