using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B3 RID: 179
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CloudModlistManagementEntry
	{
		// Token: 0x06000F2B RID: 3883 RVA: 0x00053318 File Offset: 0x00051518
		public CloudModlistManagementEntry(string slotKey, string slotLabel, [Nullable(2)] string name, [Nullable(2)] string version, string displayName, [Nullable(2)] string cachedContent)
		{
			if (slotKey == null)
			{
				throw new ArgumentNullException("slotKey");
			}
			this.SlotKey = slotKey;
			if (slotLabel == null)
			{
				throw new ArgumentNullException("slotLabel");
			}
			this.SlotLabel = slotLabel;
			this.Name = (string.IsNullOrWhiteSpace(name) ? null : name.Trim());
			this.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
			if (displayName == null)
			{
				throw new ArgumentNullException("displayName");
			}
			this.DisplayName = displayName;
			this.CachedContent = cachedContent;
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x000533A6 File Offset: 0x000515A6
		public string SlotKey { get; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x000533AE File Offset: 0x000515AE
		public string SlotLabel { get; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x000533B6 File Offset: 0x000515B6
		[Nullable(2)]
		public string Name { [NullableContext(2)] get; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x000533BE File Offset: 0x000515BE
		[Nullable(2)]
		public string Version { [NullableContext(2)] get; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x000533C6 File Offset: 0x000515C6
		public string DisplayName { get; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x000533CE File Offset: 0x000515CE
		[Nullable(2)]
		public string CachedContent { [NullableContext(2)] get; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x000533D6 File Offset: 0x000515D6
		public string EffectiveName
		{
			get
			{
				return this.Name ?? "Unnamed Modlist";
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x000533E7 File Offset: 0x000515E7
		public override string ToString()
		{
			if (!string.IsNullOrWhiteSpace(this.DisplayName))
			{
				return this.DisplayName;
			}
			if (!string.IsNullOrWhiteSpace(this.Name))
			{
				return this.Name;
			}
			return this.SlotLabel;
		}
	}
}
