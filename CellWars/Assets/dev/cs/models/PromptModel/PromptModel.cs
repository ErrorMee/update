using System;
using System.Collections.Generic;


public class PromptModel : Singleton<PromptModel>
{
    public Action<string,int> popEvent;

    public Action<bool> showLoadingEvent;

    public Action<bool> SlideEvent;

    private string lastPopStr;
    private Int64 lastMSecond;

    public void Pop(string popStr,bool force = false,int wealthId = 0)
    {
        if (popStr != null)
        {
            if (force)
            {
                //GameMgr.audioMgr.PlayeSound("Pop");
                if (popEvent != null)
                {
                    popEvent(popStr, wealthId);
                }
            }
            else
            {
                Int64 currentMSecond = (Int64)(DateTime.Now.TimeOfDay.Ticks * 0.0001f);
                if (lastPopStr != null && lastPopStr == popStr)
                {

                    if ((currentMSecond - lastMSecond) > 1000)
                    {
                        GameMgr.audioMgr.PlayeSound("Pop");
                        if (popEvent != null)
                        {
                            popEvent(popStr, wealthId);
                        }
                        lastPopStr = popStr;
                        lastMSecond = currentMSecond;
                    }
                    else
                    {
                        //
                    }
                }
                else
                {
                    GameMgr.audioMgr.PlayeSound("Pop");
                    if (popEvent != null)
                    {
                        popEvent(popStr, wealthId);
                    }
                    lastPopStr = popStr;
                    lastMSecond = currentMSecond;
                }
            }
        }
    }

    public void ShowLoading(bool isShow)
    {
        if (showLoadingEvent != null)
        {
            showLoadingEvent(isShow);
        }
    }

    public void Slide(bool isLeft)
    {
        if (SlideEvent != null)
        {
            SlideEvent(isLeft);
        }
    }

}