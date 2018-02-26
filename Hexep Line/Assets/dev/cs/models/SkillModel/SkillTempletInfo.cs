
using System;
using System.Collections;
using System.Collections.Generic;

public class SkillTempletInfo
{
    public config_skill_item config;

    public int lv = SkillTempletModel.OPEN_LEVEL;

    public bool fobid = false;

    public bool IsOpen()
    {
        return lv > 0;
    }

    public int GetCostStar(int lvParam = -1)
    {
        int tempLv = lvParam;

        if (tempLv == -1)
        {
            tempLv = lv;
        }

        if (tempLv > 0)
        {
            List<float> abc = config.GetStarABC();
            int allStars = 0;
            for (int i = 1; i <= (tempLv - SkillTempletModel.OPEN_LEVEL);i++ )
            {
                allStars += (int)(abc[0] * Math.Pow(i, 2) + abc[1] * (i) + abc[2]);
            }
            return allStars;
        }else
        {
            return 0;
        }
    }

    public bool IsUnlock()
    {
        return MapModel.Instance.IsPassed(config.unlockId);
    }

	public int LevelUpCostBottle()
	{
        List<float> abc = config.GetBottleABC();
        int x = lv - SkillTempletModel.OPEN_LEVEL;
        return (int)(abc[0] * Math.Pow(x, 2) + abc[1] * x + abc[2]);
	}

	public int LevelUpCostStar()
	{
        List<float> abc = config.GetStarABC();
        int x = lv - SkillTempletModel.OPEN_LEVEL;
        return (int)(abc[0] * Math.Pow(x, 2) + abc[1] * x + abc[2]);
	}

	public int GetShowLv()
	{
		return lv - SkillTempletModel.OPEN_LEVEL + 1;
	}

}