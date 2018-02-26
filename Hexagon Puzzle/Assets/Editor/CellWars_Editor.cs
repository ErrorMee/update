using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;

public class CellWars_Editor : Editor
{
    public const string editor_name = "CellWars";

    public const string config_cell_name = "config_cell";
    public const string config_module_name = "config_module";

    public static config_cell config_cell;
    public static config_module config_module;

    public static config_base crt_config;
    public static config_item_base crt_item;

    public static void LoadConfigs()
    {
        JsonWriter.SHIELD_CN = false;
        FileUtil.Instance().SetBasePath("Resources/config/");

        TextAsset textAsset = AssetDatabase.LoadAssetAtPath("Assets/Resources/config/config_cell.json", typeof(TextAsset)) as TextAsset;
        config_cell = JsonMapper.ToObject<config_cell>(textAsset.text);
        textAsset = AssetDatabase.LoadAssetAtPath("Assets/Resources/config/config_module.json", typeof(TextAsset)) as TextAsset;
        config_module = JsonMapper.ToObject<config_module>(textAsset.text);
	}
	
    public static void EditConfigItem(int id = 0)
    {
        if (id > 0)
        {
            crt_item = crt_config.GetItem(id);
        }
        else
        {
            switch (crt_config.name)
            {
            case CellWars_Editor.config_cell_name:
                crt_item = new config_cell_item();
                config_cell_item config_cell_item = (config_cell_item)crt_item;
                config_cell_item.Clear();
                config_cell_item.id = config_cell.data[config_cell.GetDataCount() - 1].id + 1;
                config_cell_item.hide_id = config_cell_item.id;
                config_cell_item.icon = config_cell_item.id;
                break;
            case CellWars_Editor.config_module_name:
                crt_item = new config_module_item();
                config_module_item config_module_item = (config_module_item)crt_item;
                config_module_item.Clear();
                config_module_item.id = config_module.data[config_module.GetDataCount() - 1].id + 1;
                break;
			}
		}
	}
	
	public static void DelCrtItem()
	{
		if (crt_item != null)
        {
            for (int i = 0; i < crt_config.GetDataCount(); i++)
            {
                config_item_base item = crt_config.GetItemAt(i);

                if (item.id == crt_item.id)
                {
                    switch (crt_config.name)
                    {
                    case CellWars_Editor.config_cell_name:
                        config_cell.data.RemoveAt(i);
                        break;
                    case CellWars_Editor.config_module_name:
                        config_module.data.RemoveAt(i);
                        break;
					}
					break;
				}
            }
        }
        FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(crt_config), true);
    }

    public static void SaveCrtItem(bool isDelete = false)
    {
        if(crt_item != null)
        {
            switch (crt_config.name)
            {
            case CellWars_Editor.config_cell_name:
                config_cell_item config_cell_item = (config_cell_item)config_cell.GetItem(crt_item.id);
                if (config_cell_item == null)
                {
                    config_cell.data.Add((config_cell_item)crt_item);
                }
                break;
            case CellWars_Editor.config_module_name:
                config_module_item config_module_item = (config_module_item)config_module.GetItem(crt_item.id);
                if (config_module_item == null)
                {
                    config_module.data.Add((config_module_item)crt_item);
                }
                break;
			}
		}
		
		switch (crt_config.name)
		{
		case CellWars_Editor.config_cell_name:
            config_cell.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_cell), true);
            break;
            
        case CellWars_Editor.config_module_name:
            config_module.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_module), true);
            break;
		}
	}
}
