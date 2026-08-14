using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000035 RID: 53
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NegativeVoteReason
	{
		// Token: 0x060004EE RID: 1262 RVA: 0x000234FB File Offset: 0x000216FB
		public NegativeVoteReason(string text, int count)
		{
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00023511 File Offset: 0x00021711
		public string Text { get; } = text;

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00023519 File Offset: 0x00021719
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x00023521 File Offset: 0x00021721
		public int Count { get; set; } = count;

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0002352A File Offset: 0x0002172A
		public string CountDisplay
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, "×{0}", this.Count);
			}
		}
	}
}
