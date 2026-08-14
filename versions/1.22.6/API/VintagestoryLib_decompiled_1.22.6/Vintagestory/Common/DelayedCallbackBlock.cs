using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Common;

public class DelayedCallbackBlock : DelayedCallbackBase
{
	public BlockPos Pos;

	public Action<IWorldAccessor, BlockPos, float> Handler;

	public string? ProfileName => Handler.Target?.GetType().ToString();

	public DelayedCallbackBlock(long listenerId, long callAtEllapsedMilliseconds, BlockPos pos, Action<IWorldAccessor, BlockPos, float> handler)
		: base(listenerId, callAtEllapsedMilliseconds)
	{
		Pos = pos;
		Handler = handler;
	}

	public override string ToString()
	{
		return $"{Handler.Target}:{Handler.Method}";
	}
}
