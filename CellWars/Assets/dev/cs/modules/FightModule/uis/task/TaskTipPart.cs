using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TaskTipPart : MonoBehaviour
{
    public Text title;

	public Transform closeTrans;

    private GameObject itemPrefab;
    public BaseList list;

    public Animation contentAnim;

	private CellLayer cellLayer;

    public RectTransform taskTrans;
	public RectTransform starTrans;
	public RectTransform stepTrans;
	public RectTransform propTrans;

    void Awake()
    {
        //gameObject.SetActive(false);
        itemPrefab = GameMgr.resourceMgr.GetGameObject("prefab/fightmodule.ab", "TaskTipItem");
        LeanTween.delayedCall(0.25f, AddCloseEvent);
        title.text = LanguageUtil.GetTxt(11401);
        taskTrans.anchoredPosition = new Vector2(0, 1100);
		starTrans.anchoredPosition = new Vector2(-700, 815);
		stepTrans.anchoredPosition = new Vector2(640, 890);
		propTrans.anchoredPosition = new Vector2(0, -200);
    }

    private void AddCloseEvent()
    {
        EventTriggerListener.Get(closeTrans.gameObject).onClick = ClickCloseHander;
    }

    void Start()
    {
        list.itemPrefab = itemPrefab;

        gameObject.SetActive(true);
        contentAnim.Play("UI_ENTER");
        InitTasks();
		LeanTween.delayedCall(3.0f, UIOut);
    }

    private void UIOut()
    {
		if(gameObject == null)
		{
			return;
		}
        contentAnim.Play("UI_OUT");

        LeanTween.delayedCall(0.20f, UIDestroy);
    }

    private void UIDestroy()
    {
		if(gameObject.activeSelf == true)
		{
			//FightModule.crtFightStadus = FightStadus.idle;
			gameObject.SetActive(false);
			GuildModel.Instance.CheckGuild();
			TaskTipEnd();
        }
		Destroy(gameObject);
    }

    public void InitTasks()
    {
        list.ClearList();

        List<TIVInfo> taskList = BattleModel.Instance.crtConfig.GetTaskList();

        int taskLen = taskList.Count;

        for (int i = 0; i < taskLen; i++)
        {
            CreateTaskItem(i, taskList[i]);
        }
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    private void CreateTaskItem(int index, TIVInfo tiv)
    {
        GameObject item = list.NewItem();
        TaskTipItem itemCtr = item.GetComponent<TaskTipItem>();

        itemCtr.icon = (int)tiv.id;
        item.name = "task_" + tiv.id;
        itemCtr.count = (int)tiv.value;
    }

	private void ClickCloseHander(GameObject go)
	{
		gameObject.SetActive(false);
		GuildModel.Instance.CheckGuild();
		TaskTipEnd();
    }

    private void TaskTipEnd()
    {
        LTDescr tween = LeanTween.move(taskTrans, new Vector3(0, 900, 0), 0.3f);
        tween.delay = 0.2f;
        tween.setEase(LeanTweenType.easeInOutBack);

		tween.onComplete = TaskShowEnd;
    }

	private void TaskShowEnd()
	{
		LTDescr tween = LeanTween.move(starTrans, new Vector3(-400, 815, 0), 0.3f);
		tween.delay = 0.3f;
		tween.setEase(LeanTweenType.easeInOutBack);

		LTDescr tween2 = LeanTween.move(stepTrans, new Vector3(440, 890, 0), 0.3f);
		tween2.delay = 0.3f;
		tween2.setEase(LeanTweenType.easeInOutBack);

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
            LTDescr tween3 = LeanTween.move(propTrans, new Vector3(0, 0, 0), 0.25f);
		    tween3.delay = 0.25f;
			tween3.setEase(LeanTweenType.easeInOutBack);
        }
	}
}
