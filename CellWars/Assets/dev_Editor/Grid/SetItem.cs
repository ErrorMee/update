using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetItem : MonoBehaviour {

	public Text label;

	public InputField input;

	void Awake()
	{
	}

	public void SetLabel(string str)
	{
		label.text = str;
	}

	public void SetVaule(ref int value)
	{
		input.text = "" + value;
	}

	public void SetVaule(ref short value)
	{
		input.text = "" + value;
	}

	public void SetVaule(ref string value)
	{
		input.text = "" + value;
	}

	public string GetValueStr()
	{
		if(input.text == null)
		{
			return "";
		}
		return input.text;
	}
}
