using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PrepareModule : BaseModule {

    public Animation winOpenAnim;

	public RectTransform bgTrans;

	public static int LayItemCount;
	public VerticalLayoutGroup layOut;

    override protected void Awake()
	{
        base.Awake();

		LayItemCount = 0;

        BattleModel.Instance.ready_map = BattleModel.Instance.crtBattle.mapId;
	}

    void OnEnable()
    {
        winOpenAnim.Play();
    }

    void Start()
    {
		bgTrans.sizeDelta = new Vector2 (bgTrans.sizeDelta.x,layOut.spacing * LayItemCount + 180);

        GuildModel.Instance.CheckGuild();

		FontUtil.FixCN();
    }
}
