using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class EditResourceMgr : MonoBehaviour
{
    public event ResourceEventHandler EventHandler;

	private string sourceArtPath = null;
    private string abRootPath = null;
    public config_cell config_cell;
    public config_wall config_wall;
    public config_map config_map;
    public config_cover config_cover;
	public config_monster config_monster;

	//依赖配置字典
	private Dictionary<string, AssetBundleManifest> manifestDic = null;
	//AssetBundle字典
	private Dictionary<string, AssetBundle> abDic = null;

    void Awake()
    {
		GridMain.resourceMgr = this;

        Caching.CleanCache();
		sourceArtPath = "file:///" + Application.dataPath + "/sourceArt/";
		abRootPath = "file:///" + Application.dataPath + "/StreamingAssets/mobile/";
        config_cell = null;
        config_wall = null;
        config_map = null;
        config_cover = null;
		config_monster = null;

		manifestDic = new Dictionary<string, AssetBundleManifest>();
		abDic = new Dictionary<string, AssetBundle>();

        StartCoroutine(LoadCommonRes());
    }

    private IEnumerator LoadCommonRes()
    {
		yield return StartCoroutine(LoadManifest("mobile"));

        yield return StartCoroutine(LoadConfigs());
		yield return StartCoroutine(LoadDats());
        yield return StartCoroutine(LoadIcons());

        Debug.Log("LoadRes " + ResourceStatic.COMMON);
        if (EventHandler != null)
        {
            EventHandler(ResourceEventType.load_complete, ResourceStatic.COMMON);
        }
    }

    private IEnumerator LoadConfigs()
    {
		WWW www = new WWW (sourceArtPath + "config/config_cell.json");
		yield return www;
		config_cell = JsonMapper.ToObject<config_cell>(www.text);

		www = new WWW (sourceArtPath + "config/config_wall.json");
		yield return www;
		config_wall = JsonMapper.ToObject<config_wall>(www.text);

		www = new WWW (sourceArtPath + "config/config_map.json");
		yield return www;
		config_map = JsonMapper.ToObject<config_map>(www.text);

        www = new WWW(sourceArtPath + "config/config_cover.json");
        yield return www;
        config_cover = JsonMapper.ToObject<config_cover>(www.text);

		www = new WWW(sourceArtPath + "config/config_monster.json");
		yield return www;
		config_monster = JsonMapper.ToObject<config_monster>(www.text);
    }	

	private IEnumerator LoadDats()
	{
		for(int i = 0;i<config_map.data.Count;i++)
		{
			config_map_item item = config_map.data[i];
			BattleInfo battleInfo = new BattleInfo();
			battleInfo.mapId = item.id;
            if (FileUtil.Instance().HasFile("dat/map/" + item.id, ".bytes"))
			{
                WWW www = new WWW(sourceArtPath + "dat/map/" + item.id + ".bytes");
				yield return www;
				battleInfo.FillByte(www.bytes);
			}else
			{
                battleInfo.FillNew(GridModel.Instance.set_start_x, GridModel.Instance.set_start_y, GridModel.Instance.set_end_x, GridModel.Instance.set_end_y, GridModel.Instance.set_battle_width, GridModel.Instance.set_battle_height);
			}
            BattleModel.Instance.battles.Add(battleInfo);
		}
	}

	private IEnumerator LoadIcons()
	{
		yield return StartCoroutine(LoadDependenciesAb("icon/cell.ab"));
		yield return StartCoroutine(LoadDependenciesAb("icon/wall.ab"));
		yield return StartCoroutine(LoadDependenciesAb("icon/cover.ab"));
		yield return StartCoroutine(LoadDependenciesAb("icon/monster.ab"));
	}

	private IEnumerator LoadManifest(string manifestPath)
	{
		if (manifestDic.ContainsKey(manifestPath) == false)
		{
			string mUrl = abRootPath + manifestPath;
			WWW mwww = WWW.LoadFromCacheOrDownload(mUrl, 0);
			yield return mwww;
			if (!string.IsNullOrEmpty(mwww.error))
			{
				Debug.LogError(mwww.error + " " + manifestPath);
			}
			else
			{
				AssetBundle mab = mwww.assetBundle;
				AssetBundleManifest mainfest = (AssetBundleManifest)mab.LoadAsset("AssetBundleManifest");
				mab.Unload(false);
				manifestDic.Add(manifestPath, mainfest);
			}
		}
	}

	private IEnumerator LoadDependenciesAb(string abPath)
	{
        if (manifestDic.ContainsKey("mobile"))
		{
            AssetBundleManifest mainfest = manifestDic["mobile"];
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
						Debug.LogError(dwww.error);
					}
					else
					{
						abDic[dpPath] = dwww.assetBundle;
					}
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
			}
			yield return null;
		}
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
}
