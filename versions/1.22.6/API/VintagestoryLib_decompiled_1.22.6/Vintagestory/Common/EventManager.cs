using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Common;

public abstract class EventManager
{
	private long listenerId;

	private long callBackId;

	internal List<GameTickListener> GameTickListenersEntity = new List<GameTickListener>();

	internal ConcurrentDictionary<long, DelayedCallback> DelayedCallbacksEntity = new ConcurrentDictionary<long, DelayedCallback>(4, 16);

	internal List<GameTickListenerBlock> GameTickListenersBlock = new List<GameTickListenerBlock>();

	internal List<DelayedCallbackBlock> DelayedCallbacksBlock = new List<DelayedCallbackBlock>();

	internal ConcurrentDictionary<long, int> GameTickListenersEntityIndices = new ConcurrentDictionary<long, int>(4, 64);

	internal ConcurrentDictionary<long, int> GameTickListenersBlockIndices = new ConcurrentDictionary<long, int>(4, 64);

	internal Dictionary<BlockPos, DelayedCallbackBlock> SingleDelayedCallbacksBlock = new Dictionary<BlockPos, DelayedCallbackBlock>();

	private List<DelayedCallback> deletable = new List<DelayedCallback>();

	public abstract ILogger Logger { get; }

	public abstract string CommandPrefix { get; }

	public abstract long InWorldEllapsedMs { get; }

	public event OnGetClimateDelegate OnGetClimate;

	public event OnGetWindSpeedDelegate OnGetWindSpeed;

	public abstract bool HasPrivilege(string playerUid, string privilegecode);

	public void Trigger<T>(Delegate[] delegates, string eventName, Action<T> onDele, Action onException = null) where T : Delegate
	{
		if (delegates == null)
		{
			return;
		}
		for (int i = 0; i < delegates.Length; i++)
		{
			T dele = (T)delegates[i];
			try
			{
				onDele(dele);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event " + eventName + ". Will skip to next event");
				Logger.Error(e);
				onException?.Invoke();
			}
		}
	}

	public void Trigger<T>(Delegate[] delegates, string eventName, ActionBoolReturn<T> onDele, Action onException = null) where T : Delegate
	{
		if (delegates == null)
		{
			return;
		}
		for (int i = 0; i < delegates.Length; i++)
		{
			T dele = (T)delegates[i];
			try
			{
				if (!onDele(dele))
				{
					break;
				}
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event " + eventName + ". Will skip to next event");
				Logger.Error(e);
				onException?.Invoke();
			}
		}
	}

