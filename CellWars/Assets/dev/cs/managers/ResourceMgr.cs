using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public enum ResourceEventType
{
    load_complete = 0,
}

public delegate void ResourceEventHandler(ResourceEventType eventType,string addData);

public class ResourceMgr : MonoBehaviour
{

    public event ResourceEventHandler EventHandler;

    //assetbundle根目录
    private string abRootPath = null;
    //依赖配置字典
    private Dictionary<string, AssetBundleManifest> manifestDic = null;
    //AssetBundle字典
    private Dictionary<string, AssetBundle> abDic = null;

    //fonts字典
    private Dictionary<string, Font> fontDic = null;

    public config_cell config_cell;
    public config_wall config_wall;
    public config_map config_map;
    public config_cover config_cover;
	public config_monster config_monster;
    public config_prop config_prop;
    public config_module config_module;
	public config_guild config_guild;
    public config_chapter config_chapter;
    public config_wealth config_wealth;
    public config_skill config_skill;
    public config_sort config_sort;

    void Awake()
    {
        GameMgr.resourceMgr = this;

        Caching.CleanCache();
		abRootPath = "file:///" + Application.dataPath + "/StreamingAssets/mobile/";

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.WP8Player)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
				abRootPath = Application.dataPath + "/Raw/mobile/";
            else
            {
				abRootPath = "jar:file://" + Application.dataPath + "!/assets/mobile/";
            }
        }
        else
        {
        }
        manifestDic = new Dictionary<string, AssetBundleManifest>();
        abDic = new Dictionary<string, AssetBundle>();
        fontDic = new Dictionary<string, Font>();
    }

    public IEnumerator LoadCommonRes()
    {
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			yield return StartCoroutine(LoadManifest(true));
		}else
		{
			yield return StartCoroutine(LoadManifest(false));
		}
        
        yield return StartCoroutine(LoadFont());
        yield return StartCoroutine(LoadConfigs());
        yield return StartCoroutine(LoadIcons());
        yield return StartCoroutine(LoadDats());
        yield return StartCoroutine(LoadTxts());
        yield return StartCoroutine(LoadEffect());

        GameModel.Instance.InitGameConfig();
        //if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            SocialModel.Instance.StartUp();
			//ADModel.Instance.Init ();
        }
        IAPModel.Instance.Init();
        LanguageUtil.InitLang();
        FontUtil.ChangeFont(LanguageUtil.GetTxt(10002));
		//ClockModel.Instance.SetClock (new ClockInfo (1));
		ClockModel.Instance.StartUp ();

        yield return StartCoroutine(LoadDependenciesAb("prefab/base.ab"));
        yield return StartCoroutine(LoadDependenciesAb("audio.ab"));
        Debug.Log("LoadRes " + ResourceStatic.COMMON);
        if (EventHandler != null)
        {
            EventHandler(ResourceEventType.load_complete, ResourceStatic.COMMON);
        }
    }

    private IEnumerator LoadConfigs()
    {
        yield return StartCoroutine(LoadDependenciesAb("config/config_cell.ab"));
        config_cell = JsonMapper.ToObject<config_cell>(GetTextString("config/config_cell.ab", "config_cell"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_wall.ab"));
        config_wall = JsonMapper.ToObject<config_wall>(GetTextString("config/config_wall.ab", "config_wall"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_map.ab"));
        config_map = JsonMapper.ToObject<config_map>(GetTextString("config/config_map.ab", "config_map"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_cover.ab"));
        config_cover = JsonMapper.ToObject<config_cover>(GetTextString("config/config_cover.ab", "config_cover"));

		yield return StartCoroutine(LoadDependenciesAb("config/config_monster.ab"));
        config_monster = JsonMapper.ToObject<config_monster>(GetTextString("config/config_monster.ab", "config_monster"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_prop.ab"));
        config_prop = JsonMapper.ToObject<config_prop>(GetTextString("config/config_prop.ab", "config_prop"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_module.ab"));
        config_module = JsonMapper.ToObject<config_module>(GetTextString("config/config_module.ab", "config_module"));

		yield return StartCoroutine(LoadDependenciesAb("config/config_guild.ab"));
        config_guild = JsonMapper.ToObject<config_guild>(GetTextString("config/config_guild.ab", "config_guild"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_chapter.ab"));
        config_chapter = JsonMapper.ToObject<config_chapter>(GetTextString("config/config_chapter.ab", "config_chapter"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_wealth.ab"));
        config_wealth = JsonMapper.ToObject<config_wealth>(GetTextString("config/config_wealth.ab", "config_wealth"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_skill.ab"));
        config_skill = JsonMapper.ToObject<config_skill>(GetTextString("config/config_skill.ab", "config_skill"));

        yield return StartCoroutine(LoadDependenciesAb("config/config_sort.ab"));
        config_sort = JsonMapper.ToObject<config_sort>(GetTextString("config/config_sort.ab", "config_sort"));
    }

    private IEnumerator LoadFont()
    {
        yield return StartCoroutine(LoadDependenciesAb("font.ab"));
    }

    private IEnumerator LoadIcons()
    {
        yield return StartCoroutine(LoadDependenciesAb("icon/cell.ab"));
        yield return StartCoroutine(LoadDependenciesAb("icon/wall.ab"));
        yield return StartCoroutine(LoadDependenciesAb("icon/cover.ab"));
        yield return StartCoroutine(LoadDependenciesAb("icon/monster.ab"));
        yield return StartCoroutine(LoadDependenciesAb("icon/prop.ab"));
    }

    private IEnumerator LoadDats()
    {
        yield return StartCoroutine(LoadDependenciesAb("dat/map.ab"));
    }

    private IEnumerator LoadTxts()
    {
        yield return StartCoroutine(LoadDependenciesAb("txt.ab"));
        yield return StartCoroutine(LoadDependenciesAb("txt/fill.ab"));
        yield return StartCoroutine(LoadDependenciesAb("txt/language.ab"));
    }

    private IEnumerator LoadEffect()
    {
        yield return StartCoroutine(LoadDependenciesAb("effect.ab"));
    }

    public IEnumerator LoadModule(string abPath, string name)
    {
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			yield return StartCoroutine(LoadManifest(true));
		}else
		{
			yield return StartCoroutine(LoadManifest(false));
		}
        yield return StartCoroutine(LoadDependenciesAb(abPath));
        EventHandler(ResourceEventType.load_complete, name);
    }

    private IEnumerator LoadManifest(bool isIos)
    {
		string manifestPath = "mobile";
        if (manifestDic.ContainsKey(manifestPath) == false)
        {
            string mUrl = abRootPath + manifestPath;

			if(isIos)
			{
				if(File.Exists(mUrl))
				{
					byte[] stream = File.ReadAllBytes(mUrl);
					//AssetBundle.LoadFromMemory
                    AssetBundle mab = AssetBundle.LoadFromMemory(stream);
					AssetBundleManifest mainfest = (AssetBundleManifest)mab.LoadAsset("AssetBundleManifest");
					mab.Unload(false);
					manifestDic.Add(manifestPath, mainfest);
				}else
				{
					Debug.LogError("not Exists " + mUrl);
				}
			}else
			{
				WWW mwww = WWW.LoadFromCacheOrDownload(mUrl, 1);
				yield return mwww;
				if (!string.IsNullOrEmpty(mwww.error))
				{
					Debug.LogError(mwww.error + " - " + mUrl);
				}
				else
				{
					AssetBundle mab = mwww.assetBundle;
					AssetBundleManifest mainfest = (AssetBundleManifest)mab.LoadAsset("AssetBundleManifest");
					mab.Unload(false);
					manifestDic.Add(manifestPath, mainfest);
				}
				mwww = null;
			}
        }
		yield return null;
    }

    private IEnumerator LoadDependenciesAb(string abPath)
    {
		string osKey = "mobile";
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (manifestDic.ContainsKey(osKey))
			{
				AssetBundleManifest mainfest = manifestDic[osKey];
				string mainAbPath = abPath;
				string[] dps = mainfest.GetAllDependencies(mainAbPath);
				for (int i = 0; i < dps.Length; i++)
				{
					string dpPath = dps[i];
					if (abDic.ContainsKey(dpPath) == false)
					{
						if(File.Exists(abRootPath + dpPath))
						{
							byte[] stream = File.ReadAllBytes(abRootPath + dpPath);
                            abDic[dpPath] = AssetBundle.LoadFromMemory(stream);
							
						}else
						{
							Debug.LogError("no Exists " + abRootPath + dpPath);
						}
					}
				}
				
				if (abDic.ContainsKey(mainAbPath) == false)
				{
					if(File.Exists(abRootPath + mainAbPath))
					{
						byte[] stream = File.ReadAllBytes(abRootPath + mainAbPath);
                        abDic[mainAbPath] = AssetBundle.LoadFromMemory(stream);
						
					}else
					{
						Debug.LogError("no Exists " + abRootPath + mainAbPath);
					}
				}
			}

		}else
		{
			if (manifestDic.ContainsKey(osKey))
			{
				AssetBundleManifest mainfest = manifestDic[osKey];
				string mainAbPath = abPath;
				string[] dps = mainfest.GetAllDependencies(mainAbPath);
				for (int i = 0; i < dps.Length; i++)
				{
					string dpPath = dps[i];
					if (abDic.ContainsKey(dpPath) == false)
					{
						WWW dwww = WWW.LoadFromCacheOrDownload(abRootPath + dpPath, mainfest.GetAssetBundleHash(dpPath));
						yield return dwww;
						if (!string.IsNullOrEmpty(dwww.error))
						{
							Debug.LogError(dwww.error + " abPath " + abPath);
						}
						else
						{
							abDic[dpPath] = dwww.assetBundle;
						}
						dwww = null;
					}
				}
				
				if (abDic.ContainsKey(mainAbPath) == false)
				{
					WWW www = WWW.LoadFromCacheOrDownload(abRootPath + mainAbPath, mainfest.GetAssetBundleHash(mainAbPath));
					yield return www;
					if (!string.IsNullOrEmpty(www.error))
					{
						Debug.LogError(www.error + " - " + mainAbPath);
					}
					else
					{
						abDic[mainAbPath] = www.assetBundle;
					}
					www = null;
				}
			}
		}

		yield return null;
    }

    public AssetBundle GetAb(string abpath)
    {
        if (abDic.ContainsKey(abpath))
        {
            return abDic[abpath];
        }
        return null;
    }

    public GameObject GetGameObject(string abPath, string objName)
    {
        GameObject obj = null;
        if (abDic.ContainsKey(abPath))
        {
            AssetBundle ab = abDic[abPath];
            obj = ab.LoadAsset<GameObject>(objName);
        }
        return obj;
    }

    public Sprite GetSprite(string abpath, string iconname)
    {
        Sprite obj = null;
        if (abDic.ContainsKey(abpath))
        {
            AssetBundle ab = abDic[abpath];
            obj = ab.LoadAsset<Sprite>(iconname);
        }
        return obj;
    }

    public Font GetFont(string fontName)
    {
        if (fontDic.ContainsKey(fontName))
        {
            return fontDic[fontName];
        }

        Font font = null;
        if (abDic.ContainsKey("font.ab"))
        {
            AssetBundle ab = abDic["font.ab"];
            font = ab.LoadAsset<Font>(fontName);
            fontDic[fontName] = font;
        }
        return font;
    }

    private TextAsset GetTextAsset(string abpath, string assetName)
    {
        TextAsset obj = null;
        if (abDic.ContainsKey(abpath))
        {
            AssetBundle ab = abDic[abpath];
            obj = ab.LoadAsset<TextAsset>(assetName);
        }
        return obj;
    }

    public string GetTextString(string abpath, string assetName)
    {
        TextAsset textAsset = GetTextAsset(abpath, assetName);
        if (textAsset == null)
        {
            return null;
        }
        return textAsset.text;
    }

    public byte[] GetTextBytes(string abpath, string assetName)
    {
        TextAsset textAsset = GetTextAsset(abpath, assetName);
        if (textAsset == null)
        {
            return null;
        }
        return textAsset.bytes;
    }

    public AudioClip GetAudioClip(string audioName)
    {
        AudioClip audioClip = null;
        if (abDic.ContainsKey("audio.ab"))
        {
            AssetBundle ab = abDic["audio.ab"];
            audioClip = ab.LoadAsset<AudioClip>(audioName);
        }
        return audioClip;
    }
}
