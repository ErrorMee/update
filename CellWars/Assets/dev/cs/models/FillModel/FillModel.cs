using System;
using System.Collections;
using System.Collections.Generic;

//管理所有填充规则
public class FillModel : Singleton<FillModel>
{
    public List<FillInfo> fillInfos = new List<FillInfo>();

    public FillInfo crtFillInfo;

    public void InitFillInfo()
    {
        config_map_item map_item = BattleModel.Instance.crtConfig;

        FillInfo fillInfo = GetFillInfo(map_item.fill);
        if (fillInfo == null)
        {
            fillInfo = new FillInfo();
            fillInfo.id = map_item.fill;
            string content = GameMgr.resourceMgr.GetTextString("txt/fill.ab", fillInfo.id.ToString());
            fillInfo.FillTxt(content);
            fillInfos.Add(fillInfo);
        }
        crtFillInfo = fillInfo;
        FillInfo.FILL_COUNT = 0;
    }

    public FillInfo GetFillInfo(int id)
    {
        for (int i = 0; i < fillInfos.Count; i++)
        {
            FillInfo fillInfo = fillInfos[i];
            if (fillInfo.id == id)
            {
                return fillInfo;
            }
        }
        return null;
    }

}