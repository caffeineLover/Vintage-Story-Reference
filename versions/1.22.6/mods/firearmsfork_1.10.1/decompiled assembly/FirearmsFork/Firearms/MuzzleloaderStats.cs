using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;
using CombatOverhaul.RangedSystems.Aiming;

namespace Firearms
{
	// Token: 0x02000020 RID: 32
	[NullableContext(1)]
	[Nullable(0)]
	public class MuzzleloaderStats : WeaponStats
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005F78 File Offset: 0x00004178
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00005F80 File Offset: 0x00004180
		public bool AutomaticFire { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005F89 File Offset: 0x00004189
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00005F91 File Offset: 0x00004191
		public string ReadyAnimationOffhand { get; set; } = "";

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00005F9A File Offset: 0x0000419A
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00005FA2 File Offset: 0x000041A2
		public string IdleAnimationOffhand { get; set; } = "";

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005FAB File Offset: 0x000041AB
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00005FB3 File Offset: 0x000041B3
		public string TakeOutAnimation { get; set; } = "";

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005FBC File Offset: 0x000041BC
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00005FC4 File Offset: 0x000041C4
		public string TakeOutAnimationOffhand { get; set; } = "";

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005FCD File Offset: 0x000041CD
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00005FD5 File Offset: 0x000041D5
		public string[] LoadAnimation { get; set; } = Array.Empty<string>();

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005FDE File Offset: 0x000041DE
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005FE6 File Offset: 0x000041E6
		public string PrimeAnimation { get; set; } = "";

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00005FEF File Offset: 0x000041EF
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00005FF7 File Offset: 0x000041F7
		public string[] CockingAnimation { get; set; } = Array.Empty<string>();

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006000 File Offset: 0x00004200
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00006008 File Offset: 0x00004208
		public string[] CockingAnimationOffhand { get; set; } = Array.Empty<string>();

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006011 File Offset: 0x00004211
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00006019 File Offset: 0x00004219
		public string AimAnimation { get; set; } = "";

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00006022 File Offset: 0x00004222
		// (set) Token: 0x06000114 RID: 276 RVA: 0x0000602A File Offset: 0x0000422A
		public string[] ShootAnimation { get; set; } = Array.Empty<string>();

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006033 File Offset: 0x00004233
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000603B File Offset: 0x0000423B
		public string AimAnimationOffhand { get; set; } = "";

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006044 File Offset: 0x00004244
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000604C File Offset: 0x0000424C
		public string[] ShootAnimationOffhand { get; set; } = Array.Empty<string>();

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006055 File Offset: 0x00004255
		// (set) Token: 0x0600011A RID: 282 RVA: 0x0000605D File Offset: 0x0000425D
		public string[] LoadedAnimation { get; set; } = Array.Empty<string>();

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00006066 File Offset: 0x00004266
		// (set) Token: 0x0600011C RID: 284 RVA: 0x0000606E File Offset: 0x0000426E
		public string PrimedAnimation { get; set; } = "";

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00006077 File Offset: 0x00004277
		// (set) Token: 0x0600011E RID: 286 RVA: 0x0000607F File Offset: 0x0000427F
		public string[] CockedAnimation { get; set; } = Array.Empty<string>();

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00006088 File Offset: 0x00004288
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00006090 File Offset: 0x00004290
		public string LoadTpAnimation { get; set; } = "";

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00006099 File Offset: 0x00004299
		// (set) Token: 0x06000122 RID: 290 RVA: 0x000060A1 File Offset: 0x000042A1
		public string PrimeTpAnimation { get; set; } = "";

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000123 RID: 291 RVA: 0x000060AA File Offset: 0x000042AA
		// (set) Token: 0x06000124 RID: 292 RVA: 0x000060B2 File Offset: 0x000042B2
		public string AimTpAnimation { get; set; } = "";

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000060BB File Offset: 0x000042BB
		// (set) Token: 0x06000126 RID: 294 RVA: 0x000060C3 File Offset: 0x000042C3
		public float LoadSpeedPenalty { get; set; } = -1f;

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000127 RID: 295 RVA: 0x000060CC File Offset: 0x000042CC
		// (set) Token: 0x06000128 RID: 296 RVA: 0x000060D4 File Offset: 0x000042D4
		public float PrimeSpeedPenalty { get; set; } = -0.3f;

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000129 RID: 297 RVA: 0x000060DD File Offset: 0x000042DD
		// (set) Token: 0x0600012A RID: 298 RVA: 0x000060E5 File Offset: 0x000042E5
		public AimingStatsJson Aiming { get; set; } = new AimingStatsJson();

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600012B RID: 299 RVA: 0x000060EE File Offset: 0x000042EE
		// (set) Token: 0x0600012C RID: 300 RVA: 0x000060F6 File Offset: 0x000042F6
		public float[] DispersionMOA { get; set; } = new float[2];

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600012D RID: 301 RVA: 0x000060FF File Offset: 0x000042FF
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00006107 File Offset: 0x00004307
		public float BulletDamageMultiplier { get; set; } = 1f;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006110 File Offset: 0x00004310
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00006118 File Offset: 0x00004318
		public float BulletDamageStrength { get; set; } = 1f;

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00006121 File Offset: 0x00004321
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00006129 File Offset: 0x00004329
		public float BulletVelocity { get; set; } = 1f;

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00006132 File Offset: 0x00004332
		// (set) Token: 0x06000134 RID: 308 RVA: 0x0000613A File Offset: 0x0000433A
		public string BulletWildcard { get; set; } = "*:bullet-*";

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006143 File Offset: 0x00004343
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000614B File Offset: 0x0000434B
		public float Zeroing { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006154 File Offset: 0x00004354
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000615C File Offset: 0x0000435C
		public RecoilStats Recoil { get; set; } = RecoilStats.MuzzleloaderDefaults();

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006165 File Offset: 0x00004365
		// (set) Token: 0x0600013A RID: 314 RVA: 0x0000616D File Offset: 0x0000436D
		public int MagazineSize { get; set; } = 1;

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00006176 File Offset: 0x00004376
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000617E File Offset: 0x0000437E
		public int BulletsFiredPerShot { get; set; } = 1;

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00006187 File Offset: 0x00004387
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000618F File Offset: 0x0000438F
		public int BulletLoadedPerReload { get; set; } = 1;

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006198 File Offset: 0x00004398
		// (set) Token: 0x06000140 RID: 320 RVA: 0x000061A0 File Offset: 0x000043A0
		public int BulletsLoadedPerBulletItem { get; set; } = 1;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000141 RID: 321 RVA: 0x000061A9 File Offset: 0x000043A9
		// (set) Token: 0x06000142 RID: 322 RVA: 0x000061B1 File Offset: 0x000043B1
		public int WaddingUsedPerReload { get; set; } = 1;

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000061BA File Offset: 0x000043BA
		// (set) Token: 0x06000144 RID: 324 RVA: 0x000061C2 File Offset: 0x000043C2
		public int LoadPowderConsumption { get; set; } = 1;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000145 RID: 325 RVA: 0x000061CB File Offset: 0x000043CB
		// (set) Token: 0x06000146 RID: 326 RVA: 0x000061D3 File Offset: 0x000043D3
		public int PrimePowderConsumption { get; set; } = 1;

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000061DC File Offset: 0x000043DC
		// (set) Token: 0x06000148 RID: 328 RVA: 0x000061E4 File Offset: 0x000043E4
		public string FlaskWildcard { get; set; } = "*powderflask-*";

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000061ED File Offset: 0x000043ED
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000061F5 File Offset: 0x000043F5
		public string WaddingWildcard { get; set; } = "*linenpatch*";

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000061FE File Offset: 0x000043FE
		// (set) Token: 0x0600014C RID: 332 RVA: 0x00006206 File Offset: 0x00004406
		public string LoadingRequirementWildcard { get; set; } = "";

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000620F File Offset: 0x0000440F
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00006217 File Offset: 0x00004417
		public string PrimingRequirementWildcard { get; set; } = "";

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006220 File Offset: 0x00004420
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00006228 File Offset: 0x00004428
		public string CockingRequirementWildcard { get; set; } = "";

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00006231 File Offset: 0x00004431
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00006239 File Offset: 0x00004439
		public string LoadingRequirementMessage { get; set; } = "maltiezfirearms:requirement-missing-loading-equipment";

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00006242 File Offset: 0x00004442
		// (set) Token: 0x06000154 RID: 340 RVA: 0x0000624A File Offset: 0x0000444A
		public string PrimingRequirementMessage { get; set; } = "maltiezfirearms:requirement-missing-priming-equipment";

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00006253 File Offset: 0x00004453
		// (set) Token: 0x06000156 RID: 342 RVA: 0x0000625B File Offset: 0x0000445B
		public string CockingRequirementMessage { get; set; } = "maltiezfirearms:requirement-missing-cocking-equipment";

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00006264 File Offset: 0x00004464
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000626C File Offset: 0x0000446C
		public bool CancelReloadOnInAir { get; set; } = true;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00006275 File Offset: 0x00004475
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000627D File Offset: 0x0000447D
		public float ReloadAnimationSpeed { get; set; } = 1f;
	}
}
