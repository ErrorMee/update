using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WealthItem : MonoBehaviour {

    public Image icon;

    public Image bottle;

    public void Show(int iconId,bool showAnim = false,float size = 1)
    {
        icon.gameObject.SetActive(true);
        if (size != 1)
        {
            transform.localScale = new Vector3(size, size, 0);
        }

        if (showAnim)
        {
            Animation anim = icon.GetComponent<Animation>();
            if (anim != null)
            {
                anim.Play();
            }
        }

        if (iconId > 10105)
        {
            icon.rectTransform.sizeDelta = new Vector2(100,100);
            icon.overrideSprite = ResModel.Instance.GetSprite("icon/wealth/" + "wealth_" + iconId);
            bottle.gameObject.SetActive(false);
        } else
        {
            icon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + iconId);
            bottle.gameObject.SetActive(true);
            bottle.color = ColorUtil.GetColor(ColorUtil.GetCellColorValue(iconId));
        }
    }

}
