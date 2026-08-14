using System;
using OpenTK.Audio.OpenAL;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000CF RID: 207
	public class CheapMicEffect : SoundEffect
	{
		// Token: 0x06000795 RID: 1941 RVA: 0x0002A5A9 File Offset: 0x000287A9
		public CheapMicEffect(int source) : base("cheapmic", source)
		{
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002A5B8 File Offset: 0x000287B8
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

		// Token: 0x06000797 RID: 1943 RVA: 0x0002A6CB File Offset: 0x000288CB
		public override void Apply()
		{
			base.Apply();
			ALC.EFX.Source(this.source, 131078, this.slot, 0, 0);
			ALC.EFX.Source(this.source, 131077, this.lowpassFilter);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002A701 File Offset: 0x00028901
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

		// Token: 0x04000381 RID: 897
		private int lowpassFilter;
	}
}
