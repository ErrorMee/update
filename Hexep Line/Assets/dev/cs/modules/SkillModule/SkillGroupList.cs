
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillGroupList : MonoBehaviour 
{

	private GameObject groupItemPrefab;
    public BaseList list;

    public SkillGroupInfo skillTempletGroupInfo;

    void Awake()
    {
        groupItemPrefab = ResModel.Instance.GetPrefab("prefab/skillmodule/" + "SkillTemplet");
		list.itemPrefab = groupItemPrefab;
    }

    public void Init(SkillGroupInfo info)
    {
        skillTempletGroupInfo = info;

        gameObject.SetActive(false);

		list.ClearList ();
		int tindex = 0;
		for(int i = 0;i<info.skillTemplets.Count;i++)
		{
			SkillTempletInfo skillTempletInfo = info.skillTemplets[i];
			if(skillTempletInfo.config.type == 1)
			{
				CreatTempletItem(info,skillTempletInfo,tindex);
			}
		}
    }

	private void CreatTempletItem(SkillGroupInfo info,SkillTempletInfo skillTempletInfo,int index)
	{
		GameObject item = list.NewItem ();
        item.name = "skill_" + skillTempletInfo.config.cellId;

        PosUtil.SetCellPos(item.transform, 0, 0);

        SkillTemplet itemCtr = item.GetComponent<SkillTemplet>();
		itemCtr.Init(info,skillTempletInfo);
	}

	public void Hide(int dir = 1)
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
			LeanTween.move((RectTransform)gameObject.transform, new Vector3(-1000*dir, 0, 0), 0.2f).onComplete = OnHideComplete;
        }
    }

    private void OnHideComplete()
    {
        gameObject.SetActive(false);
    }

    public void Show(int dir = 1)
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);

			gameObject.transform.localPosition = new Vector3(1000*dir, 0, 0);

			LeanTween.move((RectTransform)gameObject.transform, new Vector3(0, 0, 0), 0.3f).onComplete = OnShowComplete;

			int i;
			int count = list.items.Count;
			for (i = 0; i < count; i++)
			{
				GameObject item = list.items[i];
				SkillTemplet itemCtr = item.GetComponent<SkillTemplet>();
				itemCtr.UpdateBtn();
			}

        }
    }

	private void OnShowComplete()
	{
		GuideModel.Instance.CheckGuide();
	}

}