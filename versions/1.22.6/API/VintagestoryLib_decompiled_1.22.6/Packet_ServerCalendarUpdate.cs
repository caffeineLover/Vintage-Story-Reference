public class Packet_ServerCalendarUpdate
{
	public long TotalSeconds;

	public long TotalSecondsStart;

	public const int TotalSecondsFieldID = 1;

	public const int TotalSecondsStartFieldID = 2;

	public int size;

	public void SetTotalSeconds(long value)
	{
		TotalSeconds = value;
	}

	public void SetTotalSecondsStart(long value)
	{
		TotalSecondsStart = value;
	}

	internal void InitializeValues()
	{
	}
}
