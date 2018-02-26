using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenSlider : MonoBehaviour
{

    private Vector2 startPos;
    private bool startValid;
    public static bool SlideOpen = true;

    public static void OpenSlid()
    {
        SlideOpen = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreen = Input.mousePosition;
            startPos = new Vector2();
            startPos.x = (mouseScreen.x - Screen.width * 0.5f) * PosUtil.GAME_WIDTH / Screen.width;
            startPos.y = (mouseScreen.y - Screen.height * 0.5f) * PosUtil.GAME_HEIGHT / Screen.height;


            if (Math.Abs(startPos.y) > PosUtil.GAME_HEIGHT * 0.33f)
            {
                startValid = false;
            }
            else
            {
                startValid = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {

            if (SlideOpen && startValid && GuidePart.ViewGuideing == false)
            {
                Vector3 mouseScreen = Input.mousePosition;
                Vector2 endPos = new Vector2();
                endPos.x = (mouseScreen.x - Screen.width * 0.5f) * PosUtil.GAME_WIDTH / Screen.width;
                endPos.y = (mouseScreen.y - Screen.height * 0.5f) * PosUtil.GAME_HEIGHT / Screen.height;

                float len = (float)Math.Sqrt(Math.Abs(startPos.x - endPos.x) * Math.Abs(startPos.x - endPos.x) + Math.Abs(startPos.y - endPos.y) * Math.Abs(startPos.y - endPos.y));
                if (len > PosUtil.GAME_WIDTH * 0.1f)
                {
                    double angle = Math.Atan2((double)(endPos.y - startPos.y), (double)(endPos.x - startPos.x)) * 180 / Math.PI;

                    if (Math.Abs(angle) < 40)
                    {
                        PromptModel.Instance.Slide(false);
                    }

                    if (Math.Abs(angle) > 140)
                    {
                        PromptModel.Instance.Slide(true);
                    }
                }
            }
        }
    }
}
