using System;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000D3 RID: 211
	public class LowpassFilter
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0002A9A4 File Offset: 0x00028BA4
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0002A9AC File Offset: 0x00028BAC
		public bool IsEnabled { get; set; }

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002A9B5 File Offset: 0x00028BB5
		public LowpassFilter(int source)
		{
			this.source = source;
			this.nullFilter = ALC.EFX.GenFilter();
			this.GenerateFilter();
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0002A9D5 File Offset: 0x00028BD5
		public void SetHFGain(float gain)
		{
			ALC.EFX.Filter(this.filter, 2, gain);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002A9E4 File Offset: 0x00028BE4
		public void SetGain(float gain)
		{
			ALC.EFX.Filter(this.filter, 1, gain);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0002A9F3 File Offset: 0x00028BF3
		public void Start()
		{
			if (this.IsEnabled)
			{
				return;
			}
			AL.Source(this.source, 131077, this.filter);
			this.IsEnabled = true;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0002AA1B File Offset: 0x00028C1B
		public void Stop()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			AL.Source(this.source, 131077, this.nullFilter);
			this.IsEnabled = false;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002AA43 File Offset: 0x00028C43
		private void GenerateFilter()
		{
			this.filter = ALC.EFX.GenFilter();
			ALC.EFX.Filter(this.filter, 32769, 1);
			ALC.EFX.Filter(this.filter, 1, 1f);
			ALC.EFX.Filter(this.filter, 2, 0.2f);
		}

		// Token: 0x04000388 RID: 904
		private int source;

		// Token: 0x04000389 RID: 905
		private int nullFilter;

		// Token: 0x0400038A RID: 906
		public int filter;
	}
}
