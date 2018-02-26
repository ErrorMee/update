using System;
using System.Collections.Generic;
using UnityEngine;

public class FightInfo
{
    public int stepLeft;

    public int score;

    public List<int> judge_score;

	public List<TIVInfo> task;

    public bool isWin = false;

    public int GetStarCount()
    {
        int starCount = 0;

        int topScore = judge_score[0];
        float progress = score / topScore;

        if (progress >= 1)
        {
            starCount = 1;
            topScore = judge_score[1];
            progress = score / topScore;
        }

        if (progress >= 1)
        {
            starCount = 2;
            topScore = judge_score[2];
            progress = score / topScore;
        }

        if (progress >= 1)
        {
            starCount = 3;
        }

        return starCount;
    }
}