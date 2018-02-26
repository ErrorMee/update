using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskListPart : MonoBehaviour {

    private GameObject itemPrefab;
    public BaseList list;

    private List<int> poss = new List<int>() { 4, 3, 5, 2, 6 };
    
	void Awake()
    {
        itemPrefab = ResModel.Instance.GetPrefab("prefab/fightmodule/" + "TaskItem");
    }

    void Start()
    {
        list.itemPrefab = itemPrefab;
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
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    public bool CollectTask(bool isTemp)
    {
        bool taskAllComplete = true;

        for (int i = 0; i < FightModel.Instance.fightInfo.task.Count; i++)
        {
            TIVInfo task = FightModel.Instance.fightInfo.task[i];

            int collectCount = CollectModel.Instance.profileCollect.GetCount((int)task.id);
            if (isTemp)
            {
                collectCount = collectCount + CollectModel.Instance.tempCollect.GetCount((int)task.id);
            }
            
            int leftCount = (int)task.value - collectCount;

            if (leftCount <= 0)
            {
                leftCount = 0;
            }
            else
            {
                taskAllComplete = false;
            }
            ShowTaskProgress((int)task.id, leftCount, isTemp);
        }

        return taskAllComplete;
    }

	private void ShowTaskProgress(int id, int count,bool isTemp)
    {
        Transform itemTrs = transform.FindChild("task_" + id);
		TaskItem itemCtr = itemTrs.GetComponent<TaskItem> ();
        itemCtr.count = count;
		//if(isTemp)
		//{
			//itemCtr.progressText.color = ColorUtil.GetColor(ColorUtil.COLOR_GREEN);
		//}
		//else
		//{
			//itemCtr.progressText.color = ColorUtil.GetColor(ColorUtil.COLOR_FONT_WIGHT);
		//}
    }

    private void CreateTaskItem(int index, TIVInfo tiv)
    {
        GameObject item = list.NewItem();
        TaskItem itemCtr = item.GetComponent<TaskItem>();

        itemCtr.icon = (int)tiv.id;
        item.name = "task_" + tiv.id;
        itemCtr.count = (int)tiv.value;
		itemCtr.pos = GetTaskPos(poss[index]);
    }

	private Vector2 GetTaskPos(int px)
	{
		float ax, ay;
		ax = (px) * PosUtil.CELL_W * PosUtil.CELL_W_MOD;
		ay = 80;
		return new Vector2(ax, -ay);
	}
}
