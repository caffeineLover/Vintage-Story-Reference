using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace Vintagestory.Client.NoObf;

public class ClientEventManager : EventManager
{
	public OnAmbientSoundScanCompleteDelegate OnAmbientSoundsScanComplete;

	public CurrentTrackSupplierDelegate CurrentTrackSupplier;

	public TrackStarterDelegate TrackStarter;

	public TrackStarterLoadedDelegate TrackStarterLoaded;

	public Dictionary<Vec3i, List<RetesselationListener>> OnChunkRetesselated = new Dictionary<Vec3i, List<RetesselationListener>>();

	public List<Action<bool>> OnGameWindowFocus = new List<Action<bool>>();

	public List<Action<GuiDialog>> OnDialogOpened = new List<Action<GuiDialog>>();

	public List<Action<GuiDialog>> OnDialogClosed = new List<Action<GuiDialog>>();

	public List<BlockChangedDelegate> OnBlockChanged = new List<BlockChangedDelegate>();

	public List<Action> OnPlayerModeChange = new List<Action>();

	public List<ActionBoolReturn> OnReloadShaders = new List<ActionBoolReturn>();

	public List<EntityDelegate> OnEntitySpawn = new List<EntityDelegate>();

	public List<EntityDelegate> OnEntityLoaded = new List<EntityDelegate>();

	public List<EntityDespawnDelegate> OnEntityDespawn = new List<EntityDespawnDelegate>();

	public List<PlayerDeathDelegate> OnPlayerDeath = new List<PlayerDeathDelegate>();

	public Dictionary<EnumProperty, List<OnPlayerPropertyChanged>> OnPlayerPropertyChanged = new Dictionary<EnumProperty, List<OnPlayerPropertyChanged>>();

	public List<Action> OnActiveSlotChanged = new List<Action>();

	public List<ChatLineDelegate> OnNewServerToClientChatLine = new List<ChatLineDelegate>();

	public List<ChatLineDelegate> OnNewClientToServerChatLine = new List<ChatLineDelegate>();

	public List<ChatLineDelegate> OnNewClientOnlyChatLine = new List<ChatLineDelegate>();

	public List<EventBusListener> EventBusListeners = new List<EventBusListener>();

	public List<RenderHandler>[] renderersByStage;

	public List<RenderOrderReservation>[] renderOrderReservationsByStage;

	private ClientMain game;

	public override ILogger Logger => ScreenManager.Platform.Logger;

	public override string CommandPrefix => ".";

	public override long InWorldEllapsedMs => game.InWorldEllapsedMs;

	public event HighlightBlocksDelegate OnHighlightBlocks;

	public event UpdateLightingDelegate OnUpdateLighting;

	public event ChunkLoadedDelegate OnChunkLoaded;

	public event Action<BlockDamage> OnUnBreakingBlock;

	public event Action<BlockDamage> OnPlayerBreakingBlock;

	public event Action<BlockDamage> OnPlayerBrokenBlock;

	public event Action OnReloadShapes;

	public event Action OnReloadTextures;

	public event IngameErrorDelegate InGameError;

	public event IngameDiscoveryDelegate InGameDiscovery;

	public event Action ColorPresetChanged;

	public ClientEventManager(ClientMain game)
	{
		this.game = game;
		renderersByStage = new List<RenderHandler>[Enum.GetNames(typeof(EnumRenderStage)).Length];
		for (int i = 0; i < renderersByStage.Length; i++)
		{
			renderersByStage[i] = new List<RenderHandler>();
		}
		renderOrderReservationsByStage = new List<RenderOrderReservation>[renderersByStage.Length];
		for (int j = 0; j < renderOrderReservationsByStage.Length; j++)
		{
			renderOrderReservationsByStage[j] = new List<RenderOrderReservation>();
		}
	}

	public void RegisterRenderer(Action<float> handler, EnumRenderStage stage, string profilingName, double renderOrder)
	{
		RegisterRenderer(new DummyRenderer
		{
			action = handler,
			RenderOrder = renderOrder
		}, stage, profilingName);
	}

