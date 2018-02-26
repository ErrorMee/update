using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class FightCellItem : FightBaseItem
{
    public CellInfo cell_info;

    private Transform bg;

    public Text index;
    public Text tip;

	public Animation touchAnim;

    public Animation moveEndAnim;

	private GameObject eyeObject;

	private GameObject lightObject;

	public Shadow shadow;

    override protected void Awake()
    {
        base.Awake();

        bg = transform.Find("bg");
        index = transform.Find("index").GetComponent<Text>();
        index.text = "";
        tip = transform.Find("tip").GetComponent<Text>();
        tip.text = "";
        touchAnim = GetComponent<Animation>();

		moveEndAnim = transform.Find ("icon").GetComponent<Animation> ();

		eyeObject = transform.Find("icon/eyes").gameObject;
		lightObject = transform.Find("icon/light").gameObject;

		shadow = iconImage.GetComponent<Shadow>();

    }

    public new int icon
    {
        set
        {
            if (_icon != value)
            {
                _icon = value;

                if (_icon <= 0 || cell_info.isBlank)
                {
                    iconImage.overrideSprite = null;
					eyeObject.SetActive(false);
					lightObject.SetActive(false);
                }
                else
                {
					if (_icon == 10001)
					{
						iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + _icon);
						//iconImage.color = ColorUtil.GetColor(ColorUtil.COLOR_FG_CELL);
						iconImage.rectTransform.sizeDelta = new Vector2(PosUtil.CELL_VIEW_W * 1f, PosUtil.CELL_W * 1f);
						eyeObject.SetActive(false);
						lightObject.SetActive(false);
						shadow.enabled = false;
					}
					else
					{
						iconImage.overrideSprite = ResModel.Instance.GetSprite("icon/cell/cell_" + _icon);
						iconImage.rectTransform.sizeDelta = new Vector2(PosUtil.CELL_VIEW_W * 0.85f, PosUtil.CELL_H * 0.85f);
						if (_icon >= 10101 && _icon <= 10105)
						{
							eyeObject.SetActive(true);
						}else
						{
							eyeObject.SetActive(false);
						}

						if(cell_info.config.cell_type == (int)CellType.bomb ||
							cell_info.config.cell_type == (int)CellType.radiate ||
							cell_info.config.cell_type == (int)CellType.line_bomb_r ||
							cell_info.config.cell_type == (int)CellType.three_bomb_r)
						{
							lightObject.SetActive(true);
							shadow.enabled = false;
						}else
						{
							lightObject.SetActive(false);
							if (cell_info.changer > 0) {
								shadow.enabled = false;
							} else {
								shadow.enabled = true;
							}
						}
					}
                }
            }
        }
        get { return _icon; }
    }

    private CellItemStadus _stadus;
    public CellItemStadus stadus
    {
        set
        {
            _stadus = value;
            if (_stadus == CellItemStadus.normal)
            {
                bg.gameObject.SetActive(false);
				if (_icon >= 10101 && _icon <= 10105)
				{
					if (cell_info.isBlank)
					{
						eyeObject.SetActive(false);
					}
					else
					{
						eyeObject.SetActive(true);
					}
				}
            }
            else
            {
                bg.gameObject.SetActive(true);
                Animation[] anims = eyeObject.GetComponentsInChildren<Animation>();
                for (int j = 0; j < anims.Length; j++)
                {
                    anims[j].Stop();
                }
                eyeObject.SetActive(false);
            }

            if (_stadus == CellItemStadus.over)
            {
                eyeObject.transform.Find("left/leftEye").localScale = new Vector3(1,1,1);
                eyeObject.transform.Find("right/rightEye").localScale = new Vector3(1, 1, 1);
                touchAnim.Play("CELL_TOUCH");
            }
        }
        get { return _stadus; }
    }

    public void UpdateTip()
    {
        if (cell_info.timer >= 0)
        {
            tip.text = "" + cell_info.timer;
        }
        else
        {
            tip.text = "";
        }
    }
}
