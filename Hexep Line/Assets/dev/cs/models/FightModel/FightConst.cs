using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightConst
{
    public const int ROTATE_BASE = -60;

    public static List<List<Vector2>> RING_HUMP = new List<List<Vector2>>()
    { 
		new List<Vector2>(){ new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(-1, 1) },
        new List<Vector2>(){ new Vector2(0, 2), new Vector2(1, 2), new Vector2(2, 1), new Vector2(2, 0), new Vector2(2, -1), new Vector2(1, -1), new Vector2(0, -2), new Vector2(-1, -1), new Vector2(-2, -1), new Vector2(-2, 0), new Vector2(-2, 1), new Vector2(-1, 2) }
    };

    public static List<List<Vector2>> RING_VALLEY = new List<List<Vector2>>()
    { 
		new List<Vector2>(){ new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, -1), new Vector2(-1, 0) },
        new List<Vector2>(){ new Vector2(0, 2), new Vector2(1, 1), new Vector2(2, 1), new Vector2(2, 0), new Vector2(2, -1), new Vector2(1, -2), new Vector2(0, -2), new Vector2(-1, -2), new Vector2(-2, -1), new Vector2(-2, 0), new Vector2(-2, 1), new Vector2(-1, 1) }
    };

    public static Vector2 GetHoleByLevel(int level,bool isHump = true)
    {
        int levelA = 0;
        int levelB = level;

        if (level >= RING_HUMP[0].Count)
        {
            levelA = 1;
            levelB = level - RING_HUMP[0].Count;
        }
        if (isHump)
        {
            return RING_HUMP[levelA][levelB];
        }
        else
        {
			return RING_VALLEY[levelA][levelB];
        }
    }
}