using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProtoBuf;
using TentBagReworked.Behaviors;
using TentBagReworked.Config;
using TentBagReworked.Items;
using TentBagReworked.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace TentBagReworked
{
	// Token: 0x02000002 RID: 2
	[NullableContext(1)]
	[Nullable(0)]
	public class TentBagReworkedModSystem : ModSystem
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static TentBagReworkedModSystem Instance { get; private set; } = null;

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002067 File Offset: 0x00000267
		public ICoreAPI Api { get; private set; } = null;

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002070 File Offset: 0x00000270
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002078 File Offset: 0x00000278
		public ICoreClientAPI ClientApi { get; private set; } = null;

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002081 File Offset: 0x00000281
		public ILogger Logger
		{
			get
			{
				return base.Mod.Logger;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000208E File Offset: 0x0000028E
		public string ModId
		{
			get
			{
				return base.Mod.Info.ModID;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000020A0 File Offset: 0x000002A0
		public TentBagReworkedConfig Config
		{
			get
			{
				return this._config ?? new TentBagReworkedConfig();
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020B1 File Offset: 0x000002B1
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000020B8 File Offset: 0x000002B8
		[Nullable(2)]
		public static TentBagReworkedConfig SyncedConfig { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020C0 File Offset: 0x000002C0
		public TentBagReworkedConfig EffectiveConfig
		{
			get
			{
				return TentBagReworkedModSystem.SyncedConfig ?? this.Config;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020D1 File Offset: 0x000002D1
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020D8 File Offset: 0x000002D8
		public static Dictionary<string, List<string>> PendingWaypointNames { get; set; } = new Dictionary<string, List<string>>();

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020E0 File Offset: 0x000002E0
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020E8 File Offset: 0x000002E8
		public Dictionary<string, List<Guid>> SchematicHistory { get; set; } = new Dictionary<string, List<Guid>>();

		// Token: 0x06000011 RID: 17 RVA: 0x000020F4 File Offset: 0x000002F4
		public TentBagReworkedModSystem()
		{
			TentBagReworkedModSystem.Instance = this;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000214A File Offset: 0x0000034A
		public override void StartPre(ICoreAPI api)
		{
			this.Api = api;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002155 File Offset: 0x00000355
		public override void Start(ICoreAPI api)
		{
			api.RegisterCollectibleBehaviorClass("tentbagreworked.packable", typeof(PackableBehavior));
			api.RegisterItemClass("tentbagreworked.itemtentbag", typeof(ItemTentBag));
			TentBagReworkedModSystem.ExportFolderPath = api.GetOrCreateDataPath("ModData/TentBagReworked");
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002194 File Offset: 0x00000394
		public override void StartClientSide(ICoreClientAPI capi)
		{
			this.ClientApi = capi;
			this._clientChannel = this.ClientApi.Network.RegisterChannel(base.Mod.Info.ModID).RegisterMessageType<TentBagReworkedModSystem.ErrorPacket>().RegisterMessageType<TentBagReworkedModSystem.ConfigSyncPacket>().RegisterMessageType<TentBagReworkedModSystem.RequestTentPreviewPacket>().RegisterMessageType<TentBagReworkedModSystem.TentPreviewPacket>().RegisterMessageType<TentBagReworkedModSystem.RotateTentPacket>().SetMessageHandler<TentBagReworkedModSystem.ErrorPacket>(delegate(TentBagReworkedModSystem.ErrorPacket packet)
			{
				bool flag2 = !string.IsNullOrEmpty(packet.Error);
				if (flag2)
				{
					string translatedText = Lang.Get(packet.Error, Array.Empty<object>());
					this.ClientApi.TriggerIngameError(this, "tentbag-error", translatedText);
				}
			}).SetMessageHandler<TentBagReworkedModSystem.ConfigSyncPacket>(new NetworkServerMessageHandler<TentBagReworkedModSystem.ConfigSyncPacket>(this.OnConfigFromServer)).SetMessageHandler<TentBagReworkedModSystem.TentPreviewPacket>(new NetworkServerMessageHandler<TentBagReworkedModSystem.TentPreviewPacket>(this.OnPreviewFromServer));
			this.ClientApi.Input.RegisterHotKey("tentbagreworked-showgraparea", Lang.HotkeyHighlightGrabArea(Array.Empty<object>()), 98, 2, false, false, false);
			this.ClientApi.Input.SetHotKeyHandler("tentbagreworked-showgraparea", new ActionConsumable<KeyCombination>(this.OnToggleGrabArea));
			bool flag = this.ClientApi.Settings.Bool.Exists("tentbagreworked-graparea-shown");
			if (flag)
			{
				this._grabAreaShown = this.ClientApi.Settings.Bool["tentbagreworked-graparea-shown"];
			}
			this.ClientApi.Event.RegisterGameTickListener(delegate(float _)
			{
				bool grabAreaShown = this._grabAreaShown;
				if (grabAreaShown)
				{
					this.UpdateGrabArea();
				}
			}, 150, 0);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022C8 File Offset: 0x000004C8
		private bool OnToggleGrabArea(KeyCombination comb)
		{
			this._grabAreaShown = !this._grabAreaShown;
			this.ClientApi.Settings.Bool["tentbagreworked-graparea-shown"] = this._grabAreaShown;
			bool grabAreaShown = this._grabAreaShown;
			if (grabAreaShown)
			{
				this._lastGrabAreaPos = null;
				this._lastGrabAreaFace = null;
				this.UpdateGrabArea();
			}
			else
			{
				this.ClearGrabArea();
			}
			return true;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002338 File Offset: 0x00000538
		private void UpdateGrabArea()
		{
			ICoreClientAPI clientApi = this.ClientApi;
			IClientPlayer player = (clientApi != null) ? clientApi.World.Player : null;
			BlockSelection sel = (player != null) ? player.CurrentBlockSelection : null;
			ItemStack itemStack;
			if (player == null)
			{
				itemStack = null;
			}
			else
			{
				IPlayerInventoryManager inventoryManager = player.InventoryManager;
				if (inventoryManager == null)
				{
					itemStack = null;
				}
				else
				{
					ItemSlot activeHotbarSlot = inventoryManager.ActiveHotbarSlot;
					itemStack = ((activeHotbarSlot != null) ? activeHotbarSlot.Itemstack : null);
				}
			}
			ItemStack stack = itemStack;
			bool flag3;
			if (stack != null)
			{
				CollectibleObject collectible = stack.Collectible;
				bool? flag;
				if (collectible == null)
				{
					flag = default(bool?);
				}
				else
				{
					CollectibleBehavior[] collectibleBehaviors = collectible.CollectibleBehaviors;
					if (collectibleBehaviors == null)
					{
						flag = default(bool?);
					}
					else
					{
						flag = new bool?(Enumerable.Any<CollectibleBehavior>(collectibleBehaviors, (CollectibleBehavior b) => b is PackableBehavior));
					}
				}
				bool? flag2 = flag;
				if (flag2.GetValueOrDefault())
				{
					ITreeAttribute attributes = stack.Attributes;
					if (((attributes != null) ? attributes.GetString("tent-contents", null) : null) == null)
					{
						ITreeAttribute attributes2 = stack.Attributes;
						flag3 = (((attributes2 != null) ? attributes2.GetString("packed-contents", null) : null) == null);
						goto IL_E8;
					}
				}
			}
			flag3 = false;
			IL_E8:
			bool isEmptyBag = flag3;
			bool flag4 = player == null || ((sel != null) ? sel.Position : null) == null || !isEmptyBag;
			if (flag4)
			{
				bool flag5 = this._lastGrabAreaPos != null;
				if (flag5)
				{
					this.ClearGrabArea();
				}
			}
			else
			{
				bool flag6 = sel.Position.Equals(this._lastGrabAreaPos) && object.Equals(sel.Face, this._lastGrabAreaFace);
				if (!flag6)
				{
					this._lastGrabAreaPos = sel.Position.Copy();
					this._lastGrabAreaFace = sel.Face;
					BlockPos start;
					BlockPos end;
					TentArea.GetPackBounds(this.EffectiveConfig, this.ClientApi.World.BlockAccessor, sel.Position, out start, out end);
					List<BlockPos> edges = TentArea.GetCuboidEdges(start, end);
					string hex = (sel.Face == BlockFacing.UP) ? this.EffectiveConfig.HighlightPickupColor : this.EffectiveConfig.HighlightErrorColor;
					int color = hex.ToColor().Reverse();
					List<int> colors = Enumerable.ToList<int>(Enumerable.Repeat<int>(color, edges.Count));
					this.ClientApi.World.HighlightBlocks(player, 1338, edges, colors, 0, 0, 1f);
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002568 File Offset: 0x00000768
		private void ClearGrabArea()
		{
			this._lastGrabAreaPos = null;
			this._lastGrabAreaFace = null;
			ICoreClientAPI clientApi = this.ClientApi;
			IClientPlayer player = (clientApi != null) ? clientApi.World.Player : null;
			bool flag = player != null;
			if (flag)
			{
				this.ClientApi.World.HighlightBlocks(player, 1338, new List<BlockPos>(), 0, 0);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000025C4 File Offset: 0x000007C4
		public override void StartServerSide(ICoreServerAPI sapi)
		{
			this._sapi = sapi;
			this._channel = sapi.Network.RegisterChannel(base.Mod.Info.ModID).RegisterMessageType<TentBagReworkedModSystem.ErrorPacket>().RegisterMessageType<TentBagReworkedModSystem.ConfigSyncPacket>().RegisterMessageType<TentBagReworkedModSystem.RequestTentPreviewPacket>().RegisterMessageType<TentBagReworkedModSystem.TentPreviewPacket>().RegisterMessageType<TentBagReworkedModSystem.RotateTentPacket>().SetMessageHandler<TentBagReworkedModSystem.RequestTentPreviewPacket>(new NetworkClientMessageHandler<TentBagReworkedModSystem.RequestTentPreviewPacket>(this.OnRequestPreview)).SetMessageHandler<TentBagReworkedModSystem.RotateTentPacket>(new NetworkClientMessageHandler<TentBagReworkedModSystem.RotateTentPacket>(this.OnRotateTent));
			sapi.Event.PlayerJoin += new PlayerDelegate(this.OnPlayerJoin);
			sapi.Event.PlayerDisconnect += new PlayerDelegate(this.OnPlayerDisconnect);
			this.ReloadConfig();
			this.EnsureTentSchematicExists(sapi);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002674 File Offset: 0x00000874
		private void OnPlayerJoin(IServerPlayer byPlayer)
		{
			this.SendConfig(byPlayer);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002680 File Offset: 0x00000880
		private void OnConfigFromServer(TentBagReworkedModSystem.ConfigSyncPacket packet)
		{
			bool flag = string.IsNullOrEmpty(packet.Json);
			if (!flag)
			{
				try
				{
					TentBagReworkedModSystem.SyncedConfig = JsonConvert.DeserializeObject<TentBagReworkedConfig>(packet.Json);
				}
				catch (Exception e)
				{
					ILogger logger = this.Logger;
					string text = "[TentBagReworked] Failed to read synced config: ";
					Exception ex = e;
					logger.Warning(text + ((ex != null) ? ex.ToString() : null));
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026F0 File Offset: 0x000008F0
		public void SendConfig(IServerPlayer player)
		{
			bool flag = this._channel == null;
			if (!flag)
			{
				try
				{
					this._channel.SendPacket<TentBagReworkedModSystem.ConfigSyncPacket>(new TentBagReworkedModSystem.ConfigSyncPacket
					{
						Json = JsonConvert.SerializeObject(this.Config)
					}, new IServerPlayer[]
					{
						player
					});
				}
				catch (Exception e)
				{
					ILogger logger = this.Logger;
					string text = "[TentBagReworked] Failed to send config: ";
					Exception ex = e;
					logger.Warning(text + ((ex != null) ? ex.ToString() : null));
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002778 File Offset: 0x00000978
		public void SendPreviewRequest(bool enable, int x = 0, int y = 0, int z = 0, int dim = 0)
		{
			IClientNetworkChannel clientChannel = this._clientChannel;
			if (clientChannel != null)
			{
				clientChannel.SendPacket<TentBagReworkedModSystem.RequestTentPreviewPacket>(new TentBagReworkedModSystem.RequestTentPreviewPacket
				{
					Enable = enable,
					X = x,
					Y = y,
					Z = z,
					Dim = dim
				});
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000027B7 File Offset: 0x000009B7
		public void SendRotateRequest(int delta = 90)
		{
			IClientNetworkChannel clientChannel = this._clientChannel;
			if (clientChannel != null)
			{
				clientChannel.SendPacket<TentBagReworkedModSystem.RotateTentPacket>(new TentBagReworkedModSystem.RotateTentPacket
				{
					Delta = delta
				});
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027D8 File Offset: 0x000009D8
		private void OnPreviewFromServer(TentBagReworkedModSystem.TentPreviewPacket packet)
		{
			TentHologramRenderer previewInstance = TentHologramRenderer.PreviewInstance;
			if (previewInstance != null)
			{
				previewInstance.OnServerPreviewDim(packet.DimId, packet.PosX, packet.PosY, packet.PosZ);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002804 File Offset: 0x00000A04
		private void OnRequestPreview(IServerPlayer fromPlayer, TentBagReworkedModSystem.RequestTentPreviewPacket packet)
		{
			bool flag = this._sapi == null;
			if (!flag)
			{
				bool flag2 = !packet.Enable;
				if (flag2)
				{
					this.DestroyServerPreview(fromPlayer);
				}
				else
				{
					IPlayerInventoryManager inventoryManager = fromPlayer.InventoryManager;
					ItemStack itemStack;
					if (inventoryManager == null)
					{
						itemStack = null;
					}
					else
					{
						ItemSlot activeHotbarSlot = inventoryManager.ActiveHotbarSlot;
						itemStack = ((activeHotbarSlot != null) ? activeHotbarSlot.Itemstack : null);
					}
					ItemStack stack = itemStack;
					object obj;
					if (stack == null)
					{
						obj = null;
					}
					else
					{
						ITreeAttribute attributes = stack.Attributes;
						obj = ((attributes != null) ? attributes.GetString("packed-contents", null) : null);
					}
					object obj2;
					if ((obj2 = obj) == null)
					{
						if (stack == null)
						{
							obj2 = null;
						}
						else
						{
							ITreeAttribute attributes2 = stack.Attributes;
							obj2 = ((attributes2 != null) ? attributes2.GetString("tent-contents", null) : null);
						}
					}
					string contents = obj2;
					bool flag3 = string.IsNullOrEmpty(contents);
					if (flag3)
					{
						this.DestroyServerPreview(fromPlayer);
					}
					else
					{
						int angle = stack.Attributes.GetInt("packed-rotation", 0);
						IMiniDimension dim;
						int dimId;
						int sizeX;
						int sizeZ;
						bool flag4 = !this.EnsureServerPreview(fromPlayer.PlayerUID, contents, angle, out dim, out dimId, out sizeX, out sizeZ) || dim == null;
						if (!flag4)
						{
							int tdim = packet.Dim;
							int plantRock = this.IsPlantOrRockServer(this._sapi.World.BlockAccessor.GetBlock(new BlockPos(packet.X, packet.Y, packet.Z, tdim))) ? 1 : 0;
							BlockPos corner = new BlockPos(packet.X - sizeX / 2, packet.Y + 1 - plantRock, packet.Z - sizeZ / 2, tdim);
							dim.CurrentPos.SetPos(corner);
							IServerNetworkChannel channel = this._channel;
							if (channel != null)
							{
								channel.SendPacket<TentBagReworkedModSystem.TentPreviewPacket>(new TentBagReworkedModSystem.TentPreviewPacket
								{
									DimId = dimId,
									PosX = corner.X,
									PosY = corner.Y,
									PosZ = corner.Z
								}, new IServerPlayer[]
								{
									fromPlayer
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029C8 File Offset: 0x00000BC8
		private void OnRotateTent(IServerPlayer fromPlayer, TentBagReworkedModSystem.RotateTentPacket packet)
		{
			bool flag = this._sapi == null;
			if (!flag)
			{
				IPlayerInventoryManager inventoryManager = fromPlayer.InventoryManager;
				ItemSlot slot = (inventoryManager != null) ? inventoryManager.ActiveHotbarSlot : null;
				ItemStack stack = (slot != null) ? slot.Itemstack : null;
				object obj;
				if (stack == null)
				{
					obj = null;
				}
				else
				{
					ITreeAttribute attributes = stack.Attributes;
					obj = ((attributes != null) ? attributes.GetString("packed-contents", null) : null);
				}
				object obj2;
				if ((obj2 = obj) == null)
				{
					if (stack == null)
					{
						obj2 = null;
					}
					else
					{
						ITreeAttribute attributes2 = stack.Attributes;
						obj2 = ((attributes2 != null) ? attributes2.GetString("tent-contents", null) : null);
					}
				}
				string contents = obj2;
				bool flag2 = slot == null || ((stack != null) ? stack.Attributes : null) == null || string.IsNullOrEmpty(contents);
				if (!flag2)
				{
					int angle = GameMath.Mod(stack.Attributes.GetInt("packed-rotation", 0) + packet.Delta, 360);
					stack.Attributes.SetInt("packed-rotation", angle);
					slot.MarkDirty();
					this._previewContents[fromPlayer.PlayerUID] = "";
					TentBagReworkedModSystem.SendClientChatMessage(fromPlayer, Lang.RotationInfo(new object[]
					{
						angle
					}));
				}
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002AE0 File Offset: 0x00000CE0
		private bool EnsureServerPreview(string uid, string contents, int angle, [Nullable(2)] out IMiniDimension dim, out int dimId, out int sizeX, out int sizeZ)
		{
			dim = null;
			dimId = -1;
			sizeX = 0;
			sizeZ = 0;
			bool flag = this._sapi == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this._previewDims.TryGetValue(uid, ref dim) || dim == null;
				if (flag2)
				{
					dim = this._sapi.World.BlockAccessor.CreateMiniDimension(new Vec3d());
					int newId = this._sapi.Server.LoadMiniDimension(dim);
					dim.SetSubDimensionId(newId);
					dim.BlocksPreviewSubDimension_Server = newId;
					this._previewDims[uid] = dim;
					this._previewDimIds[uid] = newId;
					this._previewContents[uid] = "";
					this.Logger.VerboseDebug("[TentBagReworked] preview dim created for " + uid + ": id=" + newId.ToString());
				}
				dimId = this._previewDimIds[uid];
				string error = "";
				BlockSchematic schematic = BlockSchematic.LoadFromString(contents, ref error);
				bool flag3 = ((schematic != null) ? schematic.BlockCodes : null) == null || !string.IsNullOrEmpty(error);
				if (flag3)
				{
					this.Logger.Warning("[TentBagReworked] Preview schematic load failed: " + error);
					result = false;
				}
				else
				{
					bool flag4 = angle != 0;
					if (flag4)
					{
						schematic.TransformWhilePacked(this._sapi.World, 1, angle, default(EnumAxis?));
					}
					sizeX = schematic.SizeX;
					sizeZ = schematic.SizeZ;
					string token = contents + "@" + angle.ToString();
					bool flag5 = CollectionExtensions.GetValueOrDefault<string, string>(this._previewContents, uid) != token;
					if (flag5)
					{
						dim.ClearChunks();
						BlockPos pasteOrigin = new BlockPos(0, 0, 0, 1);
						dim.AdjustPosForSubDimension(pasteOrigin);
						schematic.PasteToMiniDimension(this._sapi, this._sapi.World.BlockAccessor, dim, pasteOrigin, true);
						dim.UnloadUnusedServerChunks();
						this._previewContents[uid] = token;
						this.Logger.VerboseDebug(string.Concat(new string[]
						{
							"[TentBagReworked] preview pasted for ",
							uid,
							": ",
							sizeX.ToString(),
							"x",
							schematic.SizeY.ToString(),
							"x",
							sizeZ.ToString(),
							" rot=",
							angle.ToString()
						}));
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002D70 File Offset: 0x00000F70
		[NullableContext(2)]
		private bool IsPlantOrRockServer(Block b)
		{
			bool result;
			if (this.Config.ReplacePlantsAndRocks)
			{
				int? num = (b != null) ? new int?(b.Replaceable) : default(int?);
				if (num != null)
				{
					int valueOrDefault = num.GetValueOrDefault();
					if (valueOrDefault >= 5500)
					{
						result = (valueOrDefault <= 6500);
						goto IL_4E;
					}
				}
				result = false;
				IL_4E:;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002DD0 File Offset: 0x00000FD0
		private void DestroyServerPreview(IServerPlayer player)
		{
			string uid = player.PlayerUID;
			IMiniDimension dim;
			bool flag = this._previewDims.TryGetValue(uid, ref dim);
			if (flag)
			{
				try
				{
					dim.ClearChunks();
					dim.UnloadUnusedServerChunks();
				}
				catch (Exception e)
				{
					ILogger logger = this.Logger;
					string text = "[TentBagReworked] Failed to clear preview: ";
					Exception ex = e;
					logger.Warning(text + ((ex != null) ? ex.ToString() : null));
				}
				this._previewContents[uid] = "";
			}
			IServerNetworkChannel channel = this._channel;
			if (channel != null)
			{
				channel.SendPacket<TentBagReworkedModSystem.TentPreviewPacket>(new TentBagReworkedModSystem.TentPreviewPacket
				{
					DimId = -1
				}, new IServerPlayer[]
				{
					player
				});
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002E80 File Offset: 0x00001080
		private void OnPlayerDisconnect(IServerPlayer player)
		{
			string uid = player.PlayerUID;
			IMiniDimension dim;
			bool flag = this._previewDims.TryGetValue(uid, ref dim);
			if (flag)
			{
				try
				{
					dim.ClearChunks();
					dim.UnloadUnusedServerChunks();
				}
				catch
				{
				}
			}
			this._previewDims.Remove(uid);
			this._previewDimIds.Remove(uid);
			this._previewContents.Remove(uid);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002EF8 File Offset: 0x000010F8
		public void SendClientError([Nullable(2)] IPlayer player, string error)
		{
			IServerPlayer serverPlayer = player as IServerPlayer;
			bool flag = serverPlayer != null;
			if (flag)
			{
				IServerNetworkChannel channel = this._channel;
				if (channel != null)
				{
					channel.SendPacket<TentBagReworkedModSystem.ErrorPacket>(new TentBagReworkedModSystem.ErrorPacket
					{
						Error = error
					}, new IServerPlayer[]
					{
						serverPlayer
					});
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002F40 File Offset: 0x00001140
		public static void SendClientChatMessage([Nullable(2)] IPlayer player, string message)
		{
			IServerPlayer serverPlayer = player as IServerPlayer;
			bool flag = serverPlayer != null;
			if (flag)
			{
				serverPlayer.SendMessage(GlobalConstants.GeneralChatGroup, message, 4, null);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002F70 File Offset: 0x00001170
		public void ReloadConfig()
		{
			this._config = (this.Api.LoadModConfig<TentBagReworkedConfig>(this.ModId + ".json") ?? new TentBagReworkedConfig());
			FileWatcher fileWatcher;
			if ((fileWatcher = this._fileWatcher) == null)
			{
				fileWatcher = (this._fileWatcher = new FileWatcher(this));
			}
			fileWatcher.Queued = true;
			string json = JsonConvert.SerializeObject(this._config, new JsonSerializerSettings
			{
				Formatting = 1,
				NullValueHandling = 1,
				DefaultValueHandling = 0,
				ContractResolver = new CamelCasePropertyNamesContractResolver()
			});
			FileInfo fileInfo = new FileInfo(Path.Combine(GamePaths.ModConfig, this.ModId + ".json"));
			GamePaths.EnsurePathExists(fileInfo.Directory.FullName);
			File.WriteAllText(fileInfo.FullName, json);
			this.Api.Event.RegisterCallback(delegate(float _)
			{
				this._fileWatcher.Queued = false;
			}, 100);
			bool flag = this.Api is ICoreServerAPI && this._channel != null;
			if (flag)
			{
				try
				{
					this._channel.BroadcastPacket<TentBagReworkedModSystem.ConfigSyncPacket>(new TentBagReworkedModSystem.ConfigSyncPacket
					{
						Json = JsonConvert.SerializeObject(this._config)
					}, Array.Empty<IServerPlayer>());
				}
				catch (Exception e)
				{
					ILogger logger = this.Logger;
					string text = "[TentBagReworked] Failed to broadcast config: ";
					Exception ex = e;
					logger.Warning(text + ((ex != null) ? ex.ToString() : null));
				}
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000030E0 File Offset: 0x000012E0
		public override void Dispose()
		{
			FileWatcher fileWatcher = this._fileWatcher;
			if (fileWatcher != null)
			{
				fileWatcher.Dispose();
			}
			this._fileWatcher = null;
			ICoreServerAPI sapi = this.Api as ICoreServerAPI;
			bool flag = sapi != null;
			if (flag)
			{
				sapi.Event.PlayerJoin -= new PlayerDelegate(this.OnPlayerJoin);
				sapi.Event.PlayerDisconnect -= new PlayerDelegate(this.OnPlayerDisconnect);
				foreach (IMiniDimension dim in this._previewDims.Values)
				{
					try
					{
						dim.ClearChunks();
						dim.UnloadUnusedServerChunks();
					}
					catch
					{
					}
				}
			}
			this._previewDims.Clear();
			this._previewDimIds.Clear();
			this._previewContents.Clear();
			TentBagReworkedModSystem.SyncedConfig = null;
			this._channel = null;
			this._clientChannel = null;
			this._sapi = null;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000031FC File Offset: 0x000013FC
		private void EnsureTentSchematicExists(ICoreServerAPI sapi)
		{
			string worldEditDir = Path.Combine(GamePaths.DataPath, "WorldEdit");
			GamePaths.EnsurePathExists(worldEditDir);
			string targetFilePath = Path.Combine(worldEditDir, "tent.json");
			bool flag = !File.Exists(targetFilePath);
			if (flag)
			{
				AssetLocation assetLocation = new AssetLocation("tentbagreworked", "config/tent.json");
				IAsset asset = sapi.Assets.Get(assetLocation);
				bool flag2 = asset != null;
				if (flag2)
				{
					try
					{
						File.WriteAllBytes(targetFilePath, asset.Data);
					}
					catch (Exception e)
					{
						this.Logger.Error("[TentBagReworked] Fehler beim Kopieren von tent.json: " + e.Message);
					}
				}
				else
				{
					this.Logger.Warning("[TentBagReworked] Die ursprüngliche Datei „tent.json“ wurde in den Mod-Assets nicht gefunden!");
				}
			}
		}

		// Token: 0x04000007 RID: 7
		internal static string ExportFolderPath = null;

		// Token: 0x04000008 RID: 8
		[Nullable(2)]
		private TentBagReworkedConfig _config;

		// Token: 0x04000009 RID: 9
		[Nullable(2)]
		private FileWatcher _fileWatcher;

		// Token: 0x0400000A RID: 10
		[Nullable(2)]
		private IServerNetworkChannel _channel;

		// Token: 0x0400000B RID: 11
		[Nullable(2)]
		private IClientNetworkChannel _clientChannel;

		// Token: 0x0400000C RID: 12
		private const string GrabAreaHotkey = "tentbagreworked-showgraparea";

		// Token: 0x0400000D RID: 13
		private const int GrabAreaHighlightId = 1338;

		// Token: 0x0400000E RID: 14
		private const string GrabAreaShownSetting = "tentbagreworked-graparea-shown";

		// Token: 0x0400000F RID: 15
		private bool _grabAreaShown;

		// Token: 0x04000010 RID: 16
		[Nullable(2)]
		private BlockPos _lastGrabAreaPos;

		// Token: 0x04000011 RID: 17
		[Nullable(2)]
		private BlockFacing _lastGrabAreaFace;

		// Token: 0x04000012 RID: 18
		[Nullable(2)]
		private ICoreServerAPI _sapi;

		// Token: 0x04000013 RID: 19
		private readonly Dictionary<string, IMiniDimension> _previewDims = new Dictionary<string, IMiniDimension>();

		// Token: 0x04000014 RID: 20
		private readonly Dictionary<string, int> _previewDimIds = new Dictionary<string, int>();

		// Token: 0x04000015 RID: 21
		private readonly Dictionary<string, string> _previewContents = new Dictionary<string, string>();

		// Token: 0x0200000B RID: 11
		[NullableContext(0)]
		[ProtoContract(ImplicitFields = 2)]
		private class ErrorPacket
		{
			// Token: 0x04000049 RID: 73
			[Nullable(2)]
			public string Error;
		}

		// Token: 0x0200000C RID: 12
		[NullableContext(0)]
		[ProtoContract(ImplicitFields = 2)]
		private class ConfigSyncPacket
		{
			// Token: 0x0400004A RID: 74
			[Nullable(2)]
			public string Json;
		}

		// Token: 0x0200000D RID: 13
		[NullableContext(0)]
		[ProtoContract(ImplicitFields = 2)]
		private class RequestTentPreviewPacket
		{
			// Token: 0x0400004B RID: 75
			public bool Enable;

			// Token: 0x0400004C RID: 76
			public int X;

			// Token: 0x0400004D RID: 77
			public int Y;

			// Token: 0x0400004E RID: 78
			public int Z;

			// Token: 0x0400004F RID: 79
			public int Dim;
		}

		// Token: 0x0200000E RID: 14
		[NullableContext(0)]
		[ProtoContract(ImplicitFields = 2)]
		private class TentPreviewPacket
		{
			// Token: 0x04000050 RID: 80
			public int DimId;

			// Token: 0x04000051 RID: 81
			public int PosX;

			// Token: 0x04000052 RID: 82
			public int PosY;

			// Token: 0x04000053 RID: 83
			public int PosZ;
		}

		// Token: 0x0200000F RID: 15
		[NullableContext(0)]
		[ProtoContract(ImplicitFields = 2)]
		private class RotateTentPacket
		{
			// Token: 0x04000054 RID: 84
			public int Delta = 90;
		}
	}
}
