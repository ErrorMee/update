using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PrepareGo : MonoBehaviour {

	public Button okBtn;
	
	public Text costText;

	public Transform adBtn;

	private int needEnergy;

	void OnEnable()
	{
		EventTriggerListener.Get(okBtn.gameObject).onClick = OnOkClick;

        okBtn.gameObject.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11204);
		PlayerModel.Instance.updateWealthsEvent += UpdateView;

		EventTriggerListener.Get(adBtn.gameObject).onClick = OnADClick;
	}

	private void OnADClick(GameObject go)
	{
		GameMgr.moduleMgr.AddUIModule(ModuleEnum.ADREWARD);
	}

	void Awake()
	{
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
		UpdateView ();
		PrepareModule.LayItemCount ++ ;
	}

	private void UpdateView()
	{
		needEnergy = (int)GameModel.Instance.GetGameConfig(1001);

		MapInfo mapInfo = MapModel.Instance.GetMapInfo(BattleModel.Instance.crtConfig.id);

		if (mapInfo != null) {

			needEnergy -= mapInfo.star;
		}

		if(needEnergy < 1)
		{
			needEnergy = 1;
		}



		costText.text = "" + needEnergy;

		WealthInfo energyInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Energy);
		if (energyInfo.count >= needEnergy) 
		{
			costText.color = Color.green;
		} else 
		{
			costText.color = Color.red;;
		}
	}

	void OnDisable()
	{
		PlayerModel.Instance.updateWealthsEvent -= UpdateView;
	}

	private void OnOkClick(GameObject go)
	{

		config_map_item mapConfig = BattleModel.Instance.crtConfig;
		IVInfo starLimit = mapConfig.GetStarLimit();

		if(starLimit.id > 0)
		{
			StarInfo starInfo = MapModel.Instance.starInfo;

			if(starLimit.id > starInfo.crtStar)// star short
			{
				MapInfo mapInfo = MapModel.Instance.GetMapInfo(mapConfig.id);
				if(mapInfo.buyPassed == false)// no buy
				{
					GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAPLOCK);
					return;
				}
			}
		}

		WealthInfo energyInfo = PlayerModel.Instance.GetWealth ((int)WealthTypeEnum.Energy);
		if(energyInfo.count >= needEnergy)
		{
            PlayerModel.Instance.updateWealthsEvent -= UpdateView;
			energyInfo.count -= needEnergy;
			PlayerModel.Instance.SaveWealths();
			PlayerModel.Instance.CheckEnergyRecover (false);
			
			SkillModel.Instance.InitSeeds();
			SkillModel.Instance.InitFightingEntitys();
			SkillModel.Instance.crt_entity = null;
			
			PropModel.Instance.InitProps();
			
			CollectModel.Instance.Clear();

            BattleModel.Instance.play_mapId = BattleModel.Instance.crtBattle.mapId;
            BattleModel.Instance.ready_map = 0;
			GameMgr.moduleMgr.AddUIModule(ModuleEnum.FIGHT);

		}else
		{
			PlayerModel.Instance.ExchangeWealth((int)WealthTypeEnum.Energy, needEnergy - energyInfo.count, GotoFight);
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, (int)WealthTypeEnum.Energy);
        }
		
	}

	private void GotoFight()
	{

		WealthInfo energyInfo = PlayerModel.Instance.GetWealth ((int)WealthTypeEnum.Energy);
		if(energyInfo.count >= needEnergy)
		{
			PlayerModel.Instance.updateWealthsEvent -= UpdateView;
			energyInfo.count -= needEnergy;
			PlayerModel.Instance.SaveWealths();
			PlayerModel.Instance.CheckEnergyRecover (false);

			SkillModel.Instance.InitSeeds();
			SkillModel.Instance.InitFightingEntitys();
			SkillModel.Instance.crt_entity = null;
			
			PropModel.Instance.InitProps();
			
			CollectModel.Instance.Clear();
			
			BattleModel.Instance.play_mapId = BattleModel.Instance.crtBattle.mapId;
			BattleModel.Instance.ready_map = 0;

			GameMgr.moduleMgr.AddUIModule (ModuleEnum.FIGHT);
		}
	}
}