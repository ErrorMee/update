using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PrepareRecord : MonoBehaviour {

	public Transform closeTrans;
	public Button closeBtn;
	public Text nameText;
	public Text scoreText;
	public Image iconImage;
	
	void Awake()
	{
		PrepareModule.LayItemCount ++ ;
	}

	void OnEnable()
	{
		EventTriggerListener.Get (closeBtn.gameObject).onClick = OnCloseClick;
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnCloseClick;
	}

	void OnDisable()
	{
	}
	
	void Start()
	{
		iconImage.gameObject.SetActive(false);

		nameText.text = LanguageUtil.GetTxt(11106) + ": " + BattleModel.Instance.crtConfig.name;
		
		MapInfo mapInfo = MapModel.Instance.GetMapInfo(BattleModel.Instance.crtConfig.id);
		
		if (mapInfo == null) {

            scoreText.text = LanguageUtil.GetTxt(11202);
			
		} else {

            scoreText.text = LanguageUtil.GetTxt(11201) + ":" + mapInfo.score;
			
			if(mapInfo.score == 0)
			{
                scoreText.text = LanguageUtil.GetTxt(11202);
			}
		}
		if (BattleModel.Instance.crtConfig != null && BattleModel.Instance.crtConfig.special != null && BattleModel.Instance.crtConfig.special != "")
		{
			//iconImage.gameObject.SetActive(true);
			//iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/" + BattleModel.Instance.crtConfig.special.Split(new char[] { '_' })[0] + ".ab", BattleModel.Instance.crtConfig.special);
		}
	}

	private void OnCloseClick(GameObject go)
	{
        BattleModel.Instance.ready_map = 0;
		GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
	}
}