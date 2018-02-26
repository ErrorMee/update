using UnityEngine;
using System.Collections;

public class PlayerPrefsUtil 
{
    public static string PlayerPrefsID = "UserId";

    //包含测试数据
    public const string TEST_DATA = "test_data";
    //清理数据
	public const string CLEAR_DATA = "clear_data";
	//章节数据
	public const string CHAPTER = "chapter";
	//已通过的关卡
    public const string PASS_MAP = "pass_map";
	//新开放的关卡
    public const string NEW_MAP = "new_map";
	//技能升级数据
	public const string SKILL_LV = "skill_lvs";
    //技能选择数据
    public const string SKILL_SELECT = "skill_selects";
	//玩家财产
	public const string WEALTH = "wealths";
	//体力购买次数
	public const string ENERGY_BUY = "energy_buy";

    //引导
    public const string GUILD = "guild";

    //设置music静音
    public const string MUSIC_MUTE = "music_mute";
    //设置sound静音
    public const string SOUND_MUTE = "sound_mute";
	//能量恢复时间
	public const string ENERGY_OUT_TIME = "energy_out_time";

    //语言
    public const string LANGUAGE = "game_language";
    //语言是否初始化过
    public const string LANGUAGE_INITED = "game_language_inited";

    //钻石购买次数
    public const string DIAMOND_BUY = "diamond_buy";

    //瓶子收集
    public const string BOTTLE_COLLECT = "bottle_collect";

	//ad播放次数
	public const string AD_PLAY_TIMES = "ad_play_times";
	//ad播放次数
	public const string AD_TODAY_TIMES = "ad_today_times";
	//ad重置时间
	public const string AD_RESET_DAY = "ad_reset_day";

	//huoyuedu
	public const string ACTIVITY = "activity";

    public static bool HasKey(string key)
    {
        string name = GetKey(key);
        return PlayerPrefs.HasKey(name);
    }
    public static int GetInt(string key,int defaultValue = 0)
    {
        if (HasKey(key) == false)
        {
            return defaultValue;
        }
        string name = GetKey(key);
        return PlayerPrefs.GetInt(name);
    }

    public static void SetInt(string key, int value)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetInt(name, value);
    }

    public static string GetString(string key, string defaultValue = "")
    {
        if (HasKey(key) == false)
        {
            return defaultValue;
        }
        string name = GetKey(key);
        return PlayerPrefs.GetString(name);
    }

    public static void SetString(string key, string value)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
        PlayerPrefs.SetString(name, value);
    }

    public static void RemoveData(string key)
    {
        string name = GetKey(key);
        PlayerPrefs.DeleteKey(name);
    }

    public static string GetKey(string key)
    {
        return "cellwar_" + PlayerPrefsID + "_" + key;
    }
}
