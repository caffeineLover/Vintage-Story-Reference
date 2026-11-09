namespace Vintagestory.Common;

public class GameTickListenerBase
{
	public long ListenerId;

	public int Millisecondinterval;

	public long LastUpdateMilliseconds;

	public GameTickListenerBase(long listenerId, int millisecondInterval, long lastUpdateMilliseconds)
	{
		ListenerId = listenerId;
		Millisecondinterval = millisecondInterval;
		LastUpdateMilliseconds = lastUpdateMilliseconds;
	}
}