	public void RegisterRenderer(IRenderer handler, EnumRenderStage stage, string profilingName)
	{
		List<RenderHandler> renderers = renderersByStage[(int)stage];
		int pos = 0;
		double newRenderOrder = handler.RenderOrder;
		foreach (RenderOrderReservation reservation in renderOrderReservationsByStage[(int)stage])
		{
			if (reservation.Conflicts(handler, newRenderOrder))
			{
				throw new Exception("When registering " + handler.GetType().Name + ", its RenderOrder fell within a range " + reservation.rangeStart + "-" + reservation.rangeEnd + " which is already reserved");
			}
		}
		foreach (RenderHandler renderer in renderers)
		{
			if (newRenderOrder > renderer.Renderer.RenderOrder)
			{
				pos++;
				continue;
			}
			break;
		}
		renderers.Insert(pos, new RenderHandler
		{
			Renderer = handler,
			ProfilingName = stage.ToString() + "-" + profilingName
		});
	}

	public void ReserveRenderOrderRange(IRenderer renderer, EnumRenderStage stage, double reservedFirstOrder, double reservedLastOrder, Type otherType)
	{
		renderOrderReservationsByStage[(int)stage].Add(new RenderOrderReservation
		{
			rangeStart = reservedFirstOrder,
			rangeEnd = reservedLastOrder,
			allowedType1 = renderer.GetType(),
			allowedType2 = otherType
		});
	}

	public void UnregisterRenderer(IRenderer handler, EnumRenderStage stage)
	{
		List<RenderHandler> renderers = renderersByStage[(int)stage];
		RenderHandler rh = renderers.FirstOrDefault((RenderHandler x) => x.Renderer == handler);
		if (rh != null)
		{
			renderers.Remove(rh);
		}
	}

	public void TriggerHighlightBlocks(IPlayer player, int slotId, List<BlockPos> blocks, List<int> colors, EnumHighlightBlocksMode mode = EnumHighlightBlocksMode.Absolute, EnumHighlightShape shape = EnumHighlightShape.Arbitrary, float scale = 1f)
	{
		Trigger(this.OnHighlightBlocks?.GetInvocationList(), "OnHighlightBlocks", delegate(HighlightBlocksDelegate dele)
		{
			dele?.Invoke(player, slotId, blocks, colors, mode, shape, scale);
		});
	}

	public void TriggerRenderStage(EnumRenderStage stage, float dt)
	{
		List<RenderHandler> renderers = renderersByStage[(int)stage];
		if (game.extendedDebugInfo)
		{
			for (int i = 0; i < renderers.Count; i++)
			{
				renderers[i].Renderer.OnRenderFrame(dt, stage);
				ScreenManager.Platform.CheckGlError(renderers[i].ProfilingName);
				ScreenManager.FrameProfiler.Mark(renderers[i].ProfilingName);
			}
		}
		else
		{
			for (int j = 0; j < renderers.Count; j++)
			{
				renderers[j].Renderer.OnRenderFrame(dt, stage);
			}
		}
	}

	public void TriggerLightingUpdate(int oldBlockId, int newBlockId, BlockPos pos, Dictionary<BlockPos, BlockUpdate> blockUpdatesBulk = null)
	{
		Trigger(this.OnUpdateLighting?.GetInvocationList(), "OnUpdateLighting", delegate(UpdateLightingDelegate dele)
		{
			dele?.Invoke(oldBlockId, newBlockId, pos, blockUpdatesBulk);
		});
	}

	public void TriggerChunkLoaded(Vec3i chunkpos)
	{
		Trigger(this.OnChunkLoaded?.GetInvocationList(), "OnChunkLoaded", delegate(ChunkLoadedDelegate dele)
		{
			dele?.Invoke(chunkpos);
		});
	}

	public void RegisterReloadShaders(ActionBoolReturn handler)
	{
		OnReloadShaders.Add(handler);
	}

	public void UnregisterReloadShaders(ActionBoolReturn handler)
	{
		OnReloadShaders.Remove(handler);
	}

	public void RegisterOnChunkRetesselated(Vec3i chunkPos, int atQuantityDrawn, Action listener)
	{
		if (Environment.CurrentManagedThreadId != RuntimeEnv.MainThreadId)
		{
			game.EnqueueMainThreadTask(delegate
			{
				if (!OnChunkRetesselated.TryGetValue(chunkPos, out var value))
				{
					value = new List<RetesselationListener>();
					OnChunkRetesselated[chunkPos] = value;
				}
				if (listener == null)
				{
					throw new ArgumentNullException("Listener cannot be null");
				}
				value.Add(new RetesselationListener
				{
					AtDrawCount = atQuantityDrawn,
					Handler = listener
				});
			}, "reg-chunkretess");
		}
		else
		{
			if (!OnChunkRetesselated.TryGetValue(chunkPos, out var listeners))
			{
				listeners = new List<RetesselationListener>();
				OnChunkRetesselated[chunkPos] = listeners;
			}
			if (listener == null)
			{
				throw new ArgumentNullException("Listener cannot be null");
			}
			listeners.Add(new RetesselationListener
			{
				AtDrawCount = atQuantityDrawn,
				Handler = listener
			});
		}
	}

