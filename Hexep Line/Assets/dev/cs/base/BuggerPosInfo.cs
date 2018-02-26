using System;
using UnityEngine;

public class BuggerPosInfo
{
    public Vector2 fromPos;
    public Vector2 toPos;

    public BuggerPosInfo(Vector2 fromPosP, Vector2 toPosP)
    {
        fromPos = fromPosP;
        toPos = toPosP;
    }
}