using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200017A RID: 378
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoundFrameJson
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060012D8 RID: 4824 RVA: 0x0005B55C File Offset: 0x0005975C
		// (set) Token: 0x060012D9 RID: 4825 RVA: 0x0005B564 File Offset: 0x00059764
		public string[] Code { get; set; } = Array.Empty<string>();

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060012DA RID: 4826 RVA: 0x0005B56D File Offset: 0x0005976D
		// (set) Token: 0x060012DB RID: 4827 RVA: 0x0005B575 File Offset: 0x00059775
		public float DurationFraction { get; set; }

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0005B57E File Offset: 0x0005977E
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x0005B586 File Offset: 0x00059786
		public bool RandomizePitch { get; set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0005B58F File Offset: 0x0005978F
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x0005B597 File Offset: 0x00059797
		public float Range { get; set; }

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x0005B5A0 File Offset: 0x000597A0
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x0005B5A8 File Offset: 0x000597A8
		public float Volume { get; set; }

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x0005B5B1 File Offset: 0x000597B1
		// (set) Token: 0x060012E3 RID: 4835 RVA: 0x0005B5B9 File Offset: 0x000597B9
		public bool Synchronize { get; set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0005B5C2 File Offset: 0x000597C2
		// (set) Token: 0x060012E5 RID: 4837 RVA: 0x0005B5CA File Offset: 0x000597CA
		public float Pitch { get; set; } = 1f;

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0005B5D3 File Offset: 0x000597D3
		// (set) Token: 0x060012E7 RID: 4839 RVA: 0x0005B5DB File Offset: 0x000597DB
		public bool PitchFollowsAnimationSpeed { get; set; }

		// Token: 0x060012E8 RID: 4840 RVA: 0x0005B5E4 File Offset: 0x000597E4
		public SoundFrame ToSoundFrame()
		{
			return new SoundFrame(this.Code, this.DurationFraction, this.RandomizePitch, this.Range, this.Volume, this.Synchronize, this.Pitch, this.PitchFollowsAnimationSpeed);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0005B61C File Offset: 0x0005981C
		public static SoundFrameJson FromSoundFrame(SoundFrame frame)
		{
			return new SoundFrameJson
			{
				Code = Enumerable.ToArray<string>(frame.Code),
				DurationFraction = frame.DurationFraction,
				RandomizePitch = frame.RandomizePitch,
				Range = frame.Range,
				Volume = frame.Volume,
				Synchronize = frame.Synchronize,
				Pitch = frame.Pitch,
				PitchFollowsAnimationSpeed = frame.PitchFollowsAnimationSpeed
			};
		}
	}
}
