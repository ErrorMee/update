
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DebugUtil : MonoBehaviour
{
    static List<string> mLines = new List<string>();

    void Start()
    {
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            Log(logString);
            Log(stackTrace);
            //////////////////////////////////////////////////
            string strContent4Mail = string.Empty;
            for (int i = 0, imax = mLines.Count; i < imax; ++i)
            {
                strContent4Mail += mLines[i] + "<br>\n\r";
            }
            MailUtil.SendErrorLog(strContent4Mail);
            /////////////////////////////////////////////////
        }
    }

    static public void Log(params object[] objs)
    {
        string text = "";
        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }
        if (Application.isPlaying)
        {
            if (mLines.Count > 20)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);
        }
    }
}
