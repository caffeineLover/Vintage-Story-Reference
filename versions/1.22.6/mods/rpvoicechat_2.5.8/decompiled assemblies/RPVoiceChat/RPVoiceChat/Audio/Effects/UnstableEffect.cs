using System;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x020000D7 RID: 215
	public class UnstableEffect : SoundEffect
	{
		// Token: 0x060007BD RID: 1981 RVA: 0x0002AE38 File Offset: 0x00029038
		public UnstableEffect(int source) : base("unstable", source)
		{
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002AE46 File Offset: 0x00029046
		protected override int GenerateEffect()
		{
			throw new NotImplementedException();
		}
	}
}
