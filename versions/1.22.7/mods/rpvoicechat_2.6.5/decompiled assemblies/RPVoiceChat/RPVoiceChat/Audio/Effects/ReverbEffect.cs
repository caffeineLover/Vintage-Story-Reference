using System;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x02000110 RID: 272
	public class ReverbEffect : SoundEffect
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x000372BC File Offset: 0x000354BC
		public ReverbEffect(int source) : base("reverb", source)
		{
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x000372CC File Offset: 0x000354CC
		protected override int GenerateEffect()
		{
			this.effect = ALC.EFX.GenEffect();
			this.slot = ALC.EFX.GenAuxiliaryEffectSlot();
			ALC.EFX.Effect(this.effect, 32769, 1);
			ALC.EFX.Effect(this.effect, 5, 3f);
			ALC.EFX.Effect(this.effect, 6, 0.91f);
			ALC.EFX.Effect(this.effect, 1, 0.7f);
			ALC.EFX.Effect(this.effect, 2, 0.9f);
			ALC.EFX.Effect(this.effect, 12, 3.1f);
			ALC.EFX.Effect(this.effect, 7, 0.723f);
			ALC.EFX.Effect(this.effect, 8, 0.03f);
			ALC.EFX.Effect(this.effect, 3, 0.23f);
			ALC.EFX.AuxiliaryEffectSlot(this.slot, 1, this.effect);
			return this.effect;
		}
	}
}