	public virtual void TriggerOnGetClimate(ref ClimateCondition climate, BlockPos pos, EnumGetClimateMode mode = EnumGetClimateMode.WorldGenValues, double totalDays = 0.0)
	{
		if (this.OnGetClimate == null)
		{
			return;
		}
		Delegate[] invocationList = this.OnGetClimate.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			OnGetClimateDelegate val = (OnGetClimateDelegate)invocationList[i];
			try
			{
				val(ref climate, pos, mode, totalDays);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnGetClimate. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public virtual void TriggerOnGetWindSpeed(Vec3d pos, ref Vec3d windSpeed)
	{
		if (this.OnGetWindSpeed == null)
		{
			return;
		}
		Delegate[] invocationList = this.OnGetWindSpeed.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			OnGetWindSpeedDelegate val = (OnGetWindSpeedDelegate)invocationList[i];
			try
			{
				val(pos, ref windSpeed);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnGetWindSpeed. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public virtual void TriggerGameTick(long ellapsedMilliseconds, IWorldAccessor world)
	{
		FrameProfilerUtil FrameProfiler = world.FrameProfiler;
		List<GameTickListener> gameTickListenersEntity = GameTickListenersEntity;
		for (int i = 0; i < gameTickListenersEntity.Count; i++)
		{
			GameTickListener listener = gameTickListenersEntity[i];
			if (listener != null && ellapsedMilliseconds - listener.LastUpdateMilliseconds > listener.Millisecondinterval)
			{
				listener.OnTriggered(ellapsedMilliseconds);
			}
		}
		FrameProfiler.Mark("tick-gtentity");
		List<GameTickListenerBlock> gameTickListenersBlock = GameTickListenersBlock;
		for (int j = 0; j < gameTickListenersBlock.Count; j++)
		{
			GameTickListenerBlock listener2 = gameTickListenersBlock[j];
			if (listener2 != null && ellapsedMilliseconds - listener2.LastUpdateMilliseconds > listener2.Millisecondinterval)
			{
				listener2.OnTriggered(world, listener2.Pos, ellapsedMilliseconds);
			}
		}
		FrameProfiler.Mark("tick-gtblock");
		deletable.Clear();
		foreach (KeyValuePair<long, DelayedCallback> entry in DelayedCallbacksEntity)
		{
			if (ellapsedMilliseconds - entry.Value.CallAtEllapsedMilliseconds >= 0)
			{
				DelayedCallback callback = entry.Value;
				callback.Handler((float)(ellapsedMilliseconds - callback.CallAtEllapsedMilliseconds) / 1000f);
				deletable.Add(callback);
			}
		}
		FrameProfiler.Mark("tick-dcentity");
		foreach (DelayedCallback callback2 in deletable)
		{
			DelayedCallbacksEntity.TryRemove(callback2.ListenerId, out var _);
		}
		List<DelayedCallbackBlock> delayedCallbacksBlock = DelayedCallbacksBlock;
		for (int k = 0; k < delayedCallbacksBlock.Count; k++)
		{
			DelayedCallbackBlock callback3 = delayedCallbacksBlock[k];
			if (ellapsedMilliseconds - callback3.CallAtEllapsedMilliseconds >= 0)
			{
				delayedCallbacksBlock.RemoveAt(k);
				k--;
				callback3.Handler(world, callback3.Pos, (float)(ellapsedMilliseconds - callback3.CallAtEllapsedMilliseconds) / 1000f);
			}
		}
		Dictionary<BlockPos, DelayedCallbackBlock> singleDelayedCallbacksBlock = SingleDelayedCallbacksBlock;
		if (singleDelayedCallbacksBlock.Count > 0)
		{
			foreach (BlockPos pos in new List<BlockPos>(singleDelayedCallbacksBlock.Keys))
			{
				DelayedCallbackBlock callback4 = singleDelayedCallbacksBlock[pos];
				if (ellapsedMilliseconds - callback4.CallAtEllapsedMilliseconds >= 0)
				{
					singleDelayedCallbacksBlock.Remove(pos);
					callback4.Handler(world, callback4.Pos, (float)(ellapsedMilliseconds - callback4.CallAtEllapsedMilliseconds) / 1000f);
				}
			}
		}
		FrameProfiler.Mark("tick-dcblock");
	}

	public virtual void TriggerGameTickDebug(long ellapsedMilliseconds, IWorldAccessor world)
	{
		List<GameTickListener> gameTickListenersEntity = GameTickListenersEntity;
		for (int i = 0; i < gameTickListenersEntity.Count; i++)
		{
			GameTickListener listener = gameTickListenersEntity[i];
			if (listener != null && ellapsedMilliseconds - listener.LastUpdateMilliseconds > listener.Millisecondinterval)
			{
				listener.OnTriggered(ellapsedMilliseconds);
				world.FrameProfiler.Mark("gmle", listener.ProfilerName);
			}
		}
		List<GameTickListenerBlock> gameTickListenersBlock = GameTickListenersBlock;
		for (int j = 0; j < gameTickListenersBlock.Count; j++)
		{
			GameTickListenerBlock listener2 = gameTickListenersBlock[j];
			if (listener2 != null && ellapsedMilliseconds - listener2.LastUpdateMilliseconds > listener2.Millisecondinterval)
			{
				listener2.OnTriggered(world, listener2.Pos, ellapsedMilliseconds);
				world.FrameProfiler.Mark("gmlb", listener2.ProfilerName);
			}
		}
		deletable.Clear();
		foreach (KeyValuePair<long, DelayedCallback> entry in DelayedCallbacksEntity)
		{
			if (ellapsedMilliseconds - entry.Value.CallAtEllapsedMilliseconds >= 0)
			{
				DelayedCallback callback = entry.Value;
				callback.Handler((float)(ellapsedMilliseconds - callback.CallAtEllapsedMilliseconds) / 1000f);
				deletable.Add(callback);
				world.FrameProfiler.Mark("dce", callback.ProfileName);
			}
		}
		foreach (DelayedCallback callback2 in deletable)
		{
			DelayedCallbacksEntity.TryRemove(callback2.ListenerId, out var _);
		}
		List<DelayedCallbackBlock> delayedCallbacksBlock = DelayedCallbacksBlock;
		for (int k = 0; k < delayedCallbacksBlock.Count; k++)
		{
			DelayedCallbackBlock callback3 = delayedCallbacksBlock[k];
			if (ellapsedMilliseconds - callback3.CallAtEllapsedMilliseconds >= 0)
			{
				delayedCallbacksBlock.RemoveAt(k);
				k--;
				callback3.Handler(world, callback3.Pos, (float)(ellapsedMilliseconds - callback3.CallAtEllapsedMilliseconds) / 1000f);
				world.FrameProfiler.Mark("dcb", callback3.ProfileName);
			}
		}
		Dictionary<BlockPos, DelayedCallbackBlock> singleDelayedCallbacksBlock = SingleDelayedCallbacksBlock;
		if (singleDelayedCallbacksBlock.Count <= 0)
		{
			return;
		}
		foreach (BlockPos pos in new List<BlockPos>(singleDelayedCallbacksBlock.Keys))
		{
			DelayedCallbackBlock callback4 = singleDelayedCallbacksBlock[pos];
			if (ellapsedMilliseconds - callback4.CallAtEllapsedMilliseconds >= 0)
			{
				singleDelayedCallbacksBlock.Remove(pos);
				callback4.Handler(world, callback4.Pos, (float)(ellapsedMilliseconds - callback4.CallAtEllapsedMilliseconds) / 1000f);
				world.FrameProfiler.Mark("sdcb", callback4.ProfileName);
			}
		}
	}

	public virtual long AddGameTickListener(Action<float> handler, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		return AddGameTickListener(handler, null, millisecondInterval, initialDelayOffsetMs);
	}

	public virtual long AddGameTickListener(Action<float> handler, Action<Exception> errorHandler, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		long newListenerId = ++listenerId;
		GameTickListener listener = new GameTickListener(newListenerId, millisecondInterval, InWorldEllapsedMs + initialDelayOffsetMs, handler, errorHandler);
		List<GameTickListener> gameTickListenersEntity = GameTickListenersEntity;
		for (int i = 0; i < gameTickListenersEntity.Count; i++)
		{
			if (gameTickListenersEntity[i] == null)
			{
				gameTickListenersEntity[i] = listener;
				GameTickListenersEntityIndices[newListenerId] = i;
				if (gameTickListenersEntity[GameTickListenersEntityIndices[newListenerId]] != listener)
				{
					throw new InvalidOperationException("Failed to add listener properly");
				}
				return newListenerId;
			}
		}
		gameTickListenersEntity.Add(listener);
		GameTickListenersEntityIndices[newListenerId] = gameTickListenersEntity.Count - 1;
		if (gameTickListenersEntity[GameTickListenersEntityIndices[newListenerId]] != listener)
		{
			throw new InvalidOperationException("Failed to add listener properly");
		}
		return newListenerId;
	}

	public virtual long AddGameTickListener(Action<IWorldAccessor, BlockPos, float> handler, BlockPos pos, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		return AddGameTickListener(handler, pos, null, millisecondInterval, initialDelayOffsetMs);
	}

	public virtual long AddGameTickListener(Action<float> handler, BlockPos pos, Action<Exception> errorHandler, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		long newListenerId = ++listenerId;
		GameTickListenerBlock listener = new GameTickListenerBlock(newListenerId, millisecondInterval, InWorldEllapsedMs + initialDelayOffsetMs, pos.Copy(), handler, errorHandler);
		return AddGameTickListenerBlockInternal(listener, newListenerId);
	}

	public virtual long AddGameTickListener(Action<IWorldAccessor, BlockPos, float> handler, BlockPos pos, Action<Exception> errorHandler, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		long newListenerId = ++listenerId;
		GameTickListenerBlock listener = new GameTickListenerBlock(newListenerId, millisecondInterval, InWorldEllapsedMs + initialDelayOffsetMs, pos.Copy(), handler, errorHandler);
		return AddGameTickListenerBlockInternal(listener, newListenerId);
	}

	private long AddGameTickListenerBlockInternal(GameTickListenerBlock listener, long newListenerId)
	{
		List<GameTickListenerBlock> gameTickListenersBlock = GameTickListenersBlock;
		for (int i = 0; i < gameTickListenersBlock.Count; i++)
		{
			if (gameTickListenersBlock[i] == null)
			{
				gameTickListenersBlock[i] = listener;
				GameTickListenersBlockIndices[newListenerId] = i;
				return newListenerId;
			}
		}
		gameTickListenersBlock.Add(listener);
		GameTickListenersBlockIndices[newListenerId] = gameTickListenersBlock.Count - 1;
		return newListenerId;
	}

	public virtual long AddDelayedCallback(Action<float> handler, long callAfterEllapsedMS)
	{
		long newCallbackId = Interlocked.Increment(ref callBackId);
		DelayedCallback newCallback = new DelayedCallback(newCallbackId, InWorldEllapsedMs + callAfterEllapsedMS, handler);
		DelayedCallbacksEntity[newCallbackId] = newCallback;
		return newCallbackId;
	}

	public virtual long AddDelayedCallback(Action<IWorldAccessor, BlockPos, float> handler, BlockPos pos, long callAfterEllapsedMS)
	{
		long newCallbackId = Interlocked.Increment(ref callBackId);
		DelayedCallbacksBlock.Add(new DelayedCallbackBlock(newCallbackId, InWorldEllapsedMs + callAfterEllapsedMS, pos.Copy(), handler));
		return newCallbackId;
	}

	internal virtual long AddSingleDelayedCallback(Action<IWorldAccessor, BlockPos, float> handler, BlockPos pos, long callAfterEllapsedMs)
	{
		BlockPos cpos = pos.Copy();
		long newCallbackId = Interlocked.Increment(ref callBackId);
		SingleDelayedCallbacksBlock[cpos] = new DelayedCallbackBlock(newCallbackId, InWorldEllapsedMs + callAfterEllapsedMs, cpos, handler);
		return newCallbackId;
	}

	public void RemoveDelayedCallback(long callbackId)
	{
		if (callbackId == 0L || DelayedCallbacksEntity.TryRemove(callbackId, out var _))
		{
			return;
		}
		foreach (DelayedCallbackBlock val in DelayedCallbacksBlock)
		{
			if (val.ListenerId == callbackId)
			{
				DelayedCallbacksBlock.Remove(val);
				return;
			}
		}
		foreach (KeyValuePair<BlockPos, DelayedCallbackBlock> val2 in SingleDelayedCallbacksBlock)
		{
			if (val2.Value.ListenerId == callbackId)
			{
				SingleDelayedCallbacksBlock.Remove(val2.Key);
				break;
			}
		}
	}

	public void RemoveGameTickListener(long listenerId)
	{
		if (listenerId == 0L)
		{
			return;
		}
		int indexB;
		if (GameTickListenersEntityIndices.TryRemove(listenerId, out var index))
		{
			GameTickListener listener = GameTickListenersEntity[index];
			if (listener != null && listener.ListenerId == listenerId)
			{
				GameTickListenersEntity[index] = null;
				return;
			}
		}
		else if (GameTickListenersBlockIndices.TryRemove(listenerId, out indexB))
		{
			GameTickListenerBlock listener2 = GameTickListenersBlock[indexB];
			if (listener2 != null && listener2.ListenerId == listenerId)
			{
				GameTickListenersBlock[indexB] = null;
				return;
			}
		}
		List<GameTickListener> GameTickListenersEntityLocal = GameTickListenersEntity;
		for (int i = 0; i < GameTickListenersEntityLocal.Count; i++)
		{
			GameTickListener listener3 = GameTickListenersEntityLocal[i];
			if (listener3 != null && listener3.ListenerId == listenerId)
			{
				GameTickListenersEntityLocal[i] = null;
				return;
			}
		}
		List<GameTickListenerBlock> GameTickListenersBlockLocal = GameTickListenersBlock;
		for (int j = 0; j < GameTickListenersBlockLocal.Count; j++)
		{
			GameTickListenerBlock listener4 = GameTickListenersBlockLocal[j];
			if (listener4 != null && listener4.ListenerId == listenerId)
			{
				GameTickListenersBlockLocal[j] = null;
				break;
			}
		}
	}
}
