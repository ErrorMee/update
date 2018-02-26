using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class WealthModule : BaseModule
{
	public Transform closeTrans;
	public Button closeBtn;

	public Transform adBtn;

	public Text title;
    public Text productLoading;
    
    public Transform rechargeNo;
    public Transform rechargeA;
    public Transform rechargeB;
    public Transform rechargeC;

    private int productAItemIndex;
    private int productBItemIndex;
    private int productCItemIndex;

	override protected void Awake()
	{
		base.Awake();
		title.text = LanguageUtil.GetTxt(11906);
        productLoading.text = LanguageUtil.GetTxt(11907);
	}

    void Start()
    {
        InitEvents();
        InitView();
    }

    private void InitEvents()
    {
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClickClose;
        EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClickClose;
        EventTriggerListener.Get(rechargeA.gameObject).onClick = OnRechargeA;
        EventTriggerListener.Get(rechargeB.gameObject).onClick = OnRechargeB;
        EventTriggerListener.Get(rechargeC.gameObject).onClick = OnRechargeC;

		EventTriggerListener.Get(adBtn.gameObject).onClick = OnADClick;
    }

	private void OnADClick(GameObject go)
	{
		GameMgr.moduleMgr.AddUIModule(ModuleEnum.ADREWARD);
	}

    private void InitView()
    {

        rechargeA.gameObject.SetActive(false);
        rechargeB.gameObject.SetActive(false);
        rechargeC.gameObject.SetActive(false);
        rechargeNo.gameObject.SetActive(false);

        Product[] products = IAPModel.Instance.GetIAPProducts();
        if (products == null || products.Length < 1)
        {
            rechargeNo.gameObject.SetActive(true);
        }else
        {
            int findIndex = 0;
            for (int i = 0; i < products.Length;i++ )
            {
                Product product = products[i];
                if (product.availableToPurchase)
                {
                    if (findIndex < 3)
                    {
                        string getStr = "";
                        config_wealth_item config = GameMgr.resourceMgr.config_wealth.GetItemByType((int)WealthTypeEnum.Money);
                        string[] prices = config.price.Split(new char[] { ',' });

                        for (int j = 0; j < prices.Length; j++)
                        {
                            string price = prices[j];

                            string[] price2s = price.Split(new char[] { '|' });

                            if (price2s[0] == product.definition.id)
                            {
                                getStr = price2s[2];
                                break;
                            }
                        }

                        switch (findIndex)
                        {
                            case 0:
                                rechargeA.gameObject.SetActive(true);
                                productAItemIndex = i;
                                
                                rechargeA.FindChild("cost").GetComponent<Text>().text = product.metadata.localizedPriceString;
                                rechargeA.FindChild("get").GetComponent<Text>().text = getStr;

                                break;
                            case 1:
                                rechargeB.gameObject.SetActive(true);
                                productBItemIndex = i;

                                rechargeB.FindChild("cost").GetComponent<Text>().text = product.metadata.localizedPriceString;
                                rechargeB.FindChild("get").GetComponent<Text>().text = getStr;
                                break;
                            case 2:
                                rechargeC.gameObject.SetActive(true);
                                productCItemIndex = i;

                                rechargeC.FindChild("cost").GetComponent<Text>().text = product.metadata.localizedPriceString;
                                rechargeC.FindChild("get").GetComponent<Text>().text = getStr;

                                break;
                        }
                        findIndex++;
                    }
                }
            }
        }
    }

    private void OnRechargeA(GameObject go)
    {
        IAPModel.Instance.BuyProduct(productAItemIndex);
    }

    private void OnRechargeB(GameObject go)
    {
        IAPModel.Instance.BuyProduct(productBItemIndex);
    }

    private void OnRechargeC(GameObject go)
    {
        IAPModel.Instance.BuyProduct(productCItemIndex);
    }

    private void OnClickClose(GameObject go)
    {
        GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.WEALTH);
		ScreenSlider.OpenSlid ();
    }
}