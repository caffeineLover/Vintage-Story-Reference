using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000090 RID: 144
	[NullableContext(2)]
	[Nullable(0)]
	public static class TargetGameVersionReleaseSelector
	{
		// Token: 0x06000BCC RID: 3020 RVA: 0x000488F4 File Offset: 0x00046AF4
		public static ModReleaseInfo SelectLatestCompatibleRelease(IEnumerable<ModReleaseInfo> releases, string targetGameVersion, bool requireExactVersionMatch)
		{
			if (releases == null)
			{
				return null;
			}
			string text = VersionStringUtility.Normalize(targetGameVersion);
			foreach (ModReleaseInfo modReleaseInfo in releases)
			{
				if (modReleaseInfo != null)
				{
					if (string.IsNullOrWhiteSpace(text))
					{
						return modReleaseInfo;
					}
					if (TargetGameVersionReleaseSelector.ReleaseSupportsTarget(modReleaseInfo, text, requireExactVersionMatch))
					{
						return modReleaseInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00048964 File Offset: 0x00046B64
		[NullableContext(1)]
		public static bool ReleaseSupportsTarget(ModReleaseInfo release, [Nullable(2)] string targetGameVersion, bool requireExactVersionMatch)
		{
			ArgumentNullException.ThrowIfNull(release, "release");
			string text = VersionStringUtility.Normalize(targetGameVersion);
			return string.IsNullOrWhiteSpace(text) || TargetGameVersionReleaseSelector.TagsSupportTarget(release.GameVersionTags, text, requireExactVersionMatch);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0004899C File Offset: 0x00046B9C
		public static bool TagsSupportTarget([Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<string> gameVersionTags, string targetGameVersion, bool requireExactVersionMatch)
		{
			string text = VersionStringUtility.Normalize(targetGameVersion);
			if (string.IsNullOrWhiteSpace(text))
			{
				return true;
			}
			if (gameVersionTags == null || gameVersionTags.Count <= 0)
			{
				return false;
			}
			using (IEnumerator<string> enumerator = gameVersionTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (VersionStringUtility.SupportsVersion(enumerator.Current, text, requireExactVersionMatch))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
