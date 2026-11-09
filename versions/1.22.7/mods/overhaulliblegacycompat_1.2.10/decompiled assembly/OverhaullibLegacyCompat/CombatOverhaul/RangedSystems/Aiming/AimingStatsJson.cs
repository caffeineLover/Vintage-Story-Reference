using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E9 RID: 233
	[NullableContext(1)]
	[Nullable(0)]
	public class AimingStatsJson
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0003BA12 File Offset: 0x00039C12
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x0003BA1A File Offset: 0x00039C1A
		public float AimDifficulty { get; set; } = 1f;

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0003BA23 File Offset: 0x00039C23
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0003BA2B File Offset: 0x00039C2B
		public string CursorType { get; set; } = "Moving";

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0003BA34 File Offset: 0x00039C34
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0003BA3C File Offset: 0x00039C3C
		public bool InvertMouseYAxis { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0003BA45 File Offset: 0x00039C45
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x0003BA4D File Offset: 0x00039C4D
		public float VerticalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0003BA56 File Offset: 0x00039C56
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0003BA5E File Offset: 0x00039C5E
		public float HorizontalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0003BA67 File Offset: 0x00039C67
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x0003BA6F File Offset: 0x00039C6F
		public float AimDriftFrequency { get; set; } = 0.001f;

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0003BA78 File Offset: 0x00039C78
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x0003BA80 File Offset: 0x00039C80
		public float AimDrift { get; set; } = 150f;

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0003BA89 File Offset: 0x00039C89
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x0003BA91 File Offset: 0x00039C91
		public float AimTwitch { get; set; } = 40f;

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0003BA9A File Offset: 0x00039C9A
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x0003BAA2 File Offset: 0x00039CA2
		public float HorizontalLimit { get; set; } = 0.125f;

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0003BAAB File Offset: 0x00039CAB
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x0003BAB3 File Offset: 0x00039CB3
		public float VerticalLimit { get; set; } = 0.35f;

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0003BABC File Offset: 0x00039CBC
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x0003BAC4 File Offset: 0x00039CC4
		public float VerticalOffset { get; set; } = -0.15f;

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0003BACD File Offset: 0x00039CCD
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x0003BAD5 File Offset: 0x00039CD5
		public int AimTwitchDuration { get; set; } = 300;

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0003BADE File Offset: 0x00039CDE
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x0003BAE6 File Offset: 0x00039CE6
		public bool AllowSprint { get; set; } = true;

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0003BAEF File Offset: 0x00039CEF
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x0003BAF7 File Offset: 0x00039CF7
		public float MoveSpeedPenalty { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x0003BB00 File Offset: 0x00039D00
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x0003BB08 File Offset: 0x00039D08
		public float AccuracyOvertimeSec { get; set; } = 6f;

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0003BB11 File Offset: 0x00039D11
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x0003BB19 File Offset: 0x00039D19
		public float AccuracyMovePenalty { get; set; } = 1f;

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0003BB22 File Offset: 0x00039D22
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x0003BB2A File Offset: 0x00039D2A
		public string CursorTextureReady { get; set; } = "";

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0003BB33 File Offset: 0x00039D33
		// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x0003BB3B File Offset: 0x00039D3B
		public string CursorTextureNotReady { get; set; } = "";

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0003BB44 File Offset: 0x00039D44
		// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x0003BB4C File Offset: 0x00039D4C
		public float AnimationFollowX { get; set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0003BB55 File Offset: 0x00039D55
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x0003BB5D File Offset: 0x00039D5D
		public float AnimationFollowY { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0003BB66 File Offset: 0x00039D66
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0003BB6E File Offset: 0x00039D6E
		public float AnimationOffsetX { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0003BB77 File Offset: 0x00039D77
		// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0003BB7F File Offset: 0x00039D7F
		public float AnimationOffsetY { get; set; }

		// Token: 0x06000BBE RID: 3006 RVA: 0x0003BB88 File Offset: 0x00039D88
		public AimingStats ToStats()
		{
			return new AimingStats
			{
				AimDifficulty = this.AimDifficulty,
				CursorType = Enum.Parse<AimingCursorType>(this.CursorType, true),
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
				AccuracyOvertime = TimeSpan.FromSeconds((double)this.AccuracyOvertimeSec),
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
