using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoseA : MonoBehaviour
{
    public ReportModule reportModule;

    public Image star1Image;
    public Image star2Image;
    public Image star3Image;

    public Button closeButton;
    public Button againButton;
    public Text scoreLabel;	
    public Text scoreText;
	public Text nameText;
    public Text resultText;

    void Start()
    {
        UpdateView();
    }

    protected virtual void UpdateView()
    {
		nameText.text = LanguageUtil.GetTxt(11106) + ": " + BattleModel.Instance.crtConfig.name;
        resultText.text = LanguageUtil.GetTxt(11808);
        closeButton.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11804);
        againButton.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11803);
        scoreLabel.text = LanguageUtil.GetTxt(11801);

        EventTriggerListener.Get(closeButton.gameObject).onClick = reportModule.OnCloseClick;
        EventTriggerListener.Get(againButton.gameObject).onClick = reportModule.OnAgainClick;

        scoreText.text = "" + FightModel.Instance.fightInfo.score;
        int star = FightModel.Instance.fightInfo.GetStarCount();

        star1Image.gameObject.SetActive(false);
        star2Image.gameObject.SetActive(false);
        star3Image.gameObject.SetActive(false);
        if (star >= 1)
        {
            star1Image.gameObject.SetActive(true);
        }

        if (star >= 2)
        {
            star2Image.gameObject.SetActive(true);
        }

        if (star >= 3)
        {
            star3Image.gameObject.SetActive(true);
        }
    }
}
