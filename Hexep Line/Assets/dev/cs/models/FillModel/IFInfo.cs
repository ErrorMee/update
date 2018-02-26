using System;

public class IFInfo
{
    public int id;

    public float value;

    public void FillTxt(string content)
    {
        string[] contentArr = content.Split('*');

        id = Convert.ToInt32(contentArr[0]);

        if (contentArr.Length > 1)
        {
            value = (float)Convert.ToDouble(contentArr[1]);
        }
        else
        {
            value = 0;
        }
    }
}