using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200005F RID: 95
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeveloperProfile
	{
		// Token: 0x06000950 RID: 2384 RVA: 0x00038D82 File Offset: 0x00036F82
		internal DeveloperProfile(string id, string displayName, string dataDirectory, [Nullable(2)] string firebaseStateFilePath, [Nullable(2)] string playerName, [Nullable(2)] string playerUid, bool isOriginal)
		{
			this.Id = id;
			this.DisplayName = displayName;
			this.DataDirectory = dataDirectory;
			this.FirebaseStateFilePath = firebaseStateFilePath;
			this.PlayerName = playerName;
			this.PlayerUid = playerUid;
			this.IsOriginal = isOriginal;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00038DBF File Offset: 0x00036FBF
		public string Id { get; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00038DC7 File Offset: 0x00036FC7
		public string DisplayName { get; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00038DCF File Offset: 0x00036FCF
		public string DataDirectory { get; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00038DD7 File Offset: 0x00036FD7
		[Nullable(2)]
		public string FirebaseStateFilePath { [NullableContext(2)] get; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x00038DDF File Offset: 0x00036FDF
		[Nullable(2)]
		public string PlayerName { [NullableContext(2)] get; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x00038DE7 File Offset: 0x00036FE7
		[Nullable(2)]
		public string PlayerUid { [NullableContext(2)] get; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x00038DEF File Offset: 0x00036FEF
		public bool IsOriginal { get; }
	}
}
