using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtil
{
	//黑色
	public const int COLOR_BLACK = 0x000000;
	//白色
	public const int COLOR_WIGHT = 0xffffff;
    //绿色
    public const int COLOR_GREEN = 0x00ff00;
    //红色
    public const int COLOR_RED = 0xff0000;

    //相机颜色
    public const int COLOR_CAMERA = 0x454545;
    //UI格子颜色
    public const int COLOR_UI_CELL = 0x282828;

	//背景格子颜色
	public static int COLOR_BG_CELL = 0xC5F5d5;
	//前景格子颜色
    public static int COLOR_FG_CELL = 0x282828;//0x;0x4E8312

    //按钮颜色
    public const int COLOR_UI_BOTTON1 = 0xD5D5D4;

    //字体颜色和滤镜
    public const int COLOR_FONT1 = 0x59595A;
    public const int COLOR_FONT_FILTER1 = 0xFFFBF0;
	//白黄
	public const int COLOR_FONT_WIGHT = 0xF7F9E8;
	
	public const int COLOR_FISHSTAR = 0x417CF1;

    //
    public const int MAP_CLOSE = 0x151515;
	public const int MAP_PASS = 0xfffff8;
	public const int MAP_OPEN = 0x0EFF0E;

	public static List<int> CELLIDS = new List<int>() { 10101, 10102, 10103, 10104, 10105 };
    //public static List<int> OLDCELLS = new List<int>() { 0xFF6532, 0x00CC98, 0xFFCC32, 0x3265CC, 0x983298 };
    public static List<int> CELLS = new List<int>() { 0xFF7040, 0x17E8B3, 0xF7F111, 0x417CF1, 0xC62AC6 };
	public static List<int> DARKCELLS = new List<int>() { 0x8A3C23, 0x087B5E, 0x8A7023, 0x264687, 0x562356 };
	public static List<int> LIGHTCELLS = new List<int>() { 0xFFB8A0, 0x8BF4D9, 0xFBF888, 0xA0BEF8, 0xE395E3 };

    public static Color GetColor(int color,float a = 0)
	{
		int r = color >> 16;
		int g = color >> 8 & 0xff;
		int b = color & 0xff;
		if (a > 0) {
			return new Color(r/255.0f,g/255.0f,b/255.0f,a);
		} else {
			return new Color(r/255.0f,g/255.0f,b/255.0f);
		}
	}

    public static int GetCellColorValue(int id)
    {
        for (int i = 0; i < CELLIDS.Count;i++ )
        {
            if (id == CELLIDS[i])
            {
                return CELLS[i];
            }
        }
		return 0x99F60C;
    }

	public static string GetColorStr(int color, string sour)
	{
		return "<color=#" + color.ToString("x") + ">" + sour + "</color>";
	}
}