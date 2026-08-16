using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000DF RID: 223
	[NullableContext(1)]
	[Nullable(0)]
	public class AimingStats
	{
		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0003AD7F File Offset: 0x00038F7F
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x0003AD87 File Offset: 0x00038F87
		public float AimDifficulty { get; set; } = 1f;

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0003AD90 File Offset: 0x00038F90
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x0003AD98 File Offset: 0x00038F98
		public AimingCursorType CursorType { get; set; } = AimingCursorType.Moving;

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0003ADA1 File Offset: 0x00038FA1
		// (set) Token: 0x06000B69 RID: 2921 RVA: 0x0003ADA9 File Offset: 0x00038FA9
		public bool InvertMouseYAxis { get; set; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0003ADB2 File Offset: 0x00038FB2
		// (set) Token: 0x06000B6B RID: 2923 RVA: 0x0003ADBA File Offset: 0x00038FBA
		public float VerticalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0003ADC3 File Offset: 0x00038FC3
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0003ADCB File Offset: 0x00038FCB
		public float HorizontalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0003ADD4 File Offset: 0x00038FD4
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0003ADDC File Offset: 0x00038FDC
		public float AimDriftFrequency { get; set; } = 0.001f;

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0003ADE5 File Offset: 0x00038FE5
		// (set) Token: 0x06000B71 RID: 2929 RVA: 0x0003ADED File Offset: 0x00038FED
		public float AimDrift { get; set; } = 150f;

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0003ADF6 File Offset: 0x00038FF6
		// (set) Token: 0x06000B73 RID: 2931 RVA: 0x0003ADFE File Offset: 0x00038FFE
		public float AimTwitch { get; set; } = 40f;

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0003AE07 File Offset: 0x00039007
		// (set) Token: 0x06000B75 RID: 2933 RVA: 0x0003AE0F File Offset: 0x0003900F
		public float HorizontalLimit { get; set; } = 0.125f;

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0003AE18 File Offset: 0x00039018
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0003AE20 File Offset: 0x00039020
		public float VerticalLimit { get; set; } = 0.35f;

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0003AE29 File Offset: 0x00039029
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0003AE31 File Offset: 0x00039031
		public float VerticalOffset { get; set; } = -0.15f;

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0003AE3A File Offset: 0x0003903A
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0003AE42 File Offset: 0x00039042
		public int AimTwitchDuration { get; set; } = 300;

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0003AE4B File Offset: 0x0003904B
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0003AE53 File Offset: 0x00039053
		public bool AllowSprint { get; set; } = true;

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0003AE5C File Offset: 0x0003905C
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0003AE64 File Offset: 0x00039064
		public float MoveSpeedPenalty { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0003AE6D File Offset: 0x0003906D
		// (set) Token: 0x06000B81 RID: 2945 RVA: 0x0003AE75 File Offset: 0x00039075
		public TimeSpan AccuracyOvertime { get; set; } = TimeSpan.FromSeconds(6L);

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0003AE7E File Offset: 0x0003907E
		// (set) Token: 0x06000B83 RID: 2947 RVA: 0x0003AE86 File Offset: 0x00039086
		public float AccuracyMovePenalty { get; set; } = 1f;

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0003AE8F File Offset: 0x0003908F
		// (set) Token: 0x06000B85 RID: 2949 RVA: 0x0003AE97 File Offset: 0x00039097
		public string CursorTextureReady { get; set; } = "";

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0003AEA0 File Offset: 0x000390A0
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x0003AEA8 File Offset: 0x000390A8
		public string CursorTextureNotReady { get; set; } = "";

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0003AEB1 File Offset: 0x000390B1
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x0003AEB9 File Offset: 0x000390B9
		public float AnimationFollowX { get; set; }

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0003AEC2 File Offset: 0x000390C2
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x0003AECA File Offset: 0x000390CA
		public float AnimationFollowY { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0003AED3 File Offset: 0x000390D3
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x0003AEDB File Offset: 0x000390DB
		public float AnimationOffsetX { get; set; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0003AEE4 File Offset: 0x000390E4
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x0003AEEC File Offset: 0x000390EC
		public float AnimationOffsetY { get; set; }

		// Token: 0x06000B90 RID: 2960 RVA: 0x0003AEF8 File Offset: 0x000390F8
		public AimingStats Clone()
		{
			return new AimingStats
			{
				AimDifficulty = this.AimDifficulty,
				CursorType = this.CursorType,
				InvertMouseYAxis = this.InvertMouseYAxis,
				VerticalAccuracyMultiplier = this.VerticalAccuracyMultiplier,
				HorizontalAccuracyMultiplier = this.HorizontalAccuracyMultiplier,
				AimDriftFrequency = this.AimDriftFrequency,
				AimDrift = this.AimDrift,
				AimTwitch = this.AimTwitch,
				HorizontalLimit = this.HorizontalLimit,
				VerticalLimit = this.VerticalLimit,
				VerticalOffset = this.VerticalOffset,
				AimTwitchDuration = this.AimTwitchDuration,
				AllowSprint = this.AllowSprint,
				MoveSpeedPenalty = this.MoveSpeedPenalty,
				AccuracyOvertime = this.AccuracyOvertime,
				AccuracyMovePenalty = this.AccuracyMovePenalty,
				CursorTextureReady = this.CursorTextureReady,
				CursorTextureNotReady = this.CursorTextureNotReady,
				AnimationFollowX = this.AnimationFollowX,
				AnimationFollowY = this.AnimationFollowY,
				AnimationOffsetX = this.AnimationOffsetX,
				AnimationOffsetY = this.AnimationOffsetY
			};
		}
	}
}
