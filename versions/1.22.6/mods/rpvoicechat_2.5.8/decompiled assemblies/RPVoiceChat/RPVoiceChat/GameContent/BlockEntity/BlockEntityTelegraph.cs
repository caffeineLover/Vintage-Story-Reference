using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Systems;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000B7 RID: 183
	public class BlockEntityTelegraph : BEWireNode, INetworkRoot, IWireTypedNode, ITelegraphEndpoint
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001F8A9 File Offset: 0x0001DAA9
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001F8AC File Offset: 0x0001DAAC
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001F8AF File Offset: 0x0001DAAF
		public long CreatedNetworkID
		{
			get
			{
				return this.originalCreatedNetworkID;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001F8B7 File Offset: 0x0001DAB7
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0001F8BF File Offset: 0x0001DABF
		public bool IsPlaying { get; private set; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001F8C8 File Offset: 0x0001DAC8
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
		public int Volume { get; set; } = 8;

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001F8D9 File Offset: 0x0001DAD9
		public bool GenuineMorseCharacters
		{
			get
			{
				return ServerConfigManager.TelegraphGenuineMorseCharacters;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x0001F8E0 File Offset: 0x0001DAE0
		private int MaxMessageLength
		{
			get
			{
				return ServerConfigManager.TelegraphMaxMessageLength;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001F8E7 File Offset: 0x0001DAE7
		private int MessageDeletionDelaySeconds
		{
			get
			{
				return ServerConfigManager.TelegraphMessageDeletionDelaySeconds;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001F8EE File Offset: 0x0001DAEE
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001F8F6 File Offset: 0x0001DAF6
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

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001F909 File Offset: 0x0001DB09
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Telegraph;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001F90C File Offset: 0x0001DB0C
		public string CustomEndpointName
		{
			get
			{
				return this.customEndpointName;
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001F986 File Offset: 0x0001DB86
		public override void OnNetworkCreated(long networkID)
		{
			base.OnNetworkCreated(networkID);
			if (this.originalCreatedNetworkID == 0L)
			{
				this.originalCreatedNetworkID = networkID;
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001F9A8 File Offset: 0x0001DBA8
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.IsPlaying = false;
			this.lastReceivedActivityTime = api.World.ElapsedMilliseconds;
			this.lastSentActivityTime = api.World.ElapsedMilliseconds;
			base.OnReceivedSignalEvent = (EventHandler<WireNetworkMessage>)Delegate.Combine(base.OnReceivedSignalEvent, new EventHandler<WireNetworkMessage>(this.HandleReceivedSignal));
			if (api.Side == 2)
			{
				this.InitializeClientAnimator();
				this.UpdateDisplayMessages();
				api.Event.RegisterGameTickListener(new Action<float>(this.OnClientGameTick), 1000, 0);
			}
			else if (base.NetworkUID != 0L)
			{
				WireNetworkHandler.RefreshTelegraphRoutingSnapshot(base.NetworkUID);
			}
			this.CheckForPrinterBelow();
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001FA58 File Offset: 0x0001DC58
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.sentMessageOriginal = tree.GetString("sentMessage", null);
			this.receivedMessageOriginal = tree.GetString("receivedMessage", null);
			this.customEndpointName = tree.GetString("customEndpointName", "");
			this.targetEndpointName = tree.GetString("targetEndpointName", "all");
			long savedOriginalCreatedNetworkID = tree.GetLong("originalCreatedNetworkID", 0L);
			if (savedOriginalCreatedNetworkID != 0L)
			{
				this.originalCreatedNetworkID = savedOriginalCreatedNetworkID;
			}
			this.routingManagedBySwitchboard = tree.GetBool("rpvc:routingManaged", false);
			this.routingAdvancedUnlocked = tree.GetBool("rpvc:routingAdvanced", false);
			this.routingDisabledReasonLangKey = tree.GetString("rpvc:routingDisabledReason", "Telegraph.Settings.DisabledNoPower");
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && base.NetworkUID != 0L)
			{
				WireNetworkHandler.RefreshTelegraphRoutingSnapshot(base.NetworkUID);
			}
			this.UpdateDisplayMessages();
			if (this.IsDialogOpen())
			{
				TelegraphMenuDialog telegraphMenuDialog = this.dialog;
				if (telegraphMenuDialog != null)
				{
					telegraphMenuDialog.UpdateSentText(this.sentMessage);
				}
				TelegraphMenuDialog telegraphMenuDialog2 = this.dialog;
				if (telegraphMenuDialog2 != null)
				{
					telegraphMenuDialog2.UpdateReceivedText(this.receivedMessage);
				}
				TelegraphMenuDialog telegraphMenuDialog3 = this.dialog;
				if (telegraphMenuDialog3 == null)
				{
					return;
				}
				telegraphMenuDialog3.RefreshRoutingControls();
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001FB88 File Offset: 0x0001DD88
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("sentMessage", this.sentMessageOriginal);
			tree.SetString("receivedMessage", this.receivedMessageOriginal);
			tree.SetString("customEndpointName", this.customEndpointName);
			tree.SetString("targetEndpointName", this.targetEndpointName);
			tree.SetLong("originalCreatedNetworkID", this.originalCreatedNetworkID);
			tree.SetBool("rpvc:routingManaged", this.routingManagedBySwitchboard);
			tree.SetBool("rpvc:routingAdvanced", this.routingAdvancedUnlocked);
			tree.SetString("rpvc:routingDisabledReason", this.routingDisabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower");
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001FC30 File Offset: 0x0001DE30
		public void ApplyServerRoutingFlags(bool managedBySwitchboard, bool advancedRoutingUnlocked, string disabledReasonLangKey = null)
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 1)
			{
				return;
			}
			bool flag = this.routingManagedBySwitchboard != managedBySwitchboard || this.routingAdvancedUnlocked != advancedRoutingUnlocked || !string.Equals(this.routingDisabledReasonLangKey, disabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower", 4);
			this.routingManagedBySwitchboard = managedBySwitchboard;
			this.routingAdvancedUnlocked = advancedRoutingUnlocked;
			this.routingDisabledReasonLangKey = (disabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower");
			if (flag)
			{
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001FCAE File Offset: 0x0001DEAE
		public bool IsManagedBySwitchboard()
		{
			return this.routingManagedBySwitchboard;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001FCB6 File Offset: 0x0001DEB6
		public bool HasAdvancedRoutingEnabled()
		{
			return this.routingAdvancedUnlocked;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001FCBE File Offset: 0x0001DEBE
		public string GetRoutingDisabledReasonLangKey()
		{
			return this.routingDisabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower";
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001FCCF File Offset: 0x0001DECF
		public string GetTargetEndpointName()
		{
			if (!this.HasAdvancedRoutingEnabled())
			{
				return "all";
			}
			if (string.IsNullOrWhiteSpace(this.targetEndpointName))
			{
				return "all";
			}
			return this.targetEndpointName;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001FCF8 File Offset: 0x0001DEF8
		public string[] GetAvailableEndpointNames()
		{
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return Array.Empty<string>();
			}
			List<string> names = new List<string>();
			foreach (BlockEntityTelegraph endpoint in Enumerable.OfType<BlockEntityTelegraph>(network.Nodes.ToArray()))
			{
				if (!endpoint.Pos.Equals(this.Pos) && !string.IsNullOrWhiteSpace(endpoint.CustomEndpointName) && !names.Contains(endpoint.CustomEndpointName))
				{
					names.Add(endpoint.CustomEndpointName);
				}
			}
			names.Sort(StringComparer.OrdinalIgnoreCase);
			return names.ToArray();
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001FDB0 File Offset: 0x0001DFB0
		public bool SetCustomEndpointName(string desiredName, out string failureLangKey)
		{
			failureLangKey = null;
			desiredName = (((desiredName != null) ? desiredName.Trim() : null) ?? "");
			if (!this.IsManagedBySwitchboard())
			{
				return false;
			}
			if (!this.HasAdvancedRoutingEnabled())
			{
				failureLangKey = this.GetRoutingDisabledReasonLangKey();
				return false;
			}
			if (desiredName.Length == 0)
			{
				this.customEndpointName = "";
				this.MarkDirty(false, null);
				return true;
			}
			if (WireNetworkHandler.IsEndpointNameTaken(base.NetworkUID, desiredName, this))
			{
				failureLangKey = "Telegraph.Settings.NameAlreadyUsed";
				return false;
			}
			this.customEndpointName = desiredName;
			this.MarkDirty(false, null);
			if (!string.Equals(this.targetEndpointName, "all", 5) && !Enumerable.Contains<string>(this.GetAvailableEndpointNames(), this.targetEndpointName, StringComparer.OrdinalIgnoreCase))
			{
				this.targetEndpointName = "all";
			}
			return true;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001FE70 File Offset: 0x0001E070
		public void SetTargetEndpointName(string name)
		{
			if (!this.IsManagedBySwitchboard() || !this.HasAdvancedRoutingEnabled())
			{
				this.targetEndpointName = "all";
				this.MarkDirty(false, null);
				return;
			}
			if (string.IsNullOrWhiteSpace(name) || string.Equals(name, "all", 5))
			{
				this.targetEndpointName = "all";
				this.MarkDirty(false, null);
				return;
			}
			if (Enumerable.Contains<string>(this.GetAvailableEndpointNames(), name, StringComparer.OrdinalIgnoreCase))
			{
				this.targetEndpointName = name;
			}
			else
			{
				this.targetEndpointName = "all";
			}
			this.MarkDirty(false, null);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001FEFC File Offset: 0x0001E0FC
		public bool OnInteract()
		{
			if (this.Api.Side == 1)
			{
				return true;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi == null)
			{
				return true;
			}
			TelegraphMenuDialog telegraphMenuDialog = this.dialog;
			if (telegraphMenuDialog != null && telegraphMenuDialog.IsOpened())
			{
				return true;
			}
			this.dialog = new TelegraphMenuDialog(capi, this);
			this.UpdateDisplayMessages();
			this.MarkDirty(false, null);
			TelegraphMenuDialog telegraphMenuDialog2 = this.dialog;
			if (telegraphMenuDialog2 != null)
			{
				telegraphMenuDialog2.UpdateSentText(this.sentMessage);
			}
			TelegraphMenuDialog telegraphMenuDialog3 = this.dialog;
			if (telegraphMenuDialog3 != null)
			{
				telegraphMenuDialog3.UpdateReceivedText(this.receivedMessage);
			}
			TelegraphMenuDialog telegraphMenuDialog4 = this.dialog;
			if (telegraphMenuDialog4 != null)
			{
				telegraphMenuDialog4.RefreshRoutingControls();
			}
			this.dialog.TryOpen();
			return true;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001FFA8 File Offset: 0x0001E1A8
		public void RequestSaveCustomEndpointName(string desiredName)
		{
			if (this.Api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel telegraphSettingsClientChannel = RPVoiceChatMod.TelegraphSettingsClientChannel;
			if (telegraphSettingsClientChannel == null)
			{
				return;
			}
			telegraphSettingsClientChannel.SendPacket<TelegraphSettingsPacket>(new TelegraphSettingsPacket
			{
				TelegraphPos = this.Pos,
				Operation = TelegraphSettingsOperation.SetCustomName,
				Value = (desiredName ?? "")
			});
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		public void RequestTargetEndpointChange(string targetName)
		{
			if (this.Api.Side != 2)
			{
				return;
			}
			IClientNetworkChannel telegraphSettingsClientChannel = RPVoiceChatMod.TelegraphSettingsClientChannel;
			if (telegraphSettingsClientChannel == null)
			{
				return;
			}
			telegraphSettingsClientChannel.SendPacket<TelegraphSettingsPacket>(new TelegraphSettingsPacket
			{
				TelegraphPos = this.Pos,
				Operation = TelegraphSettingsOperation.SetTarget,
				Value = (targetName ?? "all")
			});
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00020050 File Offset: 0x0001E250
		private Task ProcessNextSignalAsync()
		{
			BlockEntityTelegraph.<ProcessNextSignalAsync>d__72 <ProcessNextSignalAsync>d__;
			<ProcessNextSignalAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ProcessNextSignalAsync>d__.<>4__this = this;
			<ProcessNextSignalAsync>d__.<>1__state = -1;
			<ProcessNextSignalAsync>d__.<>t__builder.Start<BlockEntityTelegraph.<ProcessNextSignalAsync>d__72>(ref <ProcessNextSignalAsync>d__);
			return <ProcessNextSignalAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00020094 File Offset: 0x0001E294
		public void SendSignal(char keyChar)
		{
			if (this.Api.Side != 2)
			{
				return;
			}
			ICoreClientAPI clientApi = this.Api as ICoreClientAPI;
			if (clientApi == null)
			{
				return;
			}
			if (this.sentMessageOriginal.Length >= this.MaxMessageLength)
			{
				return;
			}
			if (base.NetworkUID == 0L)
			{
				this.Api.Logger.Warning("Cannot send signal: NetworkUID is 0 (not connected to network)");
				return;
			}
			this.UpdateActivityTime();
			this.UpdateSentActivityTime();
			this.pendingSignals.Enqueue(keyChar);
			string messageToSend = keyChar.ToString();
			ReadOnlySpan<char> readOnlySpan = this.sentMessageOriginal;
			char c = keyChar;
			this.sentMessageOriginal = readOnlySpan + new ReadOnlySpan<char>(ref c);
			this.UpdateDisplayMessages();
			this.MarkDirty(false, null);
			if (this.IsDialogOpen())
			{
				TelegraphMenuDialog telegraphMenuDialog = this.dialog;
				if (telegraphMenuDialog != null)
				{
					telegraphMenuDialog.UpdateSentText(this.sentMessage);
				}
			}
			this.TriggerKeyClickAnimation();
			if (!string.IsNullOrEmpty(messageToSend))
			{
				string resolvedTarget = this.GetTargetEndpointName();
				WireRouteMode routeMode = string.Equals(resolvedTarget, "all", 5) ? WireRouteMode.All : WireRouteMode.NamedEndpoint;
				IClientNetworkChannel channel = clientApi.Network.GetChannel(WireNetworkHandler.NetworkChannel);
				WireNetworkMessage wireNetworkMessage = new WireNetworkMessage();
				wireNetworkMessage.NetworkUID = base.NetworkUID;
				wireNetworkMessage.Message = messageToSend;
				wireNetworkMessage.SenderPos = this.Pos;
				IClientPlayer player = clientApi.World.Player;
				wireNetworkMessage.SenderPlayerUID = ((player != null) ? player.PlayerUID : null);
				wireNetworkMessage.RouteMode = routeMode;
				wireNetworkMessage.TargetEndpointName = ((routeMode == WireRouteMode.NamedEndpoint) ? resolvedTarget : null);
				channel.SendPacket<WireNetworkMessage>(wireNetworkMessage);
			}
			if (!this.IsPlaying)
			{
				this.ProcessNextSignalAsync();
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00020208 File Offset: 0x0001E408
		public void OnReceivedSignal(char keyChar, BlockPos senderPos)
		{
			if (this.Api.Side != 2)
			{
				return;
			}
			this.UpdateActivityTime();
			if (this.Api is ICoreClientAPI)
			{
				ReadOnlySpan<char> readOnlySpan = this.receivedMessageOriginal;
				char keyChar2 = keyChar;
				this.receivedMessageOriginal = readOnlySpan + new ReadOnlySpan<char>(ref keyChar2);
				this.UpdateDisplayMessages();
				this.MarkDirty(false, null);
				if (this.IsDialogOpen())
				{
					TelegraphMenuDialog telegraphMenuDialog = this.dialog;
					if (telegraphMenuDialog != null)
					{
						telegraphMenuDialog.UpdateReceivedText(this.receivedMessage);
					}
				}
				this.printPacketSentForCurrentMessage = false;
				if (this.Pos.Equals(senderPos))
				{
					this.TriggerKeyClickAnimation();
				}
			}
			Task.Run(() => this.PlayMorseAsync(BlockEntityTelegraph.ConvertKeyCodeToMorse(keyChar)));
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000202CC File Offset: 0x0001E4CC
		private void HandleReceivedSignal(object sender, WireNetworkMessage e)
		{
			if (e == null || e.Message == null || e.Message.Length == 0)
			{
				return;
			}
			this.lastReceivedRouteMode = e.RouteMode;
			this.lastReceivedSourceEndpointName = this.ResolveTelegraphNameAt(e.SenderPos);
			this.lastReceivedTargetEndpointName = e.TargetEndpointName;
			char keyChar = e.Message.get_Chars(0);
			this.OnReceivedSignal(keyChar, e.SenderPos);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00020338 File Offset: 0x0001E538
		private Task PlayMorseAsync(string morse)
		{
			BlockEntityTelegraph.<PlayMorseAsync>d__76 <PlayMorseAsync>d__;
			<PlayMorseAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PlayMorseAsync>d__.<>4__this = this;
			<PlayMorseAsync>d__.morse = morse;
			<PlayMorseAsync>d__.<>1__state = -1;
			<PlayMorseAsync>d__.<>t__builder.Start<BlockEntityTelegraph.<PlayMorseAsync>d__76>(ref <PlayMorseAsync>d__);
			return <PlayMorseAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00020384 File Offset: 0x0001E584
		public void ProcessPrintPacket(string message, string sourceEndpointName = null, string targetEndpointName = null, string networkName = null)
		{
			if (this.connectedPrinter != null && !string.IsNullOrEmpty(message))
			{
				string messageToPrint = this.GenuineMorseCharacters ? this.ConvertStringToMorse(message) : message;
				string networkUid = base.NetworkUID.ToString();
				string resolvedNetworkName = (!string.IsNullOrWhiteSpace(networkName)) ? networkName : this.ResolvePrintableNetworkName();
				this.connectedPrinter.CreateTelegram(messageToPrint, networkUid, resolvedNetworkName, sourceEndpointName, targetEndpointName);
				this.telegramPrinted = true;
			}
			this.receivedMessage = "";
			this.receivedMessageOriginal = "";
			this.isReceivedCountdownActive = false;
			this.receivedCountdownSeconds = 0;
			this.receivedCountdownEndTime = 0L;
			this.printPacketSentForCurrentMessage = false;
			this.MarkDirty(false, null);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00020429 File Offset: 0x0001E629
		public string GetSentMessage()
		{
			return this.sentMessage;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00020431 File Offset: 0x0001E631
		public string GetReceivedMessage()
		{
			return this.receivedMessage;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00020439 File Offset: 0x0001E639
		public string GetCustomEndpointName()
		{
			return this.customEndpointName ?? "";
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0002044C File Offset: 0x0001E64C
		public void ClearMessages()
		{
			this.sentMessage = "";
			this.receivedMessage = "";
			this.sentMessageOriginal = "";
			this.receivedMessageOriginal = "";
			this.MarkDirty(false, null);
			this.pendingSignals.Clear();
			if (this.IsDialogOpen())
			{
				TelegraphMenuDialog telegraphMenuDialog = this.dialog;
				if (telegraphMenuDialog != null)
				{
					telegraphMenuDialog.UpdateSentText("");
				}
				TelegraphMenuDialog telegraphMenuDialog2 = this.dialog;
				if (telegraphMenuDialog2 != null)
				{
					telegraphMenuDialog2.UpdateReceivedText("");
				}
			}
			this.isReceivedCountdownActive = false;
			this.receivedCountdownSeconds = 0;
			this.receivedCountdownEndTime = 0L;
			this.isSentCountdownActive = false;
			this.sentCountdownSeconds = 0;
			this.sentCountdownEndTime = 0L;
			this.telegramPrinted = false;
			this.printPacketSentForCurrentMessage = false;
			if (this.IsDialogOpen())
			{
				TelegraphMenuDialog telegraphMenuDialog3 = this.dialog;
				if (telegraphMenuDialog3 != null)
				{
					telegraphMenuDialog3.UpdateCountdown(-1);
				}
				TelegraphMenuDialog telegraphMenuDialog4 = this.dialog;
				if (telegraphMenuDialog4 == null)
				{
					return;
				}
				telegraphMenuDialog4.UpdateSentCountdown(-1);
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00020534 File Offset: 0x0001E734
		private void UpdateDisplayMessages()
		{
			this.sentMessage = "";
			this.receivedMessage = "";
			string text = this.sentMessageOriginal;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text.get_Chars(i);
				this.sentMessage += (this.GenuineMorseCharacters ? BlockEntityTelegraph.ConvertKeyCodeToMorse(c) : c.ToString());
			}
			text = this.receivedMessageOriginal;
			for (int i = 0; i < text.Length; i++)
			{
				char c2 = text.get_Chars(i);
				this.receivedMessage += (this.GenuineMorseCharacters ? BlockEntityTelegraph.ConvertKeyCodeToMorse(c2) : c2.ToString());
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000205E8 File Offset: 0x0001E7E8
		private static string ConvertKeyCodeToMorse(char keyChar)
		{
			switch (char.ToUpper(keyChar))
			{
			case '.':
				return ".-.-.-";
			case '0':
				return "-----";
			case '1':
				return ".----";
			case '2':
				return "..---";
			case '3':
				return "...--";
			case '4':
				return "....-";
			case '5':
				return ".....";
			case '6':
				return "-....";
			case '7':
				return "--...";
			case '8':
				return "---..";
			case '9':
				return "----.";
			case 'A':
				return ".-";
			case 'B':
				return "-...";
			case 'C':
				return "-.-.";
			case 'D':
				return "-..";
			case 'E':
				return ".";
			case 'F':
				return "..-.";
			case 'G':
				return "--.";
			case 'H':
				return "....";
			case 'I':
				return "..";
			case 'J':
				return ".---";
			case 'K':
				return "-.-";
			case 'L':
				return ".-..";
			case 'M':
				return "--";
			case 'N':
				return "-.";
			case 'O':
				return "---";
			case 'P':
				return ".--.";
			case 'Q':
				return "--.-";
			case 'R':
				return ".-.";
			case 'S':
				return "...";
			case 'T':
				return "-";
			case 'U':
				return "..-";
			case 'V':
				return "...-";
			case 'W':
				return ".--";
			case 'X':
				return "-..-";
			case 'Y':
				return "-.--";
			case 'Z':
				return "--..";
			}
			return "";
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000207A4 File Offset: 0x0001E9A4
		private string ConvertStringToMorse(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			StringBuilder morse = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				string morseChar = BlockEntityTelegraph.ConvertKeyCodeToMorse(text.get_Chars(i));
				if (!string.IsNullOrEmpty(morseChar))
				{
					morse.Append(morseChar);
				}
			}
			return morse.ToString();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000207FC File Offset: 0x0001E9FC
		private void CheckForPrinterBelow()
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
				return;
			}
			BlockPos belowPos = this.Pos.DownCopy(1);
			BlockEntityPrinter printer = this.Api.World.BlockAccessor.GetBlockEntity(belowPos) as BlockEntityPrinter;
			if (printer != null)
			{
				this.ConnectToPrinter(printer);
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0002085D File Offset: 0x0001EA5D
		private void ConnectToPrinter(BlockEntityPrinter printer)
		{
			this.connectedPrinter = printer;
			if (this.Api.Side == 1)
			{
				PrinterSystem modSystem = this.Api.ModLoader.GetModSystem<PrinterSystem>(true);
				if (modSystem == null)
				{
					return;
				}
				modSystem.RegisterTelegraphWithPrinter(this);
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00020890 File Offset: 0x0001EA90
		public void SetPrinter(BlockEntityPrinter printer)
		{
			this.connectedPrinter = printer;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00020899 File Offset: 0x0001EA99
		private void UpdateActivityTime()
		{
			this.lastReceivedActivityTime = this.Api.World.ElapsedMilliseconds;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000208B1 File Offset: 0x0001EAB1
		private void UpdateSentActivityTime()
		{
			this.lastSentActivityTime = this.Api.World.ElapsedMilliseconds;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000208C9 File Offset: 0x0001EAC9
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
			this.CheckForPrinterBelow();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000208D8 File Offset: 0x0001EAD8
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			this.DisconnectFromPrinter();
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000208E6 File Offset: 0x0001EAE6
		private void DisconnectFromPrinter()
		{
			if (this.connectedPrinter != null)
			{
				if (this.Api.Side == 1)
				{
					PrinterSystem modSystem = this.Api.ModLoader.GetModSystem<PrinterSystem>(true);
					if (modSystem != null)
					{
						modSystem.UnregisterTelegraphWithPrinter(this);
					}
				}
				this.connectedPrinter = null;
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00020922 File Offset: 0x0001EB22
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.DisconnectFromPrinter();
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00020930 File Offset: 0x0001EB30
		private void OnClientGameTick(float dt)
		{
			if (this.Api.Side != 2)
			{
				return;
			}
			long elapsedMilliseconds = this.Api.World.ElapsedMilliseconds;
			double secondsSinceLastReceivedActivity = (double)(elapsedMilliseconds - this.lastReceivedActivityTime) / 1000.0;
			if (!string.IsNullOrEmpty(this.receivedMessageOriginal) && !this.isReceivedCountdownActive && !this.printPacketSentForCurrentMessage && secondsSinceLastReceivedActivity >= 2.0)
			{
				this.StartReceivedCountdown();
			}
			if (this.isReceivedCountdownActive)
			{
				this.UpdateReceivedCountdown();
			}
			double secondsSinceLastSentActivity = (double)(elapsedMilliseconds - this.lastSentActivityTime) / 1000.0;
			if (!string.IsNullOrEmpty(this.sentMessageOriginal) && !this.isSentCountdownActive && secondsSinceLastSentActivity >= 3.0)
			{
				this.StartSentCountdown();
			}
			if (this.isSentCountdownActive)
			{
				this.UpdateSentCountdown();
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000209F8 File Offset: 0x0001EBF8
		public void CheckAutoSave()
		{
			double secondsSinceLastReceivedActivity = (double)(this.Api.World.ElapsedMilliseconds - this.lastReceivedActivityTime) / 1000.0;
			if (!string.IsNullOrEmpty(this.receivedMessageOriginal) && !this.isReceivedCountdownActive && !this.printPacketSentForCurrentMessage && secondsSinceLastReceivedActivity >= 2.0)
			{
				this.StartReceivedCountdown();
			}
			if (this.isReceivedCountdownActive)
			{
				this.UpdateReceivedCountdown();
			}
			if (secondsSinceLastReceivedActivity >= (double)(this.MessageDeletionDelaySeconds + 2) && (!string.IsNullOrEmpty(this.receivedMessageOriginal) || this.telegramPrinted))
			{
				this.receivedMessage = "";
				this.receivedMessageOriginal = "";
				this.isReceivedCountdownActive = false;
				this.receivedCountdownSeconds = 0;
				this.receivedCountdownEndTime = 0L;
				this.telegramPrinted = false;
				this.printPacketSentForCurrentMessage = false;
				this.MarkDirty(false, null);
				if (this.IsDialogOpen())
				{
					TelegraphMenuDialog telegraphMenuDialog = this.dialog;
					if (telegraphMenuDialog != null)
					{
						telegraphMenuDialog.UpdateReceivedText("");
					}
					TelegraphMenuDialog telegraphMenuDialog2 = this.dialog;
					if (telegraphMenuDialog2 == null)
					{
						return;
					}
					telegraphMenuDialog2.UpdateCountdown(-1);
				}
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00020AFA File Offset: 0x0001ECFA
		private void StartReceivedCountdown()
		{
			this.isReceivedCountdownActive = true;
			this.receivedCountdownSeconds = this.MessageDeletionDelaySeconds;
			if (this.IsDialogOpen())
			{
				TelegraphMenuDialog telegraphMenuDialog = this.dialog;
				if (telegraphMenuDialog == null)
				{
					return;
				}
				telegraphMenuDialog.UpdateCountdown(this.receivedCountdownSeconds);
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00020B2D File Offset: 0x0001ED2D
		private void StartSentCountdown()
		{
			this.isSentCountdownActive = true;
			this.sentCountdownSeconds = this.MessageDeletionDelaySeconds;
			if (this.IsDialogOpen())
			{
				TelegraphMenuDialog telegraphMenuDialog = this.dialog;
				if (telegraphMenuDialog == null)
				{
					return;
				}
				telegraphMenuDialog.UpdateSentCountdown(this.sentCountdownSeconds);
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00020B60 File Offset: 0x0001ED60
		private void UpdateReceivedCountdown()
		{
			long currentTime = this.Api.World.ElapsedMilliseconds;
			double secondsSinceLastReceivedActivity = (double)(currentTime - this.lastReceivedActivityTime) / 1000.0;
			int newCountdown = Math.Max(0, this.MessageDeletionDelaySeconds - (int)secondsSinceLastReceivedActivity + 2);
			if (newCountdown != this.receivedCountdownSeconds)
			{
				this.receivedCountdownSeconds = newCountdown;
				if (this.IsDialogOpen())
				{
					TelegraphMenuDialog telegraphMenuDialog = this.dialog;
					if (telegraphMenuDialog != null)
					{
						telegraphMenuDialog.UpdateCountdown(this.receivedCountdownSeconds);
					}
				}
				if (this.receivedCountdownSeconds == 0 && this.receivedCountdownEndTime == 0L)
				{
					this.receivedCountdownEndTime = currentTime;
					if (this.Api.Side == 2 && !this.printPacketSentForCurrentMessage && !string.IsNullOrEmpty(this.receivedMessageOriginal))
					{
						CommDeliveryPacket packet = new CommDeliveryPacket
						{
							DevicePos = this.Pos,
							PayloadType = CommPayloadType.Text,
							TextMessage = this.receivedMessageOriginal,
							NetworkName = this.ResolvePrintableNetworkName(),
							SourceEndpointName = this.lastReceivedSourceEndpointName,
							TargetEndpointName = ((this.lastReceivedRouteMode == WireRouteMode.NamedEndpoint) ? this.lastReceivedTargetEndpointName : null)
						};
						RPVoiceChatMod.TelegraphPrintClientChannel.SendPacket<CommDeliveryPacket>(packet);
						this.printPacketSentForCurrentMessage = true;
					}
				}
			}
			if (this.receivedCountdownSeconds == 0 && this.receivedCountdownEndTime > 0L && (double)(currentTime - this.receivedCountdownEndTime) / 1000.0 >= 2.0)
			{
				if (this.IsDialogOpen())
				{
					TelegraphMenuDialog telegraphMenuDialog2 = this.dialog;
					if (telegraphMenuDialog2 != null)
					{
						telegraphMenuDialog2.UpdateCountdown(-1);
					}
				}
				this.isReceivedCountdownActive = false;
				this.receivedCountdownEndTime = 0L;
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00020CD8 File Offset: 0x0001EED8
		private string ResolveTelegraphNameAt(BlockPos nodePos)
		{
			if (nodePos == null)
			{
				return null;
			}
			WireNetwork network = WireNetworkHandler.GetNetwork(base.NetworkUID);
			if (network == null)
			{
				return null;
			}
			BlockEntityTelegraph endpoint = Enumerable.FirstOrDefault<BlockEntityTelegraph>(Enumerable.OfType<BlockEntityTelegraph>(network.Nodes), (BlockEntityTelegraph t) => t.Pos != null && t.Pos.Equals(nodePos));
			if (endpoint == null)
			{
				return null;
			}
			string name = endpoint.GetCustomEndpointName();
			if (!string.IsNullOrWhiteSpace(name))
			{
				return name;
			}
			return null;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00020D48 File Offset: 0x0001EF48
		private string ResolvePrintableNetworkName()
		{
			if (!this.routingAdvancedUnlocked)
			{
				return null;
			}
			string displayName = WireNetworkHandler.GetDisplayName(base.NetworkUID);
			if (!string.Equals(displayName, base.NetworkUID.ToString(), 5))
			{
				return displayName;
			}
			return null;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00020D88 File Offset: 0x0001EF88
		private void UpdateSentCountdown()
		{
			long currentTime = this.Api.World.ElapsedMilliseconds;
			double secondsSinceLastSentActivity = (double)(currentTime - this.lastSentActivityTime) / 1000.0;
			int newCountdown = Math.Max(0, this.MessageDeletionDelaySeconds - (int)secondsSinceLastSentActivity + 2);
			if (newCountdown != this.sentCountdownSeconds)
			{
				this.sentCountdownSeconds = newCountdown;
				if (this.IsDialogOpen())
				{
					TelegraphMenuDialog telegraphMenuDialog = this.dialog;
					if (telegraphMenuDialog != null)
					{
						telegraphMenuDialog.UpdateSentCountdown(this.sentCountdownSeconds);
					}
				}
				if (this.sentCountdownSeconds == 0 && this.sentCountdownEndTime == 0L)
				{
					this.sentCountdownEndTime = currentTime;
				}
			}
			if (this.sentCountdownSeconds == 0 && this.sentCountdownEndTime > 0L && (double)(currentTime - this.sentCountdownEndTime) / 1000.0 >= 2.0)
			{
				this.sentMessage = "";
				this.sentMessageOriginal = "";
				this.isSentCountdownActive = false;
				this.sentCountdownSeconds = 0;
				this.sentCountdownEndTime = 0L;
				this.MarkDirty(false, null);
				if (this.IsDialogOpen())
				{
					TelegraphMenuDialog telegraphMenuDialog2 = this.dialog;
					if (telegraphMenuDialog2 != null)
					{
						telegraphMenuDialog2.UpdateSentText("");
					}
					TelegraphMenuDialog telegraphMenuDialog3 = this.dialog;
					if (telegraphMenuDialog3 == null)
					{
						return;
					}
					telegraphMenuDialog3.UpdateSentCountdown(-1);
				}
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00020EAC File Offset: 0x0001F0AC
		private void TriggerKeyClickAnimation()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil == null)
			{
				return;
			}
			if (animUtil.activeAnimationsByAnimCode.ContainsKey("click"))
			{
				animUtil.StopAnimation("click");
			}
			animUtil.StartAnimation(new AnimationMetaData
			{
				Animation = "click",
				Code = "click"
			});
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00020F04 File Offset: 0x0001F104
		private void InitializeClientAnimator()
		{
			BlockEntityAnimationUtil animUtil = this.AnimUtil;
			if (animUtil != null && animUtil.animator == null)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					Block block = base.Block;
					if (((block != null) ? block.Code : null) != null)
					{
						AssetLocation assetLoc = new AssetLocation(base.Block.Code.Domain, "shapes/block/telegraphkey.json");
						Shape shape = Shape.TryGet(this.Api, assetLoc);
						if (((shape != null) ? shape.Animations : null) != null && shape.Animations.Length != 0)
						{
							shape.InitForAnimations(this.Api.Logger, "telegraphkey", Array.Empty<string>());
						}
					}
					float rotYDeg = this.GetBlockSideRotY();
					animUtil.InitializeAnimator("telegraphkey", null, null, new Vec3f(0f, rotYDeg, 0f));
					return;
				}
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00020FD8 File Offset: 0x0001F1D8
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

		// Token: 0x06000614 RID: 1556 RVA: 0x00021088 File Offset: 0x0001F288
		private bool IsDialogOpen()
		{
			return this.dialog != null && this.dialog.IsOpened();
		}

		// Token: 0x040002A0 RID: 672
		private const string TelegraphShapeCode = "telegraphkey";

		// Token: 0x040002A1 RID: 673
		private const string ClickAnimationCode = "click";

		// Token: 0x040002A2 RID: 674
		private TelegraphMenuDialog dialog;

		// Token: 0x040002A3 RID: 675
		private long originalCreatedNetworkID;

		// Token: 0x040002A6 RID: 678
		private string sentMessage = "";

		// Token: 0x040002A7 RID: 679
		private string receivedMessage = "";

		// Token: 0x040002A8 RID: 680
		private string sentMessageOriginal = "";

		// Token: 0x040002A9 RID: 681
		private string receivedMessageOriginal = "";

		// Token: 0x040002AA RID: 682
		private string customEndpointName = "";

		// Token: 0x040002AB RID: 683
		private string targetEndpointName = "all";

		// Token: 0x040002AC RID: 684
		private bool routingManagedBySwitchboard;

		// Token: 0x040002AD RID: 685
		private bool routingAdvancedUnlocked;

		// Token: 0x040002AE RID: 686
		private string routingDisabledReasonLangKey = "Telegraph.Settings.DisabledNoPower";

		// Token: 0x040002AF RID: 687
		private WireRouteMode lastReceivedRouteMode;

		// Token: 0x040002B0 RID: 688
		private string lastReceivedSourceEndpointName;

		// Token: 0x040002B1 RID: 689
		private string lastReceivedTargetEndpointName;

		// Token: 0x040002B2 RID: 690
		private Queue<char> pendingSignals = new Queue<char>();

		// Token: 0x040002B3 RID: 691
		private BlockEntityPrinter connectedPrinter;

		// Token: 0x040002B4 RID: 692
		private long lastReceivedActivityTime;

		// Token: 0x040002B5 RID: 693
		private long lastSentActivityTime;

		// Token: 0x040002B6 RID: 694
		private bool isReceivedCountdownActive;

		// Token: 0x040002B7 RID: 695
		private int receivedCountdownSeconds;

		// Token: 0x040002B8 RID: 696
		private long receivedCountdownEndTime;

		// Token: 0x040002B9 RID: 697
		private bool isSentCountdownActive;

		// Token: 0x040002BA RID: 698
		private int sentCountdownSeconds;

		// Token: 0x040002BB RID: 699
		private long sentCountdownEndTime;

		// Token: 0x040002BC RID: 700
		private bool telegramPrinted;

		// Token: 0x040002BD RID: 701
		private bool printPacketSentForCurrentMessage;
	}
}
