using System;
using System.Collections.Generic;

using UnityEngine;

public class CellMoveInfo
{
    public CellInfo cellInfo;

    public List<Vector2> paths = new List<Vector2>();

    private bool sliderMark = false;

    public void AddPath(int toX, int toY, bool isSlider)
    {
        if (paths.Count > 0)
        {
            Vector2 endPos = paths[paths.Count - 1];
            if (endPos.x != toX)
            {
                sliderMark = true;
                paths.Add(new Vector2(toX, toY));
            }
            else
            {
                if (sliderMark)
                {
                    sliderMark = false;
                    paths.Add(new Vector2(toX, toY));
                }
                else
                {
                    endPos.y = toY;
                    paths[paths.Count - 1] = endPos;
                }
            }
        }
        else
        {
            sliderMark = isSlider;
            paths.Add(new Vector2(toX, toY));
        }
    }

	public void AddFixPath(int toX, int toY)
	{
		paths.Add(new Vector2(toX, toY));
	}

	public void CutTail()
	{
		for (int i = paths.Count; i > 1; i --)
		{
			Vector2 path = paths[i - 1];
			Vector2 pathPre = paths[i - 2];
			if (path.x == pathPre.x && path.y == pathPre.y)
			{
				paths.RemoveAt(i - 1);
			}
			else
			{
				return;
			}
		}
	}
}
