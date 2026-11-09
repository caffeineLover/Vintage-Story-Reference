using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace Vintagestory.Client.NoObf;

public class ClientEventAPI : IClientEventAPI, IEventAPI
{
	private static string strThreadException = "Cannot call this method outside the main thread. Not thread safe to do so. You can use the thread safe method .EnqueueMainThreadTask() to queue up the register on the main thread instead.";

	private ClientMain game;

	internal Dictionary<int, ItemRenderDelegate>[][] itemStackRenderersByTarget;

	public event MouseEventDelegate MouseDown;

	public event MouseEventDelegate MouseUp;

	public event MouseEventDelegate MouseMove;

	public event MouseWheelEventDelegate MouseWheelMove;

	public event KeyEventDelegate KeyDown;

	public event KeyEventDelegate KeyUp;

	public event FileDropDelegate FileDrop;

	public event PlayerEventDelegate PlayerJoin;

	public event PlayerEventDelegate PlayerLeave;

	public event PlayerEventDelegate PlayerEntitySpawn;

	public event PlayerEventDelegate PlayerEntityDespawn;

	public event Action BlockTexturesLoaded;

	public event IsPlayerReadyDelegate IsPlayerReady;

	public event OnGamePauseResume PauseResume;

	public event Action LeaveWorld;

	public event Action LeftWorld;

	public event ChatLineDelegate ChatMessage;

	public event BlockChangedDelegate BlockChanged;

	public event Vintagestory.API.Common.Func<ActiveSlotChangeEventArgs, EnumHandling> BeforeActiveSlotChanged;

	public event Action<ActiveSlotChangeEventArgs> AfterActiveSlotChanged;

	public event ChunkDirtyDelegate ChunkDirty;

	public event Action LevelFinalize;

	public event Action HotkeysChanged;

	public event ClientChatLineDelegate OnSendChatMessage;

	public event EntityDeathDelegate OnEntityDeath;

	public event MatchGridRecipeDelegate MatchesGridRecipe;

	public event MatchRecipeDelegate MatchesRecipe;

	public event PlayerEventDelegate PlayerDeath;

	public event TestBlockAccessDelegate OnTestBlockAccess;

	public event TestBlockAccessClaimDelegate OnTestBlockAccessClaim;

	public event MapRegionLoadedDelegate MapRegionLoaded;

	public event MapRegionUnloadDelegate MapRegionUnloaded;

	public event PlayerCommonDelegate PlayerDimensionChanged;

	public event EntityMountDelegate EntityMounted;

	public event EntityMountDelegate EntityUnmounted;

	public event Action<BlockDamage> OnUnBreakingBlock
	{
		add
		{
			game.eventManager.OnUnBreakingBlock += value;
		}
		remove
		{
			game.eventManager.OnUnBreakingBlock -= value;
		}
	}

	public event Action<BlockDamage> OnPlayerBreakingBlock
	{
		add
		{
			game.eventManager.OnPlayerBreakingBlock += value;
		}
		remove
		{
			game.eventManager.OnPlayerBreakingBlock -= value;
		}
	}

	public event Action<BlockDamage> OnPlayerBrokenBlock
	{
		add
		{
			game.eventManager.OnPlayerBrokenBlock += value;
		}
		remove
		{
			game.eventManager.OnPlayerBrokenBlock -= value;
		}
	}

