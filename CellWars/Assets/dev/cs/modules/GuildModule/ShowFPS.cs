
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour {
	public float f_UpdateInterval = 0.5F;
	
	private float f_LastInterval;
	
	private int i_Frames = 0;
	
	private float f_Fps;
    private float f_CMem;
    private float f_AMem;

	public Text fpsText;
	
	// Use this for initialization
	void Start () {
		f_LastInterval = Time.realtimeSinceStartup;
		
		i_Frames = 0;
	}
	
	// Update is called once per frame
	void Update () {
		++i_Frames;
		
		if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
		{
			f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            f_CMem = (int)(Profiler.GetTotalReservedMemory() / 1024.0f / 1024.0f);
            f_AMem = (int)(Profiler.GetTotalAllocatedMemory() / 1024.0f / 1024.0f);

			i_Frames = 0;
			
			f_LastInterval = Time.realtimeSinceStartup;
		}

		if(fpsText != null)
		{
            fpsText.text = "F:" + f_Fps.ToString("f2") + " M:" + f_AMem + "/" + f_CMem;
		}
	}
}