using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
//using UnityEditor;

public class GridMain : MonoBehaviour {

	public static EditResourceMgr resourceMgr;

	public Transform newBotton;
	public Transform setBotton;
	public Transform saveButton;
	public Transform chapterButton;

	public ChapterEdit chapterEdit;

    public TogglePart togglePart;

    public EditPart editPart;

	public SetPart setPart;

	void Awake()
	{
		Application.targetFrameRate = 30;

		togglePart.gameObject.SetActive(false);
		editPart.gameObject.SetActive(false);
		setPart.gameObject.SetActive(false);

		EventTriggerListener.Get(newBotton.gameObject).onClick = OnNew;
		EventTriggerListener.Get (setBotton.gameObject).onClick = OnSet;
		EventTriggerListener.Get(saveButton.gameObject).onClick = OnSave;
	}

	private void OnNew(GameObject go)
	{
		config_map_item config_map_item = new config_map_item();

		config_map_item.id = resourceMgr.config_map.data[resourceMgr.config_map.GetDataCount() - 1].id + 1;
		config_map_item.icon = config_map_item.id;
		config_map_item.name = "" + (config_map_item.id % 10000);
		config_map_item.desc = "";
		config_map_item.pre_id = (config_map_item.id - 1);
		config_map_item.task = "10101|20,10102|20,10103|20,10104|20,10105|20";
		config_map_item.step = 32;

		config_map_item.build = 10000;
		config_map_item.fill = 10000;
		config_map_item.judge = "1800,3500,5200";
        config_map_item.forbid_skill = "";

		resourceMgr.config_map.data.Add (config_map_item);

		BattleInfo battleInfo = new BattleInfo();
		battleInfo.mapId = config_map_item.id;
        battleInfo.FillNew(GridModel.Instance.set_start_x, GridModel.Instance.set_start_y, GridModel.Instance.set_end_x, GridModel.Instance.set_end_y, GridModel.Instance.set_battle_width, GridModel.Instance.set_battle_height);
        BattleModel.Instance.battles.Add(battleInfo);

		ToggleList toggleList = togglePart.UpDateList (FightLayerType.map);
		GameObject toggleItem = toggleList.list.GetItemByName("item_" + config_map_item.id);
		Toggle toggle = toggleItem.GetComponent<Toggle> ();
		toggle.isOn = true;
	}

	private void OnSet(GameObject go)
	{
        if (BattleModel.Instance.crtBattle != null)
		{
			setPart.gameObject.SetActive(true);
			setPart.GetComponent<SetPart> ().UpdateMap ();
		}
	}

	private void OnSave(GameObject go)
	{
        if (BattleModel.Instance.crtBattle == null)
		{
			Debug.Log("crtBattle == null");
			return;
		}

		resourceMgr.config_map.data.Sort();
		FileUtil.Instance().WriteFile("config/" + resourceMgr.config_map.name, JsonMapper.ToJson(resourceMgr.config_map), true);

        BattleModel.Instance.crtBattle.SaveBattleInfo();

		//AssetDatabase.Refresh();
	}

    void Start()
    {
		JsonWriter.SHIELD_CN = false;
        FileUtil.Instance().SetBasePath("sourceArt/");
        resourceMgr.EventHandler += OnEventHandler;
    }

    private void OnEventHandler(ResourceEventType eventType, string addData)
    {
        switch (eventType)
        {
            case ResourceEventType.load_complete:
                switch (addData)
                {
                    case ResourceStatic.COMMON:
                        togglePart.gameObject.SetActive(true);
                        editPart.gameObject.SetActive(true);
                        break;
                }
                break;
        }
    }

	public void OnChapterSelect(bool isSelect)
	{
		chapterEdit.gameObject.SetActive(isSelect);
	}
}
