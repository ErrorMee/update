using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapModule : BaseModule {

    public Transform energyButton;
    public Transform starButton;
	public Animation starTipAnim;
    public Transform coinButton;
    public Transform gemButton;

    public Transform skillBtn;
	public Animation skillTipAnim;

	public Transform adBtn;
    public Transform setBtn;

    private GameObject mapItemPrefab;
    public BaseList mapList;

    private GameObject chapterItemPrefab;
    public BaseList chapterList;
    public RectTransform chapterRect;
    public ToggleGroup chapterToggleGroup;

	public Transform buggerPos;
	private Bugger bugger;

	public Transform comingTrans;

    override protected void Awake()
    {
        base.Awake();

		comingTrans.gameObject.SetActive(false);

		EventTriggerListener.Get(energyButton.gameObject).onClick = OnEnergyClick;
        EventTriggerListener.Get(gemButton.gameObject).onClick = OnGemClick;
		EventTriggerListener.Get(coinButton.gameObject).onClick = OnCoinClick;
		EventTriggerListener.Get(starButton.gameObject).onClick = OnStarClick;

        EventTriggerListener.Get(skillBtn.gameObject).onClick = OnSkillClick;
        EventTriggerListener.Get(setBtn.gameObject).onClick = OnSetClick;
		EventTriggerListener.Get(adBtn.gameObject).onClick = OnADClick;

        PlayerModel.Instance.updateWealthEvent += OnUpdateWealthEvent;
		MapModel.Instance.StarRewardEvent += OnStarRewardEvent;

        PromptModel.Instance.SlideEvent = OnSlideEvent;
		MapModel.Instance.SwitchChapterEvent = OnSwitchChapterEvent;
        MapModel.Instance.SelectChapterEvent = OnSelectChapterEvent;

		mapItemPrefab = ResModel.Instance.GetPrefab("prefab/mapmodule/MapItem");
        mapList.itemPrefab = mapItemPrefab;

		chapterItemPrefab = ResModel.Instance.GetPrefab("prefab/mapmodule/ChapterItem");
        chapterList.itemPrefab = chapterItemPrefab;

		ScreenSlider.OpenSlid();

        InitChapterList();

        UpdateUI();

        AudioModel.Instance.PlayMusic("beach_brird");
    }

	void OnDestroy()
	{
		PlayerModel.Instance.updateWealthEvent -= OnUpdateWealthEvent;
		MapModel.Instance.StarRewardEvent -= OnStarRewardEvent;
	}

    void OnDisable()
    {
        PlayerModel.Instance.updateWealthsEvent -= UpdateWealths;
		MapModel.Instance.StarRewardEvent -= OnStarRewardEvent;
        PromptModel.Instance.SlideEvent = null;
    }

    void Start()
    {
        PlayerModel.Instance.updateWealthsEvent += UpdateWealths;

        GuideModel.Instance.CheckGuide();
    }

    private void UpdateUI()
    {
		
		bool adOpen = MapModel.Instance.IsPassed ((int)GameModel.Instance.GetGameConfig(1010));
		if (adOpen && ADModel.Instance.ADIsReady ()) {
			adBtn.gameObject.SetActive (true);
		} else {
			adBtn.gameObject.SetActive (false);
		}

        UpdateWealths();

		ShowStar();

		comingTrans.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11105);

        skillBtn.gameObject.SetActive(SkillTempletModel.Instance.SkillModuleIsUnlock());
        SkillTempletModel.Instance.selectGroupIndex = 0;
		skillTipAnim.Stop();
		for (int i = 0; i < SkillTempletModel.Instance.skillGroups.Count; i++)
		{
			SkillGroupInfo skillTempletGroupInfo = SkillTempletModel.Instance.skillGroups[i];
			skillTempletGroupInfo.index = i;
			int bottleId = skillTempletGroupInfo.GetGroupId();
			for(int j = 0;j<skillTempletGroupInfo.skillTemplets.Count;j++)
			{
				SkillTempletInfo skillTempletInfo = skillTempletGroupInfo.skillTemplets[j];

				if(skillTempletInfo.IsUnlock() && skillTempletInfo.config.type == 1)
				{
					if(skillTempletInfo.lv < SkillTempletModel.MAX_LEVEL)
					{
						int leftStar = MapModel.Instance.starInfo.GetSkillCanUseStar();
						if(leftStar >= skillTempletInfo.LevelUpCostStar())
						{
							int levelUpNeedBottle = skillTempletInfo.LevelUpCostBottle();
							WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(bottleId);
							if(bottleInfo.count >= levelUpNeedBottle )
							{
								SkillTempletModel.Instance.selectGroupIndex = i;
								Image skillIcon = skillBtn.FindChild("icon").GetComponent<Image>();

								skillIcon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + bottleId);
								skillTipAnim.Play();
								return;
							}
						}
					}
				}
			}
		}
    }

	private void OnStarRewardEvent(config_chapter_item config)
	{
		ShowStar();
	}

	private void ShowStar()
	{
		StarInfo starInfo = MapModel.Instance.starInfo;
		starButton.GetComponentInChildren<Text>().text = starInfo.crtStar + "/" + starInfo.openMapFullStar;
		starTipAnim.Stop();

		List<config_chapter_item> datas = ResModel.Instance.config_chapter.data;
		int totalChapterCount = datas.Count;
		int i;
		for (i = 0; i < totalChapterCount; i++)
		{
			config_chapter_item config_chapter = datas[i];
			List<int> mapIds = config_chapter.GetMapIds();

			if(mapIds.Count > 0)
			{
				int allStars = 0;
				int fullStar = 0;
				int j;
				for (j = 0; j < mapIds.Count; j++)
				{
					config_map_item config_map = (config_map_item)ResModel.Instance.config_map.GetItem(mapIds[j]);

					MapInfo mapInfo = MapModel.Instance.GetMapInfo(config_map.id);

					if(mapInfo != null)
					{
						allStars += mapInfo.star;
					}

					fullStar += 3;
				}

				if(allStars >= fullStar)
				{
					ChapterInfo chapter = MapModel.Instance.GetChapterInfo(config_chapter.id);

					if(chapter == null || chapter.reward == false)
					{
						starTipAnim.Play();
						return;
					}
				}
			}
		}

	}

    private void OnUpdateWealthEvent(int id)
    {
        WealthInfo wealthInfo = PlayerModel.Instance.GetWealth(id);

        if (id == (int)WealthTypeEnum.Gem)
        {
            NumberText numberText = gemButton.GetComponentInChildren<NumberText>();
            int startNum = Convert.ToInt32(numberText.text);
            numberText.RollNumberStart(wealthInfo.count, startNum);
        }
    }

    private void UpdateWealths()
    {
        WealthInfo energyInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Energy);
        energyButton.GetComponentInChildren<Text>().text = energyInfo.count + "/" + energyInfo.limit;

        WealthInfo coinInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
        coinButton.GetComponentInChildren<Text>().text = coinInfo.count + "";

        gemButton.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem).count + "";
    }

	private void OnEnergyClick(GameObject go)
	{
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.ENERGY);
	}

	private void OnGemClick(GameObject go)
	{
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.WEALTH);
	}

	private void OnCoinClick(GameObject go)
	{
		PlayerModel.Instance.ExchangeWealth ((int)WealthTypeEnum.Coin, 5000, null);
	}

	private void OnStarClick(GameObject go)
	{
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.STAR);
	}

    private void OnSkillClick(GameObject go)
    {
        ModuleModel.Instance.AddUIModule((int)ModuleEnum.SKILL);
    }

    private void OnSetClick(GameObject go)
    {
        ModuleModel.Instance.AddUIModule((int)ModuleEnum.SET);
    }

	private void OnADClick(GameObject go)
    {
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.ADREWARD);
    }

    private void InitChapterList()
    {
        chapterList.ClearList();

        List<config_chapter_item> datas = ResModel.Instance.config_chapter.data;
        int totalChapterCount = datas.Count;
        int i;
        for (i = 0; i < totalChapterCount; i++)
        {
			config_chapter_item config_chapter = datas[i];
			CreateChapterItem(config_chapter, i + 1);
        }
        SwitchChapter();
    }

    private void OnSlideEvent(bool isLeft)
    {
		MapModel.Instance.SlideChapter(isLeft);
    }

	private void OnSwitchChapterEvent()
	{
		SwitchChapter();
	}

    private void SwitchChapter()
    {
		ChapterItem.ForceSelect = true;
        List<config_chapter_item> datas = ResModel.Instance.config_chapter.data;
        int totalChapterCount = datas.Count;

		if(MapModel.Instance.selectChapter == null)
		{
			MapModel.Instance.selectChapter = datas[datas.Count - 1];
		}

		if (MapModel.Instance.selectChapter.GetIndex() <= 3)
		{
			chapterRect.anchoredPosition = new Vector2(-500, chapterRect.anchoredPosition.y);
		}
		else if (MapModel.Instance.selectChapter.GetIndex() > (totalChapterCount - 5))
		{
			chapterRect.anchoredPosition = new Vector2(-500 - (totalChapterCount - 5) * 200, chapterRect.anchoredPosition.y);
		}
		else
		{
			chapterRect.anchoredPosition = new Vector2(-500 - (MapModel.Instance.selectChapter.GetIndex() - 1) * 200, chapterRect.anchoredPosition.y);
		}

        int i;
        int count = chapterList.items.Count;
        for (i = 0; i < count; i++)
        {
            GameObject item = chapterList.items[i];
            ChapterItem itemCtr = item.GetComponent<ChapterItem>();
            if (itemCtr.viewIndex == MapModel.Instance.selectChapter.GetIndex())
            {
                itemCtr.toggle.isOn = true;
                break;
            }
        }
    }

    private void InitMapList()
    {
        mapList.ClearList();
		List<int> mapIds = MapModel.Instance.selectChapter.GetMapIds();
		AudioModel.Instance.PlayeSound("click");
		if(mapIds.Count < 1)
		{
			comingTrans.gameObject.SetActive(true);
		}else
		{
			comingTrans.gameObject.SetActive(false);

			//string[] mapIndexs = MapModel.Instance.selectChapter.GetMapIndexs();
			int i;
			for (i = 0; i < mapIds.Count; i++)
			{
				config_map_item config_map = (config_map_item)ResModel.Instance.config_map.GetItem(mapIds[i]);

				CreateMapItem(config_map,i);
			}
		}
    }

    private ChapterItem CreateChapterItem(config_chapter_item config_chapter, int index)
    {
        GameObject item = chapterList.NewItem();
        ChapterItem itemCtr = item.GetComponent<ChapterItem>();
        itemCtr.viewIndex = index;

        itemCtr.toggle.group = chapterToggleGroup;
        
        itemCtr.SetConfig(config_chapter);

        itemCtr.toggle.onValueChanged.AddListener(itemCtr.OnSelect);
        return itemCtr;
    }

    private void OnSelectChapterEvent()
    {
        InitMapList();
    }

	private void CreateMapItem(config_map_item config_map,int index)
    {
        GameObject item = mapList.NewItem();

		RectTransform rectTransform = (RectTransform)item.transform;
		int offX = index % 3 - 1;
        float offY = 2.5f - (int)Mathf.Ceil((index + 1) / 3.0f);
        rectTransform.anchoredPosition = new Vector2(offX * 260, offY * 270 + 30);

        item.name = "" + config_map.id;
        MapInfo mapInfo = MapModel.Instance.GetMapInfo(config_map.id);
		SetMapInfo(item, mapInfo,config_map);
	}

    private void SetMapInfo(GameObject item, MapInfo mapInfo,config_map_item config = null)
    {
		if(item == null)
		{
			return;
		}

		MapItem itemCtr = item.GetComponent<MapItem>();
		itemCtr.config = config;

        if (mapInfo == null)
        {
            if (config != null && config.id < MapModel.Instance.TestMapEnd)
            {
                itemCtr.SetView(true,0);
                EventTriggerListener.Get(item).onClick = OnMapClick;
            }
            else
            {
                itemCtr.SetView(false,0);
                EventTriggerListener.Get(item).onClick = OnMapClick;
            }
        }
        else
        {
            itemCtr.SetView(true, mapInfo.star,mapInfo.UseOldStar());
            EventTriggerListener.Get(item).onClick = OnMapClick;
        }
    }

    private void OnMapClick(GameObject go)
    {
		MapItem itemCtr = go.GetComponent<MapItem>();

		config_map_item config = itemCtr.config;

		if (config != null)
        {
			int endlevel = (int)GameModel.Instance.GetGameConfig(1007);
			if (config.id > endlevel)
            {
				config_item_base mapconfigitem = ResModel.Instance.config_map.GetItem(endlevel);
                PromptModel.Instance.Pop(string.Format(LanguageUtil.GetTxt(11101), Convert.ToInt32(mapconfigitem.name)));
                return;
            }

            if (itemCtr.btn.interactable == false)
            {
                PromptModel.Instance.Pop(LanguageUtil.GetTxt(11103));
                return;
            }

            BattleModel.Instance.InitCrtBattle(config);
            BattleModel.Instance.InitFight();
			
            SkillTempletModel.Instance.UpdataTemplet();

            FillModel.Instance.InitFillInfo();

            ModuleModel.Instance.AddUIModule((int)ModuleEnum.PREPARE);
        }
        else
        {

            Debug.Log(LanguageUtil.GetTxt(11102));
        }
    }
}
