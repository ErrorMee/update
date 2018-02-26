using System;

public class ConfirmInfo
{
    private static int LAST_RUN_ID = 0;

	public int runId = -1;

    public string content = "";

    public string title = "";

    public Action okEvent;

    public Action noEvent;

    public bool canClose = true;

    public ConfirmInfo(string contentP, string titleP = "", Action okEventP = null, Action noEventP = null, bool canCloseP = true)
	{
		runId = ++LAST_RUN_ID;

        content = contentP;
        title = titleP;
        okEvent = okEventP;
        noEvent = noEventP;
        canClose = canCloseP;
	}
}