
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class config_map_item :config_item_base, IComparable
{
    public int step;
    public int pre_id;
    public string task;
    public int build;
    public int fill;
    public string judge;
    public string forbid_skill;
    public string forbid_prop;
	public string starLimit;

    private List<int> forbid_skill_list;

    override public void Clear()
    {
        base.Clear();
        icon = 0;
        step = 0;
        pre_id = 0;
        task = "";
        build = 0;
        fill = 0;
        judge = "";
        forbid_skill = "";
        forbid_prop = "";
		starLimit = "";
    }

    public List<int> GetJudgeScores()
    {
        string[] strs = judge.Split(new char[] { ',' });

        List<int> ints = new List<int>();

        for (int i = 0; i < strs.Length;i++ )
        {
            string str = strs[i];

            int newint = int.Parse(str);

            ints.Add(newint);
        }

        return ints;
    }

    public List<TIVInfo> GetTaskList()
    {
        List<TIVInfo> tasks = new List<TIVInfo>();

        string[] strs = task.Split(new char[] { ',' });

        for (int i = 0; i < strs.Length; i++)
        {
            string str = strs[i];

            string[] str2s = str.Split(new char[] { '|' });

            TIVInfo tiv = new TIVInfo();

            tiv.id = int.Parse(str2s[0]);
            tiv.value = float.Parse(str2s[1]);

            tasks.Add(tiv);
        }

        return tasks;
    }

	public IVInfo GetStarLimit()
	{
		IVInfo iv = new IVInfo();

		if(starLimit == null)
		{
			return iv;
		}

		string[] starLimits = starLimit.Split(new char[] { '|' });

		if(starLimits.Length > 1)
		{
			iv.id = int.Parse(starLimits[0]);
			iv.value = float.Parse(starLimits[1]);
		}
		return iv;
	}

    public bool IsForbidProp(int propConfigId)
    {
        if (forbid_prop == null)
        {
            forbid_prop = "";
        }
        string[] strs = forbid_prop.Split(new char[] { ',' });

        for (int i = 0; i < strs.Length; i++)
        {
            string str = strs[i];
            if (str == "")
            {
                continue;
            }
            int newint = int.Parse(str);
            if (newint == propConfigId)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsForbidSkill(int skillConfigId)
    {
        InitForbidSkillList();
        return forbid_skill_list.Contains(skillConfigId);
    }

    private void InitForbidSkillList()
    {
        if (forbid_skill_list != null)
        {
            return;
        }

        forbid_skill_list = new List<int>();

        if (forbid_skill == null)
        {
            forbid_skill = "";
        }
        string[] strs = forbid_skill.Split(new char[] { ',' });

        for (int i = 0; i < strs.Length; i++)
        {
            string str = strs[i];
            if (str == "")
            {
                continue;
            }
            int newint = int.Parse(str);
            forbid_skill_list.Add(newint);
        }
    }

    public bool IsForbidSkillList(List<int> skills)
    {
        InitForbidSkillList();
        for (int i = 0; i < skills.Count;i++ )
        {
            if (!forbid_skill_list.Contains(skills[i]))
            {
                return false;
            }
        }

        return true;
    }
}