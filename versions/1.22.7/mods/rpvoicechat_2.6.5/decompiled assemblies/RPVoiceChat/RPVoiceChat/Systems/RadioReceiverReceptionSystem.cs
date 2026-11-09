using System;
using System.Collections.Generic;
using RPVoiceChat.Networking;
using RPVoiceChat.Server;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003A RID: 58
	public class RadioReceiverReceptionSystem : ModSystem, IVoiceRecipientExpander
	{
		// Token: 0x0600015F RID: 351 RVA: 0x0000A167 File Offset: 0x00008367
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A170 File Offset: 0x00008370
		public void ExpandRoutedRecipients(AudioPacket packet, IReadOnlyList<VoiceRoute> routes, Dictionary<string, RoutedVoiceRecipient> recipients)
		{
			if (this.sapi == null || routes == null || routes.Count == 0)
			{
				return;
			}
			foreach (RadioRfReceiverPresence receiver in RadioRfPresenceRegistry.GetReceivers())
			{
				string tunedFrequency = RadioFrequencyUtil.Normalize(receiver.TunedFrequency);
				if (receiver.IsEnabled && tunedFrequency.Length != 0 && RadioReceiverReceptionSystem.RoutesContainFrequency(routes, tunedFrequency))
				{
					if (receiver.AcousticPoints != null && receiver.AcousticPoints.Count > 0)
					{
						using (List<RadioRfAcousticPresence>.Enumerator enumerator2 = receiver.AcousticPoints.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								RadioRfAcousticPresence acoustic = enumerator2.Current;
								if (!(((acoustic != null) ? acoustic.Pos : null) == null) && acoustic.RangeBlocks > 0)
								{
									this.EmitAroundPoint(acoustic.Pos.ToVec3d().Add(0.5, 0.5, 0.5), acoustic.RangeBlocks, acoustic.Dimension, tunedFrequency, recipients);
								}
							}
							continue;
						}
					}
					if (!(receiver.Pos == null) && receiver.PlaybackRangeBlocks > 0)
					{
						this.EmitAroundPoint(receiver.Pos.ToVec3d().Add(0.5, 0.5, 0.5), receiver.PlaybackRangeBlocks, receiver.Dimension, tunedFrequency, recipients);
					}
				}
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000A328 File Offset: 0x00008528
		private void EmitAroundPoint(Vec3d emissionPos, int rangeBlocks, int dimension, string tunedFrequency, Dictionary<string, RoutedVoiceRecipient> recipients)
		{
			if (rangeBlocks <= 0)
			{
				return;
			}
			double rangeSq = (double)rangeBlocks * (double)rangeBlocks;
			VoiceRoute listenRoute = new VoiceRoute(emissionPos, rangeBlocks, dimension, tunedFrequency, true);
			foreach (IServerPlayer player in this.sapi.World.AllOnlinePlayers)
			{
				bool flag;
				if (player == null)
				{
					flag = (null != null);
				}
				else
				{
					EntityPlayer entity = player.Entity;
					flag = (((entity != null) ? entity.Pos : null) != null);
				}
				if (flag && player.Entity.Pos.Dimension == dimension)
				{
					double distanceSq = RadioReceiverReceptionSystem.SquareDistance(player.Entity.Pos.XYZ, emissionPos);
					if (distanceSq <= rangeSq)
					{
						RadioReceiverReceptionSystem.TrySetRecipient(player.PlayerUID, listenRoute, distanceSq, recipients);
					}
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000A3D8 File Offset: 0x000085D8
		private static bool RoutesContainFrequency(IReadOnlyList<VoiceRoute> routes, string tunedFrequency)
		{
			for (int routeIndex = 0; routeIndex < routes.Count; routeIndex++)
			{
				VoiceRoute route = routes[routeIndex];
				if (!string.IsNullOrEmpty(route.RadioFrequency) && RadioFrequencyUtil.Matches(route.RadioFrequency, tunedFrequency))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000A420 File Offset: 0x00008620
		private static void TrySetRecipient(string playerUid, VoiceRoute route, double distanceSq, Dictionary<string, RoutedVoiceRecipient> recipients)
		{
			RoutedVoiceRecipient existing;
			if (recipients.TryGetValue(playerUid, ref existing) && existing.DistanceSq <= distanceSq && !RadioReceiverReceptionSystem.ShouldPreferReceiverRelay(existing.DistanceSq, route, distanceSq))
			{
				return;
			}
			recipients[playerUid] = new RoutedVoiceRecipient(playerUid, route, distanceSq);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000A460 File Offset: 0x00008660
		private static bool ShouldPreferReceiverRelay(double existingDistanceSq, VoiceRoute newRoute, double newDistanceSq)
		{
			return existingDistanceSq <= 4.0 && !string.IsNullOrEmpty(newRoute.RadioFrequency) && newDistanceSq > existingDistanceSq;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000A484 File Offset: 0x00008684
		private static double SquareDistance(Vec3d listener, Vec3d source)
		{
			double num = listener.X - source.X;
			double dy = listener.Y - source.Y;
			double dz = listener.Z - source.Z;
			return num * num + dy * dy + dz * dz;
		}

		// Token: 0x04000090 RID: 144
		private ICoreServerAPI sapi;
	}
}
