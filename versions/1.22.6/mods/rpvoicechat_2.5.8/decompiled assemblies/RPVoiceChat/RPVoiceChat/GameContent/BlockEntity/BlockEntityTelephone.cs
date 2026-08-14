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
	// Token: 0x020000B8 RID: 184
	public class BlockEntityTelephone : BEWireNode, INetworkRoot, IWireTypedNode, ITelephoneVoiceEndpoint
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0002109F File Offset: 0x0001F29F
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x000210A7 File Offset: 0x0001F2A7
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x000210BA File Offset: 0x0001F2BA
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x000210BD File Offset: 0x0001F2BD
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x000210C0 File Offset: 0x0001F2C0
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Telephone;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x000210C3 File Offset: 0x0001F2C3
		public int VoiceEmissionRangeBlocks
		{
			get
			{
				return ServerConfigManager.TelephoneAudibleDistance;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x000210CA File Offset: 0x0001F2CA
		public long CreatedNetworkID
		{
			get
			{
				return this.originalCreatedNetworkID;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000210D2 File Offset: 0x0001F2D2
		public override void OnNetworkCreated(long networkID)
		{
			base.OnNetworkCreated(networkID);
			if (this.originalCreatedNetworkID == 0L)
			{
				this.originalCreatedNetworkID = networkID;
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x000210F4 File Offset: 0x0001F2F4
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

		// Token: 0x0600061E RID: 1566 RVA: 0x00021174 File Offset: 0x0001F374
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

		// Token: 0x0600061F RID: 1567 RVA: 0x000211DC File Offset: 0x0001F3DC
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

		// Token: 0x06000620 RID: 1568 RVA: 0x000213AC File Offset: 0x0001F5AC
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

		// Token: 0x06000621 RID: 1569 RVA: 0x000215AC File Offset: 0x0001F7AC
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

		// Token: 0x06000622 RID: 1570 RVA: 0x000216BC File Offset: 0x0001F8BC
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

		// Token: 0x06000623 RID: 1571 RVA: 0x00021768 File Offset: 0x0001F968
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

		// Token: 0x06000624 RID: 1572 RVA: 0x000217B6 File Offset: 0x0001F9B6
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

		// Token: 0x06000625 RID: 1573 RVA: 0x000217F4 File Offset: 0x0001F9F4
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

		// Token: 0x06000626 RID: 1574 RVA: 0x00021854 File Offset: 0x0001FA54
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

		// Token: 0x06000627 RID: 1575 RVA: 0x000218D2 File Offset: 0x0001FAD2
		public bool IsManagedBySwitchboard()
		{
			return this.composeManagedBySwitchboard;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000218DA File Offset: 0x0001FADA
		public bool CanCompose()
		{
			return this.composeEnabled;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x000218E2 File Offset: 0x0001FAE2
		public string GetComposeDisabledReasonLangKey()
		{
			return this.composeDisabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower";
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x000218F3 File Offset: 0x0001FAF3
		public string GetPhoneNumber()
		{
			return this.phoneNumber ?? "";
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00021904 File Offset: 0x0001FB04
		public string GetTargetNumber()
		{
			return this.targetNumber ?? "";
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00021915 File Offset: 0x0001FB15
		public bool IsInCall()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.InCall;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00021920 File Offset: 0x0001FB20
		public bool IsWaitingForAnswer()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.Ringing && this.incomingCallerTelephonePos == null;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00021939 File Offset: 0x0001FB39
		public bool HasIncomingCall()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.Ringing && this.incomingCallerTelephonePos != null;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00021952 File Offset: 0x0001FB52
		public bool IsNotInService()
		{
			return this.callState == BlockEntityTelephone.TelephoneCallState.NotInService;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0002195D File Offset: 0x0001FB5D
		public bool IsCallSessionActive()
		{
			return this.callState > BlockEntityTelephone.TelephoneCallState.Idle;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00021968 File Offset: 0x0001FB68
		public string[] GetAvailableTargetNumbers()
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return Array.Empty<string>();
			}
			return Enumerable.ToArray<string>(Enumerable.OrderBy<string, string>(Enumerable.Distinct<string>(Enumerable.Where<string>(Enumerable.Select<BlockEntityTelephone, string>(Enumerable.Where<BlockEntityTelephone>(Enumerable.OfType<BlockEntityTelephone>(network.Nodes), (BlockEntityTelephone t) => t != null && !t.Pos.Equals(this.Pos)), (BlockEntityTelephone t) => t.phoneNumber), (string n) => !string.IsNullOrWhiteSpace(n)), StringComparer.OrdinalIgnoreCase), (string n) => n, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00021A28 File Offset: 0x0001FC28
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

		// Token: 0x06000633 RID: 1587 RVA: 0x00021A94 File Offset: 0x0001FC94
		public void SetPhoneNumber(string desired)
		{
			BlockEntityTelephone.<>c__DisplayClass60_0 CS$<>8__locals1 = new BlockEntityTelephone.<>c__DisplayClass60_0();
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
			BlockEntityTelephone.<>c__DisplayClass60_0 CS$<>8__locals2 = CS$<>8__locals1;
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

		// Token: 0x06000634 RID: 1588 RVA: 0x00021BD8 File Offset: 0x0001FDD8
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

		// Token: 0x06000635 RID: 1589 RVA: 0x00021C40 File Offset: 0x0001FE40
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

		// Token: 0x06000636 RID: 1590 RVA: 0x00021CA0 File Offset: 0x0001FEA0
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

		// Token: 0x06000637 RID: 1591 RVA: 0x00021D00 File Offset: 0x0001FF00
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

		// Token: 0x06000638 RID: 1592 RVA: 0x00021D60 File Offset: 0x0001FF60
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

		// Token: 0x06000639 RID: 1593 RVA: 0x00021DBC File Offset: 0x0001FFBC
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

		// Token: 0x0600063A RID: 1594 RVA: 0x000220C6 File Offset: 0x000202C6
		public void EndCall()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			this.EndCallInternal(true);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x000220EC File Offset: 0x000202EC
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

		// Token: 0x0600063C RID: 1596 RVA: 0x000221E0 File Offset: 0x000203E0
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

		// Token: 0x0600063D RID: 1597 RVA: 0x00022284 File Offset: 0x00020484
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

		// Token: 0x0600063E RID: 1598 RVA: 0x00022460 File Offset: 0x00020660
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

		// Token: 0x0600063F RID: 1599 RVA: 0x000224B4 File Offset: 0x000206B4
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

		// Token: 0x06000640 RID: 1600 RVA: 0x0002259E File Offset: 0x0002079E
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

		// Token: 0x06000641 RID: 1601 RVA: 0x000225D4 File Offset: 0x000207D4
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

		// Token: 0x06000642 RID: 1602 RVA: 0x00022618 File Offset: 0x00020818
		private void StopAnimation(string animationCode)
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			animUtil.StopAnimation(animationCode);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00022638 File Offset: 0x00020838
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

		// Token: 0x06000644 RID: 1604 RVA: 0x000226E8 File Offset: 0x000208E8
		private IEnumerable<BEWireNode> GetReachableTelephoneVoiceEndpoints()
		{
			return Enumerable.Where<BEWireNode>(WireNetworkHandler.GetReachableNodes(this), (BEWireNode n) => n != null && n != this && n is ITelephoneVoiceEndpoint);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00022704 File Offset: 0x00020904
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

		// Token: 0x06000646 RID: 1606 RVA: 0x000227A4 File Offset: 0x000209A4
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

		// Token: 0x06000647 RID: 1607 RVA: 0x00022854 File Offset: 0x00020A54
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

		// Token: 0x06000648 RID: 1608 RVA: 0x000228A0 File Offset: 0x00020AA0
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

		// Token: 0x06000649 RID: 1609 RVA: 0x000228EC File Offset: 0x00020AEC
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

		// Token: 0x040002BE RID: 702
		private const string TelephoneShapeCode = "block/telephone";

		// Token: 0x040002BF RID: 703
		private const string InCallAnimationCode = "playing-sound";

		// Token: 0x040002C0 RID: 704
		private const int RingDurationMs = 20000;

		// Token: 0x040002C1 RID: 705
		private const int NotInServiceDelayMs = 4000;

		// Token: 0x040002C2 RID: 706
		private const int NotInServiceToneDurationMs = 4200;

		// Token: 0x040002C3 RID: 707
		private const int IncomingRingRepeatMs = 2000;

		// Token: 0x040002C4 RID: 708
		private const int RingbackRepeatMs = 2000;

		// Token: 0x040002C5 RID: 709
		private TelephoneMenuDialog dialog;

		// Token: 0x040002C6 RID: 710
		private BlockEntityTelephone.TelephoneCallState callState;

		// Token: 0x040002C7 RID: 711
		private long stateUntilMs;

		// Token: 0x040002C8 RID: 712
		private string phoneNumber = "";

		// Token: 0x040002C9 RID: 713
		private string targetNumber = "";

		// Token: 0x040002CA RID: 714
		private bool composeManagedBySwitchboard;

		// Token: 0x040002CB RID: 715
		private bool composeEnabled;

		// Token: 0x040002CC RID: 716
		private string composeDisabledReasonLangKey = "Telegraph.Settings.DisabledNoPower";

		// Token: 0x040002CD RID: 717
		private string activeCallerPlayerUid = "";

		// Token: 0x040002CE RID: 718
		private string incomingCallerPlayerUid = "";

		// Token: 0x040002CF RID: 719
		private BlockPos incomingCallerTelephonePos;

		// Token: 0x040002D0 RID: 720
		private BlockPos activePeerTelephonePos;

		// Token: 0x040002D1 RID: 721
		private long nextRingingSoundAtMs;

		// Token: 0x040002D2 RID: 722
		private long notInServiceTransitionAtMs;

		// Token: 0x040002D3 RID: 723
		private long originalCreatedNetworkID;

		// Token: 0x02000125 RID: 293
		private enum TelephoneCallState
		{
			// Token: 0x04000487 RID: 1159
			Idle,
			// Token: 0x04000488 RID: 1160
			Ringing,
			// Token: 0x04000489 RID: 1161
			InCall,
			// Token: 0x0400048A RID: 1162
			NotInService
		}

		// Token: 0x02000126 RID: 294
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400048B RID: 1163
			public static Func<char, bool> <0>__IsDigit;
		}
	}
}
