using System;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x0200010F RID: 271
	public class IntoxicatedEffect : SoundEffect
	{
		// Token: 0x060009FB RID: 2555 RVA: 0x00037253 File Offset: 0x00035453
		public IntoxicatedEffect(int source) : base("intoxicated", source)
		{
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00037264 File Offset: 0x00035464
		protected override int GenerateEffect()
		{
			float pitch = ((double)this.toxicRate <= 0.2) ? 1f : (1f - this.toxicRate / 5f);
			OALW.Source(this.source, 4099, pitch);
			throw new NotImplementedException();
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x000372B3 File Offset: 0x000354B3
		public void SetToxicRate(float toxicRate)
		{
			this.toxicRate = toxicRate;
		}

		// Token: 0x04000486 RID: 1158
		protected float toxicRate;
	}
}
