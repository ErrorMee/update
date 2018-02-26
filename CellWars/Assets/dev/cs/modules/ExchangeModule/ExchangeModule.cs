using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class ExchangeModule : BaseModule
{
	public Transform closeTrans;
	public Transform closeBtn;

	public Transform okBtn;

	public Image buyCoinIcon;
    public Image buyEnergyIcon;
	public Text buyText;
	public Image costIcon;
	public Text costText;
    public Text titleLabel;
    public Text buyLabel;
    public Text costLabel;

	public Animation winOpenAnim;

    override protected void Awake()
    {
        base.Awake();
        titleLabel.text = LanguageUtil.GetTxt(11608);
        buyLabel.text = LanguageUtil.GetTxt(11608);
        costLabel.text = LanguageUtil.GetTxt(11609);
        okBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11605);
    }

	void Start()
	{
		winOpenAnim.Play();
		InitEvents();
		InitView();

		FontUtil.FixCN();
	}
	
	private void InitEvents()
	{
		EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClose;
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClose;
		EventTriggerListener.Get(okBtn.gameObject).onClick = OnOk;

        PlayerModel.Instance.updateWealthEvent += OnUpdateWealthEvent;
	}

    void OnDestroy()
    {
        PlayerModel.Instance.updateWealthEvent -= OnUpdateWealthEvent;
    }

    private void OnUpdateWealthEvent(int id)
    {
        if (id == (int)WealthTypeEnum.Gem)
        {
            InitView();
        }
    }
	
	private void InitView()
	{
		int buyCount = PlayerModel.Instance.exchangeInfo.count;

		buyText.text = "" + buyCount;

		config_wealth_item buyItem = GameMgr.resourceMgr.config_wealth.GetItemByType(PlayerModel.Instance.exchangeInfo.type);
        buyCoinIcon.gameObject.SetActive(false);
        buyEnergyIcon.gameObject.SetActive(false);
        switch (PlayerModel.Instance.exchangeInfo.type)
        {
            case (int)WealthTypeEnum.Coin:
                buyCoinIcon.gameObject.SetActive(true);
                break;
            case (int)WealthTypeEnum.Energy:
                buyEnergyIcon.gameObject.SetActive(true);
                break;
        }

		//buyIcon.overrideSprite = GameMgr.resourceMgr.GetSprite("dependencies.ab", "" + buyItem.icon);
		TIVInfo cost = buyItem.GetPriceList()[0];

        config_wealth_item costItem = (config_wealth_item)GameMgr.resourceMgr.config_wealth.GetItem((int)cost.id);
		//costIcon.overrideSprite = GameMgr.resourceMgr.GetSprite("dependencies.ab", "" + costItem.icon);
		int costCount = (int)Mathf.Ceil(buyCount * cost.value);

		costText.text = "" + costCount;

		WealthInfo costInfo = PlayerModel.Instance.GetWealth(costItem.id);

		if (costInfo.count >= costCount) 
		{
			costText.color = Color.green;
		} else {
			costText.color = Color.red;
		}
	}
	
	private void OnClose(GameObject go)
	{
		GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.EXCHANGE);
		if(PlayerModel.Instance.exchangeInfo.sucFun == null)
		{
			ScreenSlider.OpenSlid();
		}
	}

	private void OnOk(GameObject go)
	{

		config_wealth_item buyItem = GameMgr.resourceMgr.config_wealth.GetItemByType(PlayerModel.Instance.exchangeInfo.type);
		int buyCount = PlayerModel.Instance.exchangeInfo.count;
		WealthInfo buyInfo = PlayerModel.Instance.GetWealth(buyItem.id);

		TIVInfo cost = buyItem.GetPriceList()[0];

        config_wealth_item costItem = (config_wealth_item)GameMgr.resourceMgr.config_wealth.GetItem((int)cost.id);
		
		int costCount = (int)Mathf.Ceil(buyCount * cost.value);

		WealthInfo costInfo = PlayerModel.Instance.GetWealth(costItem.id);

		if (costInfo.count >= costCount) 
		{
			buyInfo.count += buyCount;
			costInfo.count -= costCount;
			PlayerModel.Instance.SaveWealths();

			if (PlayerModel.Instance.exchangeInfo.sucFun != null) {

				GameMgr.moduleMgr.RemoveUIModule (ModuleEnum.EXCHANGE);
				PlayerModel.Instance.exchangeInfo.sucFun ();
				PlayerModel.Instance.exchangeInfo.sucFun = null;

			} else {
				PromptModel.Instance.Pop(" +" + buyCount, true, PlayerModel.Instance.exchangeInfo.type);
			}

		} else {
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11901), false, (int)WealthTypeEnum.Gem);
			GameMgr.moduleMgr.AddUIModule(ModuleEnum.WEALTH);
		}
	}
}