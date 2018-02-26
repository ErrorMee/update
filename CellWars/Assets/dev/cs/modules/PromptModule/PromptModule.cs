using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PromptModule : BaseModule {

	public RectTransform loadingTrans;
	public RectTransform loadingImage;
    private GameObject popPrefab;
    public Transform popTrans;

    override protected void Awake()
    {
        //base.Awake();
        loadingTrans.gameObject.SetActive(false);
        popPrefab = GameMgr.resourceMgr.GetGameObject("prefab/promptmodule.ab", "PopItem");
    }

	void Start ()
    {
        PromptModel.Instance.popEvent = OnPopEvent;
        PromptModel.Instance.showLoadingEvent = OnShowLoadingEvent;
		//langFix.text = "";
	}

    private void OnPopEvent(string popStr, int wealthId)
    {
        if (popStr != null && popStr != "")
        {
            ShowPop(popStr, wealthId);
        }
    }
    
    private void ShowPop(string popStr, int wealthId)
    {
        PopItem[] oldPopItems = popTrans.GetComponentsInChildren<PopItem>();
        for (int i = 0; i < oldPopItems.Length;i++ )
        {
            PopItem oldPopItem = oldPopItems[i];
            oldPopItem.MoveUp();
        }

        GameObject popItem = GameObject.Instantiate(popPrefab);
        popItem.transform.SetParent(popTrans, false);
        popItem.transform.localPosition = new Vector3(0, 400, 0);
        PopItem popItemCtr = popItem.GetComponent<PopItem>();
        popItemCtr.SetText(popStr);
        popItemCtr.SetWealthIcon(wealthId,false);
    }

    private void OnShowLoadingEvent(bool isShow)
    {
		loadingTrans.gameObject.SetActive(isShow);
    }
}
