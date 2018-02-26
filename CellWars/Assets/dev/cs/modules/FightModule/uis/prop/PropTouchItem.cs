using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class PropTouchItem : MonoBehaviour
{
    public int control_x;
    public int control_y;

    public int control_id;

    public Image iconImage;
    public Image selectImage;
	public Image lockImage;
    public Image brushImage;

    private void Awake()
    {
        brushImage.gameObject.SetActive(false);
		iconImage.color = Color.clear;
		selectImage.gameObject.SetActive(false);
    }

    private int _icon;
    public int icon
    {
        set
        {
            _icon = value;

            if (_icon <= 0)
            {
				iconImage.color = Color.clear;
            }
            else
            {
				iconImage.color = Color.white;
				//iconImage.color = ColorMgr.GetColor(ColorMgr.GetCellColorValue(_icon));
				iconImage.overrideSprite = GameMgr.resourceMgr.GetSprite("icon/cell.ab", "cell_" + _icon);
            }
        }
        get { return _icon; }
    }

    private bool _valid;
    public bool valid
    {
        set
        {
            _valid = value;

			selectImage.gameObject.SetActive(_valid);
			lockImage.gameObject.SetActive(!_valid);
        }
        get { return _valid; }
    }

	public void Select()
	{
		iconImage.gameObject.SetActive(false);
		lockImage.gameObject.SetActive(false);
		brushImage.gameObject.SetActive(false);
		selectImage.gameObject.SetActive(true);
		//selectImage.color = ColorMgr.GetColor(ColorMgr.COLOR_GREEN,0.8f);
	}

}
