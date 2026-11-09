using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Server;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000F1 RID: 241
	public class BlockEntityTelephone : BEWireNode, INetworkRoot, IWireTypedNode, ITelephoneVoiceEndpoint
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0002C2BF File Offset: 0x0002A4BF
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0002C2C7 File Offset: 0x0002A4C7
		private BlockEntityAnimationUtil AnimUtil
		{
			get
			{
				BEBehaviorAnimatable animatable = this.Animatable;
				if (animatable == null)
				{
					return null;
				}
				return animatable.animUtil;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x0002C2DA File Offset: 0x0002A4DA
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0002C2DD File Offset: 0x0002A4DD
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0002C2E0 File Offset: 0x0002A4E0
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Telephone;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0002C2E3 File Offset: 0x0002A4E3
		public int VoiceEmissionRangeBlocks
		{
			get
			{
				return ServerConfigManager.TelephoneAudibleDistance;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0002C2EA File Offset: 0x0002A4EA
		public long CreatedNetworkID
		{
			get
			{
				return this.originalCreatedNetworkID;
			}
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002C2F2 File Offset: 0x0002A4F2
		public override void OnNetworkCreated(long networkID)
		{
			base.OnNetworkCreated(networkID);
			if (this.originalCreatedNetworkID == 0L)
			{
				this.originalCreatedNetworkID = networkID;
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002C314 File Offset: 0x0002A514
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (api.Side == 2)
			{
				this.InitializeClientAnimator();
				this.SyncInCallAnimationState();
			}
			if (api.Side == 1)
			{
				this.RegisterGameTickListener(new Action<float>(this.OnServerTick), 250, 0);
				if (base.NetworkUID != 0L)
				{
					WireNetworkHandler.RefreshTelegraphRoutingSnapshot(base.NetworkUID);
				}
			}
			BEBehaviorRingable ringable = base.GetBehavior<BEBehaviorRingable>();
			if (ringable != null && string.IsNullOrWhiteSpace(ringable.BellPartCode))
			{
				ringable.BellPartCode = "smallbellparts-silver";
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002C394 File Offset: 0x0002A594
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
			TelephoneMenuDialog telephoneMenuDialog = this.dialog;
			if (telephoneMenuDialog != null && telephoneMenuDialog.IsOpened())
			{
				return true;
			}
			this.dialog = new TelephoneMenuDialog(capi, this);
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0002C3FC File Offset: 0x0002A5FC
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			BlockEntityTelephone.TelephoneCallState previousState = this.callState;
			this.phoneNumber = tree.GetString("rpvc:phoneNumber", "");
			this.targetNumber = tree.GetString("rpvc:targetNumber", "");
			this.composeManagedBySwitchboard = tree.GetBool("rpvc:telephoneManaged", false);
			this.composeEnabled = tree.GetBool("rpvc:telephoneComposeEnabled", false);
			this.composeDisabledReasonLangKey = tree.GetString("rpvc:telephoneDisabledReason", "Telegraph.Settings.DisabledNoPower");
			this.callState = (BlockEntityTelephone.TelephoneCallState)tree.GetInt("rpvc:telephoneState", 0);
			this.stateUntilMs = tree.GetLong("rpvc:telephoneStateUntilMs", 0L);
			this.notInServiceTransitionAtMs = tree.GetLong("rpvc:telephoneNotInServiceAtMs", 0L);
			this.activeCallerPlayerUid = tree.GetString("rpvc:telephoneCallerUid", "");
			this.incomingCallerPlayerUid = tree.GetString("rpvc:telephoneIncomingCallerUid", "");
			int incomingX = tree.GetInt("rpvc:telephoneIncomingCallerPosX", int.MinValue);
			int incomingY = tree.GetInt("rpvc:telephoneIncomingCallerPosY", int.MinValue);
			int incomingZ = tree.GetInt("rpvc:telephoneIncomingCallerPosZ", int.MinValue);
			this.incomingCallerTelephonePos = ((incomingX == int.MinValue) ? null : new BlockPos(incomingX, incomingY, incomingZ));
			int peerX = tree.GetInt("rpvc:telephonePeerPosX", int.MinValue);
			int peerY = tree.GetInt("rpvc:telephonePeerPosY", int.MinValue);
			int peerZ = tree.GetInt("rpvc:telephonePeerPosZ", int.MinValue);
			this.activePeerTelephonePos = ((peerX == int.MinValue) ? null : new BlockPos(peerX, peerY, peerZ));
			long savedOriginalCreatedNetworkID = tree.GetLong("rpvc:telephoneOriginalCreatedNetworkID", 0L);
			if (savedOriginalCreatedNetworkID != 0L)
			{
				this.originalCreatedNetworkID = savedOriginalCreatedNetworkID;
			}
			if (worldForResolving.Side == 2)
			{
				this.SyncInCallAnimationState();
				if (previousState != this.callState)
				{
					this.MarkDirty(true, null);
				}
			}
			TelephoneMenuDialog telephoneMenuDialog = this.dialog;
			if (telephoneMenuDialog == null)
			{
				return;
			}
			telephoneMenuDialog.RefreshData();
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002C5CC File Offset: 0x0002A7CC
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("rpvc:phoneNumber", this.phoneNumber ?? "");
			tree.SetString("rpvc:targetNumber", this.targetNumber ?? "");
			tree.SetBool("rpvc:telephoneManaged", this.composeManagedBySwitchboard);
			tree.SetBool("rpvc:telephoneComposeEnabled", this.composeEnabled);
			tree.SetString("rpvc:telephoneDisabledReason", this.composeDisabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower");
			tree.SetInt("rpvc:telephoneState", (int)this.callState);
			tree.SetLong("rpvc:telephoneStateUntilMs", this.stateUntilMs);
			tree.SetLong("rpvc:telephoneNotInServiceAtMs", this.notInServiceTransitionAtMs);
			tree.SetString("rpvc:telephoneCallerUid", this.activeCallerPlayerUid ?? "");
			tree.SetString("rpvc:telephoneIncomingCallerUid", this.incomingCallerPlayerUid ?? "");
			if (this.incomingCallerTelephonePos != null)
			{
				tree.SetInt("rpvc:telephoneIncomingCallerPosX", this.incomingCallerTelephonePos.X);
				tree.SetInt("rpvc:telephoneIncomingCallerPosY", this.incomingCallerTelephonePos.Y);
				tree.SetInt("rpvc:telephoneIncomingCallerPosZ", this.incomingCallerTelephonePos.Z);
			}
			else
			{
				tree.SetInt("rpvc:telephoneIncomingCallerPosX", int.MinValue);
				tree.SetInt("rpvc:telephoneIncomingCallerPosY", int.MinValue);
				tree.SetInt("rpvc:telephoneIncomingCallerPosZ", int.MinValue);
			}
			if (this.activePeerTelephonePos != null)
			{
				tree.SetInt("rpvc:telephonePeerPosX", this.activePeerTelephonePos.X);
				tree.SetInt("rpvc:telephonePeerPosY", this.activePeerTelephonePos.Y);
				tree.SetInt("rpvc:telephonePeerPosZ", this.activePeerTelephonePos.Z);
			}
			else
			{
				tree.SetInt("rpvc:telephonePeerPosX", int.MinValue);
				tree.SetInt("rpvc:telephonePeerPosY", int.MinValue);
				tree.SetInt("rpvc:telephonePeerPosZ", int.MinValue);
			}
			tree.SetLong("rpvc:telephoneOriginalCreatedNetworkID", this.originalCreatedNetworkID);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0002C7CC File Offset: 0x0002A9CC
		private void RingFromNetwork(IPlayer sourcePlayer)
		{
			BEBehaviorRingable ringable = base.GetBehavior<BEBehaviorRingable>();
			if (ringable == null)
			{
				return;
			}
			double cooldown = ServerConfigManager.BellRingCooldownSeconds;
			if (ringable.LastRung != null && ringable.LastRung >= DateTime.Now.AddSeconds(-cooldown))
			{
				return;
			}
			ringable.LastRung = new DateTime?(DateTime.Now);
			this.SetState(BlockEntityTelephone.TelephoneCallState.Ringing, 20000);
			this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/callbell/callbell_1.ogg"), (double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5, sourcePlayer, false, 12f, 0.6f);
			this.nextRingingSoundAtMs = this.Api.World.ElapsedMilliseconds + 2000L;
			this.MarkDirty(true, null);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0002C8DC File Offset: 0x0002AADC
		private void OnServerTick(float _dt)
		{
			if (this.callState != BlockEntityTelephone.TelephoneCallState.Idle)
			{
				ICoreAPI api = this.Api;
				if (((api != null) ? api.World : null) != null)
				{
					if (this.callState == BlockEntityTelephone.TelephoneCallState.Ringing)
					{
						if (this.notInServiceTransitionAtMs > 0L && this.Api.World.ElapsedMilliseconds >= this.notInServiceTransitionAtMs)
						{
							this.SetState(BlockEntityTelephone.TelephoneCallState.NotInService, 4200);
							this.MarkDirty(true, null);
						}
						this.TryPlayLoopingRingingSounds();
					}
					else if (this.callState == BlockEntityTelephone.TelephoneCallState.NotInService)
					{
						this.TryPlayLoopingRingingSounds();
					}
					if (this.stateUntilMs <= 0L)
					{
						return;
					}
					if (this.Api.World.ElapsedMilliseconds < this.stateUntilMs)
					{
						return;
					}
					this.EndCall();
					return;
				}
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0002C988 File Offset: 0x0002AB88
		private void StartNotInServiceSequence()
		{
			this.SetState(BlockEntityTelephone.TelephoneCallState.Ringing, 0);
			ICoreAPI api = this.Api;
			if (((api != null) ? api.World : null) != null)
			{
				this.notInServiceTransitionAtMs = this.Api.World.ElapsedMilliseconds + 4000L;
			}
			this.nextRingingSoundAtMs = 0L;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002C9D6 File Offset: 0x0002ABD6
		private static string NormalizeDialedNumber(string value)
		{
			IEnumerable<char> enumerable = value ?? "";
			Func<char, bool> func;
			if ((func = BlockEntityTelephone.<>O.<0>__IsDigit) == null)
			{
				func = (BlockEntityTelephone.<>O.<0>__IsDigit = new Func<char, bool>(char.IsDigit));
			}
			return new string(Enumerable.ToArray<char>(Enumerable.Take<char>(Enumerable.Where<char>(enumerable, func), 6)));
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002CA14 File Offset: 0x0002AC14
		private void SetState(BlockEntityTelephone.TelephoneCallState newState, int durationMs)
		{
			this.callState = newState;
			long num;
			if (durationMs > 0)
			{
				ICoreAPI api = this.Api;
				if (((api != null) ? api.World : null) != null)
				{
					num = this.Api.World.ElapsedMilliseconds + (long)durationMs;
					goto IL_37;
				}
			}
			num = 0L;
			IL_37:
			this.stateUntilMs = num;
			this.nextRingingSoundAtMs = 0L;
			if (newState != BlockEntityTelephone.TelephoneCallState.Ringing)
			{
				this.notInServiceTransitionAtMs = 0L;
			}
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0002CA74 File Offset: 0x0002AC74
		public void ApplyServerComposeFlags(bool managedBySwitchboard, bool canCompose, string disabledReasonLangKey = null)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			bool flag = this.composeManagedBySwitchboard != managedBySwitchboard || this.composeEnabled != canCompose || !string.Equals(this.composeDisabledReasonLangKey, disabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower", 4);
			this.composeManagedBySwitchboard = managedBySwitchboard;
			this.composeEnabled = canCompose;
			this.composeDisabledReasonLangKey = (disabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower");
			if (flag)
			{
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002CAF2 File Offset: 0x0002ACF2
		public bool IsManagedBySwitchboard()
		{
			return this.composeManagedBySwitchboard;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002CAFA File Offset: 0x0002ACFA
		public bool CanCompose()
		{
			return this.composeEnabled;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002CB02 File Offset: 0x0002AD02
		public string GetComposeDisabledReasonLangKey()
		{
			return this.composeDisabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower";
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002CB13 File Offset: 0x0002AD13
		public string GetPhoneNumber()
		{
			return this.phoneNumber ?? "";
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0002CB24 File Offset: 0x0002AD24
		public string GetTargetNumber()
		{
			return this.targetNumber ?? "";
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0002CB35 File Offset: 0x0002AD35
		public bool IsCallSessionActive()
		{
			return this.callState > BlockEntityTelephone.TelephoneCallState.Idle;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0002CB40 File Offset: 0x0002AD40
		public void InvalidateCallIfDisconnected()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1 || !this.IsCallSessionActive())
			{
				return;
			}
			if (base.NetworkUID == 0L)
			{
				this.EndCall();
				return;
			}
			if (this.activePeerTelephonePos != null && !this.IsWireNodeReachable(this.activePeerTelephonePos))
			{
				this.EndCall();
				return;
			}
			if (this.incomingCallerTelephonePos != null && !this.IsWireNodeReachable(this.incomingCallerTelephonePos))
			{
				this.EndCall();
				return;
			}
			if (this.IsInCall() && !this.HasReachableVoiceEndpoint())
			{
				this.EndCall();
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0002CBDC File Offset: 0x0002ADDC
		private bool IsWireNodeReachable(BlockPos targetPos)
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
			if (!flag || targetPos == null)
			{
				return false;
			}
			BEWireNode targetNode = this.Api.World.BlockAccessor.GetBlockEntity(targetPos) as BEWireNode;
			return targetNode != null && WireNetworkHandler.GetReachableNodes(this).Contains(targetNode);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002CC41 File Offset: 0x0002AE41
		private bool HasReachableVoiceEndpoint()
		{
			return Enumerable.Any<BEWireNode>(this.GetReachableTelephoneVoiceEndpoints());
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002CC4E File Offset: 0x0002AE4E
		public bool IsInCall()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.InCall;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0002CC59 File Offset: 0x0002AE59
		public bool IsWaitingForAnswer()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.Ringing && this.incomingCallerTelephonePos == null;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0002CC72 File Offset: 0x0002AE72
		public bool HasIncomingCall()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.Ringing && this.incomingCallerTelephonePos != null;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0002CC8B File Offset: 0x0002AE8B
		public bool IsNotInService()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.NotInService;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0002CC98 File Offset: 0x0002AE98
		public string[] GetAvailableTargetNumbers()
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return Array.Empty<string>();
			}
			return Enumerable.ToArray<string>(Enumerable.OrderBy<string, string>(Enumerable.Distinct<string>(Enumerable.Where<string>(Enumerable.Select<BlockEntityTelephone, string>(Enumerable.Where<BlockEntityTelephone>(Enumerable.OfType<BlockEntityTelephone>(network.Nodes), (BlockEntityTelephone t) => t != null && !t.Pos.Equals(this.Pos)), (BlockEntityTelephone t) => t.phoneNumber), (string n) => !string.IsNullOrWhiteSpace(n)), StringComparer.OrdinalIgnoreCase), (string n) => n, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0002CD58 File Offset: 0x0002AF58
		public bool IsTargetNumberBusy(string number)
		{
			if (string.IsNullOrWhiteSpace(number))
			{
				return false;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return false;
			}
			BlockEntityTelephone target = Enumerable.FirstOrDefault<BlockEntityTelephone>(Enumerable.OfType<BlockEntityTelephone>(network.Nodes), (BlockEntityTelephone t) => !t.Pos.Equals(this.Pos) && string.Equals(t.phoneNumber, number, 5));
			return target != null && target.callState == BlockEntityTelephone.TelephoneCallState.InCall;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0002CDC4 File Offset: 0x0002AFC4
		public void SetPhoneNumber(string desired)
		{
			BlockEntityTelephone.<>c__DisplayClass63_0 CS$<>8__locals1 = new BlockEntityTelephone.<>c__DisplayClass63_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.desired = desired;
			if (!this.composeManagedBySwitchboard)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 1)
				{
					return;
				}
			}
			if (this.composeManagedBySwitchboard && !this.composeEnabled)
			{
				ICoreAPI api2 = this.Api;
				if (api2 != null && api2.Side == 1)
				{
					return;
				}
			}
			if (CS$<>8__locals1.desired == null)
			{
				CS$<>8__locals1.desired = "";
			}
			BlockEntityTelephone.<>c__DisplayClass63_0 CS$<>8__locals2 = CS$<>8__locals1;
			IEnumerable<char> desired2 = CS$<>8__locals1.desired;
			Func<char, bool> func;
			if ((func = BlockEntityTelephone.<>O.<0>__IsDigit) == null)
			{
				func = (BlockEntityTelephone.<>O.<0>__IsDigit = new Func<char, bool>(char.IsDigit));
			}
			CS$<>8__locals2.desired = new string(Enumerable.ToArray<char>(Enumerable.Take<char>(Enumerable.Where<char>(desired2, func), 6)));
			if (this.composeManagedBySwitchboard && !string.IsNullOrWhiteSpace(CS$<>8__locals1.desired))
			{
				WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
				if (network != null && Enumerable.Any<BlockEntityTelephone>(Enumerable.OfType<BlockEntityTelephone>(network.Nodes), (BlockEntityTelephone t) => t != null && !t.Pos.Equals(CS$<>8__locals1.<>4__this.Pos) && string.Equals(t.phoneNumber, CS$<>8__locals1.desired, 5)))
				{
					return;
				}
			}
			this.phoneNumber = CS$<>8__locals1.desired;
			if (!string.IsNullOrWhiteSpace(this.targetNumber) && !Enumerable.Contains<string>(this.GetAvailableTargetNumbers(), this.targetNumber, StringComparer.OrdinalIgnoreCase))
			{
				this.targetNumber = "";
			}
			this.MarkDirty(true, null);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0002CF08 File Offset: 0x0002B108
		public void SetTargetNumber(string desired)
		{
			if (this.composeManagedBySwitchboard && !this.composeEnabled)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 1)
				{
					return;
				}
			}
			if (string.IsNullOrWhiteSpace(desired))
			{
				this.targetNumber = "";
				this.MarkDirty(true, null);
				return;
			}
			this.targetNumber = desired.Trim();
			this.MarkDirty(true, null);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0002CF70 File Offset: 0x0002B170
		public void RequestSavePhoneNumber(string desired)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel telephoneSettingsClientChannel = RPVoiceChatMod.TelephoneSettingsClientChannel;
			if (telephoneSettingsClientChannel == null)
			{
				return;
			}
			telephoneSettingsClientChannel.SendPacket<TelephoneSettingsPacket>(new TelephoneSettingsPacket
			{
				TelephonePos = this.Pos,
				Operation = TelephoneSettingsOperation.SetNumber,
				Value = (desired ?? "")
			});
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002CFD0 File Offset: 0x0002B1D0
		public void RequestTargetNumberChange(string value)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel telephoneSettingsClientChannel = RPVoiceChatMod.TelephoneSettingsClientChannel;
			if (telephoneSettingsClientChannel == null)
			{
				return;
			}
			telephoneSettingsClientChannel.SendPacket<TelephoneSettingsPacket>(new TelephoneSettingsPacket
			{
				TelephonePos = this.Pos,
				Operation = TelephoneSettingsOperation.SetTarget,
				Value = (value ?? "")
			});
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0002D030 File Offset: 0x0002B230
		public void RequestStartCall(string desiredTargetNumber = "")
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel telephoneSettingsClientChannel = RPVoiceChatMod.TelephoneSettingsClientChannel;
			if (telephoneSettingsClientChannel == null)
			{
				return;
			}
			telephoneSettingsClientChannel.SendPacket<TelephoneSettingsPacket>(new TelephoneSettingsPacket
			{
				TelephonePos = this.Pos,
				Operation = TelephoneSettingsOperation.StartCall,
				Value = (desiredTargetNumber ?? "")
			});
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002D090 File Offset: 0x0002B290
		public void RequestEndCall()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel telephoneSettingsClientChannel = RPVoiceChatMod.TelephoneSettingsClientChannel;
			if (telephoneSettingsClientChannel == null)
			{
				return;
			}
			telephoneSettingsClientChannel.SendPacket<TelephoneSettingsPacket>(new TelephoneSettingsPacket
			{
				TelephonePos = this.Pos,
				Operation = TelephoneSettingsOperation.EndCall,
				Value = ""
			});
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0002D0EC File Offset: 0x0002B2EC
		public bool StartCall(IPlayer byPlayer, string desiredTargetNumber = "")
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return false;
			}
			if (base.NetworkUID == 0L)
			{
				return false;
			}
			if (this.HasIncomingCall())
			{
				return this.TryAcceptIncomingCall(byPlayer);
			}
			if (this.callState != BlockEntityTelephone.TelephoneCallState.Idle)
			{
				return false;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return false;
			}
			if (this.composeManagedBySwitchboard && !this.composeEnabled)
			{
				return false;
			}
			BEWireNode targetNode = null;
			string text;
			if (this.composeManagedBySwitchboard)
			{
				string dialedNumber = BlockEntityTelephone.NormalizeDialedNumber(desiredTargetNumber);
				if (string.IsNullOrWhiteSpace(dialedNumber))
				{
					dialedNumber = this.targetNumber;
				}
				if (string.IsNullOrWhiteSpace(dialedNumber))
				{
					return false;
				}
				targetNode = Enumerable.FirstOrDefault<BlockEntityTelephone>(Enumerable.OfType<BlockEntityTelephone>(network.Nodes), (BlockEntityTelephone t) => !t.Pos.Equals(this.Pos) && string.Equals(t.phoneNumber, dialedNumber, 5));
				if (targetNode == null)
				{
					this.StartNotInServiceSequence();
					this.MarkDirty(true, null);
					return true;
				}
			}
			else if (!this.TryResolveDirectPeerEndpoint(out targetNode, out text))
			{
				return false;
			}
			if (targetNode == null)
			{
				return false;
			}
			if (this.composeManagedBySwitchboard)
			{
				BlockEntityTelephone managedTargetTelephone = targetNode as BlockEntityTelephone;
				if (managedTargetTelephone != null && managedTargetTelephone.composeManagedBySwitchboard && !managedTargetTelephone.composeEnabled)
				{
					this.StartNotInServiceSequence();
					this.MarkDirty(true, null);
					return true;
				}
			}
			BlockEntityTelephone targetTelephone = targetNode as BlockEntityTelephone;
			if (targetTelephone != null && targetTelephone.callState != BlockEntityTelephone.TelephoneCallState.Idle)
			{
				return false;
			}
			BlockEntityTelephone targetPhone = targetNode as BlockEntityTelephone;
			if (targetPhone == null)
			{
				this.SetState(BlockEntityTelephone.TelephoneCallState.InCall, 0);
				if (!string.IsNullOrWhiteSpace((byPlayer != null) ? byPlayer.PlayerUID : null))
				{
					this.activeCallerPlayerUid = byPlayer.PlayerUID;
					ICoreAPI api2 = this.Api;
					TelephoneVoiceRoutingSystem routingSystem = (api2 != null) ? api2.ModLoader.GetModSystem<TelephoneVoiceRoutingSystem>(true) : null;
					List<VoiceRoute> speakerRoutes = Enumerable.ToList<VoiceRoute>(Enumerable.Select<BlockEntitySpeaker, VoiceRoute>(Enumerable.OfType<BlockEntitySpeaker>(this.GetReachableTelephoneVoiceEndpoints()), (BlockEntitySpeaker speaker) => new VoiceRoute(new Vec3d((double)speaker.Pos.X + 0.5, (double)speaker.Pos.Y + 1.2, (double)speaker.Pos.Z + 0.5), speaker.VoiceEmissionRangeBlocks, speaker.Pos.dimension)));
					if (speakerRoutes.Count > 0)
					{
						if (routingSystem != null)
						{
							routingSystem.SetRoutes(byPlayer.PlayerUID, speakerRoutes);
						}
					}
					else
					{
						ITelephoneVoiceEndpoint voiceEndpoint = targetNode as ITelephoneVoiceEndpoint;
						int emitRange = (voiceEndpoint != null) ? voiceEndpoint.VoiceEmissionRangeBlocks : 2;
						Vec3d emitPos = new Vec3d((double)targetNode.Pos.X + 0.5, (double)targetNode.Pos.Y + 1.2, (double)targetNode.Pos.Z + 0.5);
						if (routingSystem != null)
						{
							routingSystem.SetRoute(byPlayer.PlayerUID, emitPos, emitRange, targetNode.Pos.dimension);
						}
					}
				}
				this.MarkDirty(true, null);
				return true;
			}
			if (string.IsNullOrWhiteSpace((byPlayer != null) ? byPlayer.PlayerUID : null))
			{
				return false;
			}
			targetPhone.RingFromNetwork(byPlayer);
			targetPhone.incomingCallerPlayerUid = byPlayer.PlayerUID;
			targetPhone.incomingCallerTelephonePos = this.Pos.Copy();
			targetPhone.activePeerTelephonePos = this.Pos.Copy();
			this.activePeerTelephonePos = targetPhone.Pos.Copy();
			this.SetState(BlockEntityTelephone.TelephoneCallState.Ringing, 20000);
			this.activeCallerPlayerUid = byPlayer.PlayerUID;
			targetPhone.MarkDirty(true, null);
			this.MarkDirty(true, null);
			return true;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0002D3F6 File Offset: 0x0002B5F6
		public void EndCall()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			this.EndCallInternal(true);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0002D41C File Offset: 0x0002B61C
		private void EndCallInternal(bool notifyPeer)
		{
			if (this.callState == BlockEntityTelephone.TelephoneCallState.Idle && string.IsNullOrWhiteSpace(this.activeCallerPlayerUid) && this.incomingCallerTelephonePos == null)
			{
				return;
			}
			BlockPos peerPos = this.activePeerTelephonePos ?? this.incomingCallerTelephonePos;
			if (!string.IsNullOrWhiteSpace(this.activeCallerPlayerUid))
			{
				ICoreAPI api = this.Api;
				if (api != null)
				{
					TelephoneVoiceRoutingSystem modSystem = api.ModLoader.GetModSystem<TelephoneVoiceRoutingSystem>(true);
					if (modSystem != null)
					{
						modSystem.ClearRoute(this.activeCallerPlayerUid);
					}
				}
			}
			this.SetState(BlockEntityTelephone.TelephoneCallState.Idle, 0);
			this.activeCallerPlayerUid = "";
			this.incomingCallerPlayerUid = "";
			this.incomingCallerTelephonePos = null;
			this.activePeerTelephonePos = null;
			this.MarkDirty(true, null);
			if (!notifyPeer || peerPos == null)
			{
				return;
			}
			ICoreAPI api2 = this.Api;
			object obj;
			if (api2 == null)
			{
				obj = null;
			}
			else
			{
				IWorldAccessor world = api2.World;
				if (world == null)
				{
					obj = null;
				}
				else
				{
					IBlockAccessor blockAccessor = world.BlockAccessor;
					obj = ((blockAccessor != null) ? blockAccessor.GetBlockEntity(peerPos) : null);
				}
			}
			BlockEntityTelephone blockEntityTelephone = obj as BlockEntityTelephone;
			if (blockEntityTelephone == null)
			{
				return;
			}
			blockEntityTelephone.EndCallInternal(false);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0002D510 File Offset: 0x0002B710
		private bool TryResolveDirectPeerEndpoint(out BEWireNode targetNode, out string failureLangKey)
		{
			targetNode = null;
			failureLangKey = null;
			BEWireNode[] peers = Enumerable.ToArray<BEWireNode>(this.GetReachableTelephoneVoiceEndpoints());
			if (peers.Length == 0)
			{
				failureLangKey = "Telephone.Call.Failed.NoPeer";
				return false;
			}
			if (peers.Length > 1)
			{
				if (!Enumerable.All<BEWireNode>(peers, (BEWireNode p) => p is BlockEntitySpeaker))
				{
					failureLangKey = "Telephone.Call.Failed.NotUniquePeer";
					return false;
				}
				targetNode = Enumerable.FirstOrDefault<BEWireNode>(Enumerable.OrderBy<BEWireNode, double>(peers, delegate(BEWireNode p)
				{
					double num = (double)(p.Pos.X - this.Pos.X);
					double dy = (double)(p.Pos.Y - this.Pos.Y);
					double dz = (double)(p.Pos.Z - this.Pos.Z);
					return num * num + dy * dy + dz * dz;
				}));
				return targetNode != null;
			}
			else
			{
				BlockEntityTelephone peerTelephone = peers[0] as BlockEntityTelephone;
				if (peerTelephone != null && peerTelephone.callState != BlockEntityTelephone.TelephoneCallState.Idle)
				{
					failureLangKey = "Telephone.Call.Failed.TargetBusy";
					return false;
				}
				targetNode = peers[0];
				return true;
			}
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0002D5B4 File Offset: 0x0002B7B4
		private void TryPlayLoopingRingingSounds()
		{
			ICoreAPI api = this.Api;
			if (((api != null) ? api.World : null) == null || (this.callState != BlockEntityTelephone.TelephoneCallState.Ringing && this.callState != BlockEntityTelephone.TelephoneCallState.NotInService))
			{
				return;
			}
			long now = this.Api.World.ElapsedMilliseconds;
			if (this.nextRingingSoundAtMs > now)
			{
				return;
			}
			if (this.HasIncomingCall())
			{
				this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/callbell/callbell_1.ogg"), (double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5, null, false, 12f, 0.6f);
				this.nextRingingSoundAtMs = now + 2000L;
				return;
			}
			if (this.callState == BlockEntityTelephone.TelephoneCallState.NotInService)
			{
				this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/telephone/reorder-tone.ogg"), (double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5, null, false, 2f, 0.6f);
				this.nextRingingSoundAtMs = long.MaxValue;
				return;
			}
			if (this.IsWaitingForAnswer())
			{
				this.Api.World.PlaySoundAt(new AssetLocation(RPVoiceChatMod.modID, "sounds/block/telephone/ringback-tone.ogg"), (double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5, null, false, 2f, 0.6f);
				this.nextRingingSoundAtMs = now + 2000L;
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0002D790 File Offset: 0x0002B990
		private void SyncInCallAnimationState()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				this.InitializeClientAnimator();
			}
			if (this.AnimUtil == null)
			{
				return;
			}
			if (this.callState == BlockEntityTelephone.TelephoneCallState.InCall)
			{
				this.StartAnimationIfNotRunning("playing-sound");
				return;
			}
			this.StopAnimation("playing-sound");
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0002D7E4 File Offset: 0x0002B9E4
		private void InitializeClientAnimator()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null && animUtil.animator == null)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					string shapePath = this.ResolveShapePath();
					Block block = base.Block;
					if (((block != null) ? block.Code : null) != null && !string.IsNullOrWhiteSpace(shapePath))
					{
						AssetLocation assetLoc = new AssetLocation(base.Block.Code.Domain, "shapes/" + shapePath + ".json");
						Shape shape = Shape.TryGet(this.Api, assetLoc);
						if (((shape != null) ? shape.Animations : null) != null && shape.Animations.Length != 0)
						{
							shape.InitForAnimations(this.Api.Logger, shapePath, Array.Empty<string>());
						}
					}
					float rotYDeg = this.GetBlockSideRotY();
					animUtil.InitializeAnimator(shapePath, null, null, new Vec3f(0f, rotYDeg, 0f));
					return;
				}
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0002D8CE File Offset: 0x0002BACE
		private string ResolveShapePath()
		{
			Block block = base.Block;
			string text;
			if (block == null)
			{
				text = null;
			}
			else
			{
				CompositeShape shape = block.Shape;
				if (shape == null)
				{
					text = null;
				}
				else
				{
					AssetLocation @base = shape.Base;
					text = ((@base != null) ? @base.Path : null);
				}
			}
			return text ?? "block/telephone";
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002D904 File Offset: 0x0002BB04
		private void StartAnimationIfNotRunning(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			if (animUtil.activeAnimationsByAnimCode.ContainsKey(animationCode))
			{
				return;
			}
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = animationCode,
				Code = animationCode
			});
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002D948 File Offset: 0x0002BB48
		private void StopAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			animUtil.StopAnimation(animationCode);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0002D968 File Offset: 0x0002BB68
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

		// Token: 0x0600085C RID: 2140 RVA: 0x0002DA18 File Offset: 0x0002BC18
		private IEnumerable<BEWireNode> GetReachableTelephoneVoiceEndpoints()
		{
			return Enumerable.Where<BEWireNode>(WireNetworkHandler.GetReachableNodes(this), (BEWireNode n) => n != null && n != this && n is ITelephoneVoiceEndpoint);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0002DA34 File Offset: 0x0002BC34
		public string GetCallFailureLangKeyForUi()
		{
			if (base.NetworkUID == 0L)
			{
				return "Telephone.Call.Failed.NoNetwork";
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return "Telephone.Call.Failed.NoNetwork";
			}
			if (this.composeManagedBySwitchboard)
			{
				if (!this.composeEnabled)
				{
					return this.GetComposeDisabledReasonLangKey();
				}
				if (string.IsNullOrWhiteSpace(this.targetNumber))
				{
					return "Telephone.Call.Failed.NoTarget";
				}
				BlockEntityTelephone targetTelephone = Enumerable.FirstOrDefault<BlockEntityTelephone>(Enumerable.OfType<BlockEntityTelephone>(network.Nodes), (BlockEntityTelephone t) => !t.Pos.Equals(this.Pos) && string.Equals(t.phoneNumber, this.targetNumber, 5));
				if (targetTelephone == null)
				{
					return "Telephone.Call.Failed.TargetUnavailable";
				}
				if (targetTelephone.callState != BlockEntityTelephone.TelephoneCallState.Idle)
				{
					return "Telephone.Call.Failed.TargetBusy";
				}
				return null;
			}
			else
			{
				BEWireNode bewireNode;
				string directModeFailure;
				if (!this.TryResolveDirectPeerEndpoint(out bewireNode, out directModeFailure))
				{
					return directModeFailure;
				}
				return null;
			}
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0002DAD4 File Offset: 0x0002BCD4
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			base.GetBlockInfo(forPlayer, dsc);
			if (!string.IsNullOrWhiteSpace(this.phoneNumber))
			{
				dsc.AppendLine(UIUtils.I18n("Telephone.Number", new object[]
				{
					this.phoneNumber
				}));
			}
			if (this.composeManagedBySwitchboard && !this.composeEnabled)
			{
				dsc.AppendLine(UIUtils.I18n(this.GetComposeDisabledReasonLangKey()));
			}
			string key = "Telephone.State";
			object[] array = new object[1];
			int num = 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Telephone.State.");
			defaultInterpolatedStringHandler.AppendFormatted<BlockEntityTelephone.TelephoneCallState>(this.callState);
			array[num] = UIUtils.I18n(defaultInterpolatedStringHandler.ToStringAndClear());
			dsc.AppendLine(UIUtils.I18n(key, array));
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0002DB84 File Offset: 0x0002BD84
		public override void OnBlockRemoved()
		{
			this.EndCall();
			base.OnBlockRemoved();
			if (!string.IsNullOrWhiteSpace(this.activeCallerPlayerUid))
			{
				ICoreAPI api = this.Api;
				if (api == null)
				{
					return;
				}
				TelephoneVoiceRoutingSystem modSystem = api.ModLoader.GetModSystem<TelephoneVoiceRoutingSystem>(true);
				if (modSystem == null)
				{
					return;
				}
				modSystem.ClearRoute(this.activeCallerPlayerUid);
			}
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0002DBD0 File Offset: 0x0002BDD0
		public override void OnBlockUnloaded()
		{
			this.EndCall();
			base.OnBlockUnloaded();
			if (!string.IsNullOrWhiteSpace(this.activeCallerPlayerUid))
			{
				ICoreAPI api = this.Api;
				if (api == null)
				{
					return;
				}
				TelephoneVoiceRoutingSystem modSystem = api.ModLoader.GetModSystem<TelephoneVoiceRoutingSystem>(true);
				if (modSystem == null)
				{
					return;
				}
				modSystem.ClearRoute(this.activeCallerPlayerUid);
			}
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0002DC1C File Offset: 0x0002BE1C
		private bool TryAcceptIncomingCall(IPlayer byPlayer)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return false;
			}
			if (this.callState != BlockEntityTelephone.TelephoneCallState.Ringing)
			{
				return false;
			}
			if (this.incomingCallerTelephonePos == null)
			{
				return false;
			}
			BlockEntityTelephone callerTelephone = this.Api.World.BlockAccessor.GetBlockEntity(this.incomingCallerTelephonePos) as BlockEntityTelephone;
			if (callerTelephone == null || callerTelephone.callState != BlockEntityTelephone.TelephoneCallState.Ringing)
			{
				this.incomingCallerPlayerUid = "";
				this.incomingCallerTelephonePos = null;
				this.MarkDirty(true, null);
				return false;
			}
			this.SetState(BlockEntityTelephone.TelephoneCallState.InCall, 0);
			callerTelephone.SetState(BlockEntityTelephone.TelephoneCallState.InCall, 0);
			this.activePeerTelephonePos = callerTelephone.Pos.Copy();
			callerTelephone.activePeerTelephonePos = this.Pos.Copy();
			if (!string.IsNullOrWhiteSpace(this.incomingCallerPlayerUid))
			{
				Vec3d emitPosForCaller = new Vec3d((double)this.Pos.X + 0.5, (double)this.Pos.Y + 1.2, (double)this.Pos.Z + 0.5);
				ICoreAPI api2 = this.Api;
				if (api2 != null)
				{
					TelephoneVoiceRoutingSystem modSystem = api2.ModLoader.GetModSystem<TelephoneVoiceRoutingSystem>(true);
					if (modSystem != null)
					{
						modSystem.SetRoute(this.incomingCallerPlayerUid, emitPosForCaller, this.VoiceEmissionRangeBlocks, this.Pos.dimension);
					}
				}
			}
			if (!string.IsNullOrWhiteSpace((byPlayer != null) ? byPlayer.PlayerUID : null))
			{
				this.activeCallerPlayerUid = byPlayer.PlayerUID;
				Vec3d emitPosForCallee = new Vec3d((double)callerTelephone.Pos.X + 0.5, (double)callerTelephone.Pos.Y + 1.2, (double)callerTelephone.Pos.Z + 0.5);
				ICoreAPI api3 = this.Api;
				if (api3 != null)
				{
					TelephoneVoiceRoutingSystem modSystem2 = api3.ModLoader.GetModSystem<TelephoneVoiceRoutingSystem>(true);
					if (modSystem2 != null)
					{
						modSystem2.SetRoute(byPlayer.PlayerUID, emitPosForCallee, callerTelephone.VoiceEmissionRangeBlocks, callerTelephone.Pos.dimension);
					}
				}
			}
			this.incomingCallerPlayerUid = "";
			this.incomingCallerTelephonePos = null;
			callerTelephone.MarkDirty(true, null);
			this.MarkDirty(true, null);
			return true;
		}

		// Token: 0x0400039D RID: 925
		private const string TelephoneShapeCode = "block/telephone";

		// Token: 0x0400039E RID: 926
		private const string InCallAnimationCode = "playing-sound";

		// Token: 0x0400039F RID: 927
		private const int RingDurationMs = 20000;

		// Token: 0x040003A0 RID: 928
		private const int NotInServiceDelayMs = 4000;

		// Token: 0x040003A1 RID: 929
		private const int NotInServiceToneDurationMs = 4200;

		// Token: 0x040003A2 RID: 930
		private const int IncomingRingRepeatMs = 2000;

		// Token: 0x040003A3 RID: 931
		private const int RingbackRepeatMs = 2000;

		// Token: 0x040003A4 RID: 932
		private TelephoneMenuDialog dialog;

		// Token: 0x040003A5 RID: 933
		private BlockEntityTelephone.TelephoneCallState callState;

		// Token: 0x040003A6 RID: 934
		private long stateUntilMs;

		// Token: 0x040003A7 RID: 935
		private string phoneNumber = "";

		// Token: 0x040003A8 RID: 936
		private string targetNumber = "";

		// Token: 0x040003A9 RID: 937
		private bool composeManagedBySwitchboard;

		// Token: 0x040003AA RID: 938
		private bool composeEnabled;

		// Token: 0x040003AB RID: 939
		private string composeDisabledReasonLangKey = "Telegraph.Settings.DisabledNoPower";

		// Token: 0x040003AC RID: 940
		private string activeCallerPlayerUid = "";

		// Token: 0x040003AD RID: 941
		private string incomingCallerPlayerUid = "";

		// Token: 0x040003AE RID: 942
		private BlockPos incomingCallerTelephonePos;

		// Token: 0x040003AF RID: 943
		private BlockPos activePeerTelephonePos;

		// Token: 0x040003B0 RID: 944
		private long nextRingingSoundAtMs;

		// Token: 0x040003B1 RID: 945
		private long notInServiceTransitionAtMs;

		// Token: 0x040003B2 RID: 946
		private long originalCreatedNetworkID;

		// Token: 0x0200017C RID: 380
		private enum TelephoneCallState
		{
			// Token: 0x04000607 RID: 1543
			Idle,
			// Token: 0x04000608 RID: 1544
			Ringing,
			// Token: 0x04000609 RID: 1545
			InCall,
			// Token: 0x0400060A RID: 1546
			NotInService
		}

		// Token: 0x0200017D RID: 381
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400060B RID: 1547
			public static Func<char, bool> <0>__IsDigit;
		}
	}
}
