using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using LitJson;
using System.IO;

class ConfigSet : EditorWindow
{

    public static ConfigSet instance;
    public static void InstanceShow(int id = 0)
    {
        CellWars_Editor.EditConfigItem(id);

        if (instance != null)
        {
            instance.titleContent.text = " " + CellWars_Editor.crt_item.id;
            //instance.title = " " + CellWars_Editor.crt_item.id;
            instance.Focus();
            return;
        }
        ConfigSet wnd = (ConfigSet)EditorWindow.GetWindow(typeof(ConfigSet));
        instance = wnd;
        instance.titleContent.text = " " + CellWars_Editor.crt_item.id;
        //instance.title = " " + CellWars_Editor.crt_item.id;
        instance.Show();
        instance.Focus();
        instance.minSize = new Vector2(460, 400);
        instance.maxSize = instance.minSize + new Vector2(10, 10);
    }

    private void OnGUI()
    {
        switch (CellWars_Editor.crt_config.name)
        {
            case CellWars_Editor.config_cell_name:
                ShowCell();
                break;
            case CellWars_Editor.config_wall_name:
                ShowWall();
                break;
            case CellWars_Editor.config_map_name:
                ShowMap();
                break;
            case CellWars_Editor.config_cover_name:
                ShowCover();
                break;
		case CellWars_Editor.config_monster_name:
			ShowMonster();
			break;
        case CellWars_Editor.config_prop_name:
            ShowProp();
            break;
        case CellWars_Editor.config_module_name:
            ShowModule();
            break;
		case CellWars_Editor.config_guild_name:
			ShowGuild();
			break;
        case CellWars_Editor.config_chapter_name:
            ShowChapter();
            break;
        case CellWars_Editor.config_wealth_name:
            ShowWealth();
            break;
        case CellWars_Editor.config_skill_name:
            ShowSkill();
            break;
        case CellWars_Editor.config_sort_name:
            ShowSort();
            break;
        }
        BottomTools();
    }

    private void BottomTools()
    {
        EditorGUILayout.LabelField(" ");
        if (GUILayout.Button("delete"))
        {
            Close();
            CellWars_Editor.DelCrtItem();
            CellWars_Editor.crt_item = null;
            ConfigWnd.InstanceShow();
        }

        EditorGUILayout.LabelField(" ");
        if (GUILayout.Button("close"))
        {
            Close();
            CellWars_Editor.crt_item = null;
            ConfigWnd.InstanceShow();
        }
    }

    private void ShowWall()
    {
        ShowBase(20000, 29999);

        config_wall_item config_wall_item = (config_wall_item)CellWars_Editor.crt_item;

		config_wall_item.hide_id = (int)EditorGUILayout.Slider("hide_id " + CellWars_Editor.config_wall.template.hide_id, config_wall_item.hide_id, 20000, 29999);
        config_wall_item.life = (int)EditorGUILayout.Slider("life " + CellWars_Editor.config_wall.template.life, config_wall_item.life, -1, 10);
        config_wall_item.line = (bool)EditorGUILayout.Toggle("line" + CellWars_Editor.config_wall.template.line, config_wall_item.line);
    }

    private void ShowCell()
    {
        ShowBase(10000, 19999);

        config_cell_item config_cell_item = (config_cell_item)CellWars_Editor.crt_item;

        config_cell_item.hide_id = (int)EditorGUILayout.Slider("hide_id " + CellWars_Editor.config_cell.template.hide_id, config_cell_item.hide_id, 10000, 19999);
        config_cell_item.cell_type = (int)EditorGUILayout.Slider("cell_type " + CellWars_Editor.config_cell.template.cell_type, config_cell_item.cell_type, 0, 20);
        config_cell_item.line_type = (int)EditorGUILayout.Slider("line_type " + CellWars_Editor.config_cell.template.line_type, config_cell_item.line_type, -1, 5);

        config_cell_item.life = (int)EditorGUILayout.Slider("life " + CellWars_Editor.config_cell.template.life, config_cell_item.life, -2, 3);
        config_cell_item.move = EditorGUILayout.Toggle("move " + CellWars_Editor.config_cell.template.move, config_cell_item.move);
        config_cell_item.atk = EditorGUILayout.Toggle("atk " + CellWars_Editor.config_cell.template.atk, config_cell_item.atk);
        config_cell_item.rotate = (int)EditorGUILayout.Slider("rotate " + CellWars_Editor.config_cell.template.rotate, config_cell_item.rotate, 0, 5);
    }

