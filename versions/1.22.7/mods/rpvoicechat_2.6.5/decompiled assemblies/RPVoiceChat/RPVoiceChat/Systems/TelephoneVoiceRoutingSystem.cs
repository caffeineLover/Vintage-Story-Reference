using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.Server;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200004B RID: 75
	public class TelephoneVoiceRoutingSystem : ModSystem, IVoiceRouteProvider, IVoiceMultiRouteProvider
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0000BD94 File Offset: 0x00009F94
		public void SetRoute(string playerUid, Vec3d emissionPos, int rangeBlocks)
		{
			this.SetRoutes(playerUid, new VoiceRoute[]
			{
				new VoiceRoute(emissionPos, rangeBlocks)
			});
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000BDB1 File Offset: 0x00009FB1
		public void SetRoute(string playerUid, Vec3d emissionPos, int rangeBlocks, int dimension)
		{
			this.SetRoutes(playerUid, new VoiceRoute[]
			{
				new VoiceRoute(emissionPos, rangeBlocks, dimension)
			});
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000BDD0 File Offset: 0x00009FD0
		public void SetRoutes(string playerUid, IEnumerable<VoiceRoute> voiceRoutes)
		{
			if (string.IsNullOrWhiteSpace(playerUid) || voiceRoutes == null)
			{
				return;
			}
			List<VoiceRoute> sanitized = Enumerable.ToList<VoiceRoute>(Enumerable.Where<VoiceRoute>(voiceRoutes, (VoiceRoute route) => route.EmissionPos != null && route.RangeBlocks > 0));
			if (sanitized.Count == 0)
			{
				return;
			}
			this.routes[playerUid] = sanitized;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000BE2C File Offset: 0x0000A02C
		public void ClearRoute(string playerUid)
		{
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				return;
			}
			IReadOnlyList<VoiceRoute> readOnlyList;
			this.routes.TryRemove(playerUid, ref readOnlyList);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000BE54 File Offset: 0x0000A054
		public bool TryGetRoute(string playerUid, out Vec3d emissionPos, out int rangeBlocks)
		{
			emissionPos = null;
			rangeBlocks = 0;
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				return false;
			}
			IReadOnlyList<VoiceRoute> voiceRoutes;
			if (!this.routes.TryGetValue(playerUid, ref voiceRoutes) || voiceRoutes == null || voiceRoutes.Count == 0)
			{
				return false;
			}
			VoiceRoute route = voiceRoutes[0];
			if (route.EmissionPos == null || route.RangeBlocks <= 0)
			{
				return false;
			}
			emissionPos = route.EmissionPos;
			rangeBlocks = route.RangeBlocks;
			return true;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000BEC4 File Offset: 0x0000A0C4
		public bool TryGetRoutes(string playerUid, out IReadOnlyList<VoiceRoute> voiceRoutes)
		{
			voiceRoutes = null;
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				return false;
			}
			IReadOnlyList<VoiceRoute> routesForPlayer;
			if (!this.routes.TryGetValue(playerUid, ref routesForPlayer) || routesForPlayer == null || routesForPlayer.Count == 0)
			{
				return false;
			}
			voiceRoutes = routesForPlayer;
			return true;
		}

		// Token: 0x040000BA RID: 186
		private readonly ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>> routes = new ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>>();
	}
}
