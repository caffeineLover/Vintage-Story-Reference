using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x020000B2 RID: 178
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CloudModlistRegistryEntry
	{
		// Token: 0x06000F24 RID: 3876 RVA: 0x00053270 File Offset: 0x00051470
		public CloudModlistRegistryEntry(string ownerId, string slotKey, string contentJson, DateTimeOffset? dateAdded, bool isContentComplete = true)
		{
			if (ownerId == null)
			{
				throw new ArgumentNullException("ownerId");
			}
			this.OwnerId = ownerId;
			if (slotKey == null)
			{
				throw new ArgumentNullException("slotKey");
			}
			this.SlotKey = slotKey;
			if (contentJson == null)
			{
				throw new ArgumentNullException("contentJson");
			}
			this.ContentJson = contentJson;
			this.DateAdded = dateAdded;
			this.IsContentComplete = isContentComplete;
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x000532D5 File Offset: 0x000514D5
		public string OwnerId { get; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x000532DD File Offset: 0x000514DD
		public string SlotKey { get; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x000532E5 File Offset: 0x000514E5
		public string ContentJson { get; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x000532ED File Offset: 0x000514ED
		public DateTimeOffset? DateAdded { get; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x000532F5 File Offset: 0x000514F5
		public string RegistryKey
		{
			get
			{
				return this.OwnerId + "/" + this.SlotKey;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0005330D File Offset: 0x0005150D
		public bool IsContentComplete { get; }
	}
}
