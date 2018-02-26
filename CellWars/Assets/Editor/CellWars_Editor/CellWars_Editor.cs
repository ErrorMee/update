using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;

public class CellWars_Editor : Editor
{
    public const string editor_name = "CellWars";

    public const string config_cell_name = "config_cell";
    public const string config_wall_name = "config_wall";
    public const string config_map_name = "config_map";
    public const string config_cover_name = "config_cover";
	public const string config_monster_name = "config_monster";
    public const string config_prop_name = "config_prop";
    public const string config_module_name = "config_module";
	public const string config_guild_name = "config_guild";
    public const string config_chapter_name = "config_chapter";
    public const string config_wealth_name = "config_wealth";
    public const string config_skill_name = "config_skill";
    public const string config_sort_name = "config_sort";

    public static config_cell config_cell;
    public static config_wall config_wall;
    public static config_map config_map;
    public static config_cover config_cover;
    public static config_monster config_monster;
    public static config_prop config_prop;
    public static config_module config_module;
	public static config_guild config_guild;
    public static config_chapter config_chapter;
    public static config_wealth config_wealth;
    public static config_skill config_skill;
    public static config_sort config_sort;

    public static config_base crt_config;
    public static config_item_base crt_item;

    public static void LoadConfigs()
    {
        JsonWriter.SHIELD_CN = false;
        FileUtil.Instance().SetBasePath("sourceArt/config/");

        TextAsset textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_cell.json", typeof(TextAsset)) as TextAsset;
        config_cell = JsonMapper.ToObject<config_cell>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_wall.json", typeof(TextAsset)) as TextAsset;
        config_wall = JsonMapper.ToObject<config_wall>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_map.json", typeof(TextAsset)) as TextAsset;
        config_map = JsonMapper.ToObject<config_map>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_cover.json", typeof(TextAsset)) as TextAsset;
        config_cover = JsonMapper.ToObject<config_cover>(textAsset.text);

		textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_monster.json", typeof(TextAsset)) as TextAsset;
		config_monster = JsonMapper.ToObject<config_monster>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_prop.json", typeof(TextAsset)) as TextAsset;
        config_prop = JsonMapper.ToObject<config_prop>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_module.json", typeof(TextAsset)) as TextAsset;
        config_module = JsonMapper.ToObject<config_module>(textAsset.text);

		textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_guild.json", typeof(TextAsset)) as TextAsset;
		config_guild = JsonMapper.ToObject<config_guild>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_chapter.json", typeof(TextAsset)) as TextAsset;
        config_chapter = JsonMapper.ToObject<config_chapter>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_wealth.json", typeof(TextAsset)) as TextAsset;
        config_wealth = JsonMapper.ToObject<config_wealth>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_skill.json", typeof(TextAsset)) as TextAsset;
        config_skill = JsonMapper.ToObject<config_skill>(textAsset.text);

        textAsset = AssetDatabase.LoadAssetAtPath("Assets/sourceArt/config/config_sort.json", typeof(TextAsset)) as TextAsset;
        config_sort = JsonMapper.ToObject<config_sort>(textAsset.text);
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
                case CellWars_Editor.config_wall_name:
                    crt_item = new config_wall_item();
                    config_wall_item config_wall_item = (config_wall_item)crt_item;
                    config_wall_item.Clear();
                    config_wall_item.id = config_wall.data[config_wall.GetDataCount() - 1].id + 1;
                    break;
                case CellWars_Editor.config_map_name:
                    crt_item = new config_map_item();
                    config_map_item config_map_item = (config_map_item)crt_item;
                    config_map_item.Clear();
                    config_map_item.id = config_map.data[config_map.GetDataCount() - 1].id + 1;
                    config_map_item.name = "" + (config_map_item.id % 10000);
                    config_map_item.icon = config_map_item.id;
                    config_map_item.step = 20;
                    config_map_item.pre_id = (config_map_item.id - 1);
                    config_map_item.task = "10101|20,10102|21,10103|22,10104|23,10105|24";
                    config_map_item.build = 10000;
                    config_map_item.fill = 10000;
                    config_map_item.judge = "1000,2000,3000";
                    config_map_item.forbid_skill = "10106,10112,10108,10114,10110";
                    config_map_item.forbid_prop = "";
                    break;
                case CellWars_Editor.config_cover_name:
                    crt_item = new config_cover_item();
                    config_cover_item config_cover_item = (config_cover_item)crt_item;
                    config_cover_item.Clear();
                    config_cover_item.id = config_cover.data[config_cover.GetDataCount() - 1].id + 1;
                    break;
			case CellWars_Editor.config_monster_name:
				crt_item = new config_monster_item();
				config_monster_item config_monster_item = (config_monster_item)crt_item;
				config_monster_item.Clear();
				config_monster_item.id = config_monster.data[config_monster.GetDataCount() - 1].id + 1;
				break;
            case CellWars_Editor.config_prop_name:
                crt_item = new config_prop_item();
                config_prop_item config_prop_item = (config_prop_item)crt_item;
                config_prop_item.Clear();
                config_prop_item.id = config_prop.data[config_prop.GetDataCount() - 1].id + 1;
                break;
            case CellWars_Editor.config_module_name:
                crt_item = new config_module_item();
                config_module_item config_module_item = (config_module_item)crt_item;
                config_module_item.Clear();
                config_module_item.id = config_module.data[config_module.GetDataCount() - 1].id + 1;
                break;
			case CellWars_Editor.config_guild_name:
				crt_item = new config_guild_item();
				config_guild_item config_guild_item = (config_guild_item)crt_item;
				config_guild_item.Clear();
				config_guild_item.id = config_guild.data[config_guild.GetDataCount() - 1].id + 1;
				break;
            case CellWars_Editor.config_chapter_name:
                crt_item = new config_chapter_item();
                config_chapter_item config_chapter_item = (config_chapter_item)crt_item;
                config_chapter_item.Clear();
                config_chapter_item.id = config_chapter.data[config_chapter.GetDataCount() - 1].id + 1;
                break;
            case CellWars_Editor.config_wealth_name:
                crt_item = new config_wealth_item();
                config_wealth_item config_wealth_item = (config_wealth_item)crt_item;
                config_wealth_item.Clear();
                config_wealth_item.id = config_wealth.data[config_wealth.GetDataCount() - 1].id + 1;
                break;
            case CellWars_Editor.config_skill_name:
                crt_item = new config_skill_item();
                config_skill_item config_skill_item = (config_skill_item)crt_item;
                config_skill_item.Clear();
                config_skill_item.id = config_skill.data[config_skill.GetDataCount() - 1].id + 1;
                break;
            case CellWars_Editor.config_sort_name:
                crt_item = new config_sort_item();
                config_sort_item config_sort_item = (config_sort_item)crt_item;
                config_sort_item.Clear();
                config_sort_item.id = config_sort.data[config_sort.GetDataCount() - 1].id + 1;
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
                        case CellWars_Editor.config_wall_name:
                            config_wall.data.RemoveAt(i);
                            break;
                        case CellWars_Editor.config_map_name:
                            config_map.data.RemoveAt(i);
                            break;
                        case CellWars_Editor.config_cover_name:
                            config_cover.data.RemoveAt(i);
                            break;
					case CellWars_Editor.config_monster_name:
						config_monster.data.RemoveAt(i);
						break;
                    case CellWars_Editor.config_prop_name:
                        config_prop.data.RemoveAt(i);
                        break;
                    case CellWars_Editor.config_module_name:
                        config_module.data.RemoveAt(i);
                        break;
					case CellWars_Editor.config_guild_name:
						config_guild.data.RemoveAt(i);
                        break;
                    case CellWars_Editor.config_chapter_name:
						config_chapter.data.RemoveAt(i);
						break;
                    case CellWars_Editor.config_wealth_name:
                        config_wealth.data.RemoveAt(i);
						break;
                    case CellWars_Editor.config_skill_name:
                        config_skill.data.RemoveAt(i);
                        break;
                    case CellWars_Editor.config_sort_name:
                        config_sort.data.RemoveAt(i);
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
                case CellWars_Editor.config_wall_name:
                    config_wall_item config_wall_item = (config_wall_item)config_wall.GetItem(crt_item.id);
                    if (config_wall_item == null)
                    {
                        config_wall.data.Add((config_wall_item)crt_item);
                    }
                    break;
                case CellWars_Editor.config_map_name:
                    config_map_item config_map_item = (config_map_item)config_map.GetItem(crt_item.id);
                    if (config_map_item == null)
                    {
                        config_map.data.Add((config_map_item)crt_item);
                    }

//				int cellCount = config_cell.data.Count;
//				int coverCount = config_cover.data.Count;
//				int monsterCount = config_monster.data.Count;
//				int allCount = cellCount + coverCount + monsterCount;
//
//				int dataCount = config_map.GetDataCount();
//				for (int i = 0; i < dataCount; i++)
//				{
//					config_map_item = (config_map_item)config_map.GetItemAt(i);
//					if(config_map_item.id > 20010)
//					{
//						config_map_item.special = "";
//						int randomIndex = Random.Range(0,allCount);
//						if(randomIndex < cellCount)
//						{
//							config_map_item.special = "cell_" + config_cell.GetItemAt(randomIndex).icon;
//						}else if(randomIndex < (cellCount + coverCount))
//						{
//							config_map_item.special = "cover_" + config_cover.GetItemAt(randomIndex - cellCount).icon;
//						}else
//						{
//							config_map_item.special = "monster_" + config_monster.GetItemAt(randomIndex - cellCount - coverCount).icon;
//						}
//					}
//				}

                    break;
                case CellWars_Editor.config_cover_name:
                    config_cover_item config_cover_item = (config_cover_item)config_cover.GetItem(crt_item.id);
                    if (config_cover_item == null)
                    {
                        config_cover.data.Add((config_cover_item)crt_item);
                    }
                    break;
			case CellWars_Editor.config_monster_name:
				config_monster_item config_monster_item = (config_monster_item)config_monster.GetItem(crt_item.id);
				if (config_monster_item == null)
				{
					config_monster.data.Add((config_monster_item)crt_item);
				}
				break;

            case CellWars_Editor.config_prop_name:
                config_prop_item config_prop_item = (config_prop_item)config_prop.GetItem(crt_item.id);
                if (config_prop_item == null)
                {
                    config_prop.data.Add((config_prop_item)crt_item);
                }
                break;
            case CellWars_Editor.config_module_name:
                config_module_item config_module_item = (config_module_item)config_module.GetItem(crt_item.id);
                if (config_module_item == null)
                {
                    config_module.data.Add((config_module_item)crt_item);
                }
                break;
			case CellWars_Editor.config_guild_name:
				config_guild_item config_guild_item = (config_guild_item)config_guild.GetItem(crt_item.id);
				if (config_guild_item == null)
				{
					config_guild.data.Add((config_guild_item)crt_item);
				}
				break;
            case CellWars_Editor.config_chapter_name:
                config_chapter_item config_chapter_item = (config_chapter_item)config_chapter.GetItem(crt_item.id);
                if (config_chapter_item == null)
				{
                    config_chapter.data.Add((config_chapter_item)crt_item);
				}
				break;
            case CellWars_Editor.config_wealth_name:
                config_wealth_item config_wealth_item = (config_wealth_item)config_wealth.GetItem(crt_item.id);
                if (config_wealth_item == null)
				{
                    config_wealth.data.Add((config_wealth_item)crt_item);
				}
				break;
            case CellWars_Editor.config_skill_name:
                config_skill_item config_skill_item = (config_skill_item)config_skill.GetItem(crt_item.id);
                if (config_skill_item == null)
                {
                    config_skill.data.Add((config_skill_item)crt_item);
                }
                break;
            case CellWars_Editor.config_sort_name:
                config_sort_item config_sort_item = (config_sort_item)config_sort.GetItem(crt_item.id);
                if (config_sort_item == null)
                {
                    config_sort.data.Add((config_sort_item)crt_item);
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
            case CellWars_Editor.config_wall_name:
                config_wall.data.Sort();
                FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_wall), true);
                break;
            case CellWars_Editor.config_map_name:
                config_map.data.Sort();
                FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_map), true);
                break;
            case CellWars_Editor.config_cover_name:
                config_cover.data.Sort();
                FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_cover), true);
                break;
		case CellWars_Editor.config_monster_name:
			config_monster.data.Sort();
			FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_monster), true);
			break;
        case CellWars_Editor.config_prop_name:
            config_prop.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_prop), true);
            break;
        case CellWars_Editor.config_module_name:
            config_module.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_module), true);
            break;
		case CellWars_Editor.config_guild_name:
			config_guild.data.Sort();
			FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_guild), true);
			break;
        case CellWars_Editor.config_chapter_name:
            config_chapter.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_chapter), true);
			break;
        case CellWars_Editor.config_wealth_name:
            config_wealth.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_wealth), true);
			break;
        case CellWars_Editor.config_skill_name:
            config_skill.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_skill), true);
            break;
        case CellWars_Editor.config_sort_name:
            config_sort.data.Sort();
            FileUtil.Instance().WriteFile(crt_config.name, JsonMapper.ToJson(config_sort), true);
            break; 
		}
	}
}