	public void TriggerChunkRetesselated(Vec3i chunkPos, ClientChunk chunk)
	{
		if (!OnChunkRetesselated.TryGetValue(chunkPos, out var listeners))
		{
			return;
		}
		int i = 0;
		try
		{
			for (i = 0; i < listeners.Count; i++)
			{
				RetesselationListener listener = listeners[i];
				if (listener.AtDrawCount < chunk.quantityDrawn)
				{
					listener.Handler();
					listeners.RemoveAt(i);
					i--;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Chunk retesselated listener number " + i + " threw an exception (a=" + (listeners == null) + ", b=" + (listeners?[i] == null) + ", b=" + (listeners?[i]?.Handler == null) + ")\n" + ex);
		}
	}

	public void TriggerDialogOpened(GuiDialog dialog)
	{
		foreach (Action<GuiDialog> item in OnDialogOpened)
		{
			item(dialog);
		}
	}

	public void TriggerDialogClosed(GuiDialog dialog)
	{
		foreach (Action<GuiDialog> item in OnDialogClosed)
		{
			item(dialog);
		}
	}

	public void TriggerGameWindowFocus(bool focus)
	{
		foreach (Action<bool> item in OnGameWindowFocus)
		{
			item(focus);
		}
	}

	public void TriggerNewModChatLine(int groupid, string message, EnumChatType chattype, string data)
	{
		foreach (ChatLineDelegate item in OnNewClientOnlyChatLine)
		{
			item(groupid, message, chattype, data);
		}
	}

	public void TriggerNewClientChatLine(int groupid, string message, EnumChatType chattype, string data)
	{
		foreach (ChatLineDelegate item in OnNewClientToServerChatLine)
		{
			item(groupid, message, chattype, data);
		}
	}

	public void TriggerIngameError(object sender, string errorCode, string text)
	{
		Trigger(this.InGameError?.GetInvocationList(), "InGameError", delegate(IngameErrorDelegate dele)
		{
			dele?.Invoke(sender, errorCode, text);
		});
	}

	public void TriggerIngameDiscovery(object sender, string errorCode, string text)
	{
		Trigger(this.InGameDiscovery?.GetInvocationList(), "InGameDiscovery", delegate(IngameDiscoveryDelegate dele)
		{
			dele?.Invoke(sender, errorCode, text);
		});
	}

	public void TriggerColorPresetChanged()
	{
		Trigger(this.ColorPresetChanged?.GetInvocationList(), "ColorPresetChanged", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public void TriggerNewServerChatLine(int groupid, string message, EnumChatType chattype, string data)
	{
		foreach (ChatLineDelegate item in OnNewServerToClientChatLine)
		{
			item(groupid, message, chattype, data);
		}
	}

	public bool TriggerBeforeActiveSlotChanged(ClientMain game, int fromSlot, int toSlot)
	{
		if (!game.api.eventapi.TriggerBeforeActiveSlotChanged(game.Logger, fromSlot, toSlot))
		{
			return false;
		}
		foreach (Action handler in OnActiveSlotChanged)
		{
			try
			{
				handler();
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnActiveSlotChanged. Will skip to next event");
				Logger.Error(e);
			}
		}
		return true;
	}

	public void TriggerAfterActiveSlotChanged(ClientMain game, int fromSlot, int toSlot)
	{
		game.api.eventapi.TriggerAfterActiveSlotChanged(game.Logger, fromSlot, toSlot);
	}

	public void TriggerBlockBreaking(BlockDamage blockDamage)
	{
		if (this.OnPlayerBreakingBlock == null)
		{
			return;
		}
		Delegate[] invocationList = this.OnPlayerBreakingBlock.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			Action<BlockDamage> handler = (Action<BlockDamage>)invocationList[i];
			try
			{
				handler(blockDamage);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnPlayerBreakingBlock. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	internal void TriggerBlockUnbreaking(BlockDamage damagedBlock)
	{
		if (this.OnUnBreakingBlock == null)
		{
			return;
		}
		Delegate[] invocationList = this.OnUnBreakingBlock.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			Action<BlockDamage> handler = (Action<BlockDamage>)invocationList[i];
			try
			{
				handler(damagedBlock);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnUnBreakingBlock. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public void TriggerBlockBroken(BlockDamage blockDamage)
	{
		if (this.OnPlayerBrokenBlock == null)
		{
			return;
		}
		Delegate[] invocationList = this.OnPlayerBrokenBlock.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			Action<BlockDamage> handler = (Action<BlockDamage>)invocationList[i];
			try
			{
				handler(blockDamage);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnPlayerBrokenBlock. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public void TriggerPlayerModeChange()
	{
		foreach (Action handler in OnPlayerModeChange)
		{
			try
			{
				handler();
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnPlayerModeChange. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public void TriggerReloadShapes()
	{
		Trigger(this.OnReloadShapes?.GetInvocationList(), "OnReloadShapes", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public void TriggerReloadTextures()
	{
		Trigger(this.OnReloadTextures?.GetInvocationList(), "OnReloadTextures", delegate(Action dele)
		{
			dele?.Invoke();
		});
	}

	public bool TriggerReloadShaders()
	{
		if (ShaderRegistry.SupressShaderAndBufferReloads)
		{
			return true;
		}
		bool ok = true;
		foreach (ActionBoolReturn handler in OnReloadShaders)
		{
			try
			{
				ok &= handler();
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnReloadShaders. Will skip to next event");
				Logger.Error(e);
			}
		}
		return ok;
	}

	public void TriggerEntitySpawn(Entity entity)
	{
		foreach (EntityDelegate handler in OnEntitySpawn)
		{
			try
			{
				handler(entity);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnEntitySpawn. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public void TriggerEntityLoaded(Entity entity)
	{
		foreach (EntityDelegate handler in OnEntityLoaded)
		{
			try
			{
				handler(entity);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnEntityLoaded. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public void TriggerEntityDespawn(Entity entity, EntityDespawnData despawnReason)
	{
		foreach (EntityDespawnDelegate handler in OnEntityDespawn)
		{
			try
			{
				handler(entity, despawnReason);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnEntityDespawn. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public void RegisterPlayerPropertyChangedWatcher(EnumProperty property, OnPlayerPropertyChanged handler)
	{
		OnPlayerPropertyChanged.TryGetValue(property, out var watchers);
		if (watchers == null)
		{
			watchers = (OnPlayerPropertyChanged[property] = new List<OnPlayerPropertyChanged>());
		}
		watchers.Add(handler);
	}

	internal void TriggerPlayerDeath(int clientId, int livesLeft)
	{
		foreach (PlayerDeathDelegate handler in OnPlayerDeath)
		{
			try
			{
				handler(clientId, livesLeft);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnPlayerDeath. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	internal void TriggerBlockChanged(ClientMain game, BlockPos pos, Block oldBlock)
	{
		game.api.eventapi.TriggerBlockChanged(pos, oldBlock);
		foreach (BlockChangedDelegate handler in OnBlockChanged)
		{
			try
			{
				handler(pos, oldBlock);
			}
			catch (Exception e)
			{
				Logger.Error("Mod exception during event OnBlockChanged. Will skip to next event");
				Logger.Error(e);
			}
		}
	}

	public override bool HasPrivilege(string playerUid, string privilegecode)
	{
		return true;
	}

	public void TriggerOnMouseEnterSlot(ClientMain game, ItemSlot slot)
	{
		game.player.inventoryMgr.currentHoveredSlot = slot;
		foreach (GuiDialog loadedGui in game.LoadedGuis)
		{
			if (loadedGui.OnMouseEnterSlot(slot))
			{
				return;
			}
		}
		for (int i = 0; i < game.clientSystems.Length && !game.clientSystems[i].OnMouseEnterSlot(slot); i++)
		{
		}
	}

	public void TriggerOnMouseLeaveSlot(ClientMain game, ItemSlot itemSlot)
	{
		game.player.inventoryMgr.currentHoveredSlot = null;
		foreach (GuiDialog loadedGui in game.LoadedGuis)
		{
			if (loadedGui.OnMouseLeaveSlot(itemSlot))
			{
				return;
			}
		}
		for (int i = 0; i < game.clientSystems.Length && !game.clientSystems[i].OnMouseLeaveSlot(itemSlot); i++)
		{
		}
	}

	internal void Dispose()
	{
		List<RenderHandler>[] array = renderersByStage;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (RenderHandler item in new List<RenderHandler>(array[i]))
			{
				item.Renderer.Dispose();
			}
		}
		this.ColorPresetChanged = null;
	}
}
