using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IFSInfo
{
    public List<IFInfo> ifs = new List<IFInfo>();

    public void FillTxt(string content)
    {
        string[] contentArr = content.Split(',');

        for (int i = 0; i < contentArr.Length;i++ )
        {
            string contentTxt = contentArr[i];

            IFInfo iFInfo = new IFInfo();

            iFInfo.FillTxt(contentTxt);

            ifs.Add(iFInfo);

        }
    }

    public int GetFillId()
    {
        float value = UnityEngine.Random.Range(0, 1.00001f);

        float startValue = 0;
        float endValue = 0;
        for (int i = 0; i < ifs.Count - 1;i++ )
        {
            IFInfo iFInfo = ifs[i];
            endValue += iFInfo.value;

            if (value >= startValue && value <= endValue)
            {
                return iFInfo.id;
            }

            startValue += iFInfo.value;
        }

        return ifs[ifs.Count - 1].id;
    }

}