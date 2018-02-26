using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Bugger : MonoBehaviour {

    private List<BuggerMoveInfo> moves = new List<BuggerMoveInfo>();
    private BuggerMoveInfo move;
    private List<BuggerPosInfo> poss = new List<BuggerPosInfo>();
    private BuggerPosInfo pos;

    public RectTransform cellRect;
    
    public Animation eyeAnimL;
    public Animation eyeAnimR;
    
    public Image icon;
    public int iconId = 0;

    void Start()
    {
        EventTriggerListener.Get(icon.gameObject).onClick = OnClickIocn;
    }

    private void OnClickIocn(GameObject go)
    {
        int addCount = 10;
        config_wealth_item config_wealth = (config_wealth_item)ResModel.Instance.config_wealth.GetItem((int)WealthTypeEnum.Coin);
        WealthInfo wealthInfo = PlayerModel.Instance.GetWealth((int)WealthTypeEnum.Coin);
        wealthInfo.count += addCount;
        PlayerModel.Instance.SaveWealths();

        PromptModel.Instance.Pop(LanguageUtil.GetTxt(Convert.ToInt32(config_wealth.name)) + " + " + addCount);
    }

    private void LoadDefaultMoves()
    {
        moves.Add(new BuggerMoveInfo(0.2f, 0.2f));
        moves.Add(new BuggerMoveInfo(0.5f, 0.5f));
        moves.Add(new BuggerMoveInfo(1.2f, 1.2f));
    }

    public void AddPos(BuggerPosInfo posInfo)
    {
        poss.Add(posInfo);
    }

    public void Show()
    {
        if (moves == null || moves.Count == 0)
        {
            LoadDefaultMoves();
        }
        RecursionShow();
    }

    private void RecursionShow()
    {
        float waitTime = UnityEngine.Random.Range(2000, 6000) / 1000.00f;
        LeanTween.delayedCall(waitTime, OnShow);
    }

    private void OnShow()
    {
        int randomValue = UnityEngine.Random.Range(0, moves.Count);

        move = moves[randomValue];

        randomValue = UnityEngine.Random.Range(0, poss.Count);

        pos = poss[randomValue];

        BuggerAppear();
    }

    private void BuggerAppear()
    {
        float angle = PosUtil.VectorAngle(pos.fromPos, pos.toPos);
        cellRect.localRotation = Quaternion.Euler(0, 0, angle - 90);

        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.anchoredPosition = new Vector2(pos.fromPos.x, pos.fromPos.y);

        if (iconId > 0)
        {
            icon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + iconId);
        }
        else {
            int randomValue = UnityEngine.Random.Range(10101, 10106);
            icon.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + randomValue);
        }

        LeanTween.move(rectTransform, pos.toPos, move.appearTime).onComplete = AppearComplete;
    }

    private void AppearComplete()
    {
        int winkType = UnityEngine.Random.Range(0, 3);//0 都眨眼 1左 2右

        switch (winkType)
        {
            case 1:
                eyeAnimL.Play();
                break;
            case 2:
                eyeAnimR.Play();
                break;
            default:
                eyeAnimL.Play();
                eyeAnimR.Play();
                break;
        }

        float waitTime = UnityEngine.Random.Range(400, 700) / 1000.00f;
        LeanTween.delayedCall(waitTime, BuggerDisAppear);
    }

    private void BuggerDisAppear()
    {
        RectTransform rectTransform = (RectTransform)transform;

        int randomValue = UnityEngine.Random.Range(0, moves.Count);

        BuggerMoveInfo moveDisAppear = moves[randomValue];

        LeanTween.move(rectTransform, pos.fromPos, moveDisAppear.disappearTime).onComplete = DisAppearComplete;
    }

    private void DisAppearComplete()
    {
        RecursionShow();
    }
}
