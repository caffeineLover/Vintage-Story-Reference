using System;
using System.Collections.Generic;
using RPVoiceChat.Config;
using RPVoiceChat.GameContent.Items;
using RPVoiceChat.Networking;
using RPVoiceChat.Server;
using RPVoiceChat.Util;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000041 RID: 65
	public class RadioRfReceptionSystem : ModSystem, IVoiceRecipientExpander
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0000AC21 File Offset: 0x00008E21
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public void ExpandRoutedRecipients(AudioPacket packet, IReadOnlyList<VoiceRoute> routes, Dictionary<string, RoutedVoiceRecipient> recipients)
		{
			ICoreServerAPI coreServerAPI = this.sapi;
			if (((coreServerAPI != null) ? coreServerAPI.World : null) == null || routes == null || routes.Count == 0 || recipients == null)
			{
				return;
			}
			int talkieRange = ServerConfigManager.RadioTalkieRangeBlocks;
			foreach (IServerPlayer player in this.sapi.World.AllOnlinePlayers)
			{
				bool flag;
				if (player == null)
				{
					flag = (null != null);
				}
				else
				{
					EntityPlayer entity2 = player.Entity;
					flag = (((entity2 != null) ? entity2.Pos : null) != null);
				}
				if (flag && !(player.PlayerUID == packet.PlayerId))
				{
					try
					{
						EntityAgent entity = player.Entity;
						if (((entity != null) ? entity.Pos : null) != null)
						{
							Vec3d listenerPos = entity.Pos.XYZ;
							int dimension = entity.Pos.Dimension;
							foreach (string tunedFrequency in ItemRadio.GetActiveListenFrequencies(player))
							{
								if (!string.IsNullOrEmpty(tunedFrequency))
								{
									for (int routeIndex = 0; routeIndex < routes.Count; routeIndex++)
									{
										VoiceRoute route = routes[routeIndex];
										if (!(route.EmissionPos == null) && route.RangeBlocks > 0 && route.Dimension == dimension && !string.IsNullOrEmpty(route.RadioFrequency) && RadioFrequencyUtil.Matches(route.RadioFrequency, tunedFrequency))
										{
											double distanceSq = RadioRfReceptionSystem.SquareDistance(listenerPos, route.EmissionPos);
											double rfRangeSq = (double)route.RangeBlocks * (double)route.RangeBlocks;
											if (distanceSq <= rfRangeSq)
											{
												VoiceRoute acousticAtPlayer = new VoiceRoute(listenerPos, talkieRange, dimension, route.RadioFrequency, true);
												RadioRfReceptionSystem.TrySetRecipient(player.PlayerUID, acousticAtPlayer, distanceSq, recipients);
											}
										}
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Logger server = Logger.server;
						if (server != null)
						{
							server.Warning("[RadioRfReception] Skipped talkie routing for " + player.PlayerUID + ": " + ex.Message);
						}
					}
				}
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000AE5C File Offset: 0x0000905C
		private static void TrySetRecipient(string playerUid, VoiceRoute route, double distanceSq, Dictionary<string, RoutedVoiceRecipient> recipients)
		{
			RoutedVoiceRecipient existing;
			if (recipients.TryGetValue(playerUid, ref existing) && existing.DistanceSq <= distanceSq)
			{
				return;
			}
			recipients[playerUid] = new RoutedVoiceRecipient(playerUid, route, distanceSq);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000AE90 File Offset: 0x00009090
		private static double SquareDistance(Vec3d listener, Vec3d source)
		{
			double num = listener.X - source.X;
			double dy = listener.Y - source.Y;
			double dz = listener.Z - source.Z;
			return num * num + dy * dy + dz * dz;
		}

		// Token: 0x040000AE RID: 174
		private ICoreServerAPI sapi;
	}
}
