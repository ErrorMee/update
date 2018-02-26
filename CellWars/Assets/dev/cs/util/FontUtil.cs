using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FontUtil
{
    public const string FONT_1 = "msyhbd";
    public const string FONT_2 = "tahomabd";

    public static string FONT_DEFAULT = FONT_2;

    public static void SetAllFont(Transform trans, string fontName)
    {
        Text[] texts = trans.GetComponentsInChildren<Text>(true);
        Font font = GameMgr.resourceMgr.GetFont(fontName);
        foreach(Text text in texts)
        {
            text.font = font;
        }
    }

    public static void ChangeFont(string fontName)
    {
        FontUtil.FONT_DEFAULT = fontName;
		UpdateFont();
    }

	public static void UpdateFont()
	{
		Transform canvas = GameObject.Find("Canvas").transform;
		SetAllFont(canvas, FontUtil.FONT_DEFAULT);
	}

	public static void FixCN()
	{
		if(FontUtil.FONT_DEFAULT == FONT_1)
		{
			OnFixCN();
			//LeanTween.delayedCall(0.1f,FontUtil.OnFixCN);
		}
	}

	public static void OnFixCN()
	{
		Transform canvas = GameObject.Find("Canvas").transform;
		Text[] texts = canvas.GetComponentsInChildren<Text>();
		foreach(Text text in texts)
		{
			text.gameObject.SetActive(false);
			text.gameObject.SetActive(true);
		}
	}
}
