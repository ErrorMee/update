﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmModel : Singleton<ConfirmModel>
{
    private List<ConfirmInfo> confirmInfos = new List<ConfirmInfo>();

    public ConfirmInfo crtConfirm;

    public void AddConfirm(ConfirmInfo confirmInfo)
    {
        confirmInfos.Add(confirmInfo);
        ShowConfirm();
    }

    private void ShowConfirm()
    {
		if (confirmInfos.Count > 0 && crtConfirm == null)
        {
            crtConfirm = confirmInfos[0];
            confirmInfos.RemoveAt(0);
			if (crtConfirm != null)
            {
                ModuleModel.Instance.AddUIModule((int)ModuleEnum.CONFIRM);
            }
        }
    }

    public void RemoveConfirm()
    {
        crtConfirm = null;
        ModuleModel.Instance.RemoveUIModule((int)ModuleEnum.CONFIRM);
		ScreenSlider.SlideOpen = true;
        ShowConfirm();
    }
}