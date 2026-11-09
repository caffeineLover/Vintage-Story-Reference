using System;
using System.Collections.Generic;

namespace NVorbis.Contracts
{
	// Token: 0x02000032 RID: 50
	public interface ITagData
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001F9 RID: 505
		IReadOnlyDictionary<string, IReadOnlyList<string>> All { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001FA RID: 506
		string EncoderVendor { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001FB RID: 507
		string Title { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001FC RID: 508
		string Version { get; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001FD RID: 509
		string Album { get; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001FE RID: 510
		string TrackNumber { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001FF RID: 511
		string Artist { get; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000200 RID: 512
		IReadOnlyList<string> Performers { get; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000201 RID: 513
		string Copyright { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000202 RID: 514
		string License { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000203 RID: 515
		string Organization { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000204 RID: 516
		string Description { get; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000205 RID: 517
		IReadOnlyList<string> Genres { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000206 RID: 518
		IReadOnlyList<string> Dates { get; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000207 RID: 519
		IReadOnlyList<string> Locations { get; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000208 RID: 520
		string Contact { get; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000209 RID: 521
		string Isrc { get; }

		// Token: 0x0600020A RID: 522
		string GetTagSingle(string key, bool concatenate = false);

		// Token: 0x0600020B RID: 523
		IReadOnlyList<string> GetTagMulti(string key);
	}
}
