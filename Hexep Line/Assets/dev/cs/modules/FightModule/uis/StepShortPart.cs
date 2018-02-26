using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StepShortPart : MonoBehaviour
{
	public Transform closeTrans;
	public Button closeBtn;

    public Text title;
    public Text content;

	public Button noBtn;

    public Button sureBtn;

    public Transform costTrans;
	public Text costText;
	
	public Text timesText;

    public Animation winOpenAnim;

	private PropInfo propInfo;

    void Awake()
    {
		FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
        title.text = LanguageUtil.GetTxt(11416);
        content.text = LanguageUtil.GetTxt(11417);
		sureBtn.transform.FindChild("Text").GetComponent<Text>().text = LanguageUtil.GetTxt(11407);
        noBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11418);
    }

    void OnEnable()
    {
        EventTriggerListener.Get(closeBtn.gameObject).onClick = OnCloseClick;
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnCloseClick;
		EventTriggerListener.Get(noBtn.gameObject).onClick = OnCloseClick;

        EventTriggerListener.Get(sureBtn.gameObject).onClick = OnSureClick;

		PlayerModel.Instance.updateWealthsEvent += UpdateView;

    }

    void OnDisable()
    {
		PlayerModel.Instance.updateWealthsEvent -= UpdateView;
    }

	private void UpdateView()
	{
		propInfo = PropModel.Instance.GetProp(10004);
		
		WealthInfo coin = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
		
		costText.text = "" + propInfo.config.GetCost(propInfo.countUsed + 1);
        if (propInfo.config.GetCost(propInfo.countUsed + 1) > coin.count)
		{
			costText.color = Color.red;
		}else
		{
			costText.color = Color.green;
		}
		
		timesText.text = propInfo.count + "/" + propInfo.config.times; 
		
		if(propInfo.count <= 0)
		{
			timesText.color = Color.red;
            costTrans.gameObject.SetActive(false);
		}else
		{
			timesText.color = Color.green;
            costTrans.gameObject.SetActive(true);
		}
	}

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        if (isShow)
        {
            winOpenAnim.Play();

			UpdateView();
        }
    }

    private void OnCloseClick(GameObject go)
    {
        Show(false);
        ModuleModel.Instance.AddUIModule((int)ModuleEnum.REPORT);
    }

    private void OnSureClick(GameObject go)
    {
		if(propInfo.count <= 0)
		{
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11405));
			return;
		}

		WealthInfo coin = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
        if (propInfo.config.GetCost(propInfo.countUsed + 1) > coin.count)
		{
            PlayerModel.Instance.ExchangeWealth((int)WealthTypeEnum.Coin, propInfo.config.GetCost(propInfo.countUsed + 1) - coin.count, SureBuyMoves);
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, (int)WealthTypeEnum.Coin);
            return;
		}
		SureBuyMoves ();
    }

	private void SureBuyMoves()
	{
		Show(false);
		
		PropModel.Instance.PropAddSetpEvent(3);
        PromptModel.Instance.Pop(LanguageUtil.GetTxt(11407));
		propInfo.Use ();
		FightModule.crtFightStadus = FightStadus.idle;
	}

}
