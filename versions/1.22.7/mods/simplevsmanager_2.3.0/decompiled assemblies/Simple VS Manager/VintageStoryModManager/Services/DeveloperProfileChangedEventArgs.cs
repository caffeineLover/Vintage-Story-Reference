using System;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000060 RID: 96
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DeveloperProfileChangedEventArgs : EventArgs
	{
		// Token: 0x06000958 RID: 2392 RVA: 0x00038DF7 File Offset: 0x00036FF7
		internal DeveloperProfileChangedEventArgs(DeveloperProfile profile, bool profilesUpdated)
		{
			this.Profile = profile;
			this.ProfilesUpdated = profilesUpdated;
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x00038E0D File Offset: 0x0003700D
		public DeveloperProfile Profile { get; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00038E15 File Offset: 0x00037015
		public bool ProfilesUpdated { get; }
	}
}
