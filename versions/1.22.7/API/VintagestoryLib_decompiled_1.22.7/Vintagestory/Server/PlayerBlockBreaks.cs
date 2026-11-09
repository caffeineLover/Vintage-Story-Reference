namespace Vintagestory.Server;

public class PlayerBlockBreaks
{
	public long[] EventsRingBuffer;

	public ushort LastIndex;

	public int TotalEvents;

	public long LogoutTotalMs;

	public PlayerBlockBreaks()
	{
		EventsRingBuffer = new long[PlayerAntiAbuseMonitor.BufferSize];
		LastIndex = 0;
	}
}
