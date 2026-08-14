using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B6 RID: 182
	public class BlockEntitySwitchboard : BEWireNode, IWireTypedNode, ISwitchboardNode
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0001E90E File Offset: 0x0001CB0E
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Switchboard;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x0001E911 File Offset: 0x0001CB11
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x0001E919 File Offset: 0x0001CB19
		public float PowerPercent { get; private set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x0001E922 File Offset: 0x0001CB22
		public bool UsePowerRequirements
		{
			get
			{
				return this.usePowerRequirements;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001E92A File Offset: 0x0001CB2A
		protected override int MaxConnections
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001E930 File Offset: 0x0001CB30
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.DisableConsumerInstancedRenderer();
			if (api.Side == 1)
			{
				ICoreServerAPI coreServerAPI = api as ICoreServerAPI;
				if (coreServerAPI != null)
				{
					coreServerAPI.Event.RegisterGameTickListener(new Action<float>(this.OnServerSwitchboardTick), 100, 0);
				}
				this.TryDiscoverNetwork();
				return;
			}
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi != null)
			{
				this._mechPartRenderer = new RotatingMechPartRenderer(this, capi, new AssetLocation("rpvoicechat:shapes/block/switchboard/switchboard_mechpart.json"), this.GetMechPartBaseRotY());
			}
			this.RegisterGameTickListener(delegate(float _)
			{
				this.TryApplyPersistedNetworkName();
			}, 1000, 0);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001E9C0 File Offset: 0x0001CBC0
		public override Vec3f GetWireAttachmentOffsetFor(BlockPos otherNodePos)
		{
			if (otherNodePos == null)
			{
				return base.GetWireAttachmentOffsetFor(otherNodePos);
			}
			Vec3f[] axisOffsetsCurrent = Enumerable.ToArray<Vec3f>(Enumerable.Select<Vec3f, Vec3f>(BlockEntitySwitchboard.AxisOffsetsNorth, new Func<Vec3f, Vec3f>(this.RotateLocalOffsetByBlockSide)));
			List<ValueTuple<BlockPos, Vec3f>> entries = Enumerable.ToList<ValueTuple<BlockPos, Vec3f>>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, int>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, int>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, int>(Enumerable.ThenBy<ValueTuple<BlockPos, Vec3f>, float>(Enumerable.OrderBy<ValueTuple<BlockPos, Vec3f>, float>(Enumerable.Select<BlockPos, ValueTuple<BlockPos, Vec3f>>(Enumerable.Where<BlockPos>(Enumerable.Select<WireConnection, BlockPos>(base.GetConnections(), (WireConnection c) => c.GetOtherBlockPos(this.Pos)), (BlockPos p) => p != null), (BlockPos p) => new ValueTuple<BlockPos, Vec3f>(p, new Vec3f((float)(p.X - this.Pos.X) + 0.5f, 0.5f, (float)(p.Z - this.Pos.Z) + 0.5f))), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item2.X), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item2.Z), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item1.X), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item1.Y), ([TupleElementNames(new string[]
			{
				"Pos",
				"LocalCurrent"
			})] ValueTuple<BlockPos, Vec3f> entry) => entry.Item1.Z));
			if (entries.Count == 0)
			{
				return base.GetWireAttachmentOffsetFor(otherNodePos);
			}
			int index;
			if (!this.AssignAxesByBestGlobalDistance(entries, axisOffsetsCurrent).TryGetValue(BlockEntitySwitchboard.GetPosKey(otherNodePos), ref index))
			{
				return base.GetWireAttachmentOffsetFor(otherNodePos);
			}
			index = GameMath.Clamp(index, 0, BlockEntitySwitchboard.AxisOffsetsNorth.Length - 1);
			return axisOffsetsCurrent[index];
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001EB43 File Offset: 0x0001CD43
		[return: TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})]
		private static ValueTuple<int, int, int> GetPosKey(BlockPos pos)
		{
			return new ValueTuple<int, int, int>(pos.X, pos.Y, pos.Z);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001EB5C File Offset: 0x0001CD5C
		private static float DistanceSqXZ(Vec3f a, Vec3f b)
		{
			float num = a.X - b.X;
			float dz = a.Z - b.Z;
			return num * num + dz * dz;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001EB8C File Offset: 0x0001CD8C
		[return: TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})]
		private Dictionary<ValueTuple<int, int, int>, int> AssignAxesByBestGlobalDistance([TupleElementNames(new string[]
		{
			"Pos",
			"LocalCurrent"
		})] IReadOnlyList<ValueTuple<BlockPos, Vec3f>> entries, IReadOnlyList<Vec3f> axisOffsetsCurrent)
		{
			BlockEntitySwitchboard.<>c__DisplayClass22_0 CS$<>8__locals1;
			CS$<>8__locals1.entries = entries;
			CS$<>8__locals1.axisOffsetsCurrent = axisOffsetsCurrent;
			CS$<>8__locals1.count = Math.Min(CS$<>8__locals1.entries.Count, CS$<>8__locals1.axisOffsetsCurrent.Count);
			Dictionary<ValueTuple<int, int, int>, int> assignedAxisByNode = new Dictionary<ValueTuple<int, int, int>, int>();
			if (CS$<>8__locals1.count == 0)
			{
				return assignedAxisByNode;
			}
			CS$<>8__locals1.usedAxes = new bool[CS$<>8__locals1.axisOffsetsCurrent.Count];
			CS$<>8__locals1.current = new int[CS$<>8__locals1.count];
			CS$<>8__locals1.best = new int[CS$<>8__locals1.count];
			Array.Fill<int>(CS$<>8__locals1.best, -1);
			CS$<>8__locals1.bestCost = float.MaxValue;
			BlockEntitySwitchboard.<AssignAxesByBestGlobalDistance>g__Search|22_0(0, 0f, ref CS$<>8__locals1);
			for (int i = 0; i < CS$<>8__locals1.count; i++)
			{
				int axisIndex = CS$<>8__locals1.best[i];
				if (axisIndex >= 0)
				{
					assignedAxisByNode[BlockEntitySwitchboard.GetPosKey(CS$<>8__locals1.entries[i].Item1)] = axisIndex;
				}
			}
			return assignedAxisByNode;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001EC78 File Offset: 0x0001CE78
		private static Vec3f ComputeRotatedAxisCenter(float fromX, float toX, float fromZ, float toZ, float rotationOriginX, float rotationOriginZ, float rotYDeg)
		{
			float num = (fromX + toX) * 0.5f / 16f;
			float cz = (fromZ + toZ) * 0.5f / 16f;
			Vec3f center = new Vec3f(num, 0.9140625f, cz);
			Vec3f origin = new Vec3f(rotationOriginX / 16f, center.Y, rotationOriginZ / 16f);
			return BlockEntitySwitchboard.RotateAroundPointXZ(center, origin, rotYDeg);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001ECD8 File Offset: 0x0001CED8
		private static Vec3f RotateAroundPointXZ(Vec3f point, Vec3f origin, float rotDeg)
		{
			if (Math.Abs(rotDeg) < 0.001f)
			{
				return point;
			}
			float num = rotDeg * 0.017453292f;
			float cos = GameMath.Cos(num);
			float sin = GameMath.Sin(num);
			float num2 = point.X - origin.X;
			float dz = point.Z - origin.Z;
			float x = num2 * cos + dz * sin;
			float z = -num2 * sin + dz * cos;
			return new Vec3f(x + origin.X, point.Y, z + origin.Z);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001ED50 File Offset: 0x0001CF50
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			if (base.Block == null)
			{
				return false;
			}
			CompositeShape blockShape = base.Block.Shape;
			if (((blockShape != null) ? blockShape.Base : null) == null)
			{
				return false;
			}
			AssetLocation shapeLoc = blockShape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
			Shape shape = Shape.TryGet(this.Api, shapeLoc);
			if (shape == null)
			{
				return false;
			}
			MeshData mesh;
			tesselator.TesselateShape(base.Block, shape, ref mesh, new Vec3f(blockShape.rotateX, blockShape.rotateY, blockShape.rotateZ), blockShape.QuantityElements, blockShape.SelectiveElements);
			mesher.AddMeshData(mesh, 1);
			return true;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
		public void TryDiscoverNetwork()
		{
			Block block = base.Block;
			string sideStr;
			if (((block != null) ? block.Variant : null) == null || !base.Block.Variant.TryGetValue("side", ref sideStr))
			{
				return;
			}
			BlockFacing frontFace = BlockFacing.FromCode(sideStr);
			if (frontFace == null)
			{
				return;
			}
			BlockFacing connectorFace = frontFace;
			BEBehaviorMPBase behavior = base.GetBehavior<BEBehaviorMPBase>();
			if (behavior == null)
			{
				return;
			}
			behavior.CreateJoinAndDiscoverNetwork(connectorFace);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001EE54 File Offset: 0x0001D054
		private void OnServerSwitchboardTick(float dt)
		{
			this.TryApplyPersistedNetworkName();
			ICoreAPI api = this.Api;
			BlockEntitySwitchboard blockEntitySwitchboard;
			if (api == null)
			{
				blockEntitySwitchboard = null;
			}
			else
			{
				IWorldAccessor world = api.World;
				if (world == null)
				{
					blockEntitySwitchboard = null;
				}
				else
				{
					IBlockAccessor blockAccessor = world.BlockAccessor;
					blockEntitySwitchboard = ((blockAccessor != null) ? blockAccessor.GetBlockEntity(this.Pos) : null);
				}
			}
			if (blockEntitySwitchboard != this)
			{
				return;
			}
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			float speed = (consumer != null) ? GameMath.Clamp(consumer.TrueSpeed, 0f, 1f) : 0f;
			this.PowerPercent = speed;
			float minPower = WireNetworkTypeRules.GetRequirements(this.ResolveEffectiveNetworkKind()).MinPowerPercent;
			int percentDisplay = (int)(speed * 100f);
			bool aboveMin = speed >= minPower;
			bool flag = percentDisplay != this._lastSyncedPowerPercent;
			bool thresholdChanged = aboveMin != this._lastAboveMinPower;
			if (!flag && !thresholdChanged)
			{
				return;
			}
			this._lastSyncedPowerPercent = percentDisplay;
			this._lastAboveMinPower = aboveMin;
			this.MarkDirty(false, null);
			if (base.NetworkUID != 0L)
			{
				WireNetworkHandler.RebuildNetworkState(base.NetworkUID);
			}
			if (this.IsDialogOpen())
			{
				GuiDialogSwitchboard guiDialogSwitchboard = this.dialog;
				if (guiDialogSwitchboard == null)
				{
					return;
				}
				guiDialogSwitchboard.RefreshData();
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001EF50 File Offset: 0x0001D150
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetFloat("switchboardPowerPercent", this.PowerPercent);
			tree.SetBool("switchboardUsePowerRequirements", this.usePowerRequirements);
			tree.SetString("savedNetworkCustomName", this.persistedNetworkCustomName ?? "");
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			this.persistedNetworkCustomName = tree.GetString("savedNetworkCustomName", "");
			this.networkNameApplyPending = true;
			base.FromTreeAttributes(tree, worldForResolving);
			this.PowerPercent = tree.GetFloat("switchboardPowerPercent", 0f);
			this.usePowerRequirements = tree.GetBool("switchboardUsePowerRequirements", true);
			if (base.NetworkUID != 0L)
			{
				WireNetworkHandler.RebuildNetworkState(base.NetworkUID);
			}
			if (this.IsDialogOpen())
			{
				GuiDialogSwitchboard guiDialogSwitchboard = this.dialog;
				if (guiDialogSwitchboard != null)
				{
					guiDialogSwitchboard.RefreshData();
				}
			}
			this.DisableConsumerInstancedRenderer();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0001F02C File Offset: 0x0001D22C
		private void TryApplyPersistedNetworkName()
		{
			if (!this.networkNameApplyPending)
			{
				return;
			}
			if (base.NetworkUID == 0L)
			{
				return;
			}
			WireNetworkHandler.SetPersistedNetworkName(base.NetworkUID, this.persistedNetworkCustomName);
			this.networkNameApplyPending = false;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001F058 File Offset: 0x0001D258
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			if (base.NetworkUID != 0L)
			{
				dsc.AppendLine(UIUtils.I18n("Network.NetworkId", new object[]
				{
					base.NetworkUID
				}));
			}
			string displayName = this.GetNetworkDisplayName();
			if (!string.IsNullOrWhiteSpace(displayName))
			{
				dsc.AppendLine(UIUtils.I18n("Switchboard.NetworkName", new object[]
				{
					displayName
				}));
			}
			if (base.NetworkUID != 0L)
			{
				string disabledReason = WireNetworkHandler.GetManagedRoutingDisabledReason(base.NetworkUID);
				if (!string.IsNullOrWhiteSpace(disabledReason))
				{
					dsc.AppendLine(UIUtils.I18n(disabledReason));
				}
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0001F0E8 File Offset: 0x0001D2E8
		public bool HasSufficientPowerFor(WireNetworkKind networkKind)
		{
			WireNetworkRequirements requirements = WireNetworkTypeRules.GetRequirements(networkKind);
			return this.PowerPercent >= requirements.MinPowerPercent && this.usePowerRequirements;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001F114 File Offset: 0x0001D314
		public bool OnInteract()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return true;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi == null)
			{
				return true;
			}
			GuiDialogSwitchboard guiDialogSwitchboard = this.dialog;
			if (guiDialogSwitchboard != null && guiDialogSwitchboard.IsOpened())
			{
				return true;
			}
			this.dialog = new GuiDialogSwitchboard(capi, this);
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001F180 File Offset: 0x0001D380
		public string GetNetworkDisplayName()
		{
			string custom = (this.persistedNetworkCustomName ?? "").Trim();
			if (custom.Length > 0)
			{
				return custom;
			}
			if (base.NetworkUID == 0L)
			{
				return "";
			}
			return base.NetworkUID.ToString();
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001F1C9 File Offset: 0x0001D3C9
		public string GetNetworkCustomNameForEditor()
		{
			return (this.persistedNetworkCustomName ?? "").Trim();
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001F1E0 File Offset: 0x0001D3E0
		public bool RenameNetwork(string name, out string failureLangKey)
		{
			this.persistedNetworkCustomName = (name ?? "").Trim();
			if (base.NetworkUID != 0L)
			{
				WireNetworkHandler.SetPersistedNetworkName(base.NetworkUID, this.persistedNetworkCustomName);
			}
			this.networkNameApplyPending = false;
			failureLangKey = null;
			this.MarkDirty(true, null);
			return true;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0001F230 File Offset: 0x0001D430
		public void RequestRenameNetwork(string desiredName)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel switchboardClientChannel = RPVoiceChatMod.SwitchboardClientChannel;
			if (switchboardClientChannel == null)
			{
				return;
			}
			switchboardClientChannel.SendPacket<SwitchboardRenameNetworkPacket>(new SwitchboardRenameNetworkPacket
			{
				SwitchboardPos = this.Pos,
				NetworkName = (desiredName ?? "")
			});
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0001F288 File Offset: 0x0001D488
		public void SetUsePowerRequirements(bool enabled)
		{
			if (this.usePowerRequirements == enabled)
			{
				return;
			}
			this.usePowerRequirements = enabled;
			this.MarkDirty(true, null);
			if (base.NetworkUID != 0L)
			{
				WireNetworkHandler.RebuildNetworkState(base.NetworkUID);
			}
			if (this.IsDialogOpen())
			{
				GuiDialogSwitchboard guiDialogSwitchboard = this.dialog;
				if (guiDialogSwitchboard == null)
				{
					return;
				}
				guiDialogSwitchboard.RefreshData();
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001F2DC File Offset: 0x0001D4DC
		public void RequestSetUsePowerRequirements(bool enabled)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel switchboardClientChannel = RPVoiceChatMod.SwitchboardClientChannel;
			if (switchboardClientChannel == null)
			{
				return;
			}
			switchboardClientChannel.SendPacket<SwitchboardPowerModePacket>(new SwitchboardPowerModePacket
			{
				SwitchboardPos = this.Pos,
				UsePowerRequirements = enabled
			});
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001F32C File Offset: 0x0001D52C
		public string[] GetConnectedLogicalNodeNames()
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return Array.Empty<string>();
			}
			List<string> entries = new List<string>();
			foreach (BEWireNode node in network.Nodes.ToArray())
			{
				if (node != null)
				{
					IWireTypedNode typedNode = node as IWireTypedNode;
					if (typedNode != null && typedNode.WireNodeKind != WireNodeKind.Infrastructure && typedNode.WireNodeKind != WireNodeKind.Switchboard)
					{
						string typeLabel = typedNode.WireNodeKind.ToString();
						BlockPos pos = node.Pos;
						string nodeLabel = ((pos != null) ? pos.ToString() : null) ?? "Unknown";
						ITelegraphEndpoint telegraphEndpoint = node as ITelegraphEndpoint;
						if (telegraphEndpoint != null && !string.IsNullOrWhiteSpace(telegraphEndpoint.CustomEndpointName))
						{
							nodeLabel = telegraphEndpoint.CustomEndpointName;
						}
						entries.Add(typeLabel + ": " + nodeLabel);
					}
				}
			}
			entries.Sort(StringComparer.OrdinalIgnoreCase);
			return entries.ToArray();
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0001F422 File Offset: 0x0001D622
		private bool IsDialogOpen()
		{
			return this.dialog != null && this.dialog.IsOpened();
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0001F43C File Offset: 0x0001D63C
		private WireNetworkKind ResolveEffectiveNetworkKind()
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null || network.CurrentType == WireNetworkKind.None)
			{
				return WireNetworkKind.Telegraph;
			}
			return network.CurrentType;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001F468 File Offset: 0x0001D668
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			RotatingMechPartRenderer mechPartRenderer = this._mechPartRenderer;
			if (mechPartRenderer != null)
			{
				mechPartRenderer.Dispose();
			}
			this._mechPartRenderer = null;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001F488 File Offset: 0x0001D688
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			RotatingMechPartRenderer mechPartRenderer = this._mechPartRenderer;
			if (mechPartRenderer != null)
			{
				mechPartRenderer.Dispose();
			}
			this._mechPartRenderer = null;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0001F4A8 File Offset: 0x0001D6A8
		private void DisableConsumerInstancedRenderer()
		{
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			if (consumer == null)
			{
				return;
			}
			consumer.Shape = null;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0001F4C8 File Offset: 0x0001D6C8
		private float GetMechPartBaseRotY()
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = true;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = !((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			if (flag)
			{
				return 0f;
			}
			float result;
			if (!(side == "north"))
			{
				if (!(side == "east"))
				{
					if (!(side == "west"))
					{
						if (!(side == "south"))
						{
							result = 0f;
						}
						else
						{
							result = 180f;
						}
					}
					else
					{
						result = 90f;
					}
				}
				else
				{
					result = 270f;
				}
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0001F578 File Offset: 0x0001D778
		private float GetBlockSideRotY()
		{
			Block block = base.Block;
			bool flag;
			string side;
			if (block == null)
			{
				flag = false;
			}
			else
			{
				RelaxedReadOnlyDictionary<string, string> variant = block.Variant;
				flag = ((variant != null) ? new bool?(variant.TryGetValue("side", ref side)) : default(bool?)).GetValueOrDefault();
			}
			float result;
			if (flag)
			{
				float num;
				if (!(side == "north"))
				{
					if (!(side == "east"))
					{
						if (!(side == "west"))
						{
							if (!(side == "south"))
							{
								num = 0f;
							}
							else
							{
								num = 180f;
							}
						}
						else
						{
							num = 90f;
						}
					}
					else
					{
						num = 270f;
					}
				}
				else
				{
					num = 0f;
				}
				result = num;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0001F628 File Offset: 0x0001D828
		private static Vec3f RotateAroundCenter(Vec3f point, float rotDeg)
		{
			if (Math.Abs(rotDeg) < 0.001f)
			{
				return point;
			}
			float num = rotDeg * 0.017453292f;
			float cos = GameMath.Cos(num);
			float sin = GameMath.Sin(num);
			float num2 = point.X - 0.5f;
			float dz = point.Z - 0.5f;
			float x = num2 * cos + dz * sin;
			float z = -num2 * sin + dz * cos;
			return new Vec3f(x + 0.5f, point.Y, z + 0.5f);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0001F69C File Offset: 0x0001D89C
		private Vec3f RotateLocalOffsetByBlockSide(Vec3f offsetNorth)
		{
			return BlockEntitySwitchboard.RotateAroundCenter(offsetNorth, this.GetBlockSideRotY());
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001F7F8 File Offset: 0x0001D9F8
		[CompilerGenerated]
		internal static void <AssignAxesByBestGlobalDistance>g__Search|22_0(int depth, float cost, ref BlockEntitySwitchboard.<>c__DisplayClass22_0 A_2)
		{
			if (depth == A_2.count)
			{
				if (cost < A_2.bestCost)
				{
					A_2.bestCost = cost;
					Array.Copy(A_2.current, A_2.best, A_2.count);
				}
				return;
			}
			for (int axisIndex = 0; axisIndex < A_2.axisOffsetsCurrent.Count; axisIndex++)
			{
				if (!A_2.usedAxes[axisIndex])
				{
					float nextCost = cost + BlockEntitySwitchboard.DistanceSqXZ(A_2.entries[depth].Item2, A_2.axisOffsetsCurrent[axisIndex]);
					if (nextCost < A_2.bestCost)
					{
						A_2.usedAxes[axisIndex] = true;
						A_2.current[depth] = axisIndex;
						BlockEntitySwitchboard.<AssignAxesByBestGlobalDistance>g__Search|22_0(depth + 1, nextCost, ref A_2);
						A_2.usedAxes[axisIndex] = false;
					}
				}
			}
		}

		// Token: 0x04000297 RID: 663
		private GuiDialogSwitchboard dialog;

		// Token: 0x04000298 RID: 664
		private string persistedNetworkCustomName = "";

		// Token: 0x04000299 RID: 665
		private bool networkNameApplyPending;

		// Token: 0x0400029A RID: 666
		private bool usePowerRequirements = true;

		// Token: 0x0400029B RID: 667
		private int _lastSyncedPowerPercent = -1;

		// Token: 0x0400029C RID: 668
		private bool _lastAboveMinPower;

		// Token: 0x0400029D RID: 669
		private RotatingMechPartRenderer _mechPartRenderer;

		// Token: 0x0400029F RID: 671
		private static readonly Vec3f[] AxisOffsetsNorth = new Vec3f[]
		{
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(4.5f, 5.5f, 3f, 4f, 9.5f, 6f, 45f),
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(11.5f, 12.5f, 3f, 4f, 16.5f, 6f, 45f),
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(4.5f, 5.5f, -1f, 0f, 9.5f, 2f, 45f),
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(11.5f, 12.5f, -1f, 0f, 16.5f, 2f, 45f)
		};
	}
}
