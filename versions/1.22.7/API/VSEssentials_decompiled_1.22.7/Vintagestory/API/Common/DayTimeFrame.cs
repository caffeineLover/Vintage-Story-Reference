namespace Vintagestory.API.Common;

public readonly struct DayTimeFrame(double fromHour, double toHour)
{
	public readonly double FromHour = fromHour;

	public readonly double ToHour = toHour;

	public bool Matches(double hourOfDay)
	{
		if (!(FromHour <= hourOfDay) || !(ToHour >= hourOfDay))
		{
			if (FromHour > ToHour)
			{
				if (!(FromHour <= hourOfDay))
				{
					return ToHour >= hourOfDay;
				}
				return true;
			}
			return false;
		}
		return true;
	}
}
