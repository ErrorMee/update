using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;

public class LoginModule : BaseModule
{
	public Button startBtn;

	public Button setBtn;

	void Start ()
	{
		PlayerModel.Instance.LoadWeaths();
		MapModel.Instance.LoadMaps();
		SkillTempletModel.Instance.LoadSkillTemplets();
		GuideModel.Instance.InitGuide();
		ADModel.Instance.LoadADData ();


		PosUtil.SetCellPos (setBtn.transform, 0, -2);

		EventTriggerListener.Get(startBtn.gameObject).onClick = OnClickStart;
		EventTriggerListener.Get(setBtn.gameObject).onClick = OnSetClick;
    }
		
	private void OnClickStart(GameObject go)
	{
		StartGame ();
	}

    private void StartGame()
    {
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.MAP);
    }

	private void OnSetClick(GameObject go)
	{
		ModuleModel.Instance.AddUIModule((int)ModuleEnum.SET);
	}
}
