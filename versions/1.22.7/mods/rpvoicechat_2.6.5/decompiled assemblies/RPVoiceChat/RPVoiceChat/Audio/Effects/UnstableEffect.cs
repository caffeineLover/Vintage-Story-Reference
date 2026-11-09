using System;

namespace RPVoiceChat.Audio.Effects
{
	// Token: 0x02000112 RID: 274
	public class UnstableEffect : SoundEffect
	{
		// Token: 0x06000A0A RID: 2570 RVA: 0x00037608 File Offset: 0x00035808
		public UnstableEffect(int source) : base("unstable", source)
		{
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00037616 File Offset: 0x00035816
		protected override int GenerateEffect()
		{
			throw new NotImplementedException();
		}
	}
}