    private void ShowMap()
    {
        ShowBase(10000, 29999);

        config_map_item config_map_item = (config_map_item)CellWars_Editor.crt_item;

        config_map_item.step = (int)EditorGUILayout.Slider("step " + CellWars_Editor.config_map.template.step, config_map_item.step, 1, 200);
        config_map_item.pre_id = (int)EditorGUILayout.Slider("pre_id " + CellWars_Editor.config_map.template.pre_id, config_map_item.pre_id, 10000, 29999);
        config_map_item.task = EditorGUILayout.TextField("task " + CellWars_Editor.config_map.template.task, config_map_item.task);
        config_map_item.build = (int)EditorGUILayout.Slider("build " + CellWars_Editor.config_map.template.build, config_map_item.build, 10000, 29999);
        config_map_item.fill = (int)EditorGUILayout.Slider("fill " + CellWars_Editor.config_map.template.fill, config_map_item.fill, 10000, 29999);
        config_map_item.judge = EditorGUILayout.TextField("judge " + CellWars_Editor.config_map.template.judge, config_map_item.judge);
        config_map_item.forbid_skill = EditorGUILayout.TextField("forbid_skill " + CellWars_Editor.config_map.template.forbid_skill, config_map_item.forbid_skill);
        config_map_item.forbid_prop = EditorGUILayout.TextField("forbid_prop " + CellWars_Editor.config_map.template.forbid_prop, config_map_item.forbid_prop);
		config_map_item.starLimit = EditorGUILayout.TextField("starLimit ", config_map_item.starLimit);
    }

    private void ShowCover()
    {
        ShowBase(30000, 39999);

        config_cover_item config_cover_item = (config_cover_item)CellWars_Editor.crt_item;

        config_cover_item.hide_id = (int)EditorGUILayout.Slider("hide_id " + CellWars_Editor.config_cover.template.hide_id, config_cover_item.hide_id, 30000, 39999);
        config_cover_item.show_rate = (int)EditorGUILayout.Slider("show_rate " + CellWars_Editor.config_cover.template.show_rate, config_cover_item.show_rate, -10, 10);
        config_cover_item.hide_rate = (int)EditorGUILayout.Slider("hide_rate " + CellWars_Editor.config_cover.template.hide_rate, config_cover_item.hide_rate, -10, 10);
        config_cover_item.life = (int)EditorGUILayout.Slider("life " + CellWars_Editor.config_cover.template.life, config_cover_item.life, -1, 10);
        config_cover_item.line = (bool)EditorGUILayout.Toggle("line" + CellWars_Editor.config_cover.template.line, config_cover_item.line);
        config_cover_item.move = (bool)EditorGUILayout.Toggle("move" + CellWars_Editor.config_cover.template.move, config_cover_item.move);
    }

	private void ShowMonster()
	{
        ShowBase(40000, 49999);

		config_monster_item config_monster_item = (config_monster_item)CellWars_Editor.crt_item;

        config_monster_item.hide_id = (int)EditorGUILayout.Slider("hide_id " + CellWars_Editor.config_monster.template.hide_id, config_monster_item.hide_id, 40000, 49999);
        config_monster_item.monster_type = (int)EditorGUILayout.Slider("monster_type " + CellWars_Editor.config_monster.template.monster_type, config_monster_item.monster_type, 0, 30);
		config_monster_item.size = (int)EditorGUILayout.Slider("" + CellWars_Editor.config_monster.template.size, config_monster_item.size, 0, 3);
        config_monster_item.rotate = (int)EditorGUILayout.Slider("rotate " + CellWars_Editor.config_monster.template.rotate, config_monster_item.rotate, 0, 5);
        config_monster_item.forever = EditorGUILayout.Toggle("forever " + CellWars_Editor.config_monster.template.forever, config_monster_item.forever);
        config_monster_item.release = EditorGUILayout.TextField("release " + CellWars_Editor.config_monster.template.release, config_monster_item.release);
	}

    private void ShowProp()
    {
        ShowBase(10000, 19999);

        config_prop_item config_prop_item = (config_prop_item)CellWars_Editor.crt_item;

		config_prop_item.times = (int)EditorGUILayout.Slider(CellWars_Editor.config_prop.template.times, config_prop_item.times, 0, 10);
        config_prop_item.costABC = EditorGUILayout.TextField(CellWars_Editor.config_prop.template.costABC, config_prop_item.costABC);
    }

    private void ShowModule()
    {
        ShowBase(10000, 19999);

        config_module_item config_module_item = (config_module_item)CellWars_Editor.crt_item;

        config_module_item.layer_depth = (int)EditorGUILayout.Slider(CellWars_Editor.config_module.template.layer_depth, config_module_item.layer_depth, -100, 100);
        config_module_item.ab_name = EditorGUILayout.TextField(CellWars_Editor.config_module.template.ab_name, config_module_item.ab_name);
        config_module_item.prefab_name = EditorGUILayout.TextField(CellWars_Editor.config_module.template.prefab_name, config_module_item.prefab_name);
        config_module_item.open_clear = EditorGUILayout.TextField(CellWars_Editor.config_module.template.open_clear, config_module_item.open_clear);
        config_module_item.open_add = EditorGUILayout.TextField(CellWars_Editor.config_module.template.open_add, config_module_item.open_add);
        config_module_item.close_clear = EditorGUILayout.TextField(CellWars_Editor.config_module.template.close_clear, config_module_item.close_clear);
        config_module_item.never_close = (int)EditorGUILayout.Slider(CellWars_Editor.config_module.template.never_close, config_module_item.never_close, 0, 2);
    }
	
