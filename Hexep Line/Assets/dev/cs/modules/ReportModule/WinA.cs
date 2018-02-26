using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WinA : MonoBehaviour
{
    public ReportModule reportModule;

	public Image star1BGImage;
	public Image star2BGImage;
	public Image star3BGImage;

	public Image star1Image;
	public Image star2Image;
	public Image star3Image;

	public Button skillButton;

    public Button closeButton;
	public Button againButton;
	public Button nextButton;
    public Text scoreLabel;
    public NumberText scoreText;
    public NumberText coinText;
    public NumberText gemText;
	public Text nameText;
    public Text resultText;

	public BaseList bottleList;

    void Start()
    {
        UpdateView();
    }

    protected virtual void UpdateView()
    {
		nameText.text = LanguageUtil.GetTxt(11106) + ": " + BattleModel.Instance.crtConfig.name;
        resultText.text = LanguageUtil.GetTxt(11807);
        closeButton.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11804);
        againButton.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11803);
        nextButton.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11802);
        scoreLabel.text = LanguageUtil.GetTxt(11801);

        EventTriggerListener.Get(closeButton.gameObject).onClick = reportModule.OnCloseClick;
        EventTriggerListener.Get(againButton.gameObject).onClick = reportModule.OnAgainClick;
        EventTriggerListener.Get(nextButton.gameObject).onClick = reportModule.OnNextClick;

        float rollOffTime = 0;

        scoreText.RollNumber(FightModel.Instance.fightInfo.score, "", rollOffTime);
        rollOffTime += scoreText.maxRollTime;

        List<config_sort_item> scoreItems = ResModel.Instance.config_sort.GetItemsByType(3);
        for (int c = 0; c < scoreItems.Count;c++ )
        {
            config_sort_item scoreItem = scoreItems[c];
            if (scoreItem.refer <= FightModel.Instance.fightInfo.score)
            {
                SocialModel.Instance.ReportProgress(scoreItem.gid, 1);
            }
        }

        int star = FightModel.Instance.fightInfo.GetStarCount();
        
		star1Image.gameObject.SetActive(false);
		star2Image.gameObject.SetActive(false);
		star3Image.gameObject.SetActive(false);
        if (star >= 1)
        {
			star1BGImage.color = ColorUtil.GetColor(ColorUtil.COLOR_WIGHT,0.01f);
            star1Image.gameObject.SetActive(true);
            GameObject effectPrefab = ResModel.Instance.GetPrefab("effect/" + "effect_fireworks_n");
            GameObject itemEffect = GameObject.Instantiate(effectPrefab);
            itemEffect.transform.SetParent(star1Image.transform, false);
			itemEffect.transform.SetParent(transform, true);
        }

        if (star >= 2)
        {
			star2BGImage.color = ColorUtil.GetColor(ColorUtil.COLOR_WIGHT,0.01f);
            star2Image.gameObject.SetActive(true);
            GameObject effectPrefab = ResModel.Instance.GetPrefab("effect/" + "effect_fireworks_n");
            GameObject itemEffect = GameObject.Instantiate(effectPrefab);
            itemEffect.transform.SetParent(star2Image.transform, false);
			itemEffect.transform.SetParent(transform, true);
        }

        if (star >= 3)
        {
			star3BGImage.color = ColorUtil.GetColor(ColorUtil.COLOR_WIGHT,0.01f);
            star3Image.gameObject.SetActive(true);
            GameObject effectPrefab = ResModel.Instance.GetPrefab("effect/" + "effect_fireworks_n");
            GameObject itemEffect = GameObject.Instantiate(effectPrefab);
            itemEffect.transform.SetParent(star3Image.transform, false);
			itemEffect.transform.SetParent(transform, true);
        }

        int winCoin = (int)FightModel.Instance.fightInfo.score / (int)GameModel.Instance.GetGameConfig(1009);
        WealthInfo coinInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
        int winGem = 0;
        WealthInfo gemInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);

        bool isPassed = MapModel.Instance.IsPassed(BattleModel.Instance.crtConfig.id);
        if (isPassed)
        {
            int coinAdd = (int)GameModel.Instance.GetGameConfig(1008);
            winCoin += coinAdd;
            coinInfo.count += winCoin;
            coinText.RollNumber(winCoin, "+", rollOffTime);
            rollOffTime += coinText.maxRollTime;
            gemText.RollNumber(winGem, "+", rollOffTime);
            rollOffTime += gemText.maxRollTime;
        }
        else
        {
			int coinAdd = (int)GameModel.Instance.GetGameConfig(1005);;
            winCoin += coinAdd;
            coinInfo.count += winCoin;
            coinText.RollNumber(winCoin, "+", rollOffTime);
            rollOffTime += coinText.maxRollTime;
			int gemAdd = (int)GameModel.Instance.GetGameConfig(1006);;
            winGem += gemAdd;
            gemInfo.count += winGem;
            gemText.RollNumber(winGem, "+", rollOffTime);
            rollOffTime += gemText.maxRollTime;
        }

		GameObject bottlePrefab = ResModel.Instance.GetPrefab("prefab/reportmodule/" + "ReportBottle");
		bottleList.itemPrefab = bottlePrefab;

		bool findSkillLv = false;

		for (int i = 0; i < SkillTempletModel.Instance.skillGroups.Count; i++)
		{
			SkillGroupInfo skillTempletGroupInfo = SkillTempletModel.Instance.skillGroups[i];

			int groupId = skillTempletGroupInfo.GetGroupId();

			int groupCount = CollectModel.Instance.profileCollect.GetCount(groupId);

			if(groupCount > 0)
			{
				Transform bottleTrans = bottleList.NewItem().GetComponent<Transform>();
				bottleTrans.name = "" + i;

				Image mask = bottleTrans.FindChild("mask").GetComponent<Image>();
				mask.color = ColorUtil.GetColor(ColorUtil.GetCellColorValue(groupId));
				Image icon = bottleTrans.FindChild("icon").GetComponent<Image>();
				icon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + groupId);
				NumberText numText = bottleTrans.FindChild("Text").GetComponent<NumberText>();
                numText.RollNumber(groupCount, "+", rollOffTime);
                rollOffTime += numText.maxRollTime;
				WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(groupId);
				bottleInfo.count += groupCount;

				EventTriggerListener.Get(bottleTrans.gameObject).onClick = OnOpenSkill;

                string prefStr = PlayerPrefsUtil.BOTTLE_COLLECT + groupId;
                PlayerPrefsUtil.SetInt(prefStr, PlayerPrefsUtil.GetInt(prefStr) + groupCount);

                config_sort_item config_sort_item = ResModel.Instance.config_sort.GetItemByTypeAndSpecial(2, "" + groupId);

                float bottleProgress = PlayerPrefsUtil.GetInt(prefStr) / (config_sort_item.refer + 0.00f);

                SocialModel.Instance.ReportProgress(config_sort_item.gid, bottleProgress);
			}

			SkillTempletInfo skillTempletInfo = skillTempletGroupInfo.skillTemplets[1];

			if(skillTempletInfo.IsUnlock() && skillTempletInfo.config.type == 1 && findSkillLv == false && skillButton != null)
			{
				if(skillTempletInfo.lv < SkillTempletModel.MAX_LEVEL)
				{
					int leftStar = MapModel.Instance.starInfo.GetSkillCanUseStar();
					if(leftStar >= skillTempletInfo.LevelUpCostStar())
					{
						int levelUpNeedBottle = skillTempletInfo.LevelUpCostBottle();
						WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(groupId);
						if(bottleInfo.count >= levelUpNeedBottle )
						{
							findSkillLv = true;
							skillButton.gameObject.SetActive(true);
							SkillTempletModel.Instance.selectGroupIndex = i;
							Image skillIcon = skillButton.transform.FindChild("icon").GetComponent<Image>();
							skillIcon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + groupId);
							EventTriggerListener.Get(skillButton.gameObject).onClick = OnClickSkill;
						}
					}
				}
			}
		}

		if(findSkillLv == false && skillButton != null)
		{
			skillButton.gameObject.SetActive(false);
		}

		PlayerModel.Instance.SaveWealths();

        MapInfo mapInfo = new MapInfo();
        mapInfo.configId = BattleModel.Instance.crtConfig.id;
        mapInfo.score = FightModel.Instance.fightInfo.score;
        mapInfo.star = star;

        MapModel.Instance.PassLevel(mapInfo);
		nextButton.gameObject.SetActive(FightModel.Instance.fightInfo.isWin);

        config_sort_item star_item = ResModel.Instance.config_sort.GetItemByTypeAndSpecial(1, "11104");
        SocialModel.Instance.ReportScore(star_item.gid, MapModel.Instance.starInfo.crtStar);

		config_sort_item level_item = ResModel.Instance.config_sort.GetItemByTypeAndSpecial(1, "1");
		SocialModel.Instance.ReportScore(level_item.gid, MapModel.Instance.starInfo.openMapFullStar/3);

		config_sort_item diamond_item = ResModel.Instance.config_sort.GetItemByTypeAndSpecial(1, "11101");
		SocialModel.Instance.ReportScore(diamond_item.gid, gemInfo.count);
    }

	public void OnOpenSkill(GameObject go)
	{
		BattleModel.Instance.lose_map = 0;
		SkillTempletModel.Instance.selectGroupIndex = Convert.ToInt32(go.name);
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.SKILL);
	}

	private void OnClickSkill(GameObject go)
	{
		BattleModel.Instance.lose_map = 0;
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.SKILL);
	}
}
