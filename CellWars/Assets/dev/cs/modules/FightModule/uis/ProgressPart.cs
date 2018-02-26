using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressPart : MonoBehaviour {

    public Image star1;
    public Image star2;
    public Image star3;

    public int starCount = 0;

	public Image progressCircle1;
	public Image progressCircle2;

	public Text finalScore;
    public Text tempScore;

    public Text levelText;

    private float fullFix = 0.77f;

    void Awake()
    {
        levelText.text = BattleModel.Instance.crtConfig.name;
        finalScore.text = "";
    }

	void Start () {
        star1.gameObject.SetActive(false);
        star2.gameObject.SetActive(false);
        star3.gameObject.SetActive(false);
        progressCircle1.fillAmount = 0;
        progressCircle2.fillAmount = 0;
	}
	
	void Update () {
	}

    public int CollectScore(bool isTemp)
	{

        int crtScore = CollectModel.Instance.profileScore;
        if (isTemp)
        {
            crtScore = crtScore + CollectModel.Instance.tempScore;
        }

        starCount = 0;

        int topScore = FightModel.Instance.fightInfo.judge_score[0];
        int offsetScore = 0;
        float progress = (crtScore - offsetScore + 0.000f) / (topScore - offsetScore);

        if (progress >= 1)
        {
            starCount = 1;
            topScore = FightModel.Instance.fightInfo.judge_score[1];
            offsetScore = FightModel.Instance.fightInfo.judge_score[0];
            progress = (crtScore - offsetScore + 0.000f) / (topScore - offsetScore);
        }

        if (progress >= 1)
        {
            starCount = 2;
            topScore = FightModel.Instance.fightInfo.judge_score[2];
            offsetScore = FightModel.Instance.fightInfo.judge_score[1];
            progress = (crtScore - offsetScore + 0.000f) / (topScore - offsetScore);
        }

        if (progress >= 1)
        {
            starCount = 3;
            progressCircle1.fillAmount = 1 * fullFix;
            if (isTemp == false)
            {
                progressCircle2.fillAmount = 1 * fullFix;
            }
        }
        else
        {
            progressCircle1.fillAmount = (starCount + progress) / 3.000f * fullFix;
            if (isTemp == false)
            {
                progressCircle2.fillAmount = (starCount + progress) / 3.000f * fullFix;
            }
        }

        if (isTemp == false)
        {
            if (starCount > 0)
            {
                star1.gameObject.SetActive(true);
            }
            if (starCount > 1)
            {
                star2.gameObject.SetActive(true);
            }
            if (starCount > 2)
            {
                star3.gameObject.SetActive(true);
            }
        }

		tempScore.text = "";
		if(isTemp)
		{
            if (CollectModel.Instance.profileScore > 0)
            {
                
                if (CollectModel.Instance.tempScore <= 0)
                {
                    finalScore.text = "<color=#ffffff>" + CollectModel.Instance.profileScore + "</color>";
                }
                else
                {
                    finalScore.text = "<color=#ffffff>" + CollectModel.Instance.profileScore + "</color><color=#00ff00>+" + CollectModel.Instance.tempScore + "</color>";
                }
            }
            else
            {
                if (CollectModel.Instance.tempScore <= 0)
                {
                    finalScore.text = "";
                }
                else
                {
                    finalScore.text = "<color=#00ff00>+" + CollectModel.Instance.tempScore + "</color>";
                }
            }
			if(CollectModel.Instance.isCrit)
			{
				tempScore.text = "×" + CollectModel.SCORE_CRIT;
			}
		}else
		{
            finalScore.text = "<color=#ffffff>" + CollectModel.Instance.profileScore + "</color>";
        }

        return starCount;
	}
}
