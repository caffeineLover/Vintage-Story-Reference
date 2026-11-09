using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPVoiceChat.Config;
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
using Vintagestory.GameContent.Mechanics;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000E6 RID: 230
	public class BlockEntityRadioEmitter : BEWireNode, IWireTypedNode
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00026232 File Offset: 0x00024432
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00026235 File Offset: 0x00024435
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00026238 File Offset: 0x00024438
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.RadioEmitter;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002623B File Offset: 0x0002443B
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x00026243 File Offset: 0x00024443
		public float PowerPercent { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0002624C File Offset: 0x0002444C
		public RadioEmitterOperatingMode OperatingMode
		{
			get
			{
				return this.operatingMode;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00026254 File Offset: 0x00024454
		public bool IsRepeaterMode
		{
			get
			{
				return this.operatingMode == RadioEmitterOperatingMode.Repeater;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0002625F File Offset: 0x0002445F
		public string RepeaterFrequency
		{
			get
			{
				return this.repeaterFrequency ?? "";
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x00026270 File Offset: 0x00024470
		public bool IsWirelessTransmitting
		{
			get
			{
				return this.HasSufficientTransmitPower() && !this.IsRepeaterMode;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00026285 File Offset: 0x00024485
		public bool IsRepeaterRelaying
		{
			get
			{
				return this.HasSufficientTransmitPower() && this.IsRepeaterMode && this.CanRelayRepeaterFrequency();
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000262A0 File Offset: 0x000244A0
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.DisableConsumerInstancedRenderer();
			if (api.Side == 1)
			{
				ICoreServerAPI coreServerAPI = api as ICoreServerAPI;
				if (coreServerAPI != null)
				{
					coreServerAPI.Event.RegisterGameTickListener(new Action<float>(this.OnServerTick), 100, 0);
				}
				this.TryDiscoverNetwork();
				RadioBlockIndex.RegisterEmitter(this.Pos);
				this.PublishRfPresence();
				return;
			}
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi != null)
			{
				this.mechPartRenderer = new RotatingMechPartRenderer(this, capi, new AssetLocation("rpvoicechat:shapes/block/radioemitter/radioemitter_mechpart.json"), this.GetMechPartBaseRotY());
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00026328 File Offset: 0x00024528
		public bool OnInteract(IPlayer byPlayer)
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				return true;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi == null)
			{
				return true;
			}
			RadioEmitterDialog radioEmitterDialog = this.dialog;
			if (radioEmitterDialog != null && radioEmitterDialog.IsOpened())
			{
				return true;
			}
			this.dialog = new RadioEmitterDialog(capi, this);
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00026390 File Offset: 0x00024590
		public void RequestSetOperatingMode(RadioEmitterOperatingMode mode)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetEmitterMode,
				IntValue = (int)mode
			});
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000263E8 File Offset: 0x000245E8
		public void SetOperatingMode(RadioEmitterOperatingMode mode)
		{
			bool flag = this.operatingMode == RadioEmitterOperatingMode.Repeater && mode == RadioEmitterOperatingMode.WiredSource;
			this.operatingMode = mode;
			this.MarkDirty(false, null);
			RadioEmitterDialog radioEmitterDialog = this.dialog;
			if (radioEmitterDialog != null)
			{
				radioEmitterDialog.RefreshData();
			}
			if (flag)
			{
				this.RejoinWireNetworkAfterLeavingRepeater();
			}
			this.SyncWirelessRegistration();
			this.PublishRfPresence();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002643C File Offset: 0x0002463C
		public bool HasSufficientTransmitPower()
		{
			float minPercent = (float)ServerConfigManager.RadioNetworkMinPowerPercent / 100f;
			return this.PowerPercent >= minPercent;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00026464 File Offset: 0x00024664
		public int GetEffectiveTransmitRangeBlocks()
		{
			int bonus = this.CountAntennaPartsAbove() * ServerConfigManager.RadioAntennaPartRangeBonusBlocks;
			return ServerConfigManager.RadioEmitterBaseRangeBlocks + bonus;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00026488 File Offset: 0x00024688
		public int CountAntennaPartsAbove()
		{
			ICoreAPI api = this.Api;
			bool flag;
			if (api == null)
			{
				flag = (null != null);
			}
			else
			{
				IWorldAccessor world = api.World;
				flag = (((world != null) ? world.BlockAccessor : null) != null);
			}
			if (!flag)
			{
				return 0;
			}
			int count = 0;
			BlockPos scan = this.Pos.UpCopy(1);
			while (this.Api.World.BlockAccessor.GetBlockEntity(scan) is BlockEntityRadioAntennaPart)
			{
				count++;
				scan = scan.UpCopy(1);
			}
			return count;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000264F2 File Offset: 0x000246F2
		public void OnAntennaStackChanged()
		{
			RadioEmitterDialog radioEmitterDialog = this.dialog;
			if (radioEmitterDialog != null)
			{
				radioEmitterDialog.RefreshData();
			}
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.MarkDirty(false, null);
				this.PublishRfPresence();
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002652A File Offset: 0x0002472A
		public string GetConsoleFrequency()
		{
			BlockEntityRadioSupervisionConsole blockEntityRadioSupervisionConsole = RadioWireNetworkHelper.FindSupervisionConsole(this);
			return ((blockEntityRadioSupervisionConsole != null) ? blockEntityRadioSupervisionConsole.Frequency : null) ?? "";
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00026547 File Offset: 0x00024747
		public string GetConsoleDisplayName()
		{
			BlockEntityRadioSupervisionConsole blockEntityRadioSupervisionConsole = RadioWireNetworkHelper.FindSupervisionConsole(this);
			return ((blockEntityRadioSupervisionConsole != null) ? blockEntityRadioSupervisionConsole.DisplayName : null) ?? "";
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00026564 File Offset: 0x00024764
		public void RequestSetRepeaterFrequency(string desired)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel radioSettingsClientChannel = RPVoiceChatMod.RadioSettingsClientChannel;
			if (radioSettingsClientChannel == null)
			{
				return;
			}
			radioSettingsClientChannel.SendPacket<RadioSettingsPacket>(new RadioSettingsPacket
			{
				BlockPos = this.Pos,
				Operation = RadioSettingsOperation.SetRepeaterFrequency,
				Value = (desired ?? "")
			});
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x000265C4 File Offset: 0x000247C4
		public bool TrySetRepeaterFrequency(string desired)
		{
			string normalized = (desired ?? "").Trim();
			if (RadioFrequencyUtil.Matches(normalized, this.repeaterFrequency))
			{
				return true;
			}
			this.repeaterFrequency = normalized;
			this.MarkDirty(false, null);
			RadioEmitterDialog radioEmitterDialog = this.dialog;
			if (radioEmitterDialog != null)
			{
				radioEmitterDialog.RefreshData();
			}
			this.SyncWirelessRegistration();
			this.PublishRfPresence();
			return true;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00026620 File Offset: 0x00024820
		public bool CanRelayRepeaterFrequency()
		{
			if (this.IsRepeaterMode)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 1)
				{
					string frequency = RadioFrequencyUtil.Normalize(this.repeaterFrequency);
					if (frequency.Length == 0)
					{
						return false;
					}
					IEnumerable<RadioTransmissionPoint> enumerable = RadioRfTransmissionService.CollectWiredTransmissionPoints(this.Api.World.Api as ICoreServerAPI);
					Vec3d repeaterPos = this.Pos.ToVec3d().Add(0.5, 0.5, 0.5);
					return Enumerable.Any<RadioTransmissionPoint>(enumerable, (RadioTransmissionPoint point) => point.Dimension == this.Pos.dimension && RadioFrequencyUtil.Matches(point.Frequency, frequency) && repeaterPos.DistanceTo(point.Position) <= (float)point.RangeBlocks);
				}
			}
			return false;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000266DA File Offset: 0x000248DA
		public string GetActiveRfFrequency()
		{
			if (this.IsRepeaterMode)
			{
				return this.RepeaterFrequency;
			}
			return this.GetConsoleFrequency();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000266F4 File Offset: 0x000248F4
		private void OnServerTick(float dt)
		{
			BEBehaviorMPConsumer behavior = base.GetBehavior<BEBehaviorMPConsumer>();
			float speed = (behavior != null) ? behavior.TrueSpeed : 0f;
			this.PowerPercent = Math.Max(0f, Math.Min(1f, speed));
			int syncedPercent = (int)Math.Round((double)(this.PowerPercent * 100f));
			bool transmitting = this.IsWirelessTransmitting;
			if (syncedPercent != this.lastSyncedPowerPercent || transmitting != this.wasTransmitting)
			{
				this.lastSyncedPowerPercent = syncedPercent;
				this.wasTransmitting = transmitting;
				this.MarkDirty(false, null);
			}
			this.SyncWirelessRegistration();
			this.PublishRfPresence();
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00026784 File Offset: 0x00024984
		public void PublishRfPresence()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || this.Pos == null)
			{
				return;
			}
			bool isRepeater = this.IsRepeaterMode;
			bool isActive = isRepeater ? this.HasSufficientTransmitPower() : this.IsWirelessTransmitting;
			string frequency = RadioFrequencyUtil.Normalize(this.GetActiveRfFrequency());
			RadioRfPresenceRegistry.UpsertEmitter(new RadioRfEmitterPresence
			{
				Pos = this.Pos.Copy(),
				Dimension = this.Pos.dimension,
				NetworkId = base.NetworkUID,
				IsRepeater = isRepeater,
				Frequency = frequency,
				RangeBlocks = this.GetEffectiveTransmitRangeBlocks(),
				IsActive = (isActive && frequency.Length > 0)
			});
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00026848 File Offset: 0x00024A48
		private void SyncWirelessRegistration()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			long networkId = base.NetworkUID;
			if ((!this.IsWirelessTransmitting && !this.IsRepeaterRelaying) || networkId == 0L)
			{
				WirelessTopologyRegistry.UnregisterAntenna(this.Pos);
				return;
			}
			WirelessTopologyRegistry.RegisterAntenna(this.Pos, networkId);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x000268AC File Offset: 0x00024AAC
		public void TryDiscoverNetwork()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				Block block = base.Block;
				if (((block != null) ? block.Variant : null) != null)
				{
					string sideStr;
					if (!base.Block.Variant.TryGetValue("side", ref sideStr))
					{
						return;
					}
					BlockFacing connectorFace = BlockFacing.FromCode(sideStr);
					if (connectorFace == null)
					{
						return;
					}
					BEBehaviorMPBase behavior = base.GetBehavior<BEBehaviorMPBase>();
					if (behavior == null)
					{
						return;
					}
					behavior.CreateJoinAndDiscoverNetwork(connectorFace);
					return;
				}
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00026920 File Offset: 0x00024B20
		private void RejoinWireNetworkAfterLeavingRepeater()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || base.GetConnections().Count == 0)
			{
				return;
			}
			foreach (WireConnection wireConnection in base.GetConnections())
			{
				BEWireNode other = wireConnection.GetOtherNode(this);
				if (other != null && other.NetworkUID != 0L)
				{
					WireNetwork network = WireNetworkHandler.GetNetwork(other.NetworkUID);
					if (network != null)
					{
						network.AddNode(this);
						base.NetworkUID = other.NetworkUID;
						WireNetworkHandler.PropagateNetworkUIDToConnectedNodes(this, network);
						WireNetworkHandler.RebuildNetworkState(base.NetworkUID);
						ICoreServerAPI sapi = this.Api as ICoreServerAPI;
						if (sapi != null)
						{
							WireTopologyConnectivity.NotifyNode(sapi, this);
						}
						return;
					}
				}
			}
			if (base.NetworkUID != 0L)
			{
				WireNetworkHandler.RebuildNetworkState(base.NetworkUID);
				ICoreServerAPI sapi2 = this.Api as ICoreServerAPI;
				if (sapi2 != null)
				{
					WireTopologyConnectivity.NotifyNode(sapi2, this);
				}
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00026A18 File Offset: 0x00024C18
		private float GetMechPartBaseRotY()
		{
			Block block = base.Block;
			string sideStr;
			if (((block != null) ? block.Variant : null) != null && base.Block.Variant.TryGetValue("side", ref sideStr))
			{
				float result;
				if (!(sideStr == "north"))
				{
					if (!(sideStr == "east"))
					{
						if (!(sideStr == "west"))
						{
							if (!(sideStr == "south"))
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
			return 0f;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00026AB8 File Offset: 0x00024CB8
		private void DisableConsumerInstancedRenderer()
		{
			BEBehaviorMPConsumer consumer = base.GetBehavior<BEBehaviorMPConsumer>();
			if (consumer == null)
			{
				return;
			}
			consumer.Shape = null;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00026AD8 File Offset: 0x00024CD8
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.operatingMode = (RadioEmitterOperatingMode)tree.GetInt("rpvc:radioEmitterMode", (int)this.operatingMode);
			this.repeaterFrequency = tree.GetString("rpvc:radioRepeaterFrequency", this.repeaterFrequency);
			this.PowerPercent = tree.GetFloat("rpvc:radioEmitterPowerPercent", this.PowerPercent);
			this.DisableConsumerInstancedRenderer();
			RadioEmitterDialog radioEmitterDialog = this.dialog;
			if (radioEmitterDialog == null)
			{
				return;
			}
			radioEmitterDialog.RefreshData();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00026B48 File Offset: 0x00024D48
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetInt("rpvc:radioEmitterMode", (int)this.operatingMode);
			tree.SetString("rpvc:radioRepeaterFrequency", this.repeaterFrequency ?? "");
			tree.SetFloat("rpvc:radioEmitterPowerPercent", this.PowerPercent);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00026B98 File Offset: 0x00024D98
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

		// Token: 0x0600072C RID: 1836 RVA: 0x00026C40 File Offset: 0x00024E40
		public override void OnBlockRemoved()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				WirelessTopologyRegistry.UnregisterAntenna(this.Pos);
				RadioBlockIndex.UnregisterEmitter(this.Pos);
				RadioRfPresenceRegistry.RemoveEmitter(this.Pos);
			}
			RotatingMechPartRenderer rotatingMechPartRenderer = this.mechPartRenderer;
			if (rotatingMechPartRenderer != null)
			{
				rotatingMechPartRenderer.Dispose();
			}
			this.mechPartRenderer = null;
			base.OnBlockRemoved();
			RadioEmitterDialog radioEmitterDialog = this.dialog;
			if (radioEmitterDialog == null)
			{
				return;
			}
			radioEmitterDialog.TryClose();
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00026CB4 File Offset: 0x00024EB4
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			base.GetBlockInfo(forPlayer, dsc);
			int power = (int)Math.Round((double)(this.PowerPercent * 100f));
			int minPower = ServerConfigManager.RadioNetworkMinPowerPercent;
			dsc.AppendLine(UIUtils.I18n("Radio.Emitter.Info.Power", new object[]
			{
				power,
				minPower
			}));
			dsc.AppendLine(UIUtils.I18n("Radio.Emitter.Info.Range", new object[]
			{
				this.GetEffectiveTransmitRangeBlocks()
			}));
			if (!this.HasSufficientTransmitPower())
			{
				dsc.AppendLine(UIUtils.I18n("Radio.Emitter.Gui.InsufficientPower", new object[]
				{
					minPower
				}));
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00026D5C File Offset: 0x00024F5C
		public override void OnBlockUnloaded()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1)
			{
				this.PublishRfPresence();
				WirelessTopologyRegistry.UnregisterAntenna(this.Pos);
				RadioBlockIndex.UnregisterEmitter(this.Pos);
			}
			RotatingMechPartRenderer rotatingMechPartRenderer = this.mechPartRenderer;
			if (rotatingMechPartRenderer != null)
			{
				rotatingMechPartRenderer.Dispose();
			}
			this.mechPartRenderer = null;
			base.OnBlockUnloaded();
		}

		// Token: 0x04000348 RID: 840
		private RadioEmitterDialog dialog;

		// Token: 0x04000349 RID: 841
		private RadioEmitterOperatingMode operatingMode;

		// Token: 0x0400034A RID: 842
		private string repeaterFrequency = "100.0";

		// Token: 0x0400034B RID: 843
		private int lastSyncedPowerPercent = -1;

		// Token: 0x0400034C RID: 844
		private bool wasTransmitting;

		// Token: 0x0400034D RID: 845
		private RotatingMechPartRenderer mechPartRenderer;
	}
}
