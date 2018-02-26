using System;

public class StarInfo
{
    //通关可获得总数
    public int allMapFullStar;

    //当前可获得总数
    public int openMapFullStar;
    //当前获得总数
    public int crtStar;

    //技能需要总数
    public int skillFullStar;
    //技能已经使用总数
    public int skillUsedStar;


    public int GetSkillCanUseStar()
    {

        int maxCost = crtStar;

        if (maxCost > skillFullStar)
        {
            maxCost = skillFullStar;
        }

        return maxCost - skillUsedStar;
    }

}