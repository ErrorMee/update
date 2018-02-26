using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillModule : BaseModule {

	public Image bgImage;

    public Transform starButton;
    public Transform bottleButton;

    public ToggleGroup smallToggleGroup;
    private GameObject smallPrefab;
    public BaseList smallList;

    private GameObject groupListPrefab;
    public BaseList groupList;

    public Button closeBtn;

    private int selectGroupIndex = 0;

	public Transform buggerPos;
	private Bugger bugger;

    override protected void Awake()
    {
        base.Awake();

        smallPrefab = ResModel.Instance.GetPrefab("prefab/skillmodule/" + "SkillSmallItem");
        smallList.itemPrefab = smallPrefab;

        groupListPrefab = ResModel.Instance.GetPrefab("prefab/skillmodule/" + "SkillGroupList");
        groupList.itemPrefab = groupListPrefab;

		ScreenSlider.OpenSlid();

        PosUtil.SetUIPos(starButton, 12);
		PosUtil.SetUIPos(bottleButton, 14);
        PromptModel.Instance.SlideEvent = OnSlideEvent;

		GameObject buggerPrefab = ResModel.Instance.GetPrefab("prefab/base/" + "Bugger");
		GameObject buggerItem = GameObject.Instantiate(buggerPrefab);
		buggerItem.transform.SetParent(buggerPos, false);
		buggerItem.transform.localPosition = new Vector3(0,1000,0);
		buggerItem.transform.localScale = new Vector3(0.9f,0.9f,1);
		bugger = buggerItem.GetComponent<Bugger>();
		bugger.iconId = 10101;
		bugger.AddPos(new BuggerPosInfo(new Vector2(0, 780), new Vector2(0, 680)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(-279, 780), new Vector2(-279, 680)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(279, 780), new Vector2(279, 680)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(-558, 468), new Vector2(-458, 468)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(-558, 312), new Vector2(-458, 312)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(-558, 156), new Vector2(-458, 156)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(-558, 0), new Vector2(-458, 0)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(-558, -156), new Vector2(-458, -156)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(558, 468), new Vector2(458, 468)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(558, 312), new Vector2(458, 312)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(558, 156), new Vector2(458, 156)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(558, 0), new Vector2(458, 0)));
		bugger.AddPos(new BuggerPosInfo(new Vector2(558, -156), new Vector2(458, -156)));

		bugger.Show();

		InitList();

        UpdateUI();

        AudioModel.Instance.PlayMusic("rain");
    }

    void Start()
    {

		PosUtil.SetCellPos(closeBtn.transform, 3, 5);
    }

    private void UpdateUI()
    {
        starButton.GetComponentInChildren<Text>().text = MapModel.Instance.starInfo.GetSkillCanUseStar() + "";
        //gemButton.GetComponentInChildren<Text>().text = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem).count + "";

		UpdateBottles ();

		UpdateSmalls ();

		ShowSmallItemTip();
    }

	private void UpdateBottles()
	{
		int bottleId = infoBefore.GetGroupId ();
		Image bottleMask = bottleButton.FindChild ("mask").GetComponent<Image> ();
		Image bottleIcon = bottleButton.FindChild ("icon").GetComponent<Image> ();
		bottleMask.color = ColorUtil.GetColor(ColorUtil.CELLS[infoBefore.index]);
		bottleIcon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + bottleId);
		bottleButton.GetComponentInChildren<NumberText>().text = PlayerModel.Instance.GetWealth(bottleId).count + "";
	}

	private void UpdateSmalls()
	{
		
	}

    void OnEnable()
    {
        EventTriggerListener.Get(closeBtn.gameObject).onClick = OnCloseClick;
        SkillTempletModel.Instance.updateOperateEvent += OnUpdateOperateEvent;
    }

    void OnDisable()
    {
        SkillTempletModel.Instance.updateOperateEvent -= OnUpdateOperateEvent;
        PromptModel.Instance.SlideEvent = null;
    }

    private void OnUpdateOperateEvent(SkillTempletInfo info)
    {
        UpdateUI();
    }

    private void OnCloseClick(GameObject go)
    {
        ModuleModel.Instance.AddUIModule((int)ModuleEnum.MAP);
    }

    private void InitList()
    {
        smallList.ClearList();
        groupList.ClearList();

        for (int i = 0; i < SkillTempletModel.Instance.skillGroups.Count; i++)
		{
            SkillGroupInfo skillTempletGroupInfo = SkillTempletModel.Instance.skillGroups[i];
			skillTempletGroupInfo.index = i;
            CreateSmallItem(skillTempletGroupInfo,i);
            CreateGroupItem(skillTempletGroupInfo);
		}

        selectGroupIndex = SkillTempletModel.Instance.selectGroupIndex;

        SwitchGroup();
    }

    private void SwitchGroup()
    {
        int i;
        int count = smallList.items.Count;
        for (i = 0; i < count; i++)
        {
            GameObject item = smallList.items[i];
            SkillSmallItem itemCtr = item.GetComponent<SkillSmallItem>();
            if (itemCtr.skillTempletGroupInfo.index == selectGroupIndex)
            {
				bugger.iconId = itemCtr.skillTempletGroupInfo.GetGroupId();
                itemCtr.toggle.isOn = true;
            }
        }
    }

    private void CreateGroupItem(SkillGroupInfo skillTempletGroupInfo)
    {
        GameObject item = groupList.NewItem();
        item.name = "skill_" + skillTempletGroupInfo.GetGroupId();

		SkillGroupList itemCtr = item.GetComponent<SkillGroupList>();
        itemCtr.Init(skillTempletGroupInfo);
    }

    private void CreateSmallItem(SkillGroupInfo skillTempletGroupInfo,int index)
    {
        GameObject item = smallList.NewItem();

		PosUtil.SetCellPos(item.transform, index - 2, -5);

        item.name = "skill_" + skillTempletGroupInfo.GetGroupId();
		
		SkillSmallItem itemCtr = item.GetComponent<SkillSmallItem>();

        itemCtr.toggle.group = smallToggleGroup;
        itemCtr.skillTempletGroupInfo = skillTempletGroupInfo;
		itemCtr.ShowName ("");
        itemCtr.icon = skillTempletGroupInfo.GetGroupId();
		EventTriggerListener.Get(item).onClick = OnSmallClick;
        itemCtr.selectTempletEvent += OnSelectTemplet;
    }

	private void ShowSmallItemTip()
	{
		int animCount = 0;
		for (int i = 0; i < SkillTempletModel.Instance.skillGroups.Count; i++)
		{
			SkillGroupInfo skillTempletGroupInfo = SkillTempletModel.Instance.skillGroups[i];
			skillTempletGroupInfo.index = i;
			int bottleId = skillTempletGroupInfo.GetGroupId();
			SkillTempletInfo skillTempletInfo = skillTempletGroupInfo.skillTemplets[1];

			GameObject item = smallList.GetItemByName("skill_" + bottleId);
			RectTransform iconTrans = (RectTransform)item.transform.FindChild("icon");
			Animation skillTipAnim = iconTrans.GetComponent<Animation>();

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
							if(!skillTipAnim.isPlaying)
							{
								LeanTween.delayedCall(0.2f*animCount, delegate ()
									{
										skillTipAnim.Play();
									});
							}

							animCount ++;
							continue;
						}
					}
				}
			}

			LeanTween.delayedCall(0.2f, delegate ()
				{
					skillTipAnim.Stop();
					iconTrans.anchoredPosition = new Vector2(0,-70);
					iconTrans.localScale = new Vector3(1,1,1);
				});
			
		}
	}

    private void OnSlideEvent(bool isLeft)
    {
        if (isLeft)
        {
            if (selectGroupIndex < (SkillTempletModel.Instance.skillGroups.Count - 1))
            {
				AudioModel.Instance.PlayeSound("click");
                selectGroupIndex++;
                SwitchGroup();
            }
        }
        else
        {
            if (selectGroupIndex > 0)
            {
				AudioModel.Instance.PlayeSound("click");
                selectGroupIndex--;
                SwitchGroup();
            }
        }
    }

	private void OnSmallClick(GameObject gameObject)
	{
		SkillSmallItem itemCtr = gameObject.GetComponent<SkillSmallItem>();
		bugger.iconId = itemCtr.skillTempletGroupInfo.GetGroupId();
	}

	private SkillGroupInfo infoBefore;
    private void OnSelectTemplet(SkillGroupInfo info)
    {
		int dir = 1;
		if(infoBefore != null)
		{
            if (infoBefore.GetGroupId() > info.GetGroupId())
			{
				dir = -1;
			}
		}
		infoBefore = info;
        for (int i = 0; i < groupList.items.Count; i++)
        {
            GameObject item = groupList.items[i];
			SkillGroupList itemCtr = item.GetComponent<SkillGroupList>();

            if (itemCtr.skillTempletGroupInfo == info)
            {
				itemCtr.Show(dir);
            }else
            {
				itemCtr.Hide(dir);
            }
        }

		bgImage.color = ColorUtil.GetColor(ColorUtil.CELLS[info.index]);

		UpdateBottles ();
    }
}
