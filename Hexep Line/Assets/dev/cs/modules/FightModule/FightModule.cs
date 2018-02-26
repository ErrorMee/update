using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class FightModule : BaseModule {

	//当前战斗状态
    public static FightStadus crtFightStadus = FightStadus.task_tip;

	public GameObject fightLayerPrefab;
	public RectTransform layersTrans;
	public RectTransform animTrans;
    private Dictionary<FightLayerType, FightBaseLayer> layerDic = new Dictionary<FightLayerType, FightBaseLayer>();

	public FightUI fightUI;

    override protected void Awake()
    {
        base.Awake();
        fightUI.gameObject.SetActive(true);
        InitLayers();
        AudioModel.Instance.PlayMusic("bugs");
    }

    void Start()
    {
        MonsterModel.Instance.InitCrawlCreators();
        MonsterModel.Instance.InitCrawl();
        
        foreach (FightLayerType key in layerDic.Keys)
        {
            FightBaseLayer layer = layerDic[key];
            layer.ShowList();
        }
        LeanTween.delayedCall(0.1f, fightUI.InitView);

        int hideHeight = BattleModel.Instance.crtBattle.HideHeight();

        if (hideHeight > 0)
        {
            crtFightStadus = FightStadus.roll;

            animTrans.anchoredPosition = new Vector2(0, hideHeight * PosUtil.CELL_H);

            float tweenTime = hideHeight * 0.5f;
            LeanTween.move(animTrans, new Vector2(0, 0), tweenTime).onComplete = AllRollComplete;

        }
        else
        {
            crtFightStadus = FightStadus.idle;
            animTrans.anchoredPosition = new Vector2(0, 0);
        }

        LeanTween.delayedCall(0.2f, ShowControl);
    }

	private void InitLayers()
	{
		CreateLayer(FightLayerType.bg);
		CreateLayer(FightLayerType.floor_hide);
		CreateLayer(FightLayerType.floor);
        CreateLayer(FightLayerType.line);
		CreateLayer(FightLayerType.cell);
		CreateLayer(FightLayerType.monster);
		CreateLayer(FightLayerType.fence);
        CreateLayer(FightLayerType.cover);
		CreateLayer(FightLayerType.fg);
		CreateLayer(FightLayerType.effect);
	}

    private void CreateLayer(FightLayerType type)
    {
        GameObject layer = GameObject.Instantiate(fightLayerPrefab);
        layer.name = "" + type;

        FightBaseLayer layerCtr;
        switch (type)
        {
            case FightLayerType.bg:
                layerCtr = layer.AddComponent<BgLayer>();
                layer.transform.SetParent(animTrans, false);
                break;
			case FightLayerType.floor_hide:
				layerCtr = layer.AddComponent<FloorHideLayer>();
                layer.transform.SetParent(animTrans, false);
                break;
            case FightLayerType.floor:
                layerCtr = layer.AddComponent<FloorLayer>();
                layer.transform.SetParent(animTrans, false);
                break;
            case FightLayerType.line:
                layerCtr = layer.AddComponent<LineLayer>();
                layer.transform.SetParent(animTrans, false);
                break;
            case FightLayerType.cell:
                layerCtr = layer.AddComponent<CellLayer>();
                layer.transform.SetParent(animTrans, false);
                break;
			case FightLayerType.monster:
				layerCtr = layer.AddComponent<MonsterLayer>();
				layer.transform.SetParent(animTrans, false);
				break;
            case FightLayerType.fence:
                layerCtr = layer.AddComponent<FanceLayer>();
                layer.transform.SetParent(animTrans, false);
                break;
            case FightLayerType.cover:
                layerCtr = layer.AddComponent<CoverLayer>();
                layer.transform.SetParent(animTrans, false);
                break;
            case FightLayerType.fg:
                layerCtr = layer.AddComponent<FgLayer>();
                layer.transform.SetParent(layersTrans, false);
                break;
			case FightLayerType.effect:
				layerCtr = layer.AddComponent<EffectLayer>();
				layer.transform.SetParent(layersTrans, false);
				break;
            case FightLayerType.control:
                layerCtr = layer.AddComponent<ControlLayer>();
                layer.transform.SetParent(layersTrans, false);
                break;
            default:
                layerCtr = layer.AddComponent<FightBaseLayer>();
                break;
        }
        layerCtr.type = type;
        layerDic.Add(type, layerCtr);
    }

    private void ShowControl()
    {
        CreateLayer(FightLayerType.control);
        layerDic[FightLayerType.control].ShowList();
    }

    private void AllRollComplete()
    {
        crtFightStadus = FightStadus.idle;
    }

    private int rollCount;
    public bool CheckRoll()
    {
        rollCount = CoverModel.Instance.RollCount();
        if (rollCount > 0)
        {
            crtFightStadus = FightStadus.roll;
            float tweenTime = rollCount * 0.5f;
			LeanTween.move(animTrans, new Vector2(0, rollCount * PosUtil.CELL_H), tweenTime).onComplete = StepRollComplete;

            return true;
        }
        return false;
    }

    private void StepRollComplete()
    {
        BattleModel.Instance.RollCut(rollCount);
        animTrans.anchoredPosition = new Vector2(0, 0);
        foreach (FightLayerType key in layerDic.Keys)
        {
            FightBaseLayer layer = layerDic[key];
            layer.ShowList();

            if (key == FightLayerType.cell)
            {
                CellLayer cellLayer = (CellLayer)layer;
                cellLayer.CheckAutoRefresh();
            }

        }
        crtFightStadus = FightStadus.idle;
    }
}
