using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class Packager5 
{
    private static List<string> paths = new List<string>();
    private static List<string> files = new List<string>();

    [@MenuItem(CellWars_Editor.editor_name + "/Set ABs")]
    public static void SetABs()
    {
        /////////////////////////Dependencies
        string rootPath = (Application.dataPath + "/sourceArt/prefab/").ToLower();
        InitFilesAndPaths(rootPath, true, false);

        string[] filePaths = files.ToArray();

        Debug.Log("Manually prefabs " + filePaths.Length);

        string[] dps = AssetDatabase.GetDependencies(filePaths);

        int effectiveCount = 0;
        int i;
        for (i = 0; i < dps.Length; i++)
        {
            string dp = dps[i].ToLower();
            if (files.IndexOf(dp) >= 0)
            {
                continue;
            }
            string ext = Path.GetExtension(dp);
            if (ext.Equals(".cs"))
            {
                continue;
            }

            if (dp.IndexOf("/icon/") >= 0)
            {
                continue;
            }

            if (dp.IndexOf("/fonts/") >= 0)
            {
                Debug.Log(string.Format(".......fonts abort{0} ", dp));
            }

            effectiveCount++;
            AssetImporter ai = AssetImporter.GetAtPath(dp);

            if (ai.assetBundleName != null)
            {
                string abName = AssetDatabase.AssetPathToGUID(dp);
                Debug.Log(string.Format("No.{0} id:{1} path:{2}", effectiveCount, abName, dp));
                ai.assetBundleName = "dependencies/" + abName;
            }
        }
        Debug.Log(string.Format("effective dependencies:{0}", effectiveCount));

        ////////////////////config
        rootPath = (Application.dataPath + "/sourceArt/config/").ToLower();
        InitFilesAndPaths(rootPath, true, false);
        filePaths = files.ToArray();
        effectiveCount = 0;
        for (i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i].ToLower();

            string ext = Path.GetExtension(filePath);
            if (ext.Equals(".json") && filePath.Contains("._") == false)
            {
                
                effectiveCount++;
                Debug.Log(string.Format("config.{0} name:{1}", effectiveCount, Path.GetFileNameWithoutExtension(filePath)));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = "config/" + Path.GetFileNameWithoutExtension(filePath).ToLower() + ".ab";
            }
        }

        ///////////////////dat
        rootPath = (Application.dataPath + "/sourceArt/dat/").ToLower();
        InitFilesAndPaths(rootPath, true, false);
        filePaths = files.ToArray();
        effectiveCount = 0;

        for (i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i].ToLower();
            string ext = Path.GetExtension(filePath);
            if (ext.Equals(".bytes") && filePath.Contains("._") == false)
            {
                string fileName = Path.GetFileName(filePath);
                string folderName = filePath.Replace("assets/sourceArt/dat/".ToLower(), "");
                folderName = folderName.Replace("/" + fileName, "");
                
                effectiveCount++;
                Debug.Log(string.Format("dat.{0} folder:{1} name:{2}", effectiveCount, folderName, fileName));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = "dat/" + folderName  + ".ab";
            }
        }

        ///////////////////txt
        rootPath = (Application.dataPath + "/sourceArt/txt/").ToLower();
        InitFilesAndPaths(rootPath, true, false);
        filePaths = files.ToArray();
        effectiveCount = 0;

        for (i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i].ToLower();
            string ext = Path.GetExtension(filePath);
            if (ext.Equals(".txt") && filePath.Contains("._") == false)
            {
                string fileName = Path.GetFileName(filePath);
                string folderName = filePath.Replace("assets/sourceArt/txt/".ToLower(), "");
                folderName = folderName.Replace(fileName, "");
                
                effectiveCount++;
                Debug.Log(string.Format("txt.{0} folder:{1} name:{2}", effectiveCount, folderName, fileName));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                if (folderName == "")
                {
                    ai.assetBundleName = "txt.ab";
                }else
                {
                    ai.assetBundleName = "txt/" + folderName.Replace("/", "") +".ab";
                }
                
            }
        }

        //////////////////icon
        rootPath = (Application.dataPath + "/sourceArt/icon/").ToLower();
        InitFilesAndPaths(rootPath, true, false);
        filePaths = files.ToArray();
        effectiveCount = 0;

        for (i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i].ToLower();
            string ext = Path.GetExtension(filePath);
            if (ext.Equals(".png") && filePath.Contains("._") == false)
            {
                string fileName = Path.GetFileName(filePath);
                string folderName = filePath.Replace("assets/sourceArt/icon/".ToLower(), "");
                folderName = folderName.Replace("/" + fileName, "");
                
                effectiveCount++;
                Debug.Log(string.Format("icon.{0} folder:{1} name:{2}", effectiveCount, folderName, fileName));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = "icon/" + folderName + ".ab";
            }
        }

        //////////////////audio
        rootPath = (Application.dataPath + "/sourceArt/audio/").ToLower();
        InitFilesAndPaths(rootPath, true, false);
        filePaths = files.ToArray();
        effectiveCount = 0;

        for (i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i].ToLower();
            string ext = Path.GetExtension(filePath);
            if ((ext.Equals(".mp3") || ext.Equals(".ogg")) && filePath.Contains("._") == false)
            {
                string fileName = Path.GetFileName(filePath);
                string folderName = filePath.Replace("assets/sourceArt/audio/".ToLower(), "");
                folderName = folderName.Replace("/" + fileName, "");
                
                effectiveCount++;
                Debug.Log(string.Format("audio.{0} folder:{1} name:{2}", effectiveCount, folderName, fileName));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = "audio.ab";
            }
        }

        //////////////////font
        rootPath = (Application.dataPath + "/sourceArt/font/").ToLower();
        InitFilesAndPaths(rootPath, true, false);
        filePaths = files.ToArray();
        effectiveCount = 0;

        for (i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i].ToLower();
            string ext = Path.GetExtension(filePath);
            if ((ext.Equals(".ttf") || ext.Equals(".otf")) && filePath.Contains("._") == false)
            {
                string fileName = Path.GetFileName(filePath);
                string folderName = filePath.Replace("assets/sourceArt/font/".ToLower(), "");
                folderName = folderName.Replace("/" + fileName, "");

                effectiveCount++;
                Debug.Log(string.Format("audio.{0} folder:{1} name:{2}", effectiveCount, folderName, fileName));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = "font.ab";
            }
        }

        //////////////////effect
        rootPath = (Application.dataPath + "/sourceArt/effect/").ToLower();
        InitFilesAndPaths(rootPath, true, false);
        filePaths = files.ToArray();
        effectiveCount = 0;

        for (i = 0; i < filePaths.Length; i++)
        {
            string filePath = filePaths[i].ToLower();
            string ext = Path.GetExtension(filePath);
            if (ext.Equals(".prefab") && filePath.Contains("._") == false)
            {
                string fileName = Path.GetFileName(filePath);
                string folderName = filePath.Replace("assets/sourceArt/effect/".ToLower(), "");
                folderName = folderName.Replace("/" + fileName, "");

                effectiveCount++;
                Debug.Log(string.Format("audio.{0} folder:{1} name:{2}", effectiveCount, folderName, fileName));
                AssetImporter ai = AssetImporter.GetAtPath(filePath);
                ai.assetBundleName = "effect.ab";
            }
        }

        AssetDatabase.Refresh();
    }

    [@MenuItem(CellWars_Editor.editor_name + "/Clear ABs")]
    public static void ClearABs()
    {
		Directory.Delete(Application.dataPath + GetStreamingPath() + "/icon".ToLower(), true);
		Directory.Delete(Application.dataPath + GetStreamingPath() + "/config".ToLower(), true);
		Directory.Delete(Application.dataPath + GetStreamingPath() + "/dat".ToLower(), true);
		Directory.Delete(Application.dataPath + GetStreamingPath() + "/prefab".ToLower(), true);
		Directory.Delete(Application.dataPath + GetStreamingPath() + "/txt".ToLower(), true);
		Directory.Delete(Application.dataPath + GetStreamingPath() + "/dependencies".ToLower(), true);
        AssetDatabase.Refresh();
    }

    [@MenuItem(CellWars_Editor.editor_name + "/Build ABs")]
	public static void BuildABs()
    {
		AssetDatabase.Refresh();
		string abRootPath = Application.dataPath + GetStreamingPath();
        BuildPipeline.BuildAssetBundles(abRootPath, GetBuildABOption(), GetBuildTarget());
    }

    private static BuildAssetBundleOptions GetBuildABOption()
    {
        //ANDROID_PUBLISH 安卓正式发版宏
#if ANDROID_PUBLISH
        BuildAssetBundleOptions options = 
                    BuildAssetBundleOptions.None;
#else
        BuildAssetBundleOptions options =
                    BuildAssetBundleOptions.None;
#endif

        return options;
    }

	private static string GetStreamingPath()
	{
		string path;
		if (Application.platform == RuntimePlatform.OSXEditor)
		{
			path = "/StreamingAssets/mobile";
		}
		else
		{
			path = "/StreamingAssets/mobile";
		}

		return path;
	}

    public static BuildTarget GetBuildTarget()
    {
        BuildTarget target;
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            target = BuildTarget.iOS;
        }
        else
        {
            target = BuildTarget.Android;
        }

        return target;
    }

    private static void InitFilesAndPaths(string rootPath, bool isRecursive = true, bool absolutePath = true)
    {
        paths.Clear();
        files.Clear();
        Recursive(rootPath, isRecursive, absolutePath);
    }

    //递归遍历文件和文件夹
    private static void Recursive(string path, bool isRecursive = true, bool absolutePath = true)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            string addFileName = filename.Replace('\\', '/');

            if (absolutePath == false)
            {
                int startIndex = addFileName.IndexOf("assets/");

                string relativeName = addFileName.Remove(0, startIndex);
                files.Add(relativeName.ToLower());
            }
            else
            {
                files.Add(addFileName.ToLower());
            }
        }
        foreach (string dir in dirs)
        {
            string addDirName = dir.Replace('\\', '/');
            paths.Add(addDirName.ToLower());
            if (isRecursive)
            {
                Recursive(dir, isRecursive, absolutePath);
            }
        }
    }
}
