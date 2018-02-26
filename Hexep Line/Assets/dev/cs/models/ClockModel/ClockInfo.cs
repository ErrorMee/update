using System;

public class ClockInfo
{
	public int id;

	public int value;

	private int second;

	public ClockInfo (int idP, int valueP = 0, int secondP = -1)
	{
		id = idP;
		value = valueP;
		if (secondP == -1) {
			second = ClockModel.ConvertDateTimeInt(System.DateTime.Now);
		} else {
			second = secondP;
		}
	}

	public void Correct(int currentSecond)
	{
		value += (currentSecond - second);
		second = currentSecond;
	}
}