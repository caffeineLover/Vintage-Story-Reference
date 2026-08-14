using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TentBagReworked.Config;
using TentBagReworked.Util;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace TentBagReworked.Behaviors
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	public class PackableBehavior : CollectibleBehavior
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00005488 File Offset: 0x00003688
		private TentBagReworkedConfig Config
		{
			get
			{
				return this._modSystem.Config;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00005495 File Offset: 0x00003695
		private Dictionary<string, List<Guid>> SchematicHistory
		{
			get
			{
				return this._modSystem.SchematicHistory;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000054A2 File Offset: 0x000036A2
		private static string ExportFolderPath
		{
			get
			{
				return TentBagReworkedModSystem.ExportFolderPath;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000054A9 File Offset: 0x000036A9
		[NullableContext(2)]
		private static bool IsAirOrNull(Block block)
		{
			return block == null || block.Replaceable >= 9505;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000054C4 File Offset: 0x000036C4
		[NullableContext(2)]
		private bool IsPlantOrRock(Block block)
		{
			bool result;
			if (this.Config.ReplacePlantsAndRocks)
			{
				int? num = (block != null) ? new int?(block.Replaceable) : default(int?);
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

		// Token: 0x06000099 RID: 153 RVA: 0x00005522 File Offset: 0x00003722
		[NullableContext(2)]
		private bool IsReplaceable(Block block)
		{
			return PackableBehavior.IsAirOrNull(block) || this.IsPlantOrRock(block);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005536 File Offset: 0x00003736
		private void SendClientError(EntityPlayer entity, string error)
		{
			this._modSystem.SendClientError(entity.Player, error);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000554B File Offset: 0x0000374B
		private static void SendClientChatMessage(EntityPlayer entity, string message)
		{
			TentBagReworkedModSystem.SendClientChatMessage(entity.Player, message);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000555A File Offset: 0x0000375A
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this._modSystem = api.ModLoader.GetModSystem<TentBagReworkedModSystem>(true);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005578 File Offset: 0x00003778
		public PackableBehavior(CollectibleObject obj) : base(obj)
		{
			string domain = obj.Code.Domain;
			string path = obj.CodeWithoutParts(1);
			this._emptyBag = new AssetLocation(domain, path + "-empty");
			this._packedBag = new AssetLocation(domain, path + "-packed");
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000055D8 File Offset: 0x000037D8
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, [Nullable(2)] BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
		{
			EntityPlayer entity;
			bool flag;
			if (blockSel != null)
			{
				entity = (byEntity as EntityPlayer);
				flag = (entity == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				handHandling = 3;
				bool flag3 = entity.Api.Side != 1;
				if (!flag3)
				{
					ItemStack itemstack = slot.Itemstack;
					object obj;
					if (itemstack == null)
					{
						obj = null;
					}
					else
					{
						ITreeAttribute attributes = itemstack.Attributes;
						obj = ((attributes != null) ? attributes.GetString("tent-contents", null) : null);
					}
					object obj2;
					if ((obj2 = obj) == null)
					{
						ItemStack itemstack2 = slot.Itemstack;
						if (itemstack2 == null)
						{
							obj2 = null;
						}
						else
						{
							ITreeAttribute attributes2 = itemstack2.Attributes;
							obj2 = ((attributes2 != null) ? attributes2.GetString("packed-contents", null) : null);
						}
					}
					string contents = obj2;
					ItemStack itemstack3 = slot.Itemstack;
					int? num;
					if (itemstack3 == null)
					{
						num = default(int?);
					}
					else
					{
						ITreeAttribute attributes3 = itemstack3.Attributes;
						num = ((attributes3 != null) ? new int?(attributes3.GetInt("solid-block-count", 0)) : default(int?));
					}
					int? num2 = num;
					int solidBlockCount = num2.GetValueOrDefault();
					bool flag4 = contents == null;
					if (flag4)
					{
						bool flag5 = blockSel.Face != BlockFacing.UP;
						if (flag5)
						{
							PackableBehavior.SendClientChatMessage(entity, Lang.PackFloorOnlyError(Array.Empty<object>()));
						}
						else
						{
							this.PackContents(entity, blockSel, slot, ref solidBlockCount);
						}
					}
					else
					{
						this.UnpackContents(entity, blockSel, slot, contents, ref solidBlockCount);
					}
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000570C File Offset: 0x0000390C
		private void PackContents(EntityPlayer entity, BlockSelection blockSel, ItemSlot slot, ref int solidBlockCount)
		{
			IBlockAccessor blockAccessor = entity.World.BlockAccessor;
			int y = this.IsPlantOrRock(blockAccessor.GetBlock(blockSel.Position)) ? 1 : 0;
			BlockPos start;
			BlockPos end;
			TentArea.GetPackBounds(this.Config, blockAccessor, blockSel.Position, out start, out end);
			bool flag = !this.CanPack(entity, blockAccessor, start, end, ref solidBlockCount);
			if (!flag)
			{
				BlockSchematic bs = new BlockSchematic();
				bs.AddAreaWithoutEntities(entity.World, blockAccessor, start, end);
				bs.PackIncludingAir(entity.World, start, solidBlockCount);
				this.SaveSchematic(entity, bs);
				this.CleanOldSchematics(entity);
				bool flag2 = !this.Config.CopyMode;
				if (flag2)
				{
					PackableBehavior.ClearArea(entity.World, start, end);
				}
				Item packedItem = entity.World.GetItem(this._packedBag);
				bool flag3 = packedItem != null;
				if (flag3)
				{
					ItemStack packed = new ItemStack(packedItem, slot.StackSize);
					ITreeAttribute attributes = packed.Attributes;
					if (attributes != null)
					{
						attributes.SetString("packed-contents", bs.ToJson());
					}
					ITreeAttribute attributes2 = packed.Attributes;
					if (attributes2 != null)
					{
						attributes2.SetInt("solid-block-count", solidBlockCount);
					}
					ITreeAttribute attributes3 = packed.Attributes;
					if (attributes3 != null)
					{
						attributes3.SetInt("packed-rotation", 0);
					}
					bool putTentInInventoryOnUse = this.Config.PutTentInInventoryOnUse;
					if (putTentInInventoryOnUse)
					{
						ItemStack sinkStack = slot.Itemstack.Clone();
						slot.Itemstack.StackSize = 0;
						slot.Itemstack = packed;
						slot.OnItemSlotModified(sinkStack);
					}
					else
					{
						entity.World.SpawnItemEntity(packed, blockSel.Position.ToVec3d().Add(0.0, (double)(1 - y), 0.0), null);
						slot.TakeOutWhole();
					}
				}
				EntityBehaviorHunger hunger = entity.GetBehavior<EntityBehaviorHunger>();
				EnumGameMode playerGameMode = entity.Player.WorldData.CurrentGameMode;
				bool flag4 = hunger != null && playerGameMode == 1;
				if (flag4)
				{
					entity.ReduceOnlySaturation(this.Config.BuildEffort * (float)solidBlockCount);
				}
				PackableBehavior.RemoveWaypoint(entity);
				this.ChatNotification(entity, Lang.PackSuccess(new object[]
				{
					solidBlockCount,
					(float)solidBlockCount * this.Config.BuildEffort
				}));
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005958 File Offset: 0x00003B58
		private void UnpackContents(EntityPlayer entity, BlockSelection blockSel, ItemSlot slot, string contents, ref int solidBlockCount)
		{
			IBlockAccessor blockAccessor = entity.World.BlockAccessor;
			IBulkBlockAccessor bulkBlockAccessor = entity.World.GetBlockAccessorBulkUpdate(true, true, false);
			int y = this.IsPlantOrRock(blockAccessor.GetBlock(blockSel.Position)) ? 1 : 0;
			BlockPos start = blockSel.Position.AddCopy(-this.Config.MaxRadius, 0 - y, -this.Config.MaxRadius);
			BlockPos end = blockSel.Position.AddCopy(this.Config.MaxRadius, Math.Max(this.Config.MaxHeight, 3), this.Config.MaxRadius);
			bool flag = !this.CanUnpack(entity, blockAccessor, start, end, ref solidBlockCount);
			if (!flag)
			{
				string error = null;
				BlockSchematic bs = BlockSchematic.LoadFromString(contents, ref error);
				bool flag2 = !string.IsNullOrEmpty(error);
				if (flag2)
				{
					this.SendClientError(entity, Lang.UnpackError(Array.Empty<object>()));
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					int? num;
					if (itemstack == null)
					{
						num = default(int?);
					}
					else
					{
						ITreeAttribute attributes = itemstack.Attributes;
						num = ((attributes != null) ? new int?(attributes.GetInt("packed-rotation", 0)) : default(int?));
					}
					int? num2 = num;
					int rotation = num2.GetValueOrDefault();
					bool flag3 = rotation != 0;
					if (flag3)
					{
						bs.TransformWhilePacked(entity.World, 1, rotation, default(EnumAxis?));
					}
					BlockPos adjustedStart = bs.AdjustStartPos(start.Add(this.Config.MaxRadius, 1, this.Config.MaxRadius), 1);
					bs.ReplaceMode = 1;
					bs.Place(bulkBlockAccessor, entity.World, adjustedStart, true);
					bulkBlockAccessor.Commit();
					bs.PlaceEntitiesAndBlockEntities(blockAccessor, entity.World, adjustedStart, bs.BlockCodes, bs.ItemCodes, false, null, 0, null, true);
					IBlockAccessor worldBa = entity.World.BlockAccessor;
					BlockPos nPos = new BlockPos(adjustedStart.dimension);
					for (int dx = 0; dx < bs.SizeX; dx++)
					{
						for (int dy = 0; dy < bs.SizeY; dy++)
						{
							for (int dz = 0; dz < bs.SizeZ; dz++)
							{
								nPos.Set(adjustedStart.X + dx, adjustedStart.Y + dy, adjustedStart.Z + dz);
								BlockEntity blockEntity = worldBa.GetBlockEntity(nPos);
								BEBehaviorDoor doorBeh = (blockEntity != null) ? blockEntity.GetBehavior<BEBehaviorDoor>() : null;
								bool flag4 = doorBeh != null;
								if (flag4)
								{
									MethodInfo doorSetupBoxes = PackableBehavior.DoorSetupBoxes;
									if (doorSetupBoxes != null)
									{
										doorSetupBoxes.Invoke(doorBeh, new object[]
										{
											false
										});
									}
								}
							}
						}
					}
					entity.Api.Event.RegisterCallback(delegate(float dt)
					{
						IBlockAccessor wa = entity.World.GetBlockAccessor(true, true, false, false);
						BlockPos scanPos = new BlockPos(adjustedStart.dimension);
						for (int dx2 = 0; dx2 < bs.SizeX; dx2++)
						{
							for (int dy2 = 0; dy2 < bs.SizeY; dy2++)
							{
								for (int dz2 = 0; dz2 < bs.SizeZ; dz2++)
								{
									scanPos.Set(adjustedStart.X + dx2, adjustedStart.Y + dy2, adjustedStart.Z + dz2);
									Block block = wa.GetBlock(scanPos);
									bool flag7 = block.BlockId == 0 || block.LightHsv[2] <= 0;
									if (!flag7)
									{
										BlockPos lightPos = scanPos.Copy();
										try
										{
											BlockEntity be = wa.GetBlockEntity(lightPos);
											ITreeAttribute beData = null;
											bool flag8 = be != null;
											if (flag8)
											{
												beData = new TreeAttribute();
												be.ToTreeAttributes(beData);
											}
											int blockId = block.BlockId;
											wa.SetBlock(0, lightPos);
											wa.SetBlock(blockId, lightPos);
											bool flag9 = beData != null;
											if (flag9)
											{
												BlockEntity newBe = wa.GetBlockEntity(lightPos);
												bool flag10 = newBe != null;
												if (flag10)
												{
													newBe.FromTreeAttributes(beData, entity.World);
													newBe.MarkDirty(true, null);
												}
											}
										}
										catch (Exception e)
										{
											ILogger logger = this._modSystem.Logger;
											string text = "[TentBagReworked] Light re-trigger failed at ";
											BlockPos blockPos = lightPos;
											string text2 = (blockPos != null) ? blockPos.ToString() : null;
											string text3 = ": ";
											Exception ex = e;
											logger.Warning(text + text2 + text3 + ((ex != null) ? ex.ToString() : null));
										}
									}
								}
							}
						}
					}, 250);
					Item emptyItem = entity.World.GetItem(this._emptyBag);
					bool flag5 = emptyItem != null;
					if (flag5)
					{
						ItemStack empty = new ItemStack(emptyItem, slot.StackSize);
						bool putTentInInventoryOnUse = this.Config.PutTentInInventoryOnUse;
						if (putTentInInventoryOnUse)
						{
							ItemStack sinkStack = slot.Itemstack.Clone();
							slot.Itemstack.StackSize = 0;
							slot.Itemstack = empty;
							slot.OnItemSlotModified(sinkStack);
						}
						else
						{
							entity.World.SpawnItemEntity(empty, blockSel.Position.ToVec3d().Add(0.0, (double)(1 - y), 0.0), null);
							slot.TakeOutWhole();
						}
					}
					bool dropWaypoint = this.Config.DropWaypoint;
					if (dropWaypoint)
					{
						this.CreateWaypoint(entity, blockSel);
					}
					EntityBehaviorHunger hunger = entity.GetBehavior<EntityBehaviorHunger>();
					EnumGameMode playerGameMode = entity.Player.WorldData.CurrentGameMode;
					bool flag6 = hunger != null && playerGameMode == 1;
					if (flag6)
					{
						entity.ReduceOnlySaturation(this.Config.BuildEffort * (float)solidBlockCount);
					}
					this.ChatNotification(entity, Lang.UnpackSuccess(new object[]
					{
						solidBlockCount,
						(float)solidBlockCount * this.Config.BuildEffort
					}));
				}
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005E24 File Offset: 0x00004024
		private void SaveSchematic(Entity player, BlockSchematic bs)
		{
			string playerName = player.GetName() ?? "unknown";
			Guid id = Guid.NewGuid();
			List<Guid> history;
			bool flag = !this.SchematicHistory.TryGetValue(playerName, ref history);
			if (flag)
			{
				history = new List<Guid>();
				this.SchematicHistory[playerName] = history;
			}
			history.Add(id);
			string exportFolderPath = PackableBehavior.ExportFolderPath;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(24, 2);
			defaultInterpolatedStringHandler.AppendLiteral("tentbag-schematic-");
			defaultInterpolatedStringHandler.AppendFormatted(playerName);
			defaultInterpolatedStringHandler.AppendLiteral("-");
			defaultInterpolatedStringHandler.AppendFormatted<Guid>(id);
			defaultInterpolatedStringHandler.AppendLiteral(".json");
			bs.Save(Path.Combine(exportFolderPath, defaultInterpolatedStringHandler.ToStringAndClear()));
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005ED8 File Offset: 0x000040D8
		private void CleanOldSchematics(Entity player)
		{
			string playerName = player.GetName() ?? "unknown";
			List<Guid> schematics;
			bool flag = this.SchematicHistory.TryGetValue(playerName, ref schematics) && schematics.Count > this.Config.MaxSchematicHistory;
			if (flag)
			{
				bool flag2 = schematics.Count > 0;
				if (flag2)
				{
					this.SchematicHistory[playerName].RemoveAt(0);
					string exportFolderPath = PackableBehavior.ExportFolderPath;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(24, 2);
					defaultInterpolatedStringHandler.AppendLiteral("tentbag-schematic-");
					defaultInterpolatedStringHandler.AppendFormatted(playerName);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted<Guid>(schematics[0]);
					defaultInterpolatedStringHandler.AppendLiteral(".json");
					string path = Path.Combine(exportFolderPath, defaultInterpolatedStringHandler.ToStringAndClear());
					bool flag3 = File.Exists(path);
					if (flag3)
					{
						File.Delete(path);
					}
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005FC0 File Offset: 0x000041C0
		private void ChatNotification(EntityPlayer entity, string message)
		{
			bool showChatNotification = this.Config.ShowChatNotification;
			if (showChatNotification)
			{
				PackableBehavior.SendClientChatMessage(entity, message);
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005FE8 File Offset: 0x000041E8
		private void CreateWaypoint(EntityPlayer player, BlockSelection blockSel)
		{
			bool flag = player.Api is ICoreServerAPI;
			if (flag)
			{
				WaypointMapLayer mapLayer = Enumerable.FirstOrDefault<MapLayer>(player.Api.ModLoader.GetModSystem<WorldMapManager>(true).MapLayers, (MapLayer ml) => ml is WaypointMapLayer) as WaypointMapLayer;
				bool flag2 = mapLayer != null;
				if (flag2)
				{
					Waypoint wp = new Waypoint
					{
						Position = blockSel.Position.ToVec3d(),
						Title = Lang.WaypointTitle(new object[]
						{
							player.EntityId
						}),
						Pinned = false,
						Icon = this.Config.WaypointIcon,
						Color = ColorTranslator.FromHtml(this.Config.WaypointColor).ToArgb(),
						OwningPlayerUid = player.PlayerUID
					};
					mapLayer.AddWaypoint(wp, player.Player as IServerPlayer);
				}
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000060F0 File Offset: 0x000042F0
		private static void RemoveWaypoint(EntityPlayer player)
		{
			ICoreServerAPI serverApi = player.Api as ICoreServerAPI;
			bool flag = serverApi != null;
			if (flag)
			{
				IServerPlayer serverPlayer = player.Player as IServerPlayer;
				WaypointMapLayer mapLayer = Enumerable.FirstOrDefault<MapLayer>(player.Api.ModLoader.GetModSystem<WorldMapManager>(true).MapLayers, (MapLayer ml) => ml is WaypointMapLayer) as WaypointMapLayer;
				bool flag2 = mapLayer != null;
				if (flag2)
				{
					List<Waypoint> waypoints = PackableBehavior.GetWaypoints(serverApi);
					bool flag3 = waypoints != null;
					if (flag3)
					{
						IEnumerable<Waypoint> enumerable = Enumerable.ToList<Waypoint>(waypoints);
						Func<Waypoint, bool> <>9__1;
						Func<Waypoint, bool> func;
						if ((func = <>9__1) == null)
						{
							func = (<>9__1 = ((Waypoint w) => w.OwningPlayerUid == player.PlayerUID));
						}
						foreach (Waypoint waypoint in Enumerable.Where<Waypoint>(enumerable, func))
						{
							bool flag4 = waypoint.Title == Lang.WaypointTitle(new object[]
							{
								player.EntityId
							});
							if (flag4)
							{
								bool flag5 = player == null;
								if (flag5)
								{
									player.Api.Logger.Error(Lang.WpRemoveError(Array.Empty<object>()));
									PackableBehavior.StoreWaypoint(waypoint.OwningPlayerUid, waypoint);
								}
								else
								{
									waypoints.Remove(waypoint);
									MethodInfo resendWaypoints = PackableBehavior.ResendWaypoints;
									if (resendWaypoints != null)
									{
										resendWaypoints.Invoke(mapLayer, new object[]
										{
											serverPlayer
										});
									}
									MethodInfo rebuildMapComponents = PackableBehavior.RebuildMapComponents;
									if (rebuildMapComponents != null)
									{
										rebuildMapComponents.Invoke(mapLayer, null);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000062C8 File Offset: 0x000044C8
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		public static List<Waypoint> GetWaypoints(ICoreServerAPI api)
		{
			bool flag = api == null;
			List<Waypoint> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				WaypointMapLayer mapLayer = Enumerable.FirstOrDefault<MapLayer>(api.ModLoader.GetModSystem<WorldMapManager>(true).MapLayers, (MapLayer ml) => ml is WaypointMapLayer) as WaypointMapLayer;
				result = ((mapLayer != null) ? mapLayer.Waypoints : null);
			}
			return result;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00006330 File Offset: 0x00004530
		private static void StoreWaypoint([Nullable(2)] string playerUID, Waypoint wp)
		{
			bool flag = string.IsNullOrEmpty(playerUID);
			if (!flag)
			{
				List<string> list;
				bool flag2 = !TentBagReworkedModSystem.PendingWaypointNames.TryGetValue(playerUID, ref list);
				if (flag2)
				{
					list = new List<string>();
					TentBagReworkedModSystem.PendingWaypointNames[playerUID] = list;
				}
				list.Add(wp.Title);
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006380 File Offset: 0x00004580
		private static void ClearArea(IWorldAccessor world, BlockPos start, BlockPos end)
		{
			PackableBehavior.ClearArea(world.BulkBlockAccessor, start, end);
			PackableBehavior.ClearArea(world.BlockAccessor, start, end);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000063A0 File Offset: 0x000045A0
		private static void ClearArea(IBlockAccessor blockAccessor, BlockPos start, BlockPos end)
		{
			blockAccessor.WalkBlocks(start, end, delegate(Block block, int x, int y, int z)
			{
				BlockPos pos = new BlockPos(x, y, z, 0);
				bool flag = blockAccessor.GetBlock(pos, 2).BlockId != 0;
				if (flag)
				{
					blockAccessor.SetBlock(0, pos, 2);
				}
				bool flag2 = block.BlockId != 0;
				if (flag2)
				{
					blockAccessor.SetBlock(0, pos);
				}
			}, false);
			blockAccessor.Commit();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000063E4 File Offset: 0x000045E4
		private bool CanPack(EntityPlayer entity, IBlockAccessor blockAccessor, BlockPos start, BlockPos end, ref int solidBlockCount)
		{
			List<BlockPos> blocks = new List<BlockPos>();
			bool notified = false;
			bool canPack = true;
			int localSolidBlockCount = 0;
			blockAccessor.WalkBlocks(start, end, delegate(Block block, int posX, int posY, int posZ)
			{
				BlockPos pos = new BlockPos(posX, posY, posZ, 0);
				bool flag3 = !entity.World.Claims.TryAccess(entity.Player, pos, 1);
				if (flag3)
				{
					notified = true;
					blocks.Add(pos);
				}
				else
				{
					bool flag4 = this.IsBannedBlock(block);
					if (flag4)
					{
						bool flag5 = !notified;
						if (flag5)
						{
							this.SendClientError(entity, Lang.IllegalItemError(new object[]
							{
								block.GetPlacedBlockName(entity.World, pos)
							}));
							notified = true;
						}
						blocks.Add(pos);
					}
				}
				bool flag6 = block.Id != 0;
				int localSolidBlockCount;
				if (flag6)
				{
					localSolidBlockCount = localSolidBlockCount;
					localSolidBlockCount++;
				}
				EntityBehaviorHunger hunger = entity.GetBehavior<EntityBehaviorHunger>();
				EnumGameMode playerGameMode = entity.Player.WorldData.CurrentGameMode;
				bool flag7 = hunger != null && playerGameMode == 1;
				if (flag7)
				{
					float currentHunger = hunger.Saturation;
					bool flag8 = (float)localSolidBlockCount * this.Config.BuildEffort > currentHunger;
					if (flag8)
					{
						bool flag9 = !notified;
						if (flag9)
						{
							this.SendClientError(entity, Lang.PackHungerError(Array.Empty<object>()));
							notified = true;
						}
						canPack = false;
					}
				}
			}, false);
			bool flag = localSolidBlockCount == 0;
			if (flag)
			{
				bool flag2 = !notified;
				if (flag2)
				{
					this.SendClientError(entity, Lang.EmptyBuildError(Array.Empty<object>()));
					notified = true;
				}
				canPack = false;
			}
			solidBlockCount = localSolidBlockCount;
			return canPack && !this.ShouldHighlightBlocks(entity, blocks);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000064B0 File Offset: 0x000046B0
		private bool CanUnpack(EntityPlayer entity, IBlockAccessor blockAccessor, BlockPos start, BlockPos end, ref int solidBlockCount)
		{
			List<BlockPos> blocks = new List<BlockPos>();
			bool notified = false;
			bool canUnpack = true;
			EntityBehaviorHunger hunger = entity.GetBehavior<EntityBehaviorHunger>();
			EnumGameMode playerGameMode = entity.Player.WorldData.CurrentGameMode;
			bool flag = hunger != null && playerGameMode == 1;
			if (flag)
			{
				float currentHunger = hunger.Saturation;
				bool flag2 = (float)solidBlockCount * this.Config.BuildEffort > currentHunger;
				if (flag2)
				{
					bool flag3 = !notified;
					if (flag3)
					{
						this.SendClientError(entity, Lang.UnpackHungerError(Array.Empty<object>()));
						notified = true;
					}
					canUnpack = false;
				}
			}
			bool flag4 = canUnpack;
			if (flag4)
			{
				blockAccessor.WalkBlocks(start, end, delegate(Block block, int posX, int posY, int posZ)
				{
					BlockPos pos = new BlockPos(posX, posY, posZ, 0);
					bool flag5 = !entity.World.Claims.TryAccess(entity.Player, pos, 1);
					if (flag5)
					{
						notified = true;
						blocks.Add(pos);
					}
					else
					{
						bool flag6 = pos.Y == start.Y;
						if (flag6)
						{
							bool flag7 = this.Config.RequireSolidGround && !block.SideSolid[4];
							if (flag7)
							{
								bool flag8 = !notified;
								if (flag8)
								{
									this.SendClientError(entity, Lang.SolidGroundError(Array.Empty<object>()));
									notified = true;
								}
								blocks.Add(pos);
							}
						}
						else
						{
							bool flag9 = !this.IsReplaceable(block);
							if (flag9)
							{
								bool flag10 = !notified;
								if (flag10)
								{
									this.SendClientError(entity, Lang.ClearAreaError(Array.Empty<object>()));
									notified = true;
								}
								blocks.Add(pos);
							}
						}
					}
				}, false);
			}
			return canUnpack && !this.ShouldHighlightBlocks(entity, blocks);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000065BC File Offset: 0x000047BC
		private bool IsBannedBlock(Block block)
		{
			AssetLocation blockCode = block.Code;
			bool flag = blockCode == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool allowListMode = this.Config.AllowListMode;
				if (allowListMode)
				{
					bool flag2 = block.BlockId == 0;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = this.IsPlantOrRock(block);
						if (flag3)
						{
							result = false;
						}
						else
						{
							foreach (string allowed in this.Config.AllowedBlocks)
							{
								AssetLocation code = new AssetLocation(allowed);
								bool flag4 = code.Equals(blockCode);
								if (flag4)
								{
									return false;
								}
								bool flag5 = code.IsWildCard && WildcardUtil.GetWildcardValue(code, blockCode) != null;
								if (flag5)
								{
									return false;
								}
							}
							result = true;
						}
					}
				}
				else
				{
					foreach (string banned in this.Config.BannedBlocks)
					{
						AssetLocation code2 = new AssetLocation(banned);
						bool flag6 = code2.Equals(blockCode);
						if (flag6)
						{
							return true;
						}
						bool flag7 = code2.IsWildCard && WildcardUtil.GetWildcardValue(code2, blockCode) != null;
						if (flag7)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006704 File Offset: 0x00004904
		private bool ShouldHighlightBlocks(EntityPlayer entity, List<BlockPos> blocks)
		{
			bool flag = blocks.Count <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._highlightId > 0L;
				if (flag2)
				{
					entity.Api.Event.UnregisterCallback(this._highlightId);
				}
				int color = this.Config.HighlightErrorColor.ToColor().Reverse();
				List<int> colors = Enumerable.ToList<int>(Enumerable.Repeat<int>(color, blocks.Count));
				entity.World.HighlightBlocks(entity.Player, 1337, blocks, colors, 0, 0, 1f);
				this._highlightId = entity.Api.Event.RegisterCallback(delegate(float _)
				{
					List<BlockPos> empty = new List<BlockPos>();
					entity.World.HighlightBlocks(entity.Player, 1337, empty, 0, 0);
				}, 2500);
				result = true;
			}
			return result;
		}

		// Token: 0x04000042 RID: 66
		[Nullable(2)]
		private static readonly MethodInfo ResendWaypoints = typeof(WaypointMapLayer).GetMethod("ResendWaypoints", 36);

		// Token: 0x04000043 RID: 67
		[Nullable(2)]
		private static readonly MethodInfo RebuildMapComponents = typeof(WaypointMapLayer).GetMethod("RebuildMapComponents", 36);

		// Token: 0x04000044 RID: 68
		[Nullable(2)]
		private static readonly MethodInfo DoorSetupBoxes = typeof(BEBehaviorDoor).GetMethod("SetupRotationsAndColSelBoxes", 36);

		// Token: 0x04000045 RID: 69
		private TentBagReworkedModSystem _modSystem = null;

		// Token: 0x04000046 RID: 70
		[Nullable(2)]
		private readonly AssetLocation _emptyBag;

		// Token: 0x04000047 RID: 71
		[Nullable(2)]
		private readonly AssetLocation _packedBag;

		// Token: 0x04000048 RID: 72
		private long _highlightId;
	}
}
