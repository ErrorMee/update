using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowEffectLineActor : ActionNode
{
    private EffectLayer layer;
    private CellInfo fcell;
    private CellInfo tcell;
    private int forceId;

    public ShowEffectLineActor(EffectLayer effectLayer, CellInfo cellInfo, CellInfo monsterCell,int force_id = 0)
        : base()
    {
        layer = effectLayer;
        fcell = cellInfo;
        tcell = monsterCell;
        forceId = force_id;
    }

    public override void OnExecute()
    {
        base.OnExecute();
        GameObject item = layer.ShowLine(fcell, tcell);

        Image image = item.GetComponentInChildren<Image>();

        float len = VectorLen(PosMgr.GetFightCellPos(fcell.posX, fcell.posY), PosMgr.GetFightCellPos(tcell.posX, tcell.posY));

        image.rectTransform.sizeDelta = new Vector2(len, 20);
        image.rectTransform.localScale = new Vector3(0, 1, 1);
        if (forceId > 0)
        {
            image.color = ColorMgr.GetColor(ColorMgr.GetCellColorValue(forceId));
        }
        else
        {
            image.color = ColorMgr.GetColor(ColorMgr.GetCellColorValue(fcell.config.id));
        }

        LeanTween.scaleX(image.gameObject, 1, len/1500.00f).onComplete = CompleteHander;
    }

    private float VectorLen(Vector2 from, Vector2 to)
    {
        float value = (float)Math.Sqrt(Math.Abs(from.x - to.x) * Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y) * Math.Abs(from.y - to.y));
        return value;
    }


    private void CompleteHander()
    {
        layer.DestroyLine(fcell, tcell);

        layer = null;
        fcell = null;
        tcell = null;
        OnEnd();
    }

}