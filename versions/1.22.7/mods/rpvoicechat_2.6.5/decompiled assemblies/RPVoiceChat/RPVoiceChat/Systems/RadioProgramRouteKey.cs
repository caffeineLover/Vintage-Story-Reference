using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000039 RID: 57
	public static class RadioProgramRouteKey
	{
		// Token: 0x0600015D RID: 349 RVA: 0x0000A0C0 File Offset: 0x000082C0
		public static string ForMixingConsole(BlockPos pos)
		{
			if (pos == null)
			{
				return "";
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 5);
			defaultInterpolatedStringHandler.AppendFormatted("rpvc:program:");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.X);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Y);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.Z);
			defaultInterpolatedStringHandler.AppendLiteral(":");
			defaultInterpolatedStringHandler.AppendFormatted<int>(pos.dimension);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000A150 File Offset: 0x00008350
		public static bool IsProgramSource(string playerOrSourceId)
		{
			return !string.IsNullOrWhiteSpace(playerOrSourceId) && playerOrSourceId.StartsWith("rpvc:program:");
		}

		// Token: 0x0400008F RID: 143
		public const string Prefix = "rpvc:program:";
	}
}
