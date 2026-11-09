using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000EA RID: 234
	[NullableContext(1)]
	[Nullable(0)]
	public class AimingStats
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0003BD6F File Offset: 0x00039F6F
		// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x0003BD77 File Offset: 0x00039F77
		public float AimDifficulty { get; set; } = 1f;

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0003BD80 File Offset: 0x00039F80
		// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x0003BD88 File Offset: 0x00039F88
		public AimingCursorType CursorType { get; set; } = AimingCursorType.Moving;

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0003BD91 File Offset: 0x00039F91
		// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x0003BD99 File Offset: 0x00039F99
		public bool InvertMouseYAxis { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0003BDA2 File Offset: 0x00039FA2
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0003BDAA File Offset: 0x00039FAA
		public float VerticalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0003BDB3 File Offset: 0x00039FB3
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0003BDBB File Offset: 0x00039FBB
		public float HorizontalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0003BDC4 File Offset: 0x00039FC4
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x0003BDCC File Offset: 0x00039FCC
		public float AimDriftFrequency { get; set; } = 0.001f;

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x0003BDD5 File Offset: 0x00039FD5
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x0003BDDD File Offset: 0x00039FDD
		public float AimDrift { get; set; } = 150f;

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0003BDE6 File Offset: 0x00039FE6
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0003BDEE File Offset: 0x00039FEE
		public float AimTwitch { get; set; } = 40f;

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0003BDF7 File Offset: 0x00039FF7
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x0003BDFF File Offset: 0x00039FFF
		public float HorizontalLimit { get; set; } = 0.125f;

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x0003BE08 File Offset: 0x0003A008
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x0003BE10 File Offset: 0x0003A010
		public float VerticalLimit { get; set; } = 0.35f;

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x0003BE19 File Offset: 0x0003A019
		// (set) Token: 0x06000BD5 RID: 3029 RVA: 0x0003BE21 File Offset: 0x0003A021
		public float VerticalOffset { get; set; } = -0.15f;

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0003BE2A File Offset: 0x0003A02A
		// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x0003BE32 File Offset: 0x0003A032
		public int AimTwitchDuration { get; set; } = 300;

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0003BE3B File Offset: 0x0003A03B
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x0003BE43 File Offset: 0x0003A043
		public bool AllowSprint { get; set; } = true;

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0003BE4C File Offset: 0x0003A04C
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x0003BE54 File Offset: 0x0003A054
		public float MoveSpeedPenalty { get; set; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0003BE5D File Offset: 0x0003A05D
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0003BE65 File Offset: 0x0003A065
		public TimeSpan AccuracyOvertime { get; set; } = TimeSpan.FromSeconds(6L);

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0003BE6E File Offset: 0x0003A06E
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x0003BE76 File Offset: 0x0003A076
		public float AccuracyMovePenalty { get; set; } = 1f;

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0003BE7F File Offset: 0x0003A07F
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x0003BE87 File Offset: 0x0003A087
		public string CursorTextureReady { get; set; } = "";

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0003BE90 File Offset: 0x0003A090
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x0003BE98 File Offset: 0x0003A098
		public string CursorTextureNotReady { get; set; } = "";

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0003BEA1 File Offset: 0x0003A0A1
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0003BEA9 File Offset: 0x0003A0A9
		public float AnimationFollowX { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0003BEB2 File Offset: 0x0003A0B2
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0003BEBA File Offset: 0x0003A0BA
		public float AnimationFollowY { get; set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0003BEC3 File Offset: 0x0003A0C3
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0003BECB File Offset: 0x0003A0CB
		public float AnimationOffsetX { get; set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0003BED4 File Offset: 0x0003A0D4
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0003BEDC File Offset: 0x0003A0DC
		public float AnimationOffsetY { get; set; }

		// Token: 0x06000BEC RID: 3052 RVA: 0x0003BEE8 File Offset: 0x0003A0E8
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
