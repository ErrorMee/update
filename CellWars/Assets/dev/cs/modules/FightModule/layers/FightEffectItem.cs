using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightEffectItem : MonoBehaviour
{
	public int control_x;
	public int control_y;

	public Transform bomb_select;

	void Awake()
	{
		bomb_select = transform.Find("bomb_select");
		SetBombSelect(false);
	}

	public void SetBombSelect(bool isBomb)
	{
		bomb_select.gameObject.SetActive (isBomb);
	}

}
