using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PropsPart : MonoBehaviour {

    private GameObject itemPrefab;
    public BaseList list;
    public ToggleGroup listToggleGroup;

	public PropDescPart descPart;

	public PropDoPart doPart;

	public Image mask;

    void Awake()
    {
		doPart.Show (0);
		descPart.Show (false);

        itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "PropItem");
    }

    void Start()
    {
        list.itemPrefab = itemPrefab;

        PropModel.Instance.UpdatePropStadus = UpdatePropStadus;
    }

	void Update () {

		if (FightModule.crtFightStadus == FightStadus.idle) {
			mask.gameObject.SetActive(false);
		} else {
			mask.gameObject.SetActive(true);
		}
	}

    public void InitProps()
	{
        list.ClearList();

        List<PropInfo> allProps = PropModel.Instance.allProps;

        bool hasOpen = false;
        int i;
        for (i = 0; i < allProps.Count; i++)
        {
            PropInfo propInfo = allProps[i];
            if (propInfo.isForbid == false)
            {
                hasOpen = true;
            }
        }

        if (hasOpen)
        {
            for (i = 0; i < allProps.Count; i++)
            {
                PropInfo propInfo = allProps[i];
				CreatePropItem(propInfo,i ,allProps.Count);
            }
        }
    }

	private void CreatePropItem(PropInfo propInfo,int index,int allCount)
    {
        GameObject item = list.NewItem();
		item.name = "Prop_" + propInfo.config.id;
        PropItem itemCtr = item.GetComponent<PropItem>();
		itemCtr.propInfo = propInfo;
        itemCtr.toggle.group = listToggleGroup;
		itemCtr.toggle.interactable = !propInfo.isForbid;
		itemCtr.icon = propInfo.config.icon;
		itemCtr.lockImage.gameObject.SetActive (propInfo.isForbid);

		if(!propInfo.isForbid && propInfo.count > 0)
		{
			//itemCtr.numText.text = "" + propInfo.count;
		}

		item.GetComponent<RectTransform>().anchoredPosition = new Vector2((index - (allCount - 1)/2)*200,10);

		EventTriggerListener.Get(item).onClick = itemCtr.OnSelectChanged;
    }

	private void UpdatePropStadus(PropStadus propStadus)
	{
		for(int i = 0;i<list.items.Count;i++)
		{
			GameObject itemProp = list.items[i];
			PropItem itemCtr = itemProp.GetComponent<PropItem>();
			itemCtr.toggle.isOn = false;
		}

		switch(propStadus)
		{
		case PropStadus.unSelect:
			descPart.Show (false);
			break;
		case PropStadus.select:
			doPart.Show (0);
			descPart.Show (false);
			descPart.Show (true);
            GameObject propItem = list.GetItemByName("Prop_" + PropModel.Instance.crtProp.config.id);
			PropItem ctrItem = propItem.GetComponent<PropItem>();
			ctrItem.toggle.isOn = true;
			break;
		case PropStadus.doing:
			descPart.Show (false);
			switch(PropModel.Instance.crtProp.config.id)
			{
                case 10001:

                    if (PropModel.Instance.PropRefreshEvent != null)
                    {
                        PropModel.Instance.RefreshCell();
                        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11404));
                        PropModel.Instance.PropRefreshEvent();
						PropModel.Instance.crtProp.Use();
                    }

                    break;
                case 10004:

                    if (PropModel.Instance.PropAddSetpEvent != null)
                    {
                        PropModel.Instance.PropAddSetpEvent(3);
                        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11407));
						PropModel.Instance.crtProp.Use();
                    }

                    break;

			case 10002:
			case 10003:
			case 10005:
                    doPart.Show(PropModel.Instance.crtProp.config.id);
					propItem = list.GetItemByName("Prop_" + PropModel.Instance.crtProp.config.id);
					ctrItem = propItem.GetComponent<PropItem>();
					ctrItem.toggle.isOn = true;
				break;

			}

			break;
		}
	}
}
