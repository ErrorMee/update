using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class ConfigWnd : EditorWindow
{

    public static ConfigWnd instance;
    public static void InstanceShow()
    {
        if (instance != null)
        {
            instance.titleContent.text = CellWars_Editor.crt_config.name;
            //instance.title = CellWars_Editor.crt_config.name;
            instance.Focus();
            return;
        }
        ConfigWnd wnd = (ConfigWnd)EditorWindow.GetWindow(typeof(ConfigWnd));
        instance = wnd;
        instance.Show();
        instance.titleContent.text = CellWars_Editor.crt_config.name;
        //instance.title = CellWars_Editor.crt_config.name;
        instance.Focus();
        instance.minSize = new Vector2(850, 700);
        instance.maxSize = instance.minSize + new Vector2(10, 10);
    }

    public ConfigWnd()
    {
    }

    private void OnGUI()
    {
        InitTool();
        InitList();
    }

    private void InitTool()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("new", GUILayout.Width(60)))
        {
            ConfigSet levelSet = (ConfigSet)EditorWindow.GetWindow(typeof(ConfigSet));
            if (levelSet != null)
            {
                levelSet.Close();
            }
            ConfigSet.InstanceShow();
        }

        if (GUILayout.Button("save", GUILayout.Width(60)))
        {
            CellWars_Editor.SaveCrtItem();
        }

        GUILayout.EndHorizontal();

    }

    Vector2 scrollPos;
    private void InitList()
    {
        int dataCount = CellWars_Editor.crt_config.GetDataCount();

        EditorGUILayout.LabelField("data.Count:" + dataCount);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(instance.minSize.x - 10), GUILayout.Height(instance.minSize.y - 40));
		int groupIndex = 0;
		for (int i = 0; i < dataCount; i++)
        {
			if(groupIndex == 0)
			{
				EditorGUILayout.BeginHorizontal();
			}
			groupIndex ++;


            config_item_base item = CellWars_Editor.crt_config.GetItemAt(i);

            string itemName = " " + item.id;
            if (CellWars_Editor.crt_config.name == "config_cell")
            {
                config_cell_item config_cell_item = (config_cell_item)item;
                itemName = (int)config_cell_item.cell_type + " " + item.id;
            }
            if (GUILayout.Button(itemName, GUILayout.Width(80)))
            {
                ConfigSet levelSet = (ConfigSet)EditorWindow.GetWindow(typeof(ConfigSet));
                if (levelSet != null)
                {
                    levelSet.Close();
                }
                ConfigSet.InstanceShow(item.id);
            }

			if(groupIndex == 10)
			{
				groupIndex = 0;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(" ");
				EditorGUILayout.EndHorizontal();
			}

        }
        EditorGUILayout.EndScrollView();
    }
}