using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000DE RID: 222
	[NullableContext(1)]
	[Nullable(0)]
	public class AimingStatsJson
	{
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x0003AA22 File Offset: 0x00038C22
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x0003AA2A File Offset: 0x00038C2A
		public float AimDifficulty { get; set; } = 1f;

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0003AA33 File Offset: 0x00038C33
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x0003AA3B File Offset: 0x00038C3B
		public string CursorType { get; set; } = "Moving";

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0003AA44 File Offset: 0x00038C44
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x0003AA4C File Offset: 0x00038C4C
		public bool InvertMouseYAxis { get; set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x0003AA55 File Offset: 0x00038C55
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x0003AA5D File Offset: 0x00038C5D
		public float VerticalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x0003AA66 File Offset: 0x00038C66
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x0003AA6E File Offset: 0x00038C6E
		public float HorizontalAccuracyMultiplier { get; set; } = 1f;

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x0003AA77 File Offset: 0x00038C77
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x0003AA7F File Offset: 0x00038C7F
		public float AimDriftFrequency { get; set; } = 0.001f;

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x0003AA88 File Offset: 0x00038C88
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x0003AA90 File Offset: 0x00038C90
		public float AimDrift { get; set; } = 150f;

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0003AA99 File Offset: 0x00038C99
		// (set) Token: 0x06000B45 RID: 2885 RVA: 0x0003AAA1 File Offset: 0x00038CA1
		public float AimTwitch { get; set; } = 40f;

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0003AAAA File Offset: 0x00038CAA
		// (set) Token: 0x06000B47 RID: 2887 RVA: 0x0003AAB2 File Offset: 0x00038CB2
		public float HorizontalLimit { get; set; } = 0.125f;

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x0003AABB File Offset: 0x00038CBB
		// (set) Token: 0x06000B49 RID: 2889 RVA: 0x0003AAC3 File Offset: 0x00038CC3
		public float VerticalLimit { get; set; } = 0.35f;

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0003AACC File Offset: 0x00038CCC
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0003AAD4 File Offset: 0x00038CD4
		public float VerticalOffset { get; set; } = -0.15f;

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0003AADD File Offset: 0x00038CDD
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0003AAE5 File Offset: 0x00038CE5
		public int AimTwitchDuration { get; set; } = 300;

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0003AAEE File Offset: 0x00038CEE
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x0003AAF6 File Offset: 0x00038CF6
		public bool AllowSprint { get; set; } = true;

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0003AAFF File Offset: 0x00038CFF
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x0003AB07 File Offset: 0x00038D07
		public float MoveSpeedPenalty { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x0003AB10 File Offset: 0x00038D10
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x0003AB18 File Offset: 0x00038D18
		public float AccuracyOvertimeSec { get; set; } = 6f;

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0003AB21 File Offset: 0x00038D21
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x0003AB29 File Offset: 0x00038D29
		public float AccuracyMovePenalty { get; set; } = 1f;

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0003AB32 File Offset: 0x00038D32
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x0003AB3A File Offset: 0x00038D3A
		public string CursorTextureReady { get; set; } = "";

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x0003AB43 File Offset: 0x00038D43
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x0003AB4B File Offset: 0x00038D4B
		public string CursorTextureNotReady { get; set; } = "";

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x0003AB54 File Offset: 0x00038D54
		// (set) Token: 0x06000B5B RID: 2907 RVA: 0x0003AB5C File Offset: 0x00038D5C
		public float AnimationFollowX { get; set; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0003AB65 File Offset: 0x00038D65
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x0003AB6D File Offset: 0x00038D6D
		public float AnimationFollowY { get; set; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0003AB76 File Offset: 0x00038D76
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0003AB7E File Offset: 0x00038D7E
		public float AnimationOffsetX { get; set; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0003AB87 File Offset: 0x00038D87
		// (set) Token: 0x06000B61 RID: 2913 RVA: 0x0003AB8F File Offset: 0x00038D8F
		public float AnimationOffsetY { get; set; }

		// Token: 0x06000B62 RID: 2914 RVA: 0x0003AB98 File Offset: 0x00038D98
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
