using UnityEngine;
using System.Collections;
using System.IO;
//using UnityEditor;

public class FileUtil
{
	private static FileUtil instance = null;
	private string appDirectory;
	private string baseDirectory;

	private FileUtil()
	{
		appDirectory = Application.dataPath;
		baseDirectory = appDirectory;
	}

	public static FileUtil Instance()
	{
		if(instance == null)
		{
			instance = new FileUtil();
		}
		return instance;
	}

	public void SetBasePath(string name)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(appDirectory + "/" + name);
		if(directoryInfo.Exists == false)
		{
			Directory.CreateDirectory(appDirectory + "/" + name);
		}
		baseDirectory = appDirectory + "/" + name;
	}

	public string FullName(string name)
	{
		return baseDirectory + "/" + name;
	}

	public bool HasFolder(string name)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(FullName(name));
		return directoryInfo.Exists;
	}

	public void CreateFolder(string name)
	{
		if(HasFolder(FullName(name)))
		{
			return;
		}

		DirectoryInfo directoryInfo = new DirectoryInfo(FullName(name));
		if(directoryInfo.Exists == false)
		{
			Directory.CreateDirectory(FullName(name));
		}
	}

	public bool HasFile(string name, string suffix = ".json")
	{
		name = name + suffix;
		FileInfo fileInfo = new FileInfo(FullName(name));
		return fileInfo.Exists;
	}

	public void WriteFile(string name,string content,bool overwrite, string suffix = ".json")
	{
		name = name + suffix;

		StreamWriter sw;

		FileInfo fileInfo = new FileInfo(FullName(name));

		if(fileInfo.Exists)
		{
			if(overwrite)
			{
				sw = fileInfo.CreateText();
			}else{
				sw = fileInfo.AppendText();
			}
		}else{
			sw = fileInfo.CreateText();
		}

		sw.WriteLine(content);

		sw.Close();
		sw.Dispose();

		//AssetDatabase.Refresh();
	}
}
