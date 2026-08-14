using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000089 RID: 137
	[NullableContext(1)]
	public interface ISettingsService
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000B8C RID: 2956
		// (set) Token: 0x06000B8D RID: 2957
		List<int> FavoriteMods { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000B8E RID: 2958
		// (set) Token: 0x06000B8F RID: 2959
		[Nullable(2)]
		string LastInstallationPath { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000B90 RID: 2960
		// (set) Token: 0x06000B91 RID: 2961
		string OrderBy { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000B92 RID: 2962
		// (set) Token: 0x06000B93 RID: 2963
		string OrderByDirection { get; set; }

		// Token: 0x06000B94 RID: 2964
		Task SaveAsync();

		// Token: 0x06000B95 RID: 2965
		Task LoadAsync();
	}
}
