using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PrepareTask : MonoBehaviour {

    public Text title;
	private GameObject itemPrefab;
	public BaseList list;

	void Awake()
	{
		PrepareModule.LayItemCount ++ ;
		itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/preparemodule.ab", "PrepareTaskItem");

        title.text = LanguageUtil.GetTxt(11203);
	}
	
	void Start()
	{
		list.itemPrefab = itemPrefab;
		InitTasks ();
	}

	public void InitTasks()
	{
		list.ClearList();
		
		List<TIVInfo> taskList = BattleModel.Instance.crtConfig.GetTaskList();
		
		int taskLen = taskList.Count;
		
		for (int i = 0; i < taskLen; i++ )
		{
			CreateTaskItem(i, taskList[i]);
		}
	}

	private void CreateTaskItem(int index, TIVInfo tiv)
	{
		GameObject item = list.NewItem();
		TaskItem itemCtr = item.GetComponent<TaskItem>();
		
		itemCtr.icon = tiv.id;
		item.name = "task_" + tiv.id;
		itemCtr.count = (int)tiv.value;
	}
}