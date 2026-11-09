using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Common;

public class GameTickListenerBlock : GameTickListenerBase
{
	public BlockPos Pos;

	public Action<IWorldAccessor, BlockPos, float>? Handler;

	public Action<float>? HandlerBare;

	public Action<Exception>? ErrorHandler;

	public string? ProfilerName => ((Handler == null) ? HandlerBare?.Target?.GetType() : Handler.Target?.GetType())?.ToString();

	public GameTickListenerBlock(long listenerId, int millisecondInterval, long lastUpdateMilliseconds, BlockPos pos, Action<IWorldAccessor, BlockPos, float> handler, Action<Exception> errorHandler)
		: base(listenerId, millisecondInterval, lastUpdateMilliseconds)
	{
		Pos = pos;
		Handler = handler;
		ErrorHandler = errorHandler;
	}

	public GameTickListenerBlock(long listenerId, int millisecondInterval, long lastUpdateMilliseconds, BlockPos pos, Action<float> handlerBare, Action<Exception> errorHandler)
		: base(listenerId, millisecondInterval, lastUpdateMilliseconds)
	{
		Pos = pos;
		HandlerBare = handlerBare;
		ErrorHandler = errorHandler;
	}

	public void OnTriggered(IWorldAccessor worldAccessor, BlockPos pos, long ellapsedMilliseconds)
	{
		try
		{
			if (HandlerBare != null)
			{
				HandlerBare((float)(ellapsedMilliseconds - LastUpdateMilliseconds) / 1000f);
			}
			else
			{
				Handler(worldAccessor, pos, (float)(ellapsedMilliseconds - LastUpdateMilliseconds) / 1000f);
			}
			LastUpdateMilliseconds = ellapsedMilliseconds;
		}
		catch (Exception obj)
		{
			if (ErrorHandler == null)
			{
				throw;
			}
			ErrorHandler(obj);
		}
	}

	public override string ToString()
	{
		if (Handler == null)
		{
			return $"{HandlerBare?.Target}:{HandlerBare?.Method}@{Pos}";
		}
		return $"{Handler.Target}:{Handler.Method}@{Pos}";
	}
}
