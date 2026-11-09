using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;

namespace Firearms
{
	// Token: 0x02000004 RID: 4
	[NullableContext(1)]
	[Nullable(0)]
	public class RecoilStats
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000023F0 File Offset: 0x000005F0
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000023F8 File Offset: 0x000005F8
		public bool Enabled { get; set; } = true;

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002401 File Offset: 0x00000601
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002409 File Offset: 0x00000609
		public float Strength { get; set; } = 1f;

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002412 File Offset: 0x00000612
		// (set) Token: 0x06000018 RID: 24 RVA: 0x0000241A File Offset: 0x0000061A
		public float VerticalBaseDeg { get; set; } = 2.5f;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002423 File Offset: 0x00000623
		// (set) Token: 0x0600001A RID: 26 RVA: 0x0000242B File Offset: 0x0000062B
		public float VerticalDamageScaleDeg { get; set; } = 0.45f;

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002434 File Offset: 0x00000634
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000243C File Offset: 0x0000063C
		public float VerticalMinDeg { get; set; } = 4f;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002445 File Offset: 0x00000645
		// (set) Token: 0x0600001E RID: 30 RVA: 0x0000244D File Offset: 0x0000064D
		public float VerticalMaxDeg { get; set; } = 12f;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002456 File Offset: 0x00000656
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000245E File Offset: 0x0000065E
		public float HorizontalBaseDeg { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002467 File Offset: 0x00000667
		// (set) Token: 0x06000022 RID: 34 RVA: 0x0000246F File Offset: 0x0000066F
		public float HorizontalVerticalScale { get; set; } = 0.15f;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002478 File Offset: 0x00000678
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002480 File Offset: 0x00000680
		public float HorizontalDamageScaleDeg { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002489 File Offset: 0x00000689
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002491 File Offset: 0x00000691
		public float HorizontalMinDeg { get; set; } = 0.35f;

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000249A File Offset: 0x0000069A
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000024A2 File Offset: 0x000006A2
		public float HorizontalMaxDeg { get; set; } = 1.8f;

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000024AB File Offset: 0x000006AB
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000024B3 File Offset: 0x000006B3
		public float RiseDurationSec { get; set; } = 0.18f;

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000024BC File Offset: 0x000006BC
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000024C4 File Offset: 0x000006C4
		public bool SpringBack { get; set; } = true;

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000024CD File Offset: 0x000006CD
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000024D5 File Offset: 0x000006D5
		public float SpringBackFraction { get; set; } = 0.28f;

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000024DE File Offset: 0x000006DE
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000024E6 File Offset: 0x000006E6
		public float SpringBackDelaySec { get; set; } = 0.04f;

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000024EF File Offset: 0x000006EF
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000024F7 File Offset: 0x000006F7
		public float SpringBackDurationSec { get; set; } = 0.55f;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002500 File Offset: 0x00000700
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002508 File Offset: 0x00000708
		public float MaxQueuedVerticalDeg { get; set; } = 18f;

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002511 File Offset: 0x00000711
		// (set) Token: 0x06000036 RID: 54 RVA: 0x00002519 File Offset: 0x00000719
		public float MaxQueuedHorizontalDeg { get; set; } = 3f;

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002522 File Offset: 0x00000722
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0000252A File Offset: 0x0000072A
		public int MinIntervalMs { get; set; } = 80;

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002533 File Offset: 0x00000733
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000253B File Offset: 0x0000073B
		public float CameraShakeBase { get; set; } = 0.0018f;

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002544 File Offset: 0x00000744
		// (set) Token: 0x0600003C RID: 60 RVA: 0x0000254C File Offset: 0x0000074C
		public float CameraShakeDamageScale { get; set; } = 4E-05f;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002555 File Offset: 0x00000755
		// (set) Token: 0x0600003E RID: 62 RVA: 0x0000255D File Offset: 0x0000075D
		public float CameraShakeMin { get; set; } = 0.0018f;

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002566 File Offset: 0x00000766
		// (set) Token: 0x06000040 RID: 64 RVA: 0x0000256E File Offset: 0x0000076E
		public float CameraShakeMax { get; set; } = 0.004f;

		// Token: 0x06000041 RID: 65 RVA: 0x00002577 File Offset: 0x00000777
		public static RecoilStats MuzzleloaderDefaults()
		{
			return new RecoilStats();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002580 File Offset: 0x00000780
		public static RecoilStats RevolverDefaults()
		{
			return new RecoilStats
			{
				VerticalBaseDeg = 2f,
				VerticalDamageScaleDeg = 0.35f,
				VerticalMinDeg = 3f,
				VerticalMaxDeg = 10f,
				HorizontalMinDeg = 0.3f,
				HorizontalMaxDeg = 1.5f,
				CameraShakeBase = 0.0016f,
				CameraShakeDamageScale = 3.5E-05f,
				CameraShakeMin = 0.0016f,
				CameraShakeMax = 0.0035f
			};
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002600 File Offset: 0x00000800
		public float VerticalDegrees(float estimatedDamage)
		{
			float vertical = this.VerticalBaseDeg + estimatedDamage * this.VerticalDamageScaleDeg;
			vertical = GameMath.Clamp(vertical, Math.Min(this.VerticalMinDeg, this.VerticalMaxDeg), Math.Max(this.VerticalMinDeg, this.VerticalMaxDeg));
			return Math.Max(0f, vertical * this.Strength);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002658 File Offset: 0x00000858
		public float HorizontalDegrees(float estimatedDamage, float verticalDeg)
		{
			float horizontal = this.HorizontalBaseDeg + verticalDeg * this.HorizontalVerticalScale + estimatedDamage * this.HorizontalDamageScaleDeg;
			horizontal = GameMath.Clamp(horizontal, Math.Min(this.HorizontalMinDeg, this.HorizontalMaxDeg), Math.Max(this.HorizontalMinDeg, this.HorizontalMaxDeg));
			return Math.Max(0f, horizontal * this.Strength);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000026B9 File Offset: 0x000008B9
		public float CameraShake(float estimatedDamage)
		{
			return GameMath.Clamp(this.CameraShakeBase + estimatedDamage * this.CameraShakeDamageScale, Math.Min(this.CameraShakeMin, this.CameraShakeMax), Math.Max(this.CameraShakeMin, this.CameraShakeMax));
		}
	}
}
