using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightModel : Singleton<FightModel>
{
	public FightInfo fightInfo = new FightInfo();

	public void InitFightInfo()
	{
        fightInfo.stepLeft = BattleModel.Instance.crtConfig.step;
		fightInfo.score = 0;
        fightInfo.judge_score = BattleModel.Instance.crtConfig.GetJudgeScores();
        fightInfo.task = BattleModel.Instance.crtConfig.GetTaskList();
		fightInfo.isWin = false;
	}
}

