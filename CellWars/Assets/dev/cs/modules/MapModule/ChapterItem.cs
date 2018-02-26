using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChapterItem : MonoBehaviour
{
    public static bool ForceSelect = true;

    public config_chapter_item itemConfig;

    public Text nameText;

    public Toggle toggle;

    public Image iconImage;

    public int viewIndex;

    void Awake()
    {
        FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
    }

    public void SetConfig(config_chapter_item config)
    {
        itemConfig = config;

		nameText.text = "" + int.Parse(config.desc)/10;

        //iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/" + config.special.Split(new char[] { '_' })[0] + ".ab", config.special);
    }

    public void OnSelect(bool isSelect)
    {
        if (isSelect)
        {
            if (ForceSelect || MapModel.Instance.selectChapter != itemConfig)
            {
                ForceSelect = false;
				MapModel.Instance.SelectChapter(itemConfig);
            }
			nameText.color = ColorMgr.GetColor(ColorMgr.COLOR_FISHSTAR);
        }
		else
		{
			nameText.color = Color.white;
		}
    }
}
