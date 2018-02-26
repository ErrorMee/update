using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPModel : Singleton<IAPModel>,IStoreListener
{
    private IStoreController m_Controller;
    private IAppleExtensions m_AppleExtensions;
    private bool m_PurchaseInProgress;

    public IAPModel()
    {
    }

    public void Init()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        config_wealth_item config = ResModel.Instance.config_wealth.GetItemByType((int)WealthTypeEnum.Money);

        string[] prices = config.price.Split(new char[] { ',' });

        for (int i = 0; i < prices.Length; i++)
        {
            string price = prices[i];
            string[] price2s = price.Split(new char[] { '|' });
            builder.AddProduct(price2s[0], ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();

        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

        int langInited = PlayerPrefsUtil.GetInt(PlayerPrefsUtil.LANGUAGE_INITED);

        Debug.Log("Available items:");
        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
					{
						item.metadata.localizedTitle,
						item.metadata.localizedDescription,
						item.metadata.isoCurrencyCode,
						item.metadata.localizedPrice.ToString(),
						item.metadata.localizedPriceString,
						item.transactionID,
						item.receipt
					}));
                if (langInited == 0)
                {
                    langInited = 1;
                    PlayerPrefsUtil.SetInt(PlayerPrefsUtil.LANGUAGE_INITED, 1);
                    if (item.metadata.isoCurrencyCode.Contains("CNY"))
                    {
                        LanguageUtil.SelecteLanguage(LanguageUtil.FONT_CN);
                    }
                    else
                    {
                        LanguageUtil.SelecteLanguage(LanguageUtil.LANG_EN);
                    }
                }
            }
        }
    }

    public Product[] GetIAPProducts()
    {
        if (m_Controller == null)
        {
            return null;
        }else
        {
            return m_Controller.products.all;
        }
    }

    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    /// 
    /// When the purchase is approved or rejected, the normal purchase events
    /// will fire.
    /// </summary>
    /// <param name="item">Item.</param>
    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }

    public void BuyProduct(int productIndex = -1)
    {
        if (productIndex < 0)
        {
            Debug.Log("Invalid item");
            return;
        }

        if (m_PurchaseInProgress)
        {
            PromptModel.Instance.Pop(LanguageUtil.GetTxt(11907));
            return;
        }
        m_PurchaseInProgress = true;
        m_Controller.InitiatePurchase(m_Controller.products.all[productIndex]);
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
    {
        Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
        Debug.Log("Receipt: " + e.purchasedProduct.receipt);

        m_PurchaseInProgress = false;

        int winGem = 0;
        config_wealth_item config = ResModel.Instance.config_wealth.GetItemByType((int)WealthTypeEnum.Money);

        string[] prices = config.price.Split(new char[] { ',' });

        for (int i = 0; i < prices.Length; i++)
        {
            string price = prices[i];

            string[] price2s = price.Split(new char[] { '|' });

            if (price2s[0] == e.purchasedProduct.definition.id)
            {
                winGem = int.Parse(price2s[2]);
                break;
            }
        }

        WealthInfo gemInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Gem);
        PromptModel.Instance.Pop("+" + winGem, true, gemInfo.type);

        gemInfo.count += winGem;

        PlayerModel.Instance.SaveWealths(gemInfo.type);

		config_sort_item diamond_item = ResModel.Instance.config_sort.GetItemByTypeAndSpecial(1, "11101");
		SocialModel.Instance.ReportScore(diamond_item.gid, gemInfo.count);

        string prefsKey = PlayerPrefsUtil.DIAMOND_BUY + e.purchasedProduct.definition.id;
        PlayerPrefsUtil.SetInt(prefsKey, PlayerPrefsUtil.GetInt(prefsKey) + 1);
        
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
    {
        Debug.Log("OnPurchaseFailed");
        m_PurchaseInProgress = false;
    }
}