using System;
using System.Collections.Generic;

using LitJson;

public class GameModel : Singleton<GameModel>
{
	public bool DebugDev = false;

    private Dictionary<int, float> gameConfig = null;

    public void InitGameConfig()
    {
        string content = ResModel.Instance.GetTextString("txt/" + "game");
        gameConfig = new Dictionary<int, float>();
        string[] txtArr = content.Split('\n');
        foreach (string txt in txtArr)
        {
            if (txt != "")
            {
                string[] txts = txt.Split('=');
                if (txts.Length > 1)
                {
                    gameConfig.Add(int.Parse(txts[0]), float.Parse(txts[1].Split('\r')[0]));
                }
            }
        }
    }

    public float GetGameConfig(int key)
    {
        if (gameConfig.ContainsKey(key))
        {
            return gameConfig[key];
        }
        return 0;
    }
}