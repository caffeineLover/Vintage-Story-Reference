using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul
{
	// Token: 0x02000015 RID: 21
	[NullableContext(1)]
	[Nullable(0)]
	internal class LogStuff : ModSystem
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00004730 File Offset: 0x00002930
		public override void Start(ICoreAPI api)
		{
			string mods = Enumerable.Aggregate<string>(Enumerable.Select<Mod, string>(api.ModLoader.Mods, delegate(Mod mod)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(14, 5);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				defaultInterpolatedStringHandler.AppendFormatted(mod.Info.ModID);
				defaultInterpolatedStringHandler.AppendLiteral("'\t'");
				defaultInterpolatedStringHandler.AppendFormatted(mod.Info.Version);
				defaultInterpolatedStringHandler.AppendLiteral("'\t'");
				defaultInterpolatedStringHandler.AppendFormatted(mod.Info.Name);
				defaultInterpolatedStringHandler.AppendLiteral("'\t'");
				defaultInterpolatedStringHandler.AppendFormatted(LogStuff.Aggregate(mod.Info.Authors, mod));
				defaultInterpolatedStringHandler.AppendLiteral("'\t'");
				defaultInterpolatedStringHandler.AppendFormatted(mod.FileName);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}), (string f, string s) => f + "\n" + s);
			api.Logger.Event("Loaded mods:\n" + mods);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000047A8 File Offset: 0x000029A8
		private static string Aggregate(IEnumerable<string> list, Mod mod)
		{
			if (!Enumerable.Any<string>(list))
			{
				ILogger logger = mod.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(47, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Mod '");
				defaultInterpolatedStringHandler.AppendFormatted(mod.Info.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(mod.FileName);
				defaultInterpolatedStringHandler.AppendLiteral(")' has no authors specified in mod info.");
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return "-";
			}
			return Enumerable.Aggregate<string>(list, (string f, string s) => f + ", " + s);
		}
	}
}
