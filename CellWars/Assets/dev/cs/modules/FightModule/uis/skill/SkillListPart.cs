using UnityEngine;
using System.Collections;

public class SkillListPart : MonoBehaviour {

    private GameObject itemPrefab;
    private ResourceMgr resourceMgr;
    public BaseList list;

    void Awake()
    {
        resourceMgr = GameObject.Find("GameMgr").GetComponent<ResourceMgr>();
        itemPrefab = resourceMgr.GetGameObject("prefab/fightmodule.ab", "SkillItem");
    }

    void Start()
    {
        list.itemPrefab = itemPrefab;
		gameObject.SetActive(false);
    }

    public void InitSeeds()
    {
        list.ClearList();
        bool hasOpen = false;
        int i;
        for (i = 0; i < SkillModel.Instance.seeds.Count; i++)
		{
            SkillSeedInfo seed = SkillModel.Instance.seeds[i];
            if (seed.fobid == false)
            {
                hasOpen = true;
            }
		}

        if (hasOpen)
        {
            for (i = 0; i < SkillModel.Instance.seeds.Count; i++)
            {
                SkillSeedInfo seed = SkillModel.Instance.seeds[i];
                CreateItem(seed);
            }
        }
    }

    public void ShowSkill()
    {
        for (int i = 0; i < SkillModel.Instance.seeds.Count; i++)
        {
            SkillSeedInfo seed = SkillModel.Instance.seeds[i];
            if (seed.fobid == false)
            {
                ShowSkillProgress(seed.runId, seed.progress, seed.progressTemp);
            }
        }
    }

	public void CollectSkill(bool isTemp, bool isDeductStep)
    {
		if(isDeductStep)
		{
			return;
		}
        SkillModel.Instance.SeedCollect(isTemp);

        ShowSkill();
    }

    private void ShowSkillProgress(int id, float progress, float progressTemp)
    {
        Transform itemTrs = transform.FindChild("skill_" + id);
        SkillItem itemCtr = itemTrs.GetComponent<SkillItem>();
		itemCtr.progress = progress;
        itemCtr.progressTemp = progressTemp;
    }

    private void CreateItem(SkillSeedInfo seed)
    {
        GameObject item = list.NewItem();
        item.name = "skill_" + seed.runId;
        SkillItem itemCtr = item.GetComponent<SkillItem>();

		itemCtr.icon = seed.GetCellIcon();
		itemCtr.SetHideId(seed.GetHideCellId());
		itemCtr.ShowLock (seed.fobid);
		PosMgr.SetCellPos(item.transform,seed.seed_x, seed.seed_y,1.0f);
    }
}
