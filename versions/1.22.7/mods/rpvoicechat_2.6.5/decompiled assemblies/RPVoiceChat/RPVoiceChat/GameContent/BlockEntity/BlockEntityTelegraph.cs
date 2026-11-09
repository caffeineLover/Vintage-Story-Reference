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
	// Token: 0x020000F0 RID: 240
	public class BlockEntityTelegraph : BEWireNode, INetworkRoot, IWireTypedNode, ITelegraphEndpoint
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0002AAC9 File Offset: 0x00028CC9
		protected override int MaxConnections
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0002AACC File Offset: 0x00028CCC
		public override bool IsActiveEndpoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002AACF File Offset: 0x00028CCF
		public long CreatedNetworkID
		{
			get
			{
				return this.originalCreatedNetworkID;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002AAD7 File Offset: 0x00028CD7
		// (set) Token: 0x060007EE RID: 2030 RVA: 0x0002AADF File Offset: 0x00028CDF
		public bool IsPlaying { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002AAE8 File Offset: 0x00028CE8
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0002AAF0 File Offset: 0x00028CF0
		public int Volume { get; set; } = 8;

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002AAF9 File Offset: 0x00028CF9
		public bool GenuineMorseCharacters
		{
			get
			{
				return ServerConfigManager.TelegraphGenuineMorseCharacters;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0002AB00 File Offset: 0x00028D00
		private int MaxMessageLength
		{
			get
			{
				return ServerConfigManager.TelegraphMaxMessageLength;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002AB07 File Offset: 0x00028D07
		private int MessageDeletionDelaySeconds
		{
			get
			{
				return ServerConfigManager.TelegraphMessageDeletionDelaySeconds;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0002AB0E File Offset: 0x00028D0E
		private BEBehaviorAnimatable Animatable
		{
			get
			{
				return base.GetBehavior<BEBehaviorAnimatable>();
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0002AB16 File Offset: 0x00028D16
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0002AB29 File Offset: 0x00028D29
		public WireNodeKind WireNodeKind
		{
			get
			{
				return WireNodeKind.Telegraph;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0002AB2C File Offset: 0x00028D2C
		public string CustomEndpointName
		{
			get
			{
				return this.customEndpointName;
			}
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002ABA6 File Offset: 0x00028DA6
		public override void OnNetworkCreated(long networkID)
		{
			base.OnNetworkCreated(networkID);
			if (this.originalCreatedNetworkID == 0L)
			{
				this.originalCreatedNetworkID = networkID;
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002ABC8 File Offset: 0x00028DC8
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

		// Token: 0x060007FB RID: 2043 RVA: 0x0002AC78 File Offset: 0x00028E78
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

		// Token: 0x060007FC RID: 2044 RVA: 0x0002ADA8 File Offset: 0x00028FA8
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

		// Token: 0x060007FD RID: 2045 RVA: 0x0002AE50 File Offset: 0x00029050
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

		// Token: 0x060007FE RID: 2046 RVA: 0x0002AECE File Offset: 0x000290CE
		public bool IsManagedBySwitchboard()
		{
			return this.routingManagedBySwitchboard;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002AED6 File Offset: 0x000290D6
		public bool HasAdvancedRoutingEnabled()
		{
			return this.routingAdvancedUnlocked;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002AEDE File Offset: 0x000290DE
		public string GetRoutingDisabledReasonLangKey()
		{
			return this.routingDisabledReasonLangKey ?? "Telegraph.Settings.DisabledNoPower";
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002AEEF File Offset: 0x000290EF
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

		// Token: 0x06000802 RID: 2050 RVA: 0x0002AF18 File Offset: 0x00029118
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

		// Token: 0x06000803 RID: 2051 RVA: 0x0002AFD0 File Offset: 0x000291D0
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

		// Token: 0x06000804 RID: 2052 RVA: 0x0002B090 File Offset: 0x00029290
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

		// Token: 0x06000805 RID: 2053 RVA: 0x0002B11C File Offset: 0x0002931C
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

		// Token: 0x06000806 RID: 2054 RVA: 0x0002B1C8 File Offset: 0x000293C8
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

		// Token: 0x06000807 RID: 2055 RVA: 0x0002B21C File Offset: 0x0002941C
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

		// Token: 0x06000808 RID: 2056 RVA: 0x0002B270 File Offset: 0x00029470
		private Task ProcessNextSignalAsync()
		{
			BlockEntityTelegraph.<ProcessNextSignalAsync>d__72 <ProcessNextSignalAsync>d__;
			<ProcessNextSignalAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ProcessNextSignalAsync>d__.<>4__this = this;
			<ProcessNextSignalAsync>d__.<>1__state = -1;
			<ProcessNextSignalAsync>d__.<>t__builder.Start<BlockEntityTelegraph.<ProcessNextSignalAsync>d__72>(ref <ProcessNextSignalAsync>d__);
			return <ProcessNextSignalAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002B2B4 File Offset: 0x000294B4
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

		// Token: 0x0600080A RID: 2058 RVA: 0x0002B428 File Offset: 0x00029628
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

		// Token: 0x0600080B RID: 2059 RVA: 0x0002B4EC File Offset: 0x000296EC
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

		// Token: 0x0600080C RID: 2060 RVA: 0x0002B558 File Offset: 0x00029758
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

		// Token: 0x0600080D RID: 2061 RVA: 0x0002B5A4 File Offset: 0x000297A4
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

		// Token: 0x0600080E RID: 2062 RVA: 0x0002B649 File Offset: 0x00029849
		public string GetSentMessage()
		{
			return this.sentMessage;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002B651 File Offset: 0x00029851
		public string GetReceivedMessage()
		{
			return this.receivedMessage;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0002B659 File Offset: 0x00029859
		public string GetCustomEndpointName()
		{
			return this.customEndpointName ?? "";
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0002B66C File Offset: 0x0002986C
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

		// Token: 0x06000812 RID: 2066 RVA: 0x0002B754 File Offset: 0x00029954
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

		// Token: 0x06000813 RID: 2067 RVA: 0x0002B808 File Offset: 0x00029A08
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

		// Token: 0x06000814 RID: 2068 RVA: 0x0002B9C4 File Offset: 0x00029BC4
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

		// Token: 0x06000815 RID: 2069 RVA: 0x0002BA1C File Offset: 0x00029C1C
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

		// Token: 0x06000816 RID: 2070 RVA: 0x0002BA7D File Offset: 0x00029C7D
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

		// Token: 0x06000817 RID: 2071 RVA: 0x0002BAB0 File Offset: 0x00029CB0
		public void SetPrinter(BlockEntityPrinter printer)
		{
			this.connectedPrinter = printer;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002BAB9 File Offset: 0x00029CB9
		private void UpdateActivityTime()
		{
			this.lastReceivedActivityTime = this.Api.World.ElapsedMilliseconds;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0002BAD1 File Offset: 0x00029CD1
		private void UpdateSentActivityTime()
		{
			this.lastSentActivityTime = this.Api.World.ElapsedMilliseconds;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002BAE9 File Offset: 0x00029CE9
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
			this.CheckForPrinterBelow();
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002BAF8 File Offset: 0x00029CF8
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			this.DisconnectFromPrinter();
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002BB06 File Offset: 0x00029D06
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

		// Token: 0x0600081D RID: 2077 RVA: 0x0002BB42 File Offset: 0x00029D42
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.DisconnectFromPrinter();
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002BB50 File Offset: 0x00029D50
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

		// Token: 0x0600081F RID: 2079 RVA: 0x0002BC18 File Offset: 0x00029E18
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

		// Token: 0x06000820 RID: 2080 RVA: 0x0002BD1A File Offset: 0x00029F1A
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

		// Token: 0x06000821 RID: 2081 RVA: 0x0002BD4D File Offset: 0x00029F4D
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

		// Token: 0x06000822 RID: 2082 RVA: 0x0002BD80 File Offset: 0x00029F80
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

		// Token: 0x06000823 RID: 2083 RVA: 0x0002BEF8 File Offset: 0x0002A0F8
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

		// Token: 0x06000824 RID: 2084 RVA: 0x0002BF68 File Offset: 0x0002A168
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

		// Token: 0x06000825 RID: 2085 RVA: 0x0002BFA8 File Offset: 0x0002A1A8
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

		// Token: 0x06000826 RID: 2086 RVA: 0x0002C0CC File Offset: 0x0002A2CC
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

		// Token: 0x06000827 RID: 2087 RVA: 0x0002C124 File Offset: 0x0002A324
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

		// Token: 0x06000828 RID: 2088 RVA: 0x0002C1F8 File Offset: 0x0002A3F8
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

		// Token: 0x06000829 RID: 2089 RVA: 0x0002C2A8 File Offset: 0x0002A4A8
		private bool IsDialogOpen()
		{
			return this.dialog != null && this.dialog.IsOpened();
		}

		// Token: 0x0400037F RID: 895
		private const string TelegraphShapeCode = "telegraphkey";

		// Token: 0x04000380 RID: 896
		private const string ClickAnimationCode = "click";

		// Token: 0x04000381 RID: 897
		private TelegraphMenuDialog dialog;

		// Token: 0x04000382 RID: 898
		private long originalCreatedNetworkID;

		// Token: 0x04000385 RID: 901
		private string sentMessage = "";

		// Token: 0x04000386 RID: 902
		private string receivedMessage = "";

		// Token: 0x04000387 RID: 903
		private string sentMessageOriginal = "";

		// Token: 0x04000388 RID: 904
		private string receivedMessageOriginal = "";

		// Token: 0x04000389 RID: 905
		private string customEndpointName = "";

		// Token: 0x0400038A RID: 906
		private string targetEndpointName = "all";

		// Token: 0x0400038B RID: 907
		private bool routingManagedBySwitchboard;

		// Token: 0x0400038C RID: 908
		private bool routingAdvancedUnlocked;

		// Token: 0x0400038D RID: 909
		private string routingDisabledReasonLangKey = "Telegraph.Settings.DisabledNoPower";

		// Token: 0x0400038E RID: 910
		private WireRouteMode lastReceivedRouteMode;

		// Token: 0x0400038F RID: 911
		private string lastReceivedSourceEndpointName;

		// Token: 0x04000390 RID: 912
		private string lastReceivedTargetEndpointName;

		// Token: 0x04000391 RID: 913
		private Queue<char> pendingSignals = new Queue<char>();

		// Token: 0x04000392 RID: 914
		private BlockEntityPrinter connectedPrinter;

		// Token: 0x04000393 RID: 915
		private long lastReceivedActivityTime;

		// Token: 0x04000394 RID: 916
		private long lastSentActivityTime;

		// Token: 0x04000395 RID: 917
		private bool isReceivedCountdownActive;

		// Token: 0x04000396 RID: 918
		private int receivedCountdownSeconds;

		// Token: 0x04000397 RID: 919
		private long receivedCountdownEndTime;

		// Token: 0x04000398 RID: 920
		private bool isSentCountdownActive;

		// Token: 0x04000399 RID: 921
		private int sentCountdownSeconds;

		// Token: 0x0400039A RID: 922
		private long sentCountdownEndTime;

		// Token: 0x0400039B RID: 923
		private bool telegramPrinted;

		// Token: 0x0400039C RID: 924
		private bool printPacketSentForCurrentMessage;
	}
}
