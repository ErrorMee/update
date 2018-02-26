using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StarModule : BaseModule {


	public Transform closeTrans;

	private GameObject chapterItemPrefab;
	public BaseList chapterList;

	override protected void Awake()
	{
		base.Awake();
		InitView();
		InitEvents();
	}

	void Start()
	{
		UpdateView();
	}

	override protected void UpdateView()
	{
		InitChapterList();
	}

	private void InitView()
	{
		chapterItemPrefab = ResModel.Instance.GetPrefab("prefab/starmodule/" + "StarItem");
		chapterList.itemPrefab = chapterItemPrefab;
	}

	private void InitEvents()
	{
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClickClose;
	}

	private void OnClickClose(GameObject go)
	{
		ModuleModel.Instance.RemoveUIModule((int)ModuleEnum.STAR);
		ScreenSlider.OpenSlid();
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
	}

	private void CreateChapterItem(config_chapter_item config_chapter, int index)
	{
		GameObject item = chapterList.NewItem();
		StarItem itemCtr = item.GetComponent<StarItem>();

		itemCtr.SetConfig(config_chapter);

		itemCtr.btn.onClick.AddListener(itemCtr.OnClick);
		//EventTriggerListener.Get(itemCtr.nameText.transform.parent.gameObject).onClick = itemCtr.OnClick;
	}

}
