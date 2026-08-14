using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200008F RID: 143
	[NullableContext(2)]
	[Nullable(0)]
	public static class TargetGameVersionPreference
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x000488B8 File Offset: 0x00046AB8
		public static bool IsLatest(string value)
		{
			return string.Equals(value, "__target_latest__", 4);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000488C6 File Offset: 0x00046AC6
		public static bool HasConcreteOverride(string value)
		{
			return !string.IsNullOrWhiteSpace(value) && !TargetGameVersionPreference.IsLatest(value);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000488DB File Offset: 0x00046ADB
		public static string ResolveEffectiveGameVersion(string detectedVersion, string overrideVersion)
		{
			if (TargetGameVersionPreference.IsLatest(overrideVersion))
			{
				return null;
			}
			if (!string.IsNullOrWhiteSpace(overrideVersion))
			{
				return overrideVersion;
			}
			return detectedVersion;
		}

		// Token: 0x04000563 RID: 1379
		[Nullable(1)]
		public const string Latest = "__target_latest__";
	}
}
