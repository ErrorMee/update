using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WinB :WinA
{
    public Image unlockSkill;
    public Text skillLabel;
    override protected void UpdateView()
    {
        base.UpdateView();
        skillLabel.text = LanguageUtil.GetTxt(11805);
        SkillTempletInfo skillTempletInfo = SkillTempletModel.Instance.GetUnlockSkill(BattleModel.Instance.crtConfig.id);
        unlockSkill.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + skillTempletInfo.config.cellId);

		LeanTween.delayedCall(0.15f, GuideModel.Instance.CheckGuide);

		EventTriggerListener.Get(unlockSkill.gameObject).onClick = OnNewSkill;
    }

	public void OnNewSkill(GameObject go)
	{
		
		if(go.transform.parent.parent.name == "GuideModule")
		{
			return;
		}

		BattleModel.Instance.lose_map = 0;

		SkillTempletInfo skillTempletInfo = SkillTempletModel.Instance.GetUnlockSkill(BattleModel.Instance.crtConfig.id);
		SkillTempletModel.Instance.selectGroupIndex = 0;
		for (int i = 0; i < SkillTempletModel.Instance.skillGroups.Count; i++)
		{
			SkillGroupInfo skillTempletGroupInfo = SkillTempletModel.Instance.skillGroups[i];
			skillTempletGroupInfo.index = i;
			for(int j = 0;j<skillTempletGroupInfo.skillTemplets.Count;j++)
			{
				SkillTempletInfo templetInfo = skillTempletGroupInfo.skillTemplets[j];

				if(skillTempletInfo.config == templetInfo.config)
				{
					SkillTempletModel.Instance.selectGroupIndex = i;
					ModuleModel.Instance.AddUIModule((int)ModuleEnum.SKILL);
					return;
				}
			}
		}
	}
}
