using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using RPVoiceChat.Audio.Input;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Block;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Items;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.src.Networking.Packets;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat
{
	// Token: 0x02000020 RID: 32
	public abstract class RPVoiceChatMod : ModSystem
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x0000596A File Offset: 0x00003B6A
		public override void StartPre(ICoreAPI api)
		{
			ModConfig.Init(api);
			WorldConfig.Init(api);
			new Logger(api);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005980 File Offset: 0x00003B80
		public override void Start(ICoreAPI api)
		{
			RPVoiceChatMod.ModApi = api;
			string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (!string.IsNullOrWhiteSpace(modFolder))
			{
				FfmpegLocator.SetModFolder(modFolder);
			}
			if (api.Side == 2)
			{
				RPVoiceChatMod.capi = (api as ICoreClientAPI);
				RPVoiceChatMod.ClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("welding").RegisterMessageType<WeldingHitPacket>();
				RPVoiceChatMod.TelegraphPrintClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("telegraphprint").RegisterMessageType<CommDeliveryPacket>();
				RPVoiceChatMod.TelegraphSettingsClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("telegraphsettings").RegisterMessageType<TelegraphSettingsPacket>();
				RPVoiceChatMod.TelephoneSettingsClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("telephonesettings").RegisterMessageType<TelephoneSettingsPacket>();
				RPVoiceChatMod.RadioSettingsClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("radiosettings").RegisterMessageType<RadioSettingsPacket>().RegisterMessageType<RadioClientNotificationPacket>();
				RPVoiceChatMod.RadioTalkieClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("radiotalkie").RegisterMessageType<RadioTalkieStatePacket>().RegisterMessageType<RadioTalkieSettingsPacket>();
				RPVoiceChatMod.SwitchboardClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("switchboardsettings").RegisterMessageType<SwitchboardRenameNetworkPacket>().RegisterMessageType<SwitchboardPowerModePacket>();
				RPVoiceChatMod.AnnounceClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("announce").RegisterMessageType<AnnouncePacket>();
				RPVoiceChatMod.NametagConfigClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("rpvc-nametag-config").RegisterMessageType<NametagConfigChangedPacket>();
				PlayerNameTagRenderer.Init(RPVoiceChatMod.capi, null);
			}
			else if (api.Side == 1)
			{
				RPVoiceChatMod.sapi = (api as ICoreServerAPI);
				RPVoiceChatMod.ServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("welding").RegisterMessageType<WeldingHitPacket>().SetMessageHandler<WeldingHitPacket>(new NetworkClientMessageHandler<WeldingHitPacket>(this.OnWeldingHitReceived));
				RPVoiceChatMod.TelegraphPrintServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("telegraphprint").RegisterMessageType<CommDeliveryPacket>().SetMessageHandler<CommDeliveryPacket>(new NetworkClientMessageHandler<CommDeliveryPacket>(this.OnCommDeliveryPacket));
				RPVoiceChatMod.TelegraphSettingsServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("telegraphsettings").RegisterMessageType<TelegraphSettingsPacket>().SetMessageHandler<TelegraphSettingsPacket>(new NetworkClientMessageHandler<TelegraphSettingsPacket>(this.OnTelegraphSettingsPacket));
				RPVoiceChatMod.TelephoneSettingsServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("telephonesettings").RegisterMessageType<TelephoneSettingsPacket>().SetMessageHandler<TelephoneSettingsPacket>(new NetworkClientMessageHandler<TelephoneSettingsPacket>(this.OnTelephoneSettingsPacket));
				RPVoiceChatMod.RadioSettingsServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("radiosettings").RegisterMessageType<RadioSettingsPacket>().RegisterMessageType<RadioClientNotificationPacket>().SetMessageHandler<RadioSettingsPacket>(new NetworkClientMessageHandler<RadioSettingsPacket>(this.OnRadioSettingsPacket));
				RPVoiceChatMod.RadioTalkieServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("radiotalkie").RegisterMessageType<RadioTalkieStatePacket>().RegisterMessageType<RadioTalkieSettingsPacket>().SetMessageHandler<RadioTalkieStatePacket>(new NetworkClientMessageHandler<RadioTalkieStatePacket>(this.OnRadioTalkieStatePacket)).SetMessageHandler<RadioTalkieSettingsPacket>(new NetworkClientMessageHandler<RadioTalkieSettingsPacket>(this.OnRadioTalkieSettingsPacket));
				RPVoiceChatMod.SwitchboardServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("switchboardsettings").RegisterMessageType<SwitchboardRenameNetworkPacket>().RegisterMessageType<SwitchboardPowerModePacket>().SetMessageHandler<SwitchboardRenameNetworkPacket>(new NetworkClientMessageHandler<SwitchboardRenameNetworkPacket>(this.OnSwitchboardRenameNetworkPacket)).SetMessageHandler<SwitchboardPowerModePacket>(new NetworkClientMessageHandler<SwitchboardPowerModePacket>(this.OnSwitchboardPowerModePacket));
				RPVoiceChatMod.AnnounceServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("announce").RegisterMessageType<AnnouncePacket>();
				RPVoiceChatMod.NametagConfigServerChannel = RPVoiceChatMod.sapi.Network.RegisterChannel("rpvc-nametag-config").RegisterMessageType<NametagConfigChangedPacket>();
			}
			this.patchManager = new PatchManager(RPVoiceChatMod.modID);
			this.patchManager.Patch(api);
			ItemRegistry.RegisterItems(api);
			BlockRegistry.RegisterBlocks(api);
			BlockEntityRegistry.RegisterBlockEntities(api);
			BlockBehaviorRegistry.RegisterBlockEntityBehaviors(api);
			BlockEntityBehaviorRegistry.RegisterBlockEntityBehaviors(api);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005CFC File Offset: 0x00003EFC
		public override void AssetsFinalize(ICoreAPI api)
		{
			base.AssetsFinalize(api);
			if (api.Side == 1)
			{
				BlockBehaviorRegistry.AddBehaviors(api);
				BlockEntityBehaviorRegistry.AddBlockEntityBehaviors(api);
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005D1A File Offset: 0x00003F1A
		public override void Dispose()
		{
			PatchManager patchManager = this.patchManager;
			if (patchManager == null)
			{
				return;
			}
			patchManager.Dispose();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005D2C File Offset: 0x00003F2C
		private void OnWeldingHitReceived(IPlayer fromPlayer, WeldingHitPacket packet)
		{
			BEWeldable be = fromPlayer.Entity.World.BlockAccessor.GetBlockEntity(packet.Pos) as BEWeldable;
			if (be != null)
			{
				be.OnHammerHitOver(fromPlayer, packet.HitPosition);
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005D6C File Offset: 0x00003F6C
		private void OnCommDeliveryPacket(IServerPlayer player, CommDeliveryPacket packet)
		{
			if (packet == null || packet.PayloadType != CommPayloadType.Text || string.IsNullOrEmpty(packet.TextMessage))
			{
				return;
			}
			BlockEntityTelegraph telegraph = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.DevicePos) as BlockEntityTelegraph;
			if (telegraph != null)
			{
				telegraph.ProcessPrintPacket(packet.TextMessage, packet.SourceEndpointName, packet.TargetEndpointName, packet.NetworkName);
				return;
			}
			ILogger logger = RPVoiceChatMod.sapi.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Telegraph at ");
			defaultInterpolatedStringHandler.AppendFormatted<BlockPos>(packet.DevicePos);
			defaultInterpolatedStringHandler.AppendLiteral(" not found for comm delivery packet");
			logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005E1C File Offset: 0x0000401C
		private void OnTelegraphSettingsPacket(IServerPlayer player, TelegraphSettingsPacket packet)
		{
			BlockEntityTelegraph telegraph = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.TelegraphPos) as BlockEntityTelegraph;
			if (telegraph == null)
			{
				ILogger logger = RPVoiceChatMod.sapi.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Telegraph at ");
				defaultInterpolatedStringHandler.AppendFormatted<BlockPos>(packet.TelegraphPos);
				defaultInterpolatedStringHandler.AppendLiteral(" not found for settings packet");
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			TelegraphSettingsOperation operation = packet.Operation;
			if (operation == TelegraphSettingsOperation.SetCustomName)
			{
				string text;
				telegraph.SetCustomEndpointName(packet.Value, out text);
				return;
			}
			if (operation != TelegraphSettingsOperation.SetTarget)
			{
				return;
			}
			telegraph.SetTargetEndpointName(packet.Value);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005EC0 File Offset: 0x000040C0
		private void OnSwitchboardRenameNetworkPacket(IServerPlayer player, SwitchboardRenameNetworkPacket packet)
		{
			BlockEntitySwitchboard switchboard = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.SwitchboardPos) as BlockEntitySwitchboard;
			if (switchboard == null)
			{
				ILogger logger = RPVoiceChatMod.sapi.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Switchboard at ");
				defaultInterpolatedStringHandler.AppendFormatted<BlockPos>(packet.SwitchboardPos);
				defaultInterpolatedStringHandler.AppendLiteral(" not found for rename packet");
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			string text;
			switchboard.RenameNetwork(packet.NetworkName, out text);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005F48 File Offset: 0x00004148
		private void OnTelephoneSettingsPacket(IServerPlayer player, TelephoneSettingsPacket packet)
		{
			BlockEntityTelephone telephone = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.TelephonePos) as BlockEntityTelephone;
			if (telephone == null)
			{
				ILogger logger = RPVoiceChatMod.sapi.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Telephone at ");
				defaultInterpolatedStringHandler.AppendFormatted<BlockPos>(packet.TelephonePos);
				defaultInterpolatedStringHandler.AppendLiteral(" not found for settings packet");
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			switch (packet.Operation)
			{
			case TelephoneSettingsOperation.SetNumber:
				telephone.SetPhoneNumber(packet.Value);
				return;
			case TelephoneSettingsOperation.SetTarget:
				telephone.SetTargetNumber(packet.Value);
				return;
			case TelephoneSettingsOperation.StartCall:
				telephone.StartCall(player, packet.Value);
				return;
			case TelephoneSettingsOperation.EndCall:
				telephone.EndCall();
				return;
			default:
				return;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000600B File Offset: 0x0000420B
		internal static void SendRadioClientNotification(IServerPlayer player, string langKey)
		{
			if (player == null || string.IsNullOrWhiteSpace(langKey))
			{
				return;
			}
			IServerNetworkChannel radioSettingsServerChannel = RPVoiceChatMod.RadioSettingsServerChannel;
			if (radioSettingsServerChannel == null)
			{
				return;
			}
			radioSettingsServerChannel.SendPacket<RadioClientNotificationPacket>(new RadioClientNotificationPacket
			{
				LangKey = langKey
			}, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006040 File Offset: 0x00004240
		private void OnRadioSettingsPacket(IServerPlayer player, RadioSettingsPacket packet)
		{
			if (((packet != null) ? packet.BlockPos : null) == null)
			{
				return;
			}
			switch (packet.Operation)
			{
			case RadioSettingsOperation.SetFrequency:
			case RadioSettingsOperation.SetDisplayName:
			{
				BlockEntityRadioSupervisionConsole console = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioSupervisionConsole;
				if (console != null)
				{
					if (packet.Operation != RadioSettingsOperation.SetFrequency)
					{
						console.SetDisplayName(packet.Value);
						return;
					}
					if (!console.TrySetFrequency(packet.Value))
					{
						RPVoiceChatMod.SendRadioClientNotification(player, "Radio.Error.FrequencyInUse");
						return;
					}
				}
				break;
			}
			case RadioSettingsOperation.SetEmitterMode:
			{
				BlockEntityRadioEmitter emitter = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioEmitter;
				if (emitter != null)
				{
					emitter.SetOperatingMode((RadioEmitterOperatingMode)packet.IntValue);
					return;
				}
				break;
			}
			case RadioSettingsOperation.SetReceiverFrequency:
			{
				BlockEntityRadioReceiver receiver = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioReceiver;
				if (receiver != null)
				{
					receiver.SetTunedFrequency(packet.Value);
					return;
				}
				break;
			}
			case RadioSettingsOperation.SetRepeaterFrequency:
			{
				BlockEntityRadioEmitter repeaterEmitter = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioEmitter;
				if (repeaterEmitter != null && !repeaterEmitter.TrySetRepeaterFrequency(packet.Value))
				{
					RPVoiceChatMod.SendRadioClientNotification(player, "Radio.Error.FrequencyInUse");
					return;
				}
				break;
			}
			case RadioSettingsOperation.SetMicrophoneTransmit:
			{
				BlockEntityRadioMicrophone microphone = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioMicrophone;
				if (microphone != null)
				{
					microphone.SetTransmitting(player, packet.IntValue != 0);
					return;
				}
				break;
			}
			case RadioSettingsOperation.SetMixingConsoleHlsUrl:
			{
				BlockEntityRadioMixingConsole mixingConsoleUrl = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioMixingConsole;
				if (mixingConsoleUrl != null)
				{
					mixingConsoleUrl.SetHlsUrl(packet.Value);
					return;
				}
				break;
			}
			case RadioSettingsOperation.SetMixingConsoleOnAir:
			{
				BlockEntityRadioMixingConsole mixingConsoleAir = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioMixingConsole;
				if (mixingConsoleAir != null)
				{
					string failureLangKey = BlockEntityRadioMixingConsole.GetOnAirFailureLangKey(mixingConsoleAir.SetOnAir(player, packet.IntValue != 0));
					if (!string.IsNullOrWhiteSpace(failureLangKey))
					{
						RPVoiceChatMod.SendRadioClientNotification(player, failureLangKey);
					}
				}
				break;
			}
			case RadioSettingsOperation.SetReceiverEnabled:
			{
				BlockEntityRadioReceiver receiverPower = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioReceiver;
				if (receiverPower != null)
				{
					receiverPower.SetEnabled(packet.IntValue != 0);
					return;
				}
				break;
			}
			case RadioSettingsOperation.SetReceiverPlaybackRange:
			{
				BlockEntityRadioReceiver receiverRange = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioReceiver;
				if (receiverRange != null)
				{
					receiverRange.SetPlaybackRange(packet.IntValue);
					return;
				}
				break;
			}
			case RadioSettingsOperation.SetReceiverPlaybackVolume:
			{
				BlockEntityRadioReceiver receiverVolume = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.BlockPos) as BlockEntityRadioReceiver;
				if (receiverVolume != null)
				{
					receiverVolume.SetPlaybackVolume(packet.IntValue);
					return;
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006301 File Offset: 0x00004501
		private void OnRadioTalkieStatePacket(IServerPlayer player, RadioTalkieStatePacket packet)
		{
			RadioTalkieTransmissionSystem modSystem = RPVoiceChatMod.sapi.ModLoader.GetModSystem<RadioTalkieTransmissionSystem>(true);
			if (modSystem == null)
			{
				return;
			}
			modSystem.SetTalkieTransmitting(player, packet != null && packet.Transmitting, (packet != null) ? packet.Frequency : null);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006336 File Offset: 0x00004536
		private void OnRadioTalkieSettingsPacket(IServerPlayer player, RadioTalkieSettingsPacket packet)
		{
			if (player == null || packet == null)
			{
				return;
			}
			ItemRadio.TryApplyServerSettings(player, packet.SlotNumber, packet.Frequency, packet.InventoryListen, packet.ListenVolumePercent);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006360 File Offset: 0x00004560
		private void OnSwitchboardPowerModePacket(IServerPlayer player, SwitchboardPowerModePacket packet)
		{
			BlockEntitySwitchboard switchboard = RPVoiceChatMod.sapi.World.BlockAccessor.GetBlockEntity(packet.SwitchboardPos) as BlockEntitySwitchboard;
			if (switchboard == null)
			{
				ILogger logger = RPVoiceChatMod.sapi.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Switchboard at ");
				defaultInterpolatedStringHandler.AppendFormatted<BlockPos>(packet.SwitchboardPos);
				defaultInterpolatedStringHandler.AppendLiteral(" not found for power-mode packet");
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			switchboard.SetUsePowerRequirements(packet.UsePowerRequirements);
		}

		// Token: 0x04000045 RID: 69
		public static readonly string modID = "rpvoicechat";

		// Token: 0x04000046 RID: 70
		internal static ICoreAPI ModApi;

		// Token: 0x04000047 RID: 71
		internal static ICoreClientAPI capi;

		// Token: 0x04000048 RID: 72
		internal static ICoreServerAPI sapi;

		// Token: 0x04000049 RID: 73
		internal static IClientNetworkChannel ClientChannel;

		// Token: 0x0400004A RID: 74
		internal static IServerNetworkChannel ServerChannel;

		// Token: 0x0400004B RID: 75
		internal static IClientNetworkChannel TelegraphPrintClientChannel;

		// Token: 0x0400004C RID: 76
		internal static IServerNetworkChannel TelegraphPrintServerChannel;

		// Token: 0x0400004D RID: 77
		internal static IClientNetworkChannel TelegraphSettingsClientChannel;

		// Token: 0x0400004E RID: 78
		internal static IServerNetworkChannel TelegraphSettingsServerChannel;

		// Token: 0x0400004F RID: 79
		internal static IClientNetworkChannel TelephoneSettingsClientChannel;

		// Token: 0x04000050 RID: 80
		internal static IServerNetworkChannel TelephoneSettingsServerChannel;

		// Token: 0x04000051 RID: 81
		internal static IClientNetworkChannel SwitchboardClientChannel;

		// Token: 0x04000052 RID: 82
		internal static IServerNetworkChannel SwitchboardServerChannel;

		// Token: 0x04000053 RID: 83
		internal static IClientNetworkChannel AnnounceClientChannel;

		// Token: 0x04000054 RID: 84
		internal static IServerNetworkChannel AnnounceServerChannel;

		// Token: 0x04000055 RID: 85
		internal static IClientNetworkChannel RadioSettingsClientChannel;

		// Token: 0x04000056 RID: 86
		internal static IServerNetworkChannel RadioSettingsServerChannel;

		// Token: 0x04000057 RID: 87
		internal static IClientNetworkChannel RadioTalkieClientChannel;

		// Token: 0x04000058 RID: 88
		internal static IServerNetworkChannel RadioTalkieServerChannel;

		// Token: 0x04000059 RID: 89
		internal static IClientNetworkChannel NametagConfigClientChannel;

		// Token: 0x0400005A RID: 90
		internal static IServerNetworkChannel NametagConfigServerChannel;

		// Token: 0x0400005B RID: 91
		private PatchManager patchManager;
	}
}
