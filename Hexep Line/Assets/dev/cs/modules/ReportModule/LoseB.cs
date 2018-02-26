using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoseB : LoseA
{
	public Button skillButton;
    public Text skillLabel;

    override protected void UpdateView()
    {
        base.UpdateView();

        skillLabel.text = LanguageUtil.GetTxt(11806);

        EventTriggerListener.Get(skillButton.gameObject).onClick = reportModule.OnSkillClick;
        BattleModel.Instance.lose_map = BattleModel.Instance.crtConfig.id;

		for (int i = 0; i < SkillTempletModel.Instance.skillGroups.Count; i++)
		{
			SkillGroupInfo skillTempletGroupInfo = SkillTempletModel.Instance.skillGroups[i];
			skillTempletGroupInfo.index = i;
			int bottleId = skillTempletGroupInfo.GetGroupId();
			for(int j = 0;j<skillTempletGroupInfo.skillTemplets.Count;j++)
			{
				SkillTempletInfo skillTempletInfo = skillTempletGroupInfo.skillTemplets[j];

				if(skillTempletInfo.IsUnlock() && skillTempletInfo.config.type == 1)
				{
					if(skillTempletInfo.lv < SkillTempletModel.MAX_LEVEL)
					{
						int leftStar = MapModel.Instance.starInfo.GetSkillCanUseStar();
						if(leftStar >= skillTempletInfo.LevelUpCostStar())
						{
							int levelUpNeedBottle = skillTempletInfo.LevelUpCostBottle();
							WealthInfo bottleInfo = PlayerModel.Instance.GetWealth(bottleId);
							if(bottleInfo.count >= levelUpNeedBottle )
							{
								SkillTempletModel.Instance.selectGroupIndex = i;
								Image skillIcon = skillButton.transform.FindChild("icon").GetComponent<Image>();
								skillIcon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + bottleId);
								return;
							}
						}
					}
				}
			}
		}

        LeanTween.delayedCall(0.1f, GuideModel.Instance.CheckGuide);
    }

}