	private void ShowGuild()
	{
        ShowBase(10101, 99999);

		config_guild_item config_guild_item = (config_guild_item)CellWars_Editor.crt_item;

        config_guild_item.root_id = (int)EditorGUILayout.Slider(CellWars_Editor.config_guild.template.root_id, config_guild_item.root_id, 10101, 99999);
        config_guild_item.next_id = (int)EditorGUILayout.Slider(CellWars_Editor.config_guild.template.next_id, config_guild_item.next_id, 0, 99999);
        config_guild_item.condition = EditorGUILayout.TextField(CellWars_Editor.config_guild.template.condition, config_guild_item.condition);
        config_guild_item.aims = EditorGUILayout.TextField(CellWars_Editor.config_guild.template.aims, config_guild_item.aims);
        config_guild_item.complete_tip = EditorGUILayout.TextField(CellWars_Editor.config_guild.template.complete_tip, config_guild_item.complete_tip);
		config_guild_item.wait_time = (int)EditorGUILayout.Slider(CellWars_Editor.config_guild.template.wait_time, config_guild_item.wait_time, -1, 5000);
        config_guild_item.guild_type = (int)EditorGUILayout.Slider(CellWars_Editor.config_guild.template.guild_type, config_guild_item.guild_type, 0, 10);
	}

    private void ShowChapter()
    {
        ShowBase(10001, 10100);

        config_chapter_item config_chapter_item = (config_chapter_item)CellWars_Editor.crt_item;

        config_chapter_item.maps = EditorGUILayout.TextField(CellWars_Editor.config_chapter.template.maps, config_chapter_item.maps);
        config_chapter_item.map_pos = EditorGUILayout.TextField(CellWars_Editor.config_chapter.template.map_pos, config_chapter_item.map_pos);
		config_chapter_item.reward = EditorGUILayout.TextField("reward", config_chapter_item.reward);
    }

    private void ShowWealth()
    {
        ShowBase(10001, 20000);

        config_wealth_item config_wealth_item = (config_wealth_item)CellWars_Editor.crt_item;

		config_wealth_item.firstCount = (int)EditorGUILayout.Slider(CellWars_Editor.config_wealth.template.firstCount, config_wealth_item.firstCount, 0, 99999);
		config_wealth_item.limitCount = (int)EditorGUILayout.Slider(CellWars_Editor.config_wealth.template.limitCount, config_wealth_item.limitCount, 0, 99999);

        config_wealth_item.accs = EditorGUILayout.TextField(CellWars_Editor.config_wealth.template.accs, config_wealth_item.accs);
        config_wealth_item.price = EditorGUILayout.TextField(CellWars_Editor.config_wealth.template.price, config_wealth_item.price);
    }

    private void ShowSkill()
    {
        ShowBase(10001, 20000);

        config_skill_item config_skill_item = (config_skill_item)CellWars_Editor.crt_item;

        config_skill_item.cellId = (int)EditorGUILayout.Slider(CellWars_Editor.config_skill.template.cellId, config_skill_item.cellId, 10001, 11000);
        config_skill_item.groupId = (int)EditorGUILayout.Slider(CellWars_Editor.config_skill.template.groupId, config_skill_item.groupId, 10001, 11000);
        config_skill_item.unlockId = (int)EditorGUILayout.Slider(CellWars_Editor.config_skill.template.unlockId, config_skill_item.unlockId, 20001, 20200);
		config_skill_item.type = (int)EditorGUILayout.Slider(CellWars_Editor.config_skill.template.type, config_skill_item.type, 0, 2);
        config_skill_item.bottleABC = EditorGUILayout.TextField("bottleABC", config_skill_item.bottleABC);
        config_skill_item.starABC = EditorGUILayout.TextField("starABC", config_skill_item.starABC);
		config_skill_item.dir = (int)EditorGUILayout.Slider("dir", config_skill_item.dir, 0, 5);

    }

    private void ShowSort()
    {
        ShowBase(10001, 20000);

        config_sort_item config_sort_item = (config_sort_item)CellWars_Editor.crt_item;

        config_sort_item.gid = EditorGUILayout.TextField("gid", config_sort_item.gid);
        config_sort_item.gtype = (int)EditorGUILayout.Slider("gtype", config_sort_item.gtype, 1, 10);
        config_sort_item.refer = (int)EditorGUILayout.Slider("refer", config_sort_item.refer, 0, 1000000);
    }

    private void ShowBase(int statId,int endId)
    {
        CellWars_Editor.crt_item.id = (int)EditorGUILayout.Slider("id", CellWars_Editor.crt_item.id, statId, endId);
        CellWars_Editor.crt_item.icon = (int)EditorGUILayout.Slider("icon", CellWars_Editor.crt_item.icon, statId, endId);
        CellWars_Editor.crt_item.name = EditorGUILayout.TextField("name", CellWars_Editor.crt_item.name);
        CellWars_Editor.crt_item.desc = EditorGUILayout.TextField("desc", CellWars_Editor.crt_item.desc);
        CellWars_Editor.crt_item.special = EditorGUILayout.TextField("special", CellWars_Editor.crt_item.special);
    }
}