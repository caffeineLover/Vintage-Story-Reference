using System;

namespace Vintagestory.Common;

public class GameTickListener : GameTickListenerBase
{
	public Action<float> Handler;

	public Action<Exception> ErrorHandler;

	public string? ProfilerName => Handler.Target?.GetType().ToString();

	public GameTickListener(long listenerId, int millisecondInterval, long lastUpdateMilliseconds, Action<float> handler, Action<Exception> errorHandler)
		: base(listenerId, millisecondInterval, lastUpdateMilliseconds)
	{
		Handler = handler;
		ErrorHandler = errorHandler;
	}

	public void OnTriggered(long ellapsedMilliseconds)
	{
		try
		{
			Handler((float)(ellapsedMilliseconds - LastUpdateMilliseconds) / 1000f);
		}
		catch (Exception obj)
		{
			if (ErrorHandler == null)
			{
				throw;
			}
			ErrorHandler(obj);
		}
		LastUpdateMilliseconds = ellapsedMilliseconds;
	}

	public override string ToString()
	{
		return $"{Handler.Target}:{Handler.Method}";
	}
}