	public event OnGetClimateDelegate OnGetClimate
	{
		add
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnGetClimate += value;
			}
		}
		remove
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnGetClimate -= value;
			}
		}
	}

	public event TestBlockAccessDelegate TestBlockAccess
	{
		add
		{
			OnTestBlockAccess += value;
		}
		remove
		{
			OnTestBlockAccess -= value;
		}
	}

	public event OnGetWindSpeedDelegate OnGetWindSpeed
	{
		add
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnGetWindSpeed += value;
			}
		}
		remove
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnGetWindSpeed -= value;
			}
		}
	}

	public event IngameErrorDelegate InGameError
	{
		add
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.InGameError += value;
			}
		}
		remove
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.InGameError -= value;
			}
		}
	}

	public event IngameDiscoveryDelegate InGameDiscovery
	{
		add
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.InGameDiscovery += value;
			}
		}
		remove
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.InGameDiscovery -= value;
			}
		}
	}

	public event Action ColorsPresetChanged
	{
		add
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.ColorPresetChanged += value;
			}
		}
		remove
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.ColorPresetChanged -= value;
			}
		}
	}

	public event EntityDelegate OnEntitySpawn
	{
		add
		{
			game.eventManager?.OnEntitySpawn.Add(value);
		}
		remove
		{
			game.eventManager?.OnEntitySpawn.Remove(value);
		}
	}

	public event EntityDelegate OnEntityLoaded
	{
		add
		{
			game.eventManager?.OnEntityLoaded.Add(value);
		}
		remove
		{
			game.eventManager?.OnEntityLoaded.Remove(value);
		}
	}

	public event EntityDespawnDelegate OnEntityDespawn
	{
		add
		{
			game.eventManager?.OnEntityDespawn.Add(value);
		}
		remove
		{
			game.eventManager?.OnEntityDespawn.Remove(value);
		}
	}

	public event ActionBoolReturn ReloadShader
	{
		add
		{
			game.eventManager?.OnReloadShaders.Add(value);
		}
		remove
		{
			game.eventManager?.OnReloadShaders.Remove(value);
		}
	}

	public event Action ReloadTextures
	{
		add
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnReloadTextures += value;
			}
		}
		remove
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnReloadTextures -= value;
			}
		}
	}

	public event Action ReloadShapes
	{
		add
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnReloadShapes += value;
			}
		}
		remove
		{
			ClientEventManager em = game.eventManager;
			if (em != null)
			{
				em.OnReloadShapes -= value;
			}
		}
	}

	public ClientEventAPI(ClientMain game)
	{
		this.game = game;
		game.eventManager?.OnNewServerToClientChatLine.Add(onChatLine);
		game.eventManager?.OnPlayerDeath.Add(playerDeath);
		int len = Enum.GetNames(typeof(EnumItemRenderTarget)).Length;
		itemStackRenderersByTarget = new Dictionary<int, ItemRenderDelegate>[2][];
		itemStackRenderersByTarget[0] = new Dictionary<int, ItemRenderDelegate>[len];
		itemStackRenderersByTarget[1] = new Dictionary<int, ItemRenderDelegate>[len];
		for (int i = 0; i < len; i++)
		{
			itemStackRenderersByTarget[0][i] = new Dictionary<int, ItemRenderDelegate>();
			itemStackRenderersByTarget[1][i] = new Dictionary<int, ItemRenderDelegate>();
		}
	}

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
				game.Logger.Error("Mod exception during event " + eventName + ". Will skip to next event");
				game.Logger.Error(e);
				onException?.Invoke();
			}
		}
	}

	private void playerDeath(int clientid, int livesLeft)
	{
		IClientPlayer player = game.AllPlayers.FirstOrDefault((IPlayer plr) => plr.ClientId == clientid) as IClientPlayer;
		Trigger(this.PlayerDeath?.GetInvocationList(), "PlayerDeath", delegate(PlayerEventDelegate dele)
		{
			dele?.Invoke(player);
		});
	}

	public EnumWorldAccessResponse TriggerTestBlockAccess(IPlayer player, BlockSelection blockSel, EnumBlockAccessFlags accessType, ref string claimant, LandClaim claim, EnumWorldAccessResponse response)
	{
		if (this.OnTestBlockAccess != null)
		{
			Delegate[] invocationList = this.OnTestBlockAccess.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				response = ((TestBlockAccessDelegate)invocationList[i])(player, blockSel, accessType, ref claimant, response);
			}
		}
		if (this.OnTestBlockAccessClaim != null)
		{
			Delegate[] invocationList = this.OnTestBlockAccessClaim.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				response = ((TestBlockAccessClaimDelegate)invocationList[i])(player, blockSel, accessType, ref claimant, claim, response);
			}
		}
		return response;
	}

	public void TriggerMapregionLoaded(Vec2i mapCoord, IMapRegion mapregion)
	{
		Trigger(this.MapRegionLoaded?.GetInvocationList(), "MapRegionLoaded", delegate(MapRegionLoadedDelegate dele)
		{
			dele?.Invoke(mapCoord, mapregion);
		});
	}

	public void TriggerMapregionUnloaded(Vec2i mapCoord, IMapRegion mapregion)
	{
		Trigger(this.MapRegionUnloaded?.GetInvocationList(), "MapRegionUnloaded", delegate(MapRegionUnloadDelegate dele)
		{
			dele?.Invoke(mapCoord, mapregion);
		});
	}

	public void TriggerChunkDirty(Vec3i chunkCoord, IWorldChunk chunk, EnumChunkDirtyReason reason)
	{
		Trigger(this.ChunkDirty?.GetInvocationList(), "ChunkDirty", delegate(ChunkDirtyDelegate dele)
		{
			dele?.Invoke(chunkCoord, chunk, reason);
		});
	}

	private void onChatLine(int groupId, string message, EnumChatType chattype, string data)
	{
		Trigger(this.ChatMessage?.GetInvocationList(), "ChatMessage", delegate(ChatLineDelegate dele)
		{
			dele?.Invoke(groupId, message, chattype, data);
		});
	}

	public void TriggerBlockChanged(BlockPos pos, Block oldBlock)
	{
		Trigger(this.BlockChanged?.GetInvocationList(), "BlockChanged", delegate(BlockChangedDelegate dele)
		{
			dele?.Invoke(pos, oldBlock);
		});
	}

	public void TriggerPlayerJoin(IClientPlayer plr)
	{
		Trigger(this.PlayerJoin?.GetInvocationList(), "PlayerJoin", delegate(PlayerEventDelegate dele)
		{
			dele?.Invoke(plr);
		});
	}

	public void TriggerPlayerLeave(IClientPlayer plr)
	{
		Trigger(this.PlayerLeave?.GetInvocationList(), "PlayerLeave", delegate(PlayerEventDelegate dele)
		{
			dele?.Invoke(plr);
		});
	}

	public void TriggerPlayerEntitySpawn(IClientPlayer plr)
	{
		Trigger(this.PlayerEntitySpawn?.GetInvocationList(), "PlayerEntitySpawn", delegate(PlayerEventDelegate dele)
		{
			dele?.Invoke(plr);
		});
	}

	public void TriggerPlayerEntityDespawn(IClientPlayer plr)
	{
		Trigger(this.PlayerEntityDespawn?.GetInvocationList(), "PlayerEntityDespawn", delegate(PlayerEventDelegate dele)
		{
			dele?.Invoke(plr);
		});
	}

	public bool TriggerFileDrop(string filename)
	{
		if (this.FileDrop == null)
		{
			return false;
		}
		FileDropEvent ev = new FileDropEvent
		{
			Filename = filename
		};
		Delegate[] invocationList = this.FileDrop.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			FileDropDelegate handler = (FileDropDelegate)invocationList[i];
			try
			{
				handler(ev);
				if (ev.Handled)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event FileDrop. Will skip to next event");
				game.Logger.Error(e);
			}
		}
		return ev.Handled;
	}

	public void TriggerPauseResume(bool pause)
	{
		Trigger(this.PauseResume?.GetInvocationList(), "PauseResume", delegate(OnGamePauseResume dele)
		{
			dele?.Invoke(pause);
		});
	}

	public void TriggerMouseDown(MouseEvent ev)
	{
		if (this.MouseDown == null)
		{
			return;
		}
		Delegate[] invocationList = this.MouseDown.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			MouseEventDelegate handler = (MouseEventDelegate)invocationList[i];
			try
			{
				handler(ev);
				if (ev.Handled)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event MouseDown. Will skip to next event");
				game.Logger.Error(e);
			}
		}
	}

	public void TriggerMouseUp(MouseEvent ev)
	{
		if (this.MouseUp == null)
		{
			return;
		}
		Delegate[] invocationList = this.MouseUp.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			MouseEventDelegate handler = (MouseEventDelegate)invocationList[i];
			try
			{
				handler(ev);
				if (ev.Handled)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event MouseUp. Will skip to next event");
				game.Logger.Error(e);
			}
		}
	}

	public void TriggerMouseMove(MouseEvent ev)
	{
		if (this.MouseMove == null)
		{
			return;
		}
		Delegate[] invocationList = this.MouseMove.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			MouseEventDelegate handler = (MouseEventDelegate)invocationList[i];
			try
			{
				handler(ev);
				if (ev.Handled)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event MouseMove. Will skip to next event");
				game.Logger.Error(e);
			}
		}
	}

	public void TriggerMouseWheelMove(MouseWheelEventArgs ev)
	{
		if (this.MouseWheelMove == null)
		{
			return;
		}
		Delegate[] invocationList = this.MouseWheelMove.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			MouseWheelEventDelegate handler = (MouseWheelEventDelegate)invocationList[i];
			try
			{
				handler(ev);
				if (ev.IsHandled)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event MouseWheelMove. Will skip to next event");
				game.Logger.Error(e);
			}
		}
	}

	public void TriggerKeyDown(KeyEvent ev)
	{
		if (this.KeyDown == null)
		{
			return;
		}
		Delegate[] invocationList = this.KeyDown.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			KeyEventDelegate handler = (KeyEventDelegate)invocationList[i];
			try
			{
				handler(ev);
				if (ev.Handled)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event KeyDown. Will skip to next event");
				game.Logger.Error(e);
			}
		}
	}

	public void TriggerKeyUp(KeyEvent ev)
	{
		if (this.KeyUp == null)
		{
			return;
		}
		Delegate[] invocationList = this.KeyUp.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			KeyEventDelegate handler = (KeyEventDelegate)invocationList[i];
			try
			{
				handler(ev);
				if (ev.Handled)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event KeyUp. Will skip to next event");
				game.Logger.Error(e);
			}
		}
	}

	internal bool TriggerBeforeActiveSlotChanged(ILogger logger, int fromSlot, int toSlot)
	{
		ActiveSlotChangeEventArgs args = new ActiveSlotChangeEventArgs(fromSlot, toSlot);
		return this.BeforeActiveSlotChanged?.InvokeSafeCancellable(logger, "BeforeActiveSlotChanged", args) ?? true;
	}

	internal void TriggerAfterActiveSlotChanged(ILogger logger, int fromSlot, int toSlot)
	{
		ActiveSlotChangeEventArgs args = new ActiveSlotChangeEventArgs(fromSlot, toSlot);
		this.AfterActiveSlotChanged?.InvokeSafe(logger, "AfterActiveSlotChanged", args);
	}

	public void TriggerLevelFinalize()
	{
		Trigger(this.LevelFinalize?.GetInvocationList(), "LevelFinalize", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public void TriggerLeaveWorld()
	{
		Trigger(this.LeaveWorld?.GetInvocationList(), "LeaveWorld", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public void TriggerLeftWorld()
	{
		Trigger(this.LeftWorld?.GetInvocationList(), "LeftWorld", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public void TriggerHotkeysChanged()
	{
		Trigger(this.HotkeysChanged?.GetInvocationList(), "HotkeysChanged", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public void TriggerBlockTexturesLoaded()
	{
		Trigger(this.BlockTexturesLoaded?.GetInvocationList(), "BlockTexturesLoaded", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public bool TriggerIsPlayerReady()
	{
		if (this.IsPlayerReady == null)
		{
			return true;
		}
		EnumHandling handling = EnumHandling.PassThrough;
		bool ok = true;
		Delegate[] invocationList = this.IsPlayerReady.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			IsPlayerReadyDelegate val = (IsPlayerReadyDelegate)invocationList[i];
			try
			{
				bool hereOk = val(ref handling);
				if (handling != EnumHandling.PassThrough)
				{
					ok = ok && hereOk;
				}
				if (handling == EnumHandling.PreventSubsequent)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event IsPlayerReady. Will skip to next event");
				game.Logger.Error(e);
			}
		}
		return ok;
	}

	public void RegisterRenderer(IRenderer renderer, EnumRenderStage stage, string profilingName = null)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Cannot call this method outside the main thread. Not thread safe to do so.");
		}
		game.eventManager?.RegisterRenderer(renderer, stage, profilingName);
	}

	public void RegisterRenderer(IRenderer renderer, EnumRenderStage renderStage, string profilingName, double reservedFirstOrder, double reservedLastOrder, Type firstType)
	{
		RegisterRenderer(renderer, renderStage, profilingName);
		game.eventManager?.ReserveRenderOrderRange(renderer, renderStage, reservedFirstOrder, reservedLastOrder, firstType);
	}

	public void RegisterItemstackRenderer(CollectibleObject forObj, ItemRenderDelegate rendererDelegate, EnumItemRenderTarget target)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Cannot call this method outside the main thread. Not thread safe to do so.");
		}
		itemStackRenderersByTarget[(int)forObj.ItemClass][(int)target][forObj.Id] = rendererDelegate;
	}

	public void UnregisterItemstackRenderer(CollectibleObject forObj, EnumItemRenderTarget target)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Cannot call this method outside the main thread. Not thread safe to do so.");
		}
		itemStackRenderersByTarget[(int)forObj.ItemClass][(int)target].Remove(forObj.Id);
	}

	public void UnregisterRenderer(IRenderer renderer, EnumRenderStage stage)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException("Cannot call this method outside the main thread. Not thread safe to do so.");
		}
		game.eventManager?.UnregisterRenderer(renderer, stage);
	}

	public void RegisterReloadShapes(Action handler)
	{
		ClientEventManager em = game.eventManager;
		if (em != null)
		{
			em.OnReloadShapes += handler;
		}
	}

	public void UnregisterReloadShapes(Action handler)
	{
		ClientEventManager em = game.eventManager;
		if (em != null)
		{
			em.OnReloadShapes -= handler;
		}
	}

	public void RegisterOnLeaveWorld(Action handler)
	{
		LeaveWorld += handler;
	}

	public void PushEvent(string eventName, IAttribute data = null)
	{
		EnumHandling handling = EnumHandling.PassThrough;
		for (int i = 0; i < game.eventManager?.EventBusListeners.Count; i++)
		{
			EventBusListener listener = game.eventManager?.EventBusListeners[i];
			if (listener.filterByName == null || listener.filterByName.Equals(eventName))
			{
				listener.handler(eventName, ref handling, data);
			}
			if (handling == EnumHandling.PreventSubsequent)
			{
				break;
			}
		}
	}

	public void RegisterEventBusListener(EventBusListenerDelegate OnEvent, double priority = 0.5, string filterByEventName = null)
	{
		if (game.eventManager == null)
		{
			return;
		}
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		for (int i = 0; i < game.eventManager.EventBusListeners.Count; i++)
		{
			if (!(game.eventManager.EventBusListeners[i].priority >= priority))
			{
				game.eventManager?.EventBusListeners.Insert(i, new EventBusListener
				{
					handler = OnEvent,
					priority = priority,
					filterByName = filterByEventName
				});
				return;
			}
		}
		game.eventManager?.EventBusListeners.Add(new EventBusListener
		{
			handler = OnEvent,
			priority = priority,
			filterByName = filterByEventName
		});
	}

	public void UnregisterEventBusListener(EventBusListenerDelegate OnEvent)
	{
		if (game.eventManager == null)
		{
			return;
		}
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		List<EventBusListener> list = game.eventManager.EventBusListeners;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].handler == OnEvent)
			{
				list.RemoveAt(i);
				break;
			}
		}
	}

	public long RegisterGameTickListener(Action<float> OnGameTick, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterGameTickListener(OnGameTick, millisecondInterval, initialDelayOffsetMs);
	}

	public long RegisterGameTickListener(Action<float> OnGameTick, Action<Exception> errorHandler, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterGameTickListener(OnGameTick, errorHandler, millisecondInterval, initialDelayOffsetMs);
	}

	public long RegisterGameTickListener(Action<IWorldAccessor, BlockPos, float> OnGameTick, BlockPos pos, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterGameTickListener(OnGameTick, pos, null, millisecondInterval, initialDelayOffsetMs);
	}

	public long RegisterGameTickListener(Action<float> OnGameTick, BlockPos pos, Action<Exception> errorHandler, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterGameTickListener(OnGameTick, pos, errorHandler, millisecondInterval, initialDelayOffsetMs);
	}

	public long RegisterGameTickListener(Action<IWorldAccessor, BlockPos, float> OnGameTick, BlockPos pos, Action<Exception> errorHandler, int millisecondInterval, int initialDelayOffsetMs = 0)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterGameTickListener(OnGameTick, pos, errorHandler, millisecondInterval, initialDelayOffsetMs);
	}

	public long RegisterCallback(Action<float> OnTimePassed, int millisecondDelay)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterCallback(OnTimePassed, millisecondDelay);
	}

	public long RegisterCallback(Action<float> OnTimePassed, int millisecondDelay, bool permittedWhilePaused)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterCallback(OnTimePassed, millisecondDelay, permittedWhilePaused);
	}

	public long RegisterCallback(Action<IWorldAccessor, BlockPos, float> OnTimePassed, BlockPos pos, int millisecondDelay)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		return game.RegisterCallback(OnTimePassed, pos, millisecondDelay);
	}

	public void UnregisterCallback(long listenerId)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		game.UnregisterCallback(listenerId);
	}

	public void UnregisterGameTickListener(long listenerId)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			throw new InvalidOperationException(strThreadException);
		}
		game.UnregisterGameTickListener(listenerId);
	}

	public void EnqueueMainThreadTask(Action action, string code)
	{
		game.EnqueueMainThreadTask(action, code);
	}

	internal void TriggerSendChatMessage(int groupId, ref string message, ref EnumHandling handling)
	{
		if (this.OnSendChatMessage == null)
		{
			return;
		}
		Delegate[] invocationList = this.OnSendChatMessage.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			ClientChatLineDelegate val = (ClientChatLineDelegate)invocationList[i];
			try
			{
				val(groupId, ref message, ref handling);
				if (handling == EnumHandling.PreventSubsequent)
				{
					break;
				}
			}
			catch (Exception e)
			{
				game.Logger.Error("Mod exception during event OnSendChatMessage. Will skip to next event");
				game.Logger.Error(e);
			}
		}
	}

	public void RegisterAsyncParticleSpawner(ContinousParticleSpawnTaskDelegate handler)
	{
		lock (game.asyncParticleSpawnersLock)
		{
			game.asyncParticleSpawners.Add(handler);
		}
	}

	public void TriggerEntityDeath(Entity entity, DamageSource damageSourceForDeath)
	{
		Trigger(this.OnEntityDeath?.GetInvocationList(), "OnEntityDeath", delegate(EntityDeathDelegate dele)
		{
			dele?.Invoke(entity, damageSourceForDeath);
		});
	}

	public bool TriggerMatchesRecipe(IPlayer forPlayer, GridRecipe gridRecipe, ItemSlot[] ingredients, int gridWidth)
	{
		if (this.MatchesGridRecipe == null)
		{
			return true;
		}
		return this.MatchesGridRecipe(forPlayer, gridRecipe, ingredients, gridWidth);
	}

	public bool TriggerMatchesRecipe(IPlayer forPlayer, IRecipeBase recipe, ItemSlot[] ingredients)
	{
		if (this.MatchesRecipe == null)
		{
			return true;
		}
		return this.MatchesRecipe(forPlayer, recipe, ingredients);
	}

	public void TriggerPlayerDimensionChanged(IPlayer player)
	{
		Trigger(this.PlayerDimensionChanged?.GetInvocationList(), "PlayerDimensionChanged", delegate(PlayerCommonDelegate dele)
		{
			dele?.Invoke(player);
		});
	}

	public void TriggerEntityMounted(EntityAgent entityAgent, IMountableSeat entityRideableSeat)
	{
		Trigger(this.EntityMounted?.GetInvocationList(), "EntityMounted", delegate(EntityMountDelegate dele)
		{
			dele?.Invoke(entityAgent, entityRideableSeat);
		});
	}

	public void TriggerEntityUnmounted(EntityAgent entityAgent, IMountableSeat entityRideableSeat)
	{
		Trigger(this.EntityUnmounted?.GetInvocationList(), "EntityUnmounted", delegate(EntityMountDelegate dele)
		{
			dele?.Invoke(entityAgent, entityRideableSeat);
		});
	}
}
