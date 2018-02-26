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
        case CellWars_Editor.config_module_name:
            ShowModule();
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

    private void ShowBase(int statId,int endId)
    {
        CellWars_Editor.crt_item.id = (int)EditorGUILayout.Slider("id", CellWars_Editor.crt_item.id, statId, endId);
        CellWars_Editor.crt_item.icon = (int)EditorGUILayout.Slider("icon", CellWars_Editor.crt_item.icon, statId, endId);
        CellWars_Editor.crt_item.name = EditorGUILayout.TextField("name", CellWars_Editor.crt_item.name);
        CellWars_Editor.crt_item.desc = EditorGUILayout.TextField("desc", CellWars_Editor.crt_item.desc);
        CellWars_Editor.crt_item.special = EditorGUILayout.TextField("special", CellWars_Editor.crt_item.special);
    }
}