using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200016E RID: 366
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SoundFrameJson
	{
		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x00059C3C File Offset: 0x00057E3C
		// (set) Token: 0x06001265 RID: 4709 RVA: 0x00059C44 File Offset: 0x00057E44
		public string[] Code { get; set; } = Array.Empty<string>();

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x00059C4D File Offset: 0x00057E4D
		// (set) Token: 0x06001267 RID: 4711 RVA: 0x00059C55 File Offset: 0x00057E55
		public float DurationFraction { get; set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x00059C5E File Offset: 0x00057E5E
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x00059C66 File Offset: 0x00057E66
		public bool RandomizePitch { get; set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x00059C6F File Offset: 0x00057E6F
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x00059C77 File Offset: 0x00057E77
		public float Range { get; set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x00059C80 File Offset: 0x00057E80
		// (set) Token: 0x0600126D RID: 4717 RVA: 0x00059C88 File Offset: 0x00057E88
		public float Volume { get; set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x00059C91 File Offset: 0x00057E91
		// (set) Token: 0x0600126F RID: 4719 RVA: 0x00059C99 File Offset: 0x00057E99
		public bool Synchronize { get; set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x00059CA2 File Offset: 0x00057EA2
		// (set) Token: 0x06001271 RID: 4721 RVA: 0x00059CAA File Offset: 0x00057EAA
		public float Pitch { get; set; } = 1f;

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x00059CB3 File Offset: 0x00057EB3
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x00059CBB File Offset: 0x00057EBB
		public bool PitchFollowsAnimationSpeed { get; set; }

		// Token: 0x06001274 RID: 4724 RVA: 0x00059CC4 File Offset: 0x00057EC4
		public SoundFrame ToSoundFrame()
		{
			return new SoundFrame(this.Code, this.DurationFraction, this.RandomizePitch, this.Range, this.Volume, this.Synchronize, this.Pitch, this.PitchFollowsAnimationSpeed);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x00059CFC File Offset: 0x00057EFC
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
