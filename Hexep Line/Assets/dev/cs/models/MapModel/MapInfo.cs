using System;
using System.Collections.Generic;

public class MapInfo
{
    public int configId;

    public int star = 0;

    public int score = 0;

	public int oldStar = -1;

	public bool buyPassed = false;

    public int UseOldStar()
    {
        int temp = oldStar;
        oldStar = -1;
        return temp;
    }
}