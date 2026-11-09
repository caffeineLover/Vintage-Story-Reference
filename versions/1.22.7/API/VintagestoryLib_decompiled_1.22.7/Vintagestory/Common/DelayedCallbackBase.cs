namespace Vintagestory.Common;

public class DelayedCallbackBase
{
	public long CallAtEllapsedMilliseconds;

	public long ListenerId;

	public DelayedCallbackBase(long listenerId, long callAtEllapsedMilliseconds)
	{
		CallAtEllapsedMilliseconds = callAtEllapsedMilliseconds;
		ListenerId = listenerId;
	}
}
