using System;

public class BuggerMoveInfo
{
    public float appearTime;
    public float disappearTime;

    public BuggerMoveInfo(float appearTimeP = 0.8f, float disappearTimeP = 0.2f)
    {
        appearTime = appearTimeP;
        disappearTime = disappearTimeP;
    }

}