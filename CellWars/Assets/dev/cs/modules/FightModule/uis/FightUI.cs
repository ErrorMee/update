using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class FightUI : MonoBehaviour {

    public Button suspend;

	public Text stepTip;
    public Text step;
    public Animation stepAnim;

    public ProgressPart progressPart;

	public TaskListPart taskListPart;

    public SkillListPart skillListPart;
	public TaskTipPart taskTipPart;
    public PropsPart propsPart;

    public StepShortPart stepShortPart;

	void Awake()
	{
		propsPart.gameObject.SetActive (true);
		stepShortPart.Show (false);
		taskTipPart.gameObject.SetActive (true);
		stepTip.text = LanguageUtil.GetTxt(11512);
	}

    void Start()
    {
        step.text = "" + FightModel.Instance.fightInfo.stepLeft;
        stepAnim.Play();
        InitEvents();
    }

    private void InitEvents()
    {
        EventTriggerListener.Get(suspend.gameObject).onClick = ClickSuspendHander;

        PropModel.Instance.PropAddSetpEvent = OnPropAddSetpEvent;
    }

	public void InitView()
	{
		taskListPart.InitTasks ();
		skillListPart.InitSeeds ();
        propsPart.InitProps();

        UpdateCollect(true);
	}

	private void ClickSuspendHander(GameObject go)
    {
        if (FightModule.crtFightStadus == FightStadus.idle)
        {
			GameMgr.moduleMgr.AddUIModule(ModuleEnum.PAUSE);
		}else
		{
			Debug.Log(FightModule.crtFightStadus);
		}
    }

	public void ShowSkill()
    {
        skillListPart.ShowSkill();
    }

    public void UpdateCollect(bool isTemp, bool isDeductStep = false, bool isSkillDeduct = false)
	{
        if (isTemp == false)
        {
			CollectModel.Instance.ProfileCollect(isDeductStep);

            if (!isSkillDeduct)
            {
                FightModel.Instance.fightInfo.stepLeft--;
                step.text = "" + FightModel.Instance.fightInfo.stepLeft;
                stepAnim.Play();
            }
            
            FightModel.Instance.fightInfo.score = CollectModel.Instance.profileScore;
        }
        else
        {
			CollectModel.Instance.TempCollect(isDeductStep);
        }

        int starNum = progressPart.CollectScore(isTemp);

        bool taskPass = taskListPart.CollectTask(isTemp);

		skillListPart.CollectSkill (isTemp, isDeductStep);

        if (isTemp == false)
        {
            if (starNum > 0 && taskPass)
            {
                FightModel.Instance.fightInfo.isWin = true;
            }
        }
	}

	public void CollectUnlocks(List<int> unlocks)
    {
		CollectModel.Instance.TempCollectList(unlocks);
        CollectModel.Instance.ProfileCollect(false);

        bool taskPass = taskListPart.CollectTask(false);

        if (progressPart.starCount > 0 && taskPass)
        {
            FightModel.Instance.fightInfo.isWin = true;
        }
    }

    private void OnPropAddSetpEvent(int value)
    {
        FightModel.Instance.fightInfo.stepLeft += value;
        step.text = "" + FightModel.Instance.fightInfo.stepLeft;
        stepAnim.Play();
    }
}
