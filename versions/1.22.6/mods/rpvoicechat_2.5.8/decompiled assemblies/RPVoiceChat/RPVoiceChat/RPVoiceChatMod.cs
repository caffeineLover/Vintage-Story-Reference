using System;
using System.Runtime.CompilerServices;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Block;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.GameContent.Items;
using RPVoiceChat.Gui;
using RPVoiceChat.Networking.Packets;
using RPVoiceChat.src.Networking.Packets;
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
		// Token: 0x060000B4 RID: 180 RVA: 0x000057FA File Offset: 0x000039FA
		public override void StartPre(ICoreAPI api)
		{
			ModConfig.Init(api);
			WorldConfig.Init(api);
			new Logger(api);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00005810 File Offset: 0x00003A10
		public override void Start(ICoreAPI api)
		{
			RPVoiceChatMod.ModApi = api;
			if (api.Side == 2)
			{
				RPVoiceChatMod.capi = (api as ICoreClientAPI);
				RPVoiceChatMod.ClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("welding").RegisterMessageType<WeldingHitPacket>();
				RPVoiceChatMod.TelegraphPrintClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("telegraphprint").RegisterMessageType<CommDeliveryPacket>();
				RPVoiceChatMod.TelegraphSettingsClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("telegraphsettings").RegisterMessageType<TelegraphSettingsPacket>();
				RPVoiceChatMod.TelephoneSettingsClientChannel = RPVoiceChatMod.capi.Network.RegisterChannel("telephonesettings").RegisterMessageType<TelephoneSettingsPacket>();
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

		// Token: 0x060000B6 RID: 182 RVA: 0x00005AAF File Offset: 0x00003CAF
		public override void AssetsFinalize(ICoreAPI api)
		{
			base.AssetsFinalize(api);
			if (api.Side == 1)
			{
				BlockBehaviorRegistry.AddBehaviors(api);
				BlockEntityBehaviorRegistry.AddBlockEntityBehaviors(api);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005ACD File Offset: 0x00003CCD
		public override void Dispose()
		{
			PatchManager patchManager = this.patchManager;
			if (patchManager == null)
			{
				return;
			}
			patchManager.Dispose();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005AE0 File Offset: 0x00003CE0
		private void OnWeldingHitReceived(IPlayer fromPlayer, WeldingHitPacket packet)
		{
			BEWeldable be = fromPlayer.Entity.World.BlockAccessor.GetBlockEntity(packet.Pos) as BEWeldable;
			if (be != null)
			{
				be.OnHammerHitOver(fromPlayer, packet.HitPosition);
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005B20 File Offset: 0x00003D20
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

		// Token: 0x060000BA RID: 186 RVA: 0x00005BD0 File Offset: 0x00003DD0
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

		// Token: 0x060000BB RID: 187 RVA: 0x00005C74 File Offset: 0x00003E74
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

		// Token: 0x060000BC RID: 188 RVA: 0x00005CFC File Offset: 0x00003EFC
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

		// Token: 0x060000BD RID: 189 RVA: 0x00005DC0 File Offset: 0x00003FC0
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

		// Token: 0x04000044 RID: 68
		public static readonly string modID = "rpvoicechat";

		// Token: 0x04000045 RID: 69
		internal static ICoreAPI ModApi;

		// Token: 0x04000046 RID: 70
		internal static ICoreClientAPI capi;

		// Token: 0x04000047 RID: 71
		internal static ICoreServerAPI sapi;

		// Token: 0x04000048 RID: 72
		internal static IClientNetworkChannel ClientChannel;

		// Token: 0x04000049 RID: 73
		internal static IServerNetworkChannel ServerChannel;

		// Token: 0x0400004A RID: 74
		internal static IClientNetworkChannel TelegraphPrintClientChannel;

		// Token: 0x0400004B RID: 75
		internal static IServerNetworkChannel TelegraphPrintServerChannel;

		// Token: 0x0400004C RID: 76
		internal static IClientNetworkChannel TelegraphSettingsClientChannel;

		// Token: 0x0400004D RID: 77
		internal static IServerNetworkChannel TelegraphSettingsServerChannel;

		// Token: 0x0400004E RID: 78
		internal static IClientNetworkChannel TelephoneSettingsClientChannel;

		// Token: 0x0400004F RID: 79
		internal static IServerNetworkChannel TelephoneSettingsServerChannel;

		// Token: 0x04000050 RID: 80
		internal static IClientNetworkChannel SwitchboardClientChannel;

		// Token: 0x04000051 RID: 81
		internal static IServerNetworkChannel SwitchboardServerChannel;

		// Token: 0x04000052 RID: 82
		internal static IClientNetworkChannel AnnounceClientChannel;

		// Token: 0x04000053 RID: 83
		internal static IServerNetworkChannel AnnounceServerChannel;

		// Token: 0x04000054 RID: 84
		internal static IClientNetworkChannel NametagConfigClientChannel;

		// Token: 0x04000055 RID: 85
		internal static IServerNetworkChannel NametagConfigServerChannel;

		// Token: 0x04000056 RID: 86
		private PatchManager patchManager;
	}
}
