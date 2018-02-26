using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmModule : BaseModule
{
	public Transform closeTrans;
    public Transform closeBtn;

    public Transform noBtn;

    public Transform okBtn;

    public Transform titleTrans;
    public Transform contentTrans;

    public Animation winOpenAnim;

    private ConfirmInfo crtConfirm;


    override protected void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        winOpenAnim.Play();
        InitEvents();
        InitView();
    }

    private void InitEvents()
    {
        EventTriggerListener.Get(closeBtn.gameObject).onClick = OnClose;
		EventTriggerListener.Get(closeTrans.gameObject).onClick = OnClose;
        EventTriggerListener.Get(noBtn.gameObject).onClick = OnNO;
        EventTriggerListener.Get(okBtn.gameObject).onClick = OnOk;
    }

    private void InitView()
    {
        crtConfirm = ConfirmModel.Instance.crtConfirm;

        contentTrans.GetComponentInChildren<Text>().text = crtConfirm.content;

        if (crtConfirm.title == "")
        {
            titleTrans.gameObject.SetActive(false);
        }
        else
        {
            titleTrans.GetComponentInChildren<Text>().text = crtConfirm.title;
        }

        if (crtConfirm.noEvent == null)
        {
            noBtn.gameObject.SetActive(false);
            okBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11605);
        }else
        {
            noBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11607);
            okBtn.GetComponentInChildren<Text>().text = LanguageUtil.GetTxt(11606);
        }

        closeBtn.gameObject.SetActive(crtConfirm.canClose);
    }

    private void OnClose(GameObject go)
    {
        crtConfirm = null;
        ConfirmModel.Instance.RemoveConfirm();
    }

    private void OnNO(GameObject go)
    {
        if (crtConfirm.noEvent != null)
        {
            crtConfirm.noEvent();
        }
        crtConfirm = null;
        ConfirmModel.Instance.RemoveConfirm();
    }
    private void OnOk(GameObject go)
    {
        if (crtConfirm.okEvent != null)
        {
            crtConfirm.okEvent();
        }
        crtConfirm = null;
        ConfirmModel.Instance.RemoveConfirm();
    }
}