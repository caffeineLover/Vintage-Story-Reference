using System;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x0200010A RID: 266
	public class CheapMicEffect : SoundEffect
	{
		// Token: 0x060009E2 RID: 2530 RVA: 0x00036D7A File Offset: 0x00034F7A
		public CheapMicEffect(int source) : base("cheapmic", source)
		{
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00036D88 File Offset: 0x00034F88
		protected override int GenerateEffect()
		{
			this.effect = ALC.EFX.GenEffect();
			this.slot = ALC.EFX.GenAuxiliaryEffectSlot();
			ALC.EFX.Effect(this.effect, 32769, 1);
			ALC.EFX.Effect(this.effect, 5, 1.2f);
			ALC.EFX.Effect(this.effect, 6, 0.3f);
			ALC.EFX.Effect(this.effect, 1, 0.2f);
			ALC.EFX.Effect(this.effect, 2, 0.2f);
			ALC.EFX.Effect(this.effect, 12, 0.5f);
			ALC.EFX.Effect(this.effect, 7, 0.4f);
			ALC.EFX.Effect(this.effect, 8, 0.01f);
			ALC.EFX.Effect(this.effect, 3, 0.5f);
			ALC.EFX.AuxiliaryEffectSlot(this.slot, 1, this.effect);
			this.lowpassFilter = ALC.EFX.GenFilter();
			ALC.EFX.Filter(this.lowpassFilter, 32769, 1);
			ALC.EFX.Filter(this.lowpassFilter, 1, 0.6f);
			ALC.EFX.Filter(this.lowpassFilter, 2, 0.2f);
			return this.effect;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00036E9B File Offset: 0x0003509B
		public override void Apply()
		{
			base.Apply();
			ALC.EFX.Source(this.source, 131078, this.slot, 0, 0);
			ALC.EFX.Source(this.source, 131077, this.lowpassFilter);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00036ED1 File Offset: 0x000350D1
		public override void Clear()
		{
			base.Clear();
			if (this.lowpassFilter != 0)
			{
				ALC.EFX.Source(this.source, 131077, 0);
				ALC.EFX.DeleteFilter(this.lowpassFilter);
				this.lowpassFilter = 0;
			}
		}

		// Token: 0x0400047B RID: 1147
		private int lowpassFilter;
	}
}
