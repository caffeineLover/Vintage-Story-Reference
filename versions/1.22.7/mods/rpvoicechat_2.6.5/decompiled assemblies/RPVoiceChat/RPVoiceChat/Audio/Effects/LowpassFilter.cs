using System;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x0200010E RID: 270
	public class LowpassFilter
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x00037174 File Offset: 0x00035374
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x0003717C File Offset: 0x0003537C
		public bool IsEnabled { get; set; }

		// Token: 0x060009F5 RID: 2549 RVA: 0x00037185 File Offset: 0x00035385
		public LowpassFilter(int source)
		{
			this.source = source;
			this.nullFilter = ALC.EFX.GenFilter();
			this.GenerateFilter();
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000371A5 File Offset: 0x000353A5
		public void SetHFGain(float gain)
		{
			ALC.EFX.Filter(this.filter, 2, gain);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x000371B4 File Offset: 0x000353B4
		public void SetGain(float gain)
		{
			ALC.EFX.Filter(this.filter, 1, gain);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x000371C3 File Offset: 0x000353C3
		public void Start()
		{
			if (this.IsEnabled)
			{
				return;
			}
			AL.Source(this.source, 131077, this.filter);
			this.IsEnabled = true;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x000371EB File Offset: 0x000353EB
		public void Stop()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			AL.Source(this.source, 131077, this.nullFilter);
			this.IsEnabled = false;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00037213 File Offset: 0x00035413
		private void GenerateFilter()
		{
			this.filter = ALC.EFX.GenFilter();
			ALC.EFX.Filter(this.filter, 32769, 1);
			ALC.EFX.Filter(this.filter, 1, 1f);
			ALC.EFX.Filter(this.filter, 2, 0.2f);
		}

		// Token: 0x04000482 RID: 1154
		private int source;

		// Token: 0x04000483 RID: 1155
		private int nullFilter;

		// Token: 0x04000484 RID: 1156
		public int filter;
	}
}
