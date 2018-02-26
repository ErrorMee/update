
using System;
using System.Collections;
using System.Collections.Generic;

public class SkillGroupInfo
{
	public int index;

	public List<SkillTempletInfo> skillTemplets = new List<SkillTempletInfo>();

    public int GetGroupId()
    {
        return skillTemplets[0].config.groupId;
    }

    public List<int> GetSkillCellIds()
    {
        List<int> skillCellIds = new List<int>();

        for (int i = 0; i < skillTemplets.Count;i++ )
        {
            SkillTempletInfo skillTempletInfo = skillTemplets[i];
            skillCellIds.Add(skillTempletInfo.config.cellId);
        }
        return skillCellIds;
    }
}