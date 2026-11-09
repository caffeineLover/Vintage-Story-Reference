using System;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.GameContent.BlockEntity;
using RPVoiceChat.Server;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000035 RID: 53
	public class RadioMicCaptureSystem : ModSystem
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00009298 File Offset: 0x00007498
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			this.routing = api.ModLoader.GetModSystem<RadioVoiceRoutingSystem>(true);
			api.Event.RegisterGameTickListener(new Action<float>(this.OnServerTick), 500, 0);
			api.Event.PlayerDisconnect += new PlayerDelegate(this.OnPlayerDisconnect);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000092F4 File Offset: 0x000074F4
		private void OnPlayerDisconnect(IServerPlayer player)
		{
			if (player == null || this.sapi == null)
			{
				return;
			}
			foreach (BlockEntityRadioMicrophone microphone in RadioBlockIndex.GetLoadedMicrophones(this.sapi.World))
			{
				if (microphone.ActiveOperatorPlayerUid == player.PlayerUID)
				{
					microphone.ClearTransmission();
				}
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000936C File Offset: 0x0000756C
		private void OnServerTick(float dt)
		{
			if (this.sapi == null || this.routing == null)
			{
				return;
			}
			Dictionary<string, List<VoiceRoute>> routesByPlayer = new Dictionary<string, List<VoiceRoute>>();
			foreach (BlockEntityRadioMicrophone microphone in Enumerable.ToList<BlockEntityRadioMicrophone>(RadioBlockIndex.GetLoadedMicrophones(this.sapi.World)))
			{
				if (microphone.NetworkUID != 0L && microphone.IsTransmitting && !string.IsNullOrWhiteSpace(microphone.ActiveOperatorPlayerUid) && !RadioWireNetworkHelper.HasOnAirMixingConsole(microphone))
				{
					IServerPlayer operatorPlayer = this.sapi.World.PlayerByUid(microphone.ActiveOperatorPlayerUid) as IServerPlayer;
					if (operatorPlayer == null)
					{
						microphone.ClearTransmission();
					}
					else
					{
						List<VoiceRoute> routes = RadioWiredRouteBuilder.BuildRoutesForWiredNode(this.sapi, microphone);
						if (routes.Count != 0)
						{
							routesByPlayer[operatorPlayer.PlayerUID] = routes;
						}
					}
				}
			}
			foreach (IServerPlayer player in this.sapi.World.AllOnlinePlayers)
			{
				if (player != null)
				{
					List<VoiceRoute> routes2;
					if (routesByPlayer.TryGetValue(player.PlayerUID, ref routes2) && routes2.Count > 0)
					{
						this.routing.SetMicRoutes(player.PlayerUID, routes2);
					}
					else
					{
						this.routing.ClearMicRoute(player.PlayerUID);
					}
				}
			}
		}

		// Token: 0x04000081 RID: 129
		private ICoreServerAPI sapi;

		// Token: 0x04000082 RID: 130
		private RadioVoiceRoutingSystem routing;
	}
}
