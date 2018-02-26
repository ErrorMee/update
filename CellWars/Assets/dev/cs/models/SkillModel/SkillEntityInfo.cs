using System;
using System.Collections.Generic;

public class SkillEntityInfo : IComparable
{
    public static int LAST_RUN_ID = 0;
    public int runId;
    public SkillSeedInfo seed;

    public CellInfo cell;

    //技能作用中心
    public int skill_x;
    public int skill_y;

    public SkillFightingType fightingType;

    public List<CellInfo> controlCells = new List<CellInfo>();

    public static int SORT_FLAG = 1;

    public SkillEntityInfo()
	{
		runId = ++LAST_RUN_ID;
	}

    public int CompareTo(object obj)
    {
        SkillEntityInfo target = obj as SkillEntityInfo;
        if (target == null)
        {
            throw new NotImplementedException();
        }

        int thisValue;
        if (this.IsHump())
        {
            thisValue = this.skill_x * SORT_FLAG - this.skill_y * 200 + 100;
        }
        else
        {
            thisValue = this.skill_x * SORT_FLAG - this.skill_y * 200;
        }

        int targetValue;
        if (target.IsHump())
        {
            targetValue = target.skill_x * SORT_FLAG - target.skill_y * 200 + 100;
        }
        else
        {
            targetValue = target.skill_x * SORT_FLAG - target.skill_y * 200;
        }

        return thisValue.CompareTo(targetValue);
    }

    //复制
    public SkillEntityInfo Copy()
    {
        SkillEntityInfo skillEntityInfo = new SkillEntityInfo();
        skillEntityInfo.runId = runId;

        skillEntityInfo.seed = seed;
        
        skillEntityInfo.skill_x = skill_x;
        skillEntityInfo.skill_y = skill_y;
        skillEntityInfo.fightingType = fightingType;

        skillEntityInfo.cell = cell.Copy();
        skillEntityInfo.controlCells = new List<CellInfo>();
        for (int i = 0; i < controlCells.Count;i++ )
        {
            skillEntityInfo.controlCells.Add(controlCells[i].Copy());
        }

        return skillEntityInfo;
    }

    public bool IsHump(int pos = -1)
    {
        if (pos == -1)
        {
            pos = skill_x;
        }
        return (pos - CellInfo.start_x) % 2 != 0;
    }
}