using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

class ModuleMgr : MonoBehaviour
{
    private List<config_module_item> crtModules = new List<config_module_item>();

    private Transform canvasTrans;

    private GameObject loadingObj;

    void Awake()
    {
        GameMgr.moduleMgr = this;
        canvasTrans = GameObject.Find("Canvas").transform;
    }

    void Start()
    {
        GameMgr.resourceMgr.EventHandler += OnEventHandler;
        StartCoroutine(GameMgr.resourceMgr.LoadModule("prefab/loadingmodule.ab", "LoadingModule"));
    }

    private void OnEventHandler(ResourceEventType eventType, string addData)
    {
        switch (eventType)
        {
            case ResourceEventType.load_complete:
                switch (addData)
                {
                    case "LoadingModule":

                        GameObject loadingObjPrefab = GameMgr.resourceMgr.GetGameObject("prefab/loadingmodule.ab", "LoadingModule");

                        loadingObj = Instantiate(loadingObjPrefab) as GameObject;
                        loadingObj.name = "LoadingModule";
                        loadingObj.transform.SetParent(canvasTrans, false);

                        StartCoroutine(GameMgr.resourceMgr.LoadCommonRes());
                        break;
                    case ResourceStatic.COMMON:

                        InitUILayers();

                        Application.targetFrameRate = 60;

                        GameMgr.audioMgr.InitAudio();
                        
                        #if UNITY_ANDROID

                            gameObject.AddComponent<AndroidBackButton>();

                        #endif

                        config_module_item guild = (config_module_item)GameMgr.resourceMgr.config_module.GetItem((int)ModuleEnum.GUILD);
                        StartCoroutine(GameMgr.resourceMgr.LoadModule(guild.ab_name, guild.prefab_name));
                        break;
                    case "GuildModule":
                        config_module_item guild_item = GameMgr.resourceMgr.config_module.GetItemByPrefabName(addData);
                        OnAddUIModule(guild_item);

                        config_module_item prompt = (config_module_item)GameMgr.resourceMgr.config_module.GetItem((int)ModuleEnum.PROMPT);
                        StartCoroutine(GameMgr.resourceMgr.LoadModule(prompt.ab_name, prompt.prefab_name));

                        break;
                    default:
                        if (addData == "PromptModule")
                        {
                            config_module_item login = (config_module_item)GameMgr.resourceMgr.config_module.GetItem((int)ModuleEnum.LOGIN);
                            StartCoroutine(GameMgr.resourceMgr.LoadModule(login.ab_name, login.prefab_name));
                        }

                        if (addData == "LoginModule")
                        {
                            DestroyImmediate(loadingObj);
                        }

                        config_module_item module_item = GameMgr.resourceMgr.config_module.GetItemByPrefabName(addData);
                        if (module_item != null)
                        {
                            OnAddUIModule(module_item);
                        }
                        break;
                }
                
                break;
        }
    }

    private void InitUILayers()
    {
        List<int> allLayers = GameMgr.resourceMgr.config_module.GetAllLayers();

        for (int i = 0; i < allLayers.Count;i++ )
        {
            int layerId = allLayers[i];
            GameObject layerGo = new GameObject();
            layerGo.name = "layer_" + layerId;
            layerGo.transform.SetParent(canvasTrans, false);
			layerGo.transform.localPosition = new Vector3(0,-26,0);
        }
    }

    private GameObject GetUILayer(int depth)
    {
        return canvasTrans.Find("layer_" + depth).gameObject;
    }

    public void AddUIModule(ModuleEnum moduleEnum)
    {
		ScreenSlider.SlideOpen = false;

        PromptModel.Instance.ShowLoading(true);

        config_module_item module_item = (config_module_item)GameMgr.resourceMgr.config_module.GetItem((int)moduleEnum);

        if (module_item != null)
        {
            GameObject obj = GameMgr.resourceMgr.GetGameObject(module_item.ab_name, module_item.prefab_name);
            if (obj == null)
            {
                StartCoroutine(GameMgr.resourceMgr.LoadModule(module_item.ab_name, module_item.prefab_name));
            }
            else
            {
                OnAddUIModule(module_item);
            }
        }
    }

	public void RemoveUIModule(ModuleEnum moduleEnum)
	{
		config_module_item module_item = (config_module_item)GameMgr.resourceMgr.config_module.GetItem((int)moduleEnum);
        OnRemoveUIModule(module_item);
	}

    private void OnRemoveUIModule(config_module_item config)
    {
        if (config != null)
        {
            GameObject uiModule = GetUIModule(config);
            if (uiModule != null)
            {
				LeanTween.cancelAll ();
				ClockModel.Instance.StartUp ();

				DestroyImmediate(uiModule);
                crtModules.RemoveAt(crtModules.IndexOf(config));
                return;
            }
        }
    }

    private void OnAddUIModule(config_module_item config)
    {
        //清理
        int openClearCount = config.GetOpenClearCount();
        if (openClearCount < 0)//所有
        {
            for (int i = crtModules.Count; i > 0; i--)
            {
                config_module_item module_item = crtModules[i - 1];

                if (module_item.never_close == 0)
                {
                    OnRemoveUIModule(module_item);
                }

            }
        }
        else if (openClearCount >= 0)//指定
        {

        }
        else//无
        {}

        //打开自己
        GameObject uiModule = GetUIModule(config);
        if (uiModule == null)
        {
            CreatUIModule(config);
        }
        else
        {
            uiModule.transform.SetAsLastSibling();
        }
        
        //打开其他

        PromptModel.Instance.ShowLoading(false);
    }

    private void CreatUIModule(config_module_item config)
    {
        GameObject uiModulePrefab = GameMgr.resourceMgr.GetGameObject(config.ab_name, config.prefab_name);

        if (uiModulePrefab != null)
        {
            GameObject uiModule = Instantiate(uiModulePrefab) as GameObject;
            uiModule.name = config.prefab_name;

            GameObject layerGo = GetUILayer(config.layer_depth);
            uiModule.transform.SetParent(layerGo.transform, false);

            crtModules.Add(config);
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