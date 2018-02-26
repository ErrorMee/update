using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosUtil
{
	public static float Y_SATRT_FG = 3.0f;
	public static float X_SATRT_FG = -3.0f;
	public static float Y_END_FG = -4.0f;
	public static float X_END_FG = 3.0f;


    public static float Y_HALF_COUNT = 6.0f;
    public static float X_HALF_COUNT = 4.0f;
    public static float Y_FULL_COUNT = 13.0f;
    public static float X_FULL_COUNT = 9.0f;

    public const float CELL_W_MOD = 0.8725f;
    public const float CELL_W = 160.0f;
    public const float CELL_VIEW_W = 188.0f;
    public const float CELL_H = 156.0f;

    public const int GAME_WIDTH = 1080;
    public const int GAME_HEIGHT = 1920;

	public const int CELL_WIDTH = 169;
	public const int CELL_HEIGHT = 195;

	public static Vector2 GetCellPos(int px, int py,bool yoff = true)
    {
        float ax, ay;

		if(Mathf.Abs(py % 2) == 0)
		{
			ax = px * CELL_WIDTH;
		}else
		{
			ax = (px + 0.5f) * CELL_WIDTH;
		}

		ay = py * PosUtil.CELL_HEIGHT * 0.75f;

        return new Vector2(ax, ay);
    }

	public static void SetCellPos(Transform transform,int px, int py,float scaler = 1.0f,bool yoff = true)
    {
        RectTransform rectTransform = (RectTransform)transform;

        if (rectTransform != null)
        {
			rectTransform.anchoredPosition = GetCellPos(px, py,yoff) * scaler;
        }
    }

    public static Vector2 GetFightCellPos(int px, int py)
    {
        return GetCellPos(px + BattleModel.Instance.crtBattle.start_x, - py + BattleModel.Instance.crtBattle.start_y);
    }

    public static void SetFightCellPos(Transform transform, int px, int py, float scaler = 1.0f)
    {
        SetCellPos(transform, px + BattleModel.Instance.crtBattle.start_x, - py + BattleModel.Instance.crtBattle.start_y, scaler);
    }

	public static Vector2 GetWallPos(int px, int py, int pn)
	{
		float ax, ay;
		ax = (px) * PosUtil.CELL_W * PosUtil.CELL_W_MOD;
		if (Mathf.Abs(px % 2) == 1){
			ay = (py) * PosUtil.CELL_H + PosUtil.CELL_H / 2;
		}else{
			ay = (py) * PosUtil.CELL_H;
		}
		
		float offsetX = 0, offsetY = PosUtil.CELL_H / 2;
		
		switch(pn)
		{
		case 0:
			offsetX = -(PosUtil.CELL_W/2 - 10);
			offsetY = PosUtil.CELL_H / 4;
			break;
		case 1:
			break;
		case 2:
			offsetX = (PosUtil.CELL_W/2 - 10);
			offsetY = PosUtil.CELL_H / 4;
			break;
		}
		return new Vector2(ax + offsetX,(ay + offsetY));
	}

	public static void SetWallPos(Transform transform,int px, int py, int pn,float scaler = 1.0f)
	{
		RectTransform rectTransform = (RectTransform)transform;
		
		if (rectTransform != null)
		{
			rectTransform.anchoredPosition = GetWallPos(px, py, pn) * scaler;
		}
	}

    public static Vector2 GetFightWallPos(int px, int py, int pn)
    {
        return GetWallPos(px + BattleModel.Instance.crtBattle.start_x, -py + BattleModel.Instance.crtBattle.start_y, pn);
    }

    public static void SetFightWallPos(Transform transform, int px, int py, int pn, float scaler = 1.0f)
    {
        RectTransform rectTransform = (RectTransform)transform;

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = GetFightWallPos(px, py, pn) * scaler;
        }
    }

    public static void SetUIPos(Transform transform, int index, float scaler = 1.0f)
    {
        RectTransform rectTransform = (RectTransform)transform;

        if (rectTransform != null)
        {
            Vector2 uIPos = GetUIPos(index);
            rectTransform.anchoredPosition = GetCellPos((int)uIPos.x, (int)uIPos.y) * scaler;
        }
    }

    public static Vector2 GetUIPos(int index)
    {
        float px, py;
        px = index % X_FULL_COUNT;
        if (px == 0)
        {
            px = X_FULL_COUNT;
        }
        px = px - 1 - X_HALF_COUNT;

        py = (float)Math.Ceiling(index / X_FULL_COUNT) - 1 - Y_HALF_COUNT;
        
        return new Vector2(px, -py);
    }

    public static int GetRotateByDir(CellDirType dirType)
    {
        switch(dirType)
        {
            case CellDirType.right_up:
                return 1 * FightConst.ROTATE_BASE;
            case CellDirType.right_down:
                return 2 * FightConst.ROTATE_BASE;
            case CellDirType.down:
                return 3 * FightConst.ROTATE_BASE;
            case CellDirType.left_down:
                return 4 * FightConst.ROTATE_BASE;
            case CellDirType.left_up:
                return 5 * FightConst.ROTATE_BASE;
            default:
                return 0;
        }
    }

	public static int GetDirWithRotate(CellDirType dirType, int rotate)
	{
		int dir = (int)dirType;
		int offRotate = rotate%6;
		dir = dir + offRotate;
		if(dir >= 6)
		{
			dir = dir - 6;
		}
		return dir;
	}

    public static List<CellDirType> PriorityDirList(int priorityDir,bool random = false)
    {
        List<CellDirType> dirList = new List<CellDirType>();

        List<int> offList = new List<int>() { 0, -1, 1, -2, 2, -3 };

        if (random)
        {
            int randomV = UnityEngine.Random.Range(1, 4);
            if (randomV == 2)
            {
                offList = new List<int>() { -1, 0, 1, -2, 2, -3 };
            }

            if (randomV == 3)
            {
                offList = new List<int>() { 1, 0, -1, -2, 2, -3 };
            }
        }


        int i ;
        for (i = 0; i < offList.Count;i++ )
        {
            int newInt = priorityDir + offList[i];
            if (newInt < 0)
            {
                newInt = newInt + offList.Count;
            }

            if (newInt >= offList.Count)
            {
                newInt = newInt - offList.Count;
            }

            dirList.Add((CellDirType)newInt);
        }

        return dirList;
    }

    public static float VectorAngle(Vector2 from, Vector2 to)
    {
        double angle = Math.Atan2((double)(to.y - from.y), (double)(to.x - from.x)) * 180 / Math.PI;
        return (float)angle;
    }
}