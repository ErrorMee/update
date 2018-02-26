using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StarItem : MonoBehaviour
{
	public static bool ForceSelect = true;

	public config_chapter_item itemConfig;

	public Button btn;

	public Text nameText;
	public Text starText;
	public Text rewardText;

	public Image addImage;
	public Image haveImage;

	private bool chapterIsFull;

	void Awake()
	{
		FontUtil.SetAllFont(transform, FontUtil.FONT_DEFAULT);
	}

	public void OnClick()
	{
		if(chapterIsFull)
		{
			ChapterInfo chapter = MapModel.Instance.GetChapterInfo(itemConfig.id);

			if(chapter == null || chapter.reward == false)
			{
				MapModel.Instance.ChapterReward(itemConfig);
				haveImage.gameObject.SetActive(true);
			}else
			{
				MapModel.Instance.SwitchChapter(itemConfig);
				GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.STAR);
				ScreenSlider.OpenSlid();
			}
		}else
		{
			MapModel.Instance.SwitchChapter(itemConfig);
			GameMgr.moduleMgr.RemoveUIModule(ModuleEnum.STAR);
			ScreenSlider.OpenSlid();
		}
	}

	public void SetConfig(config_chapter_item config)
	{
		itemConfig = config;

		nameText.text = itemConfig.name;
		nameText.color = ColorMgr.GetColor(ColorMgr.COLOR_FISHSTAR);

		haveImage.gameObject.SetActive(false);

		addImage.gameObject.SetActive(false);

		List<int> mapIds = itemConfig.GetMapIds();

		if(mapIds.Count < 1)
		{
			starText.text = "?";
			rewardText.text = "?";
			nameText.text = "?";
			chapterIsFull = false;
			starText.color = Color.gray;
			rewardText.color = Color.gray;
		}else
		{
			bool hasOpen = false;
			int allStars = 0;
			int fullStar = 0;
			int i;
			for (i = 0; i < mapIds.Count; i++)
			{
				config_map_item config_map = (config_map_item)GameMgr.resourceMgr.config_map.GetItem(mapIds[i]);

				MapInfo mapInfo = MapModel.Instance.GetMapInfo(config_map.id);

				if(mapInfo != null)
				{
					hasOpen = true;
					allStars += mapInfo.star;
				}

				fullStar += 3;
			}
			if(hasOpen == false)
			{
				starText.color = Color.gray;
				rewardText.color = Color.gray;
			}
			starText.text = allStars + "/" + fullStar;
			rewardText.text = "+" + itemConfig.GetRewardList()[0].value;

			if(allStars < fullStar)
			{
				rewardText.color = Color.gray;
				chapterIsFull = false;
			}else
			{
				chapterIsFull = true;
			}

			ChapterInfo chapter = MapModel.Instance.GetChapterInfo(itemConfig.id);

			if(chapter != null && chapter.reward == true)
			{
				haveImage.gameObject.SetActive(true);
			}
		}
	}
}
