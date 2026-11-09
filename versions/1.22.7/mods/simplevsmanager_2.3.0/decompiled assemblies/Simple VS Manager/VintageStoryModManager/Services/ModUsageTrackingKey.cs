using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200007E RID: 126
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ModUsageTrackingKey : IEquatable<ModUsageTrackingKey>
	{
		// Token: 0x06000B41 RID: 2881 RVA: 0x00046364 File Offset: 0x00044564
		public ModUsageTrackingKey(string modId, string modVersion, string gameVersion)
		{
			this.ModId = ModUsageTrackingKey.NormalizeValue(modId);
			this.ModVersion = ModUsageTrackingKey.NormalizeValue(modVersion);
			this.GameVersion = ModUsageTrackingKey.NormalizeValue(gameVersion);
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0004638A File Offset: 0x0004458A
		public string ModId { get; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00046392 File Offset: 0x00044592
		public string ModVersion { get; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0004639A File Offset: 0x0004459A
		public string GameVersion { get; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x000463A2 File Offset: 0x000445A2
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.ModId) && !string.IsNullOrEmpty(this.ModVersion) && !string.IsNullOrEmpty(this.GameVersion);
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000463D0 File Offset: 0x000445D0
		public bool Equals(ModUsageTrackingKey other)
		{
			return string.Equals(this.ModId, other.ModId, 5) && string.Equals(this.ModVersion, other.ModVersion, 5) && string.Equals(this.GameVersion, other.GameVersion, 5);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0004641C File Offset: 0x0004461C
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is ModUsageTrackingKey)
			{
				ModUsageTrackingKey other = (ModUsageTrackingKey)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00046441 File Offset: 0x00044641
		public override int GetHashCode()
		{
			return HashCode.Combine<int, int, int>(StringComparer.OrdinalIgnoreCase.GetHashCode(this.ModId), StringComparer.OrdinalIgnoreCase.GetHashCode(this.ModVersion), StringComparer.OrdinalIgnoreCase.GetHashCode(this.GameVersion));
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00046478 File Offset: 0x00044678
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} | {1} | {2}", this.ModId, this.ModVersion, this.GameVersion);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0004649B File Offset: 0x0004469B
		private static string NormalizeValue(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Trim();
			}
			return string.Empty;
		}
	}
}
