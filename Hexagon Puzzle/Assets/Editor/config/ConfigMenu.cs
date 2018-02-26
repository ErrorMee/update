using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


class ConfigMenu:EditorWindow
{
    private static ConfigMenu instance;
    [MenuItem(CellWars_Editor.editor_name + "/" + "Config")]
    public static void Item1()
    {
        CellWars_Editor.LoadConfigs();

        if (instance != null)
        {
            return;
        }

        ConfigMenu levelMenu = (ConfigMenu)EditorWindow.GetWindow(typeof(ConfigMenu));
        instance = levelMenu;
        instance.Show();
        instance.minSize = new Vector2(200, 600);
        instance.maxSize = instance.minSize + new Vector2(10, 10);
    }

    public ConfigMenu()
    {
        
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField(" ");
        if (GUILayout.Button(CellWars_Editor.config_cell_name, GUILayout.Width(position.width - 10)))
        {
            CellWars_Editor.crt_config = CellWars_Editor.config_cell;
            ShowConfigSet();
        }
        EditorGUILayout.LabelField(" ");
        if (GUILayout.Button(CellWars_Editor.config_module_name, GUILayout.Width(position.width - 10)))
        {
            CellWars_Editor.crt_config = CellWars_Editor.config_module;
            ShowConfigSet();
        }

    }

    private void ShowConfigSet()
    {
        ConfigSet levelSet = (ConfigSet)EditorWindow.GetWindow(typeof(ConfigSet));
        if (levelSet != null)
        {
            CellWars_Editor.crt_item = null;
            levelSet.Close();
        }

        ConfigWnd.InstanceShow();
    }
}