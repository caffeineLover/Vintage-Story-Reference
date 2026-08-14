using System;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000D4 RID: 212
	public class IntoxicatedEffect : SoundEffect
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x0002AA83 File Offset: 0x00028C83
		public IntoxicatedEffect(int source) : base("intoxicated", source)
		{
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0002AA94 File Offset: 0x00028C94
		protected override int GenerateEffect()
		{
			float pitch = ((double)this.toxicRate <= 0.2) ? 1f : (1f - this.toxicRate / 5f);
			OALW.Source(this.source, 4099, pitch);
			throw new NotImplementedException();
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0002AAE3 File Offset: 0x00028CE3
		public void SetToxicRate(float toxicRate)
		{
			this.toxicRate = toxicRate;
		}

		// Token: 0x0400038C RID: 908
		protected float toxicRate;
	}
}
