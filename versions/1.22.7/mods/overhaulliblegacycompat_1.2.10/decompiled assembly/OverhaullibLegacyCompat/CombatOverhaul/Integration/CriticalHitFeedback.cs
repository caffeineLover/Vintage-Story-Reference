using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000037 RID: 55
	[NullableContext(2)]
	[Nullable(0)]
	internal static class CriticalHitFeedback
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00011394 File Offset: 0x0000F594
		[NullableContext(1)]
		public static void Register(ICoreAPI api)
		{
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi != null && CriticalHitFeedback._clientChannel == null)
			{
				CriticalHitFeedback._clientApi = clientApi;
				CriticalHitFeedback._renderer = new CriticalHitFeedbackRenderer(clientApi);
				clientApi.Event.RegisterRenderer(CriticalHitFeedback._renderer, 10, "critical-hit-feedback");
				IClientNetworkChannel clientNetworkChannel = clientApi.Network.RegisterChannel("CombatOverhaul:critical-hit-feedback").RegisterMessageType<CriticalHitFeedbackPacket>();
				NetworkServerMessageHandler<CriticalHitFeedbackPacket> messageHandler;
				if ((messageHandler = CriticalHitFeedback.<>O.<0>__HandlePacket) == null)
				{
					messageHandler = (CriticalHitFeedback.<>O.<0>__HandlePacket = new NetworkServerMessageHandler<CriticalHitFeedbackPacket>(CriticalHitFeedback.HandlePacket));
				}
				CriticalHitFeedback._clientChannel = clientNetworkChannel.SetMessageHandler<CriticalHitFeedbackPacket>(messageHandler);
			}
			ICoreServerAPI serverApi = api as ICoreServerAPI;
			if (serverApi != null && CriticalHitFeedback._serverChannel == null)
			{
				CriticalHitFeedback._serverChannel = serverApi.Network.RegisterChannel("CombatOverhaul:critical-hit-feedback").RegisterMessageType<CriticalHitFeedbackPacket>();
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00011440 File Offset: 0x0000F640
		public static void Trigger(DamageSource damageSource)
		{
			EntityPlayer entityPlayer = (((damageSource != null) ? damageSource.CauseEntity : null) ?? ((damageSource != null) ? damageSource.SourceEntity : null)) as EntityPlayer;
			if (entityPlayer != null)
			{
				IServerPlayer player = entityPlayer.Player as IServerPlayer;
				if (player != null)
				{
					IServerNetworkChannel serverChannel = CriticalHitFeedback._serverChannel;
					if (serverChannel == null)
					{
						return;
					}
					serverChannel.SendPacket<CriticalHitFeedbackPacket>(new CriticalHitFeedbackPacket(), new IServerPlayer[]
					{
						player
					});
					return;
				}
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000114A0 File Offset: 0x0000F6A0
		public static void Dispose()
		{
			ICoreClientAPI clientApi = CriticalHitFeedback._clientApi;
			CriticalHitFeedbackRenderer renderer = CriticalHitFeedback._renderer;
			CriticalHitFeedback._renderer = null;
			CriticalHitFeedback._clientApi = null;
			CriticalHitFeedback._clientChannel = null;
			CriticalHitFeedback._serverChannel = null;
			ClientThreadCleanup.DisposeRenderer(clientApi, renderer, 10, "critical-hit-feedback-dispose");
		}

		// Token: 0x060003BB RID: 955 RVA: 0x000114DD File Offset: 0x0000F6DD
		[NullableContext(1)]
		private static void HandlePacket(CriticalHitFeedbackPacket packet)
		{
			CriticalHitFeedbackRenderer renderer = CriticalHitFeedback._renderer;
			if (renderer == null)
			{
				return;
			}
			renderer.Pulse(Math.Clamp(packet.Intensity, 0.5f, 2f));
		}

		// Token: 0x04000150 RID: 336
		[Nullable(1)]
		private const string NetworkChannelId = "CombatOverhaul:critical-hit-feedback";

		// Token: 0x04000151 RID: 337
		private static ICoreClientAPI _clientApi;

		// Token: 0x04000152 RID: 338
		private static IClientNetworkChannel _clientChannel;

		// Token: 0x04000153 RID: 339
		private static IServerNetworkChannel _serverChannel;

		// Token: 0x04000154 RID: 340
		private static CriticalHitFeedbackRenderer _renderer;

		// Token: 0x020001D6 RID: 470
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000AEA RID: 2794
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static NetworkServerMessageHandler<CriticalHitFeedbackPacket> <0>__HandlePacket;
		}
	}
}
