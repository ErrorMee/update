using UnityEngine;
using System.Collections;

public class AndroidBackButton : MonoBehaviour {

    private bool canEsc = true;

	void Update () 
	{
        if (canEsc && (Input.GetKeyDown(KeyCode.Escape)))
        {
			GameObject map = GameObject.Find("Canvas/layer_0/MapModule");
			if(map != null)
			{
				canEsc = false;
				ConfirmInfo confirmInfo = new ConfirmInfo(LanguageUtil.GetTxt(11604),LanguageUtil.GetTxt(11603),ExitGame,NotExit,false);
				ConfirmModel.Instance.AddConfirm(confirmInfo);
			}else{

				GameObject fight = GameObject.Find("Canvas/layer_0/FightModule");
				if(fight != null)
				{
					GameObject pause = GameObject.Find("Canvas/layer_8/PauseModule");
					if(pause == null)
					{
						GameMgr.moduleMgr.AddUIModule(ModuleEnum.PAUSE);
					}
				}else{
					GameMgr.moduleMgr.AddUIModule(ModuleEnum.MAP);
				}
			}
        }
	}

	private void NotExit()
	{
		canEsc = true;
	}

	private void ExitGame()
	{
		canEsc = false;
		Application.Quit ();
	}
}
