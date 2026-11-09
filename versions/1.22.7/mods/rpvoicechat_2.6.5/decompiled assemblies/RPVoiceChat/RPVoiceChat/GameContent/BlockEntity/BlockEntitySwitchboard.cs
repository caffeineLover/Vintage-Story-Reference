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
	// Token: 0x020000EF RID: 239
	public class BlockEntitySwitchboard : BEWireNode, IWireTypedNode, ISwitchboardNode
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00029B2E File Offset: 0x00027D2E
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Switchboard;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00029B31 File Offset: 0x00027D31
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00029B39 File Offset: 0x00027D39
		public float PowerPercent { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00029B42 File Offset: 0x00027D42
		public bool UsePowerRequirements
		{
			get
			{
				return this.usePowerRequirements;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00029B4A File Offset: 0x00027D4A
		protected override int MaxConnections
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00029B50 File Offset: 0x00027D50
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

		// Token: 0x060007C5 RID: 1989 RVA: 0x00029BE0 File Offset: 0x00027DE0
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

		// Token: 0x060007C6 RID: 1990 RVA: 0x00029D63 File Offset: 0x00027F63
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

		// Token: 0x060007C7 RID: 1991 RVA: 0x00029D7C File Offset: 0x00027F7C
		private static float DistanceSqXZ(Vec3f a, Vec3f b)
		{
			float num = a.X - b.X;
			float dz = a.Z - b.Z;
			return num * num + dz * dz;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00029DAC File Offset: 0x00027FAC
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

		// Token: 0x060007C9 RID: 1993 RVA: 0x00029E98 File Offset: 0x00028098
		private static Vec3f ComputeRotatedAxisCenter(float fromX, float toX, float fromZ, float toZ, float rotationOriginX, float rotationOriginZ, float rotYDeg)
		{
			float num = (fromX + toX) * 0.5f / 16f;
			float cz = (fromZ + toZ) * 0.5f / 16f;
			Vec3f center = new Vec3f(num, 0.9140625f, cz);
			Vec3f origin = new Vec3f(rotationOriginX / 16f, center.Y, rotationOriginZ / 16f);
			return BlockEntitySwitchboard.RotateAroundPointXZ(center, origin, rotYDeg);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00029EF8 File Offset: 0x000280F8
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

		// Token: 0x060007CB RID: 1995 RVA: 0x00029F70 File Offset: 0x00028170
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

		// Token: 0x060007CC RID: 1996 RVA: 0x0002A018 File Offset: 0x00028218
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

		// Token: 0x060007CD RID: 1997 RVA: 0x0002A074 File Offset: 0x00028274
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

		// Token: 0x060007CE RID: 1998 RVA: 0x0002A170 File Offset: 0x00028370
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetFloat("switchboardPowerPercent", this.PowerPercent);
			tree.SetBool("switchboardUsePowerRequirements", this.usePowerRequirements);
			tree.SetString("savedNetworkCustomName", this.persistedNetworkCustomName ?? "");
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0002A1C0 File Offset: 0x000283C0
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

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002A24C File Offset: 0x0002844C
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

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002A278 File Offset: 0x00028478
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

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002A308 File Offset: 0x00028508
		public bool HasSufficientPowerFor(WireNetworkKind networkKind)
		{
			WireNetworkRequirements requirements = WireNetworkTypeRules.GetRequirements(networkKind);
			return this.PowerPercent >= requirements.MinPowerPercent && this.usePowerRequirements;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002A334 File Offset: 0x00028534
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

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002A3A0 File Offset: 0x000285A0
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

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002A3E9 File Offset: 0x000285E9
		public string GetNetworkCustomNameForEditor()
		{
			return (this.persistedNetworkCustomName ?? "").Trim();
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002A400 File Offset: 0x00028600
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

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002A450 File Offset: 0x00028650
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

		// Token: 0x060007D8 RID: 2008 RVA: 0x0002A4A8 File Offset: 0x000286A8
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

		// Token: 0x060007D9 RID: 2009 RVA: 0x0002A4FC File Offset: 0x000286FC
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

		// Token: 0x060007DA RID: 2010 RVA: 0x0002A54C File Offset: 0x0002874C
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

		// Token: 0x060007DB RID: 2011 RVA: 0x0002A642 File Offset: 0x00028842
		private bool IsDialogOpen()
		{
			return this.dialog != null && this.dialog.IsOpened();
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0002A65C File Offset: 0x0002885C
		private WireNetworkKind ResolveEffectiveNetworkKind()
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null || network.CurrentType == WireNetworkKind.None)
			{
				return WireNetworkKind.Telegraph;
			}
			return network.CurrentType;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0002A688 File Offset: 0x00028888
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

		// Token: 0x060007DE RID: 2014 RVA: 0x0002A6A8 File Offset: 0x000288A8
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

		// Token: 0x060007DF RID: 2015 RVA: 0x0002A6C8 File Offset: 0x000288C8
		private void DisableConsumerInstancedRenderer()
		{
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			if (consumer == null)
			{
				return;
			}
			consumer.Shape = null;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0002A6E8 File Offset: 0x000288E8
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

		// Token: 0x060007E1 RID: 2017 RVA: 0x0002A798 File Offset: 0x00028998
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

		// Token: 0x060007E2 RID: 2018 RVA: 0x0002A848 File Offset: 0x00028A48
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

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002A8BC File Offset: 0x00028ABC
		private Vec3f RotateLocalOffsetByBlockSide(Vec3f offsetNorth)
		{
			return BlockEntitySwitchboard.RotateAroundCenter(offsetNorth, this.GetBlockSideRotY());
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002AA18 File Offset: 0x00028C18
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

		// Token: 0x04000376 RID: 886
		private GuiDialogSwitchboard dialog;

		// Token: 0x04000377 RID: 887
		private string persistedNetworkCustomName = "";

		// Token: 0x04000378 RID: 888
		private bool networkNameApplyPending;

		// Token: 0x04000379 RID: 889
		private bool usePowerRequirements = true;

		// Token: 0x0400037A RID: 890
		private int _lastSyncedPowerPercent = -1;

		// Token: 0x0400037B RID: 891
		private bool _lastAboveMinPower;

		// Token: 0x0400037C RID: 892
		private RotatingMechPartRenderer _mechPartRenderer;

		// Token: 0x0400037E RID: 894
		private static readonly Vec3f[] AxisOffsetsNorth = new Vec3f[]
		{
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(4.5f, 5.5f, 3f, 4f, 9.5f, 6f, 45f),
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(11.5f, 12.5f, 3f, 4f, 16.5f, 6f, 45f),
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(4.5f, 5.5f, -1f, 0f, 9.5f, 2f, 45f),
			BlockEntitySwitchboard.ComputeRotatedAxisCenter(11.5f, 12.5f, -1f, 0f, 16.5f, 2f, 45f)
		};
	}
}
