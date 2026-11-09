using System;

namespace Vintagestory.Common;

public class DelayedCallback : DelayedCallbackBase
{
	public Action<float> Handler;

	public string? ProfileName => Handler.Target?.GetType().ToString();

	public DelayedCallback(long listenerId, long callAtEllapsedMilliseconds, Action<float> handler)
		: base(listenerId, callAtEllapsedMilliseconds)
	{
		Handler = handler;
	}

	public override string ToString()
	{
		return $"{Handler.Target}:{Handler.Method}";
	}
}
