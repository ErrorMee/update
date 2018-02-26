using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LanguageUtil
{
    private static Dictionary<string, string> langDic = null;

    public const string LANG_EN = "en";
    public const string FONT_CN = "cn";
    public static string[] langnames = null;
    public static string LANG_DEFAULT = LANG_EN;

    public static void SwitchLanguage()
    {
        if (langnames == null)
        {
            AssetBundle ab = GameMgr.resourceMgr.GetAb("txt/language.ab");
            string[] langnamepaths = ab.GetAllAssetNames();
            langnames = new string[langnamepaths.Length];

            for (int i = 0; i < langnamepaths.Length; i++)
            {
                string langnamepathstr = langnamepaths[i];
                string[] langnamepathstrArr = langnamepathstr.Split('/');
                langnames[i] = langnamepathstrArr[langnamepathstrArr.Length - 1].Split('.')[0];
            }
        }

        int crtIndex = 0;
        for (int i = 0; i < langnames.Length;i++ )
        {
            string langname = langnames[i];
            if (langname == LANG_DEFAULT)
            {
                crtIndex = i;
                break;
            }
        }

        crtIndex++;

        if (crtIndex >= langnames.Length)
        {
            crtIndex = 0;
        }

        SelecteLanguage(langnames[crtIndex]);
    }

    public static void SelecteLanguage(string languageName)
    {
        PlayerPrefsUtil.SetString(PlayerPrefsUtil.LANGUAGE, languageName);
        InitLang();

        FontUtil.ChangeFont(GetTxt(10002));
    }

    public static void InitLang()
    {
        if (PlayerPrefsUtil.GetString(PlayerPrefsUtil.LANGUAGE) != "")
        {
            LANG_DEFAULT = PlayerPrefsUtil.GetString(PlayerPrefsUtil.LANGUAGE);
        }

        langDic = new Dictionary<string, string>();
        string content = GameMgr.resourceMgr.GetTextString("txt/language.ab", LANG_DEFAULT);
        string[] txtArr = content.Split('\n');
        foreach (string txt in txtArr)
        {
            if (txt != "")
            {
                string[] txts = txt.Split('=');
                if (txts.Length > 1)
                {
                    langDic.Add(txts[0], txts[1].Split('\r')[0]);
                }
            }
        }
    }

    public static string GetTxt(int id)
    {
        string idstr = "" + id;
        if(langDic.ContainsKey(idstr))
        {
            return langDic[idstr];
        }
        return "null txt";
    }

    public static string GetDefaultLanguageName()
    {
        return GetTxt(10001);
    }
}