using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B1 RID: 177
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class CloudModlistSlot
	{
		// Token: 0x06000F1C RID: 3868 RVA: 0x000531D8 File Offset: 0x000513D8
		public CloudModlistSlot([Nullable(1)] string slotKey, bool isOccupied, [Nullable(1)] string displayName, string name, string version, string cachedContent)
		{
			if (slotKey == null)
			{
				throw new ArgumentNullException("slotKey");
			}
			this.SlotKey = slotKey;
			this.IsOccupied = isOccupied;
			if (displayName == null)
			{
				throw new ArgumentNullException("displayName");
			}
			this.DisplayName = displayName;
			this.Name = name;
			this.Version = version;
			this.CachedContent = cachedContent;
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00053236 File Offset: 0x00051436
		[Nullable(1)]
		public string SlotKey { [NullableContext(1)] get; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0005323E File Offset: 0x0005143E
		public bool IsOccupied { get; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00053246 File Offset: 0x00051446
		[Nullable(1)]
		public string DisplayName { [NullableContext(1)] get; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0005324E File Offset: 0x0005144E
		public string Name { get; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00053256 File Offset: 0x00051456
		public string Version { get; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0005325E File Offset: 0x0005145E
		public string CachedContent { get; }

		// Token: 0x06000F23 RID: 3875 RVA: 0x00053266 File Offset: 0x00051466
		[NullableContext(1)]
		public override string ToString()
		{
			return this.DisplayName;
		}
	}
}
