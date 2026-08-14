using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000067 RID: 103
	[NullableContext(1)]
	public interface IModApiService
	{
		// Token: 0x060009AA RID: 2474
		Task<List<DownloadableModOnList>> QueryModsAsync([Nullable(2)] string textFilter = null, [Nullable(2)] ModAuthor authorFilter = null, [Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<GameVersion> versionsFilter = null, [Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<ModTag> tagsFilter = null, string orderBy = "follows", string orderByOrder = "desc", CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x060009AB RID: 2475
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		Task<DownloadableMod> GetModAsync(int modId, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x060009AC RID: 2476
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		Task<DownloadableMod> GetModAsync(string modIdStr, CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x060009AD RID: 2477
		Task<List<ModAuthor>> GetAuthorsAsync(CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x060009AE RID: 2478
		Task<List<GameVersion>> GetGameVersionsAsync(CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x060009AF RID: 2479
		Task<List<ModTag>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));

		// Token: 0x060009B0 RID: 2480
		Task<bool> DownloadModAsync(string downloadUrl, string destinationPath, [Nullable(2)] IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken));
	}
}
