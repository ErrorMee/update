using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ModuleEnum
{
	BG = 10000,
	LOGIN = 10001,
	MAP = 10002,
	SKILL = 10003,
	PREPARE = 10004,
	FIGHT = 10005,
	REPORT = 10006,
	PAUSE = 10007,
	PROMPT = 10008,
	Guide = 10009,
	SET = 10010,
	CONFIRM = 10011,
	WEALTH = 10012,
	EXCHANGE = 10013,
	PINBALL = 10014,
	ENERGY = 10015,
	STAR = 10016,
	MAPLOCK = 10017,
	ADREWARD = 10018,
}

public class ModuleModel : Singleton<ModuleModel>
{

	private List<config_module_item> crtModules = new List<config_module_item>();

	private Transform canvasTrans;

	private GameObject loadingObj;

	public ModuleModel ()
	{
	}

	public void StartGame()
	{
		canvasTrans = GameObject.Find("Canvas").transform;

		InitUILayers ();

		AddUIModule((int)ModuleEnum.BG);
		AddUIModule((int)ModuleEnum.Guide);
		AddUIModule((int)ModuleEnum.PROMPT);
		AddUIModule((int)ModuleEnum.LOGIN);
	}

	private void InitUILayers()
	{
		List<int> allLayers = ResModel.Instance.config_module.GetAllLayers();

		for (int i = 0; i < allLayers.Count;i++ )
		{
			int layerId = allLayers[i];
			GameObject layerGo = new GameObject();
			layerGo.name = "layer_" + layerId;
			layerGo.transform.SetParent(canvasTrans, false);
		}
	}

	private GameObject GetUILayer(int depth)
	{
		return canvasTrans.Find("layer_" + depth).gameObject;
	}

	public void AddUIModule(int moduleId)
	{
		config_module_item module_item = (config_module_item)ResModel.Instance.config_module.GetItem(moduleId);

		if (module_item != null)
		{

			//清理
			int openClearCount = module_item.GetOpenClearCount();
			if (openClearCount < 0)//所有
			{
				for (int i = crtModules.Count; i > 0; i--)
				{
					config_module_item module_temp = crtModules[i - 1];

					if (module_temp.never_close == 0)
					{
						RemoveUIModule(module_temp.id);
					}

				}
			}
			else if (openClearCount >= 0)//指定
			{

			}
			else//无
			{}

			//打开自己
			GameObject uiModule = GetUIModule(module_item);
			if (uiModule == null)
			{
				CreatUIModule(module_item);
			}
			else
			{
				uiModule.transform.SetAsLastSibling();
			}
		}
	}

	private void CreatUIModule(config_module_item config)
	{
		GameObject prefabInstance = ResModel.Instance.GetPrefabInstance(config.ab_name + "/" + config.prefab_name,config.prefab_name);

		if (prefabInstance != null)
		{
			GameObject layerGo = GetUILayer(config.layer_depth);
			prefabInstance.transform.SetParent(layerGo.transform, false);

			crtModules.Add(config);
		}
	}

	public void RemoveUIModule(int moduleId)
	{
		config_module_item module_item = (config_module_item)ResModel.Instance.config_module.GetItem(moduleId);
		if (module_item != null) {
			GameObject uiModule = GetUIModule(module_item);
			if (uiModule != null)
			{
				LeanTween.cancelAll ();
				ClockModel.Instance.StartUp ();
				GameObject.DestroyImmediate(uiModule);
				crtModules.RemoveAt(crtModules.IndexOf(module_item));
			}
		}
	}

	private GameObject GetUIModule(config_module_item config)
	{
		GameObject uiLayer = GetUILayer(config.layer_depth);
		Transform moduleTrans = uiLayer.transform.FindChild(config.prefab_name);

		if (moduleTrans == null)
		{
			return null;
		}
		else
		{
			return moduleTrans.gameObject;
		}
	}
}