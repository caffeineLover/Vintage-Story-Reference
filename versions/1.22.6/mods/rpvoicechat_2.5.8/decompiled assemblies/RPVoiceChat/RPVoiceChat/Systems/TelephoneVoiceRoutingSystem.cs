using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.Server;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000031 RID: 49
	public class TelephoneVoiceRoutingSystem : ModSystem, IVoiceRouteProvider, IVoiceMultiRouteProvider
	{
		// Token: 0x06000123 RID: 291 RVA: 0x000085CA File Offset: 0x000067CA
		public void SetRoute(string playerUid, Vec3d emissionPos, int rangeBlocks)
		{
			this.SetRoutes(playerUid, new VoiceRoute[]
			{
				new VoiceRoute(emissionPos, rangeBlocks)
			});
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000085E7 File Offset: 0x000067E7
		public void SetRoute(string playerUid, Vec3d emissionPos, int rangeBlocks, int dimension)
		{
			this.SetRoutes(playerUid, new VoiceRoute[]
			{
				new VoiceRoute(emissionPos, rangeBlocks, dimension)
			});
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008608 File Offset: 0x00006808
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

		// Token: 0x06000126 RID: 294 RVA: 0x00008664 File Offset: 0x00006864
		public void ClearRoute(string playerUid)
		{
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				return;
			}
			IReadOnlyList<VoiceRoute> readOnlyList;
			this.routes.TryRemove(playerUid, ref readOnlyList);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000868C File Offset: 0x0000688C
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

		// Token: 0x06000128 RID: 296 RVA: 0x000086FC File Offset: 0x000068FC
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

		// Token: 0x0400006F RID: 111
		private readonly ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>> routes = new ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>>();
	}
}
