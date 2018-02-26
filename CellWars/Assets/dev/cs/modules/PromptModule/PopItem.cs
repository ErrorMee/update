using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PopItem : MonoBehaviour
{
    public Image popBg;
    public Text popText;

    void Awake()
    {
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    public void SetText(string str)
    {
        popText.text = str;
        popBg.rectTransform.sizeDelta = new Vector2(popText.preferredWidth + 70, popBg.rectTransform.sizeDelta.y + 10);
    }

    public void SetWealthIcon(int id, bool showAnim = false)
    {
        config_wealth_item config_item = (config_wealth_item)GameMgr.resourceMgr.config_wealth.GetItem(id);
        if (config_item != null)
        {
            GameObject wealthPrefab = GameMgr.resourceMgr.GetGameObject("prefab/base.ab", "WealthItem");
            GameObject wealthItem = GameObject.Instantiate(wealthPrefab);
            wealthItem.transform.SetParent(popBg.transform, false);
            
            WealthItem wealthItemCtr = wealthItem.GetComponent<WealthItem>();
            
            if (config_item.icon > 10105)
            {
                wealthItemCtr.Show(config_item.icon, false);
				popText.rectTransform.localPosition = new Vector3(popText.rectTransform.localPosition.x + 50 ,0 ,0);
				popBg.rectTransform.sizeDelta = new Vector2(popText.preferredWidth + 160, popBg.rectTransform.sizeDelta.y + 30);
				wealthItem.transform.localPosition = new Vector3(-popText.preferredWidth / 2 - 10, 0, 0);
            }
            else
            {
                wealthItemCtr.Show(config_item.icon, false, 0.5f);
				popText.rectTransform.localPosition = new Vector3(popText.rectTransform.localPosition.x + 50 ,-10 ,0);
				popBg.rectTransform.sizeDelta = new Vector2(popText.preferredWidth + 170, popBg.rectTransform.sizeDelta.y + 50);
				wealthItem.transform.localPosition = new Vector3(-popText.preferredWidth / 2 - 10, -10, 0);
            }
        }
    }

    public void MoveUp()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y + 120, 0);
    }

    public void OnPopEnd()
    {
        DestroyObject(gameObject);
    }
}
