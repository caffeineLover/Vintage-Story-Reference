using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RPVoiceChat.Server;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000047 RID: 71
	public class RadioVoiceRoutingSystem : ModSystem, IVoiceRouteProvider, IVoiceMultiRouteProvider
	{
		// Token: 0x0600019A RID: 410 RVA: 0x0000B7BF File Offset: 0x000099BF
		public void SetMicRoutes(string playerUid, IEnumerable<VoiceRoute> voiceRoutes)
		{
			RadioVoiceRoutingSystem.SetLayerRoutes(this.micRoutes, playerUid, voiceRoutes);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B7CE File Offset: 0x000099CE
		public void SetProgramRoutes(string playerUid, IEnumerable<VoiceRoute> voiceRoutes)
		{
			RadioVoiceRoutingSystem.SetLayerRoutes(this.programRoutes, playerUid, voiceRoutes);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B7DD File Offset: 0x000099DD
		public void SetTalkieRoutes(string playerUid, IEnumerable<VoiceRoute> voiceRoutes)
		{
			RadioVoiceRoutingSystem.SetLayerRoutes(this.talkieRoutes, playerUid, voiceRoutes);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B7EC File Offset: 0x000099EC
		public void ClearMicRoute(string playerUid)
		{
			RadioVoiceRoutingSystem.ClearLayerRoute(this.micRoutes, playerUid);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000B7FA File Offset: 0x000099FA
		public void ClearProgramRoute(string playerUid)
		{
			RadioVoiceRoutingSystem.ClearLayerRoute(this.programRoutes, playerUid);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000B808 File Offset: 0x00009A08
		public void ClearTalkieRoute(string playerUid)
		{
			RadioVoiceRoutingSystem.ClearLayerRoute(this.talkieRoutes, playerUid);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000B816 File Offset: 0x00009A16
		public void ClearRoute(string playerUid)
		{
			this.ClearMicRoute(playerUid);
			this.ClearProgramRoute(playerUid);
			this.ClearTalkieRoute(playerUid);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B830 File Offset: 0x00009A30
		public bool TryGetRoute(string playerUid, out Vec3d emissionPos, out int rangeBlocks)
		{
			emissionPos = null;
			rangeBlocks = 0;
			IReadOnlyList<VoiceRoute> voiceRoutes;
			if (!this.TryGetRoutes(playerUid, out voiceRoutes) || voiceRoutes == null || voiceRoutes.Count == 0)
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

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B894 File Offset: 0x00009A94
		public bool TryGetRoutes(string playerUid, out IReadOnlyList<VoiceRoute> voiceRoutes)
		{
			voiceRoutes = null;
			return !string.IsNullOrWhiteSpace(playerUid) && ((this.talkieRoutes.TryGetValue(playerUid, ref voiceRoutes) && voiceRoutes != null && voiceRoutes.Count > 0) || (this.programRoutes.TryGetValue(playerUid, ref voiceRoutes) && voiceRoutes != null && voiceRoutes.Count > 0) || (this.micRoutes.TryGetValue(playerUid, ref voiceRoutes) && voiceRoutes != null && voiceRoutes.Count > 0));
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B90C File Offset: 0x00009B0C
		private static void SetLayerRoutes(ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>> layer, string playerUid, IEnumerable<VoiceRoute> voiceRoutes)
		{
			if (string.IsNullOrWhiteSpace(playerUid) || voiceRoutes == null)
			{
				return;
			}
			List<VoiceRoute> sanitized = Enumerable.ToList<VoiceRoute>(Enumerable.Where<VoiceRoute>(voiceRoutes, (VoiceRoute route) => route.EmissionPos != null && route.RangeBlocks > 0));
			if (sanitized.Count == 0)
			{
				IReadOnlyList<VoiceRoute> readOnlyList;
				layer.TryRemove(playerUid, ref readOnlyList);
				return;
			}
			layer[playerUid] = sanitized;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000B96C File Offset: 0x00009B6C
		private static void ClearLayerRoute(ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>> layer, string playerUid)
		{
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				return;
			}
			IReadOnlyList<VoiceRoute> readOnlyList;
			layer.TryRemove(playerUid, ref readOnlyList);
		}

		// Token: 0x040000B7 RID: 183
		private readonly ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>> micRoutes = new ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>>();

		// Token: 0x040000B8 RID: 184
		private readonly ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>> programRoutes = new ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>>();

		// Token: 0x040000B9 RID: 185
		private readonly ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>> talkieRoutes = new ConcurrentDictionary<string, IReadOnlyList<VoiceRoute>>();
	}
}
