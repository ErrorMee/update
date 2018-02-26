using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class ResModel : Singleton<ResModel>
{
	
	public config_cell config_cell;
	public config_wall config_wall;
	public config_map config_map;
	public config_cover config_cover;
	public config_monster config_monster;
	public config_prop config_prop;
	public config_module config_module;
	public config_guide config_guide;
	public config_chapter config_chapter;
	public config_wealth config_wealth;
	public config_skill config_skill;
	public config_sort config_sort;

	public Dictionary<string, Font> fontDic = null;

	public void Init ()
	{
		AudioModel.Instance.InitAudio ();
		GameModel.Instance.InitGameConfig ();
		LanguageUtil.InitLang ();

		InitConfigs ();
		InitFonts ();
	}

	private void InitConfigs()
	{
		config_cell = JsonMapper.ToObject<config_cell>(GetTextString("config/config_cell"));

		config_wall = JsonMapper.ToObject<config_wall>(GetTextString("config/config_wall"));

		config_map = JsonMapper.ToObject<config_map>(GetTextString("config/config_map"));

		config_cover = JsonMapper.ToObject<config_cover>(GetTextString("config/config_cover"));

		config_monster = JsonMapper.ToObject<config_monster>(GetTextString("config/config_monster"));

		config_prop = JsonMapper.ToObject<config_prop>(GetTextString("config/config_prop"));

		config_module = JsonMapper.ToObject<config_module>(GetTextString("config/config_module"));

		config_guide = JsonMapper.ToObject<config_guide>(GetTextString("config/config_guide"));

		config_chapter = JsonMapper.ToObject<config_chapter>(GetTextString("config/config_chapter"));

		config_wealth = JsonMapper.ToObject<config_wealth>(GetTextString("config/config_wealth"));

		config_skill = JsonMapper.ToObject<config_skill>(GetTextString("config/config_skill"));

		config_sort = JsonMapper.ToObject<config_sort>(GetTextString("config/config_sort"));
	}

	private void InitFonts()
	{
		fontDic = new Dictionary<string, Font> ();
		fontDic.Add(FontUtil.FONT_1,Resources.Load<Font> ("font/" + FontUtil.FONT_1));
		fontDic.Add(FontUtil.FONT_2,Resources.Load<Font> ("font/" + FontUtil.FONT_2));
	}

	public GameObject GetPrefab(string resPath)
	{
		GameObject prefab = Resources.Load<GameObject> (resPath);
		return prefab;
	}

	public GameObject GetPrefabInstance(string resPath,string name = "")
	{
		GameObject prefab = GetPrefab(resPath);

		if(prefab == null)
		{
			Debug.LogError (resPath + " not find ");
			return null;
		}

		GameObject instance =  GameObject.Instantiate(prefab) as GameObject;
		if(name != null || name != "")
		{
			if(instance != null)
			{
				instance.name = name;
			}
		}
		return instance;
	}


	private TextAsset GetTextAsset(string resPath)
	{
		TextAsset textAsset = Resources.Load<TextAsset> (resPath);
		return textAsset;
	}

	public string GetTextString(string resPath)
	{
		TextAsset textAsset = GetTextAsset(resPath);
		if (textAsset == null)
		{
			return null;
		}
		return textAsset.text;
	}

	public byte[] GetTextBytes(string resPath)
	{
		TextAsset textAsset = GetTextAsset(resPath);
		if (textAsset == null)
		{
			return null;
		}
		return textAsset.bytes;
	}

	public Sprite GetSprite(string resPath)
	{
		Sprite sprite = Resources.Load<Sprite>(resPath);
		return sprite;
	}

	public AudioClip GetAudioClip(string resPath)
	{
		AudioClip audioClip = Resources.Load<AudioClip>(resPath);
		return audioClip;
	}
}