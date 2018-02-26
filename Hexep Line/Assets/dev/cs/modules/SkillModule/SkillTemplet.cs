using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillTemplet : MonoBehaviour
{
    private GameObject holeItemPrefab;
    public BaseList listHole;

	public Image descBg;
    public Text descText;
	public Image lockImage;
    public Button upLvBtn;
	public Transform upLevel;

	public Text starCost;
	public Text bottleCost;
	public Image bottleIcon;
	public Image bottleMask;

    public Button clearBtn;
	public SkillGroupInfo skillGroupInfo;
    public SkillTempletInfo skillTempletInfo;

    void Awake()
    {
        holeItemPrefab = ResModel.Instance.GetPrefab("prefab/skillmodule/" + "SkillHoleItem");
        listHole.itemPrefab = holeItemPrefab;
        InitView();
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    private void InitView()
    {
        descText.text = "";
		upLevel.gameObject.SetActive(true);
        upLvBtn.transform.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11305);
        clearBtn.transform.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11306);
    }

    void OnEnable()
    {
        EventTriggerListener.Get(upLvBtn.gameObject).onClick = OnUpLvClick;
        EventTriggerListener.Get(clearBtn.gameObject).onClick = OnClearClick;
        SkillTempletModel.Instance.updateOperateEvent += OnUpdateOperateEvent;
    }

    void OnDisable()
    {
        SkillTempletModel.Instance.updateOperateEvent -= OnUpdateOperateEvent;
    }

    private void OnUpLvClick(GameObject go)
    {
		if(skillTempletInfo == null)
		{
			return;
		}
		
        if (!skillTempletInfo.IsUnlock())
        {

            config_map_item map_item = (config_map_item)ResModel.Instance.config_map.GetItem(skillTempletInfo.config.unlockId);

            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11303) + ":" + map_item.name);
            return;
        }
		int leftStar = MapModel.Instance.starInfo.GetSkillCanUseStar();
		if (leftStar >= skillTempletInfo.LevelUpCostStar())
        {
			WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(skillGroupInfo.GetGroupId());
			if(bottleInfo.count < skillTempletInfo.LevelUpCostBottle())
			{
                PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, bottleInfo.type);
			}else
			{
				bottleInfo.count -= skillTempletInfo.LevelUpCostBottle();
				PlayerModel.Instance.SaveWealths();
				SkillTempletModel.Instance.UpLevel(skillTempletInfo, 1);
				//PromptModel.Instance.Pop(LanguageUtil.GetTxt(11305));
			}
        }
        else
        {
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, (int)WealthTypeEnum.Star);
        }
    }

    private void OnClearClick(GameObject go)
    {
		PromptModel.Instance.Pop(LanguageUtil.GetTxt(11306));
        //SkillTempletModel.Instance.ClearLv(skillTempletInfo);
    }

	public void Init(SkillGroupInfo groupinfo,SkillTempletInfo templetinfo)
    {
		skillGroupInfo = groupinfo;
		skillTempletInfo = templetinfo;
		
		UpdateBtn();
		InitHoleList(false);
    }

    private void OnUpdateOperateEvent(SkillTempletInfo info)
    {
        if (info.config.cellId == skillTempletInfo.config.cellId)
        {
			InitHoleList(true);
        }
		UpdateBtn();
    }

	public void UpdateBtn()
    {
		descText.text = "";
		bottleMask.color = ColorUtil.GetColor(ColorUtil.CELLS[skillGroupInfo.index]);
		descBg.color = bottleMask.color;
		lockImage.color = bottleMask.color;

        if (skillTempletInfo.IsOpen() == false)
        {
            //openBtn.gameObject.SetActive(true);
			upLevel.gameObject.SetActive(false);
            clearBtn.gameObject.SetActive(false);
        }
        else
        {
            //openBtn.gameObject.SetActive(false);
            if (skillTempletInfo.lv >= SkillTempletModel.MAX_LEVEL)
            {
				upLevel.gameObject.SetActive(false);
				clearBtn.gameObject.SetActive(true);
				descText.text = "" + skillTempletInfo.GetShowLv();

            }
            else
            {
				descBg.color = ColorUtil.GetColor(ColorUtil.CELLS[skillGroupInfo.index]);
				descText.text = "" + skillTempletInfo.GetShowLv();

				clearBtn.gameObject.SetActive(false);
				upLevel.gameObject.SetActive(true);



				starCost.text = "" + skillTempletInfo.LevelUpCostStar();
				int leftStar = MapModel.Instance.starInfo.GetSkillCanUseStar();
				if(leftStar < skillTempletInfo.LevelUpCostStar())
				{
					starCost.color = Color.red;
				}else
				{
					starCost.color = Color.green;
				}

				int bottleId = skillGroupInfo.GetGroupId();
				int levelUpNeedBottle = skillTempletInfo.LevelUpCostBottle();

				bottleIcon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + bottleId);
				WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(bottleId);
				bottleCost.text = levelUpNeedBottle + "";
				if(levelUpNeedBottle > bottleInfo.count )
				{
					bottleCost.color = Color.red;
				}else
				{
					bottleCost.color = Color.green;
				}
            }
        }
		lockImage.gameObject.SetActive(!skillTempletInfo.IsUnlock());
		descBg.gameObject.SetActive(skillTempletInfo.IsUnlock());
		descText.gameObject.SetActive(skillTempletInfo.IsUnlock());

    }

	private void InitHoleList(bool playEffect)
    {
        listHole.ClearList();

        int i;
        SkillHoleItem findNextOpen = null;
        for (i = 1; i <= SkillTempletModel.MAX_LEVEL; i++)
        {
            GameObject item = listHole.NewItem();
            item.name = skillTempletInfo.config.cellId + "_" + i;

            SkillHoleItem itemCtr = item.GetComponent<SkillHoleItem>();
			itemCtr.skillGroupInfo = skillGroupInfo;
            itemCtr.islock = false;
            itemCtr.ShowName("");
            itemCtr.waitAnim.enabled = false;
            if (i > skillTempletInfo.lv)
            {
                if (findNextOpen == null)
                {
                    findNextOpen = itemCtr;
                    itemCtr.waitAnim.enabled = true;
                }
                else
                {
                    itemCtr.islock = true;
                }
            }
            else
            {
                if (i > 1)
                {
                    itemCtr.bombImage.color = ColorUtil.GetColor(ColorUtil.MAP_PASS, 0.9f);
                }
            }

			Vector2 pos = SkillTempletModel.Instance.GetHolePos (i, false, skillTempletInfo.config.dir);
            PosUtil.SetCellPos(item.transform, (int)pos.x, (int)pos.y + 1, 1.1f);

            if (i == 1)
			{
				itemCtr.icon = skillTempletInfo.config.cellId;
                itemCtr.bombImage.gameObject.SetActive(false);
            }
            else if (i == skillTempletInfo.lv && playEffect)
            {
                itemCtr.iconImage.gameObject.SetActive(false);
                GameObject effectPrefab = ResModel.Instance.GetPrefab("effect/" + "effect_fireworks");
                GameObject itemEffect = GameObject.Instantiate(effectPrefab);
                itemEffect.transform.SetParent(itemCtr.transform, false);
                itemEffect.transform.SetParent(transform, true);
            }
        }
    }
}