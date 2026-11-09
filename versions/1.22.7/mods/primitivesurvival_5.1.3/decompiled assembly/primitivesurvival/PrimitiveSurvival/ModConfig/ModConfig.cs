using System;
using System.Runtime.CompilerServices;

namespace PrimitiveSurvival.ModConfig
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	public class ModConfig
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00024E6B File Offset: 0x0002306B
		// (set) Token: 0x06000325 RID: 805 RVA: 0x00024E72 File Offset: 0x00023072
		public static ModConfig Loaded { get; set; } = new ModConfig();

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00024E7A File Offset: 0x0002307A
		// (set) Token: 0x06000327 RID: 807 RVA: 0x00024E82 File Offset: 0x00023082
		public int DeadfallBaitStolenPercent { get; set; } = 10;

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00024E8B File Offset: 0x0002308B
		// (set) Token: 0x06000329 RID: 809 RVA: 0x00024E93 File Offset: 0x00023093
		public float DeadfallMaxAnimalHeight { get; set; } = 0.7f;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00024E9C File Offset: 0x0002309C
		// (set) Token: 0x0600032B RID: 811 RVA: 0x00024EA4 File Offset: 0x000230A4
		public int DeadfallMaxDamageSet { get; set; } = 10;

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00024EAD File Offset: 0x000230AD
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00024EB5 File Offset: 0x000230B5
		public int DeadfallMaxDamageBaited { get; set; } = 20;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00024EBE File Offset: 0x000230BE
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00024EC6 File Offset: 0x000230C6
		public int DeadfallTrippedPercent { get; set; } = 10;

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00024ECF File Offset: 0x000230CF
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00024ED7 File Offset: 0x000230D7
		public int FallDamageMultiplierWoodSpikes { get; set; } = 25;

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00024EE0 File Offset: 0x000230E0
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00024EE8 File Offset: 0x000230E8
		public int FallDamageMultiplierMetalSpikes { get; set; } = 80;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00024EF1 File Offset: 0x000230F1
		// (set) Token: 0x06000335 RID: 821 RVA: 0x00024EF9 File Offset: 0x000230F9
		public int FishBasketCatchPercent { get; set; } = 5;

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00024F02 File Offset: 0x00023102
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00024F0A File Offset: 0x0002310A
		public int FishBasketBaitedCatchPercent { get; set; } = 10;

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00024F13 File Offset: 0x00023113
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00024F1B File Offset: 0x0002311B
		public int FishBasketBaitStolenPercent { get; set; } = 5;

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00024F24 File Offset: 0x00023124
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00024F2C File Offset: 0x0002312C
		public int FishBasketEscapePercent { get; set; } = 15;

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00024F35 File Offset: 0x00023135
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00024F3D File Offset: 0x0002313D
		public double FishBasketUpdateMinutes { get; set; } = 2.2;

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00024F46 File Offset: 0x00023146
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00024F4E File Offset: 0x0002314E
		public int FishBasketRotRemovedPercent { get; set; } = 10;

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00024F57 File Offset: 0x00023157
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00024F5F File Offset: 0x0002315F
		public int FishChanceOfEggsPercent { get; set; } = 20;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00024F68 File Offset: 0x00023168
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00024F70 File Offset: 0x00023170
		public int FishChunkDepletionRate { get; set; } = 10;

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00024F79 File Offset: 0x00023179
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00024F81 File Offset: 0x00023181
		public int FishChunkRepletionRate { get; set; } = 1;

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00024F8A File Offset: 0x0002318A
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00024F92 File Offset: 0x00023192
		public int FishChunkRepletionMinutes { get; set; } = 20;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00024F9B File Offset: 0x0002319B
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00024FA3 File Offset: 0x000231A3
		public int FishEggsChunkRepletionRate { get; set; } = 10;

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00024FAC File Offset: 0x000231AC
		// (set) Token: 0x0600034B RID: 843 RVA: 0x00024FB4 File Offset: 0x000231B4
		public int FishChunkMaxDepletionPercent { get; set; } = 95;

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00024FBD File Offset: 0x000231BD
		// (set) Token: 0x0600034D RID: 845 RVA: 0x00024FC5 File Offset: 0x000231C5
		public int FurrowedLandUpdateFrequency { get; set; } = 120;

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00024FCE File Offset: 0x000231CE
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00024FD6 File Offset: 0x000231D6
		public double FurrowedLandBlockageChancePercent { get; set; } = 0.05;

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00024FDF File Offset: 0x000231DF
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00024FE7 File Offset: 0x000231E7
		public bool FurrowedLandEnabled { get; set; } = true;

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00024FF0 File Offset: 0x000231F0
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00024FF8 File Offset: 0x000231F8
		public double FurrowedLandMinMoistureClose { get; set; } = 0.85;

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00025001 File Offset: 0x00023201
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00025009 File Offset: 0x00023209
		public double FurrowedLandMinMoistureFar { get; set; } = 0.6;

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00025012 File Offset: 0x00023212
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0002501A File Offset: 0x0002321A
		public int LimbTrotlineCatchPercent { get; set; } = 1;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00025023 File Offset: 0x00023223
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0002502B File Offset: 0x0002322B
		public int LimbTrotlineBaitedCatchPercent { get; set; } = 3;

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00025034 File Offset: 0x00023234
		// (set) Token: 0x0600035B RID: 859 RVA: 0x0002503C File Offset: 0x0002323C
		public int LimbTrotlineLuredCatchPercent { get; set; } = 3;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00025045 File Offset: 0x00023245
		// (set) Token: 0x0600035D RID: 861 RVA: 0x0002504D File Offset: 0x0002324D
		public int LimbTrotlineBaitedLuredCatchPercent { get; set; } = 6;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00025056 File Offset: 0x00023256
		// (set) Token: 0x0600035F RID: 863 RVA: 0x0002505E File Offset: 0x0002325E
		public int LimbTrotlineBaitStolenPercent { get; set; } = 5;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00025067 File Offset: 0x00023267
		// (set) Token: 0x06000361 RID: 865 RVA: 0x0002506F File Offset: 0x0002326F
		public double LimbTrotlineUpdateMinutes { get; set; } = 2.4;

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00025078 File Offset: 0x00023278
		// (set) Token: 0x06000363 RID: 867 RVA: 0x00025080 File Offset: 0x00023280
		public int LimbTrotlineRotRemovedPercent { get; set; } = 10;

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00025089 File Offset: 0x00023289
		// (set) Token: 0x06000365 RID: 869 RVA: 0x00025091 File Offset: 0x00023291
		public int MonkeyBridgeMaxLength { get; set; } = 120;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0002509A File Offset: 0x0002329A
		// (set) Token: 0x06000367 RID: 871 RVA: 0x000250A2 File Offset: 0x000232A2
		public bool RaftEnabled { get; set; } = true;

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000368 RID: 872 RVA: 0x000250AB File Offset: 0x000232AB
		// (set) Token: 0x06000369 RID: 873 RVA: 0x000250B3 File Offset: 0x000232B3
		public float RaftWaterSpeedModifier { get; set; } = 0.1f;

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000250BC File Offset: 0x000232BC
		// (set) Token: 0x0600036B RID: 875 RVA: 0x000250C4 File Offset: 0x000232C4
		public float RaftFlotationModifier { get; set; } = 0.025f;

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600036C RID: 876 RVA: 0x000250CD File Offset: 0x000232CD
		// (set) Token: 0x0600036D RID: 877 RVA: 0x000250D5 File Offset: 0x000232D5
		public bool ShowModNameInHud { get; set; } = true;

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000250DE File Offset: 0x000232DE
		// (set) Token: 0x0600036F RID: 879 RVA: 0x000250E6 File Offset: 0x000232E6
		public bool ShowModNameInGuis { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000250EF File Offset: 0x000232EF
		// (set) Token: 0x06000371 RID: 881 RVA: 0x000250F7 File Offset: 0x000232F7
		public int SnareBaitStolenPercent { get; set; } = 10;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00025100 File Offset: 0x00023300
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00025108 File Offset: 0x00023308
		public float SnareMaxAnimalHeight { get; set; } = 0.8f;

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00025111 File Offset: 0x00023311
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00025119 File Offset: 0x00023319
		public int SnareMaxDamageSet { get; set; } = 12;

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00025122 File Offset: 0x00023322
		// (set) Token: 0x06000377 RID: 887 RVA: 0x0002512A File Offset: 0x0002332A
		public int SnareMaxDamageBaited { get; set; } = 24;

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00025133 File Offset: 0x00023333
		// (set) Token: 0x06000379 RID: 889 RVA: 0x0002513B File Offset: 0x0002333B
		public int SnareTrippedPercent { get; set; } = 10;

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00025144 File Offset: 0x00023344
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0002514C File Offset: 0x0002334C
		public double SpawnMultiplierBioluminescentGlobe { get; set; } = 1.0;

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00025155 File Offset: 0x00023355
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0002515D File Offset: 0x0002335D
		public double SpawnMultiplierBioluminescentJelly { get; set; } = 1.0;

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00025166 File Offset: 0x00023366
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0002516E File Offset: 0x0002336E
		public double SpawnMultiplierBioluminescentOrangeJelly { get; set; } = 1.0;

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00025177 File Offset: 0x00023377
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0002517F File Offset: 0x0002337F
		public double SpawnMultiplierBioluminescentWorm { get; set; } = 1.0;

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00025188 File Offset: 0x00023388
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00025190 File Offset: 0x00023390
		public double SpawnMultiplierCrabBairdi { get; set; } = 1.0;

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00025199 File Offset: 0x00023399
		// (set) Token: 0x06000385 RID: 901 RVA: 0x000251A1 File Offset: 0x000233A1
		public double SpawnMultiplierCrabLand { get; set; } = 1.0;

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000386 RID: 902 RVA: 0x000251AA File Offset: 0x000233AA
		// (set) Token: 0x06000387 RID: 903 RVA: 0x000251B2 File Offset: 0x000233B2
		public double SpawnMultiplierLivingDead { get; set; } = 1.0;

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000388 RID: 904 RVA: 0x000251BB File Offset: 0x000233BB
		// (set) Token: 0x06000389 RID: 905 RVA: 0x000251C3 File Offset: 0x000233C3
		public double SpawnMultiplierSnakeBlackRat { get; set; } = 1.0;

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600038A RID: 906 RVA: 0x000251CC File Offset: 0x000233CC
		// (set) Token: 0x0600038B RID: 907 RVA: 0x000251D4 File Offset: 0x000233D4
		public double SpawnMultiplierSnakeChainViper { get; set; } = 1.0;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000251DD File Offset: 0x000233DD
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000251E5 File Offset: 0x000233E5
		public double SpawnMultiplierSnakeCoachWhip { get; set; } = 1.0;

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000251EE File Offset: 0x000233EE
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000251F6 File Offset: 0x000233F6
		public double SpawnMultiplierSnakePitViper { get; set; } = 1.0;

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000251FF File Offset: 0x000233FF
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00025207 File Offset: 0x00023407
		public double SpawnMultiplierWillowispGreen { get; set; } = 1.0;

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000392 RID: 914 RVA: 0x00025210 File Offset: 0x00023410
		// (set) Token: 0x06000393 RID: 915 RVA: 0x00025218 File Offset: 0x00023418
		public double SpawnMultiplierWillowispWhite { get; set; } = 1.0;

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00025221 File Offset: 0x00023421
		// (set) Token: 0x06000395 RID: 917 RVA: 0x00025229 File Offset: 0x00023429
		public double SpawnMultiplierWillowispYellow { get; set; } = 1.0;

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00025232 File Offset: 0x00023432
		// (set) Token: 0x06000397 RID: 919 RVA: 0x0002523A File Offset: 0x0002343A
		public int TreeHollowsMaxItems { get; set; } = 8;

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000398 RID: 920 RVA: 0x00025243 File Offset: 0x00023443
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0002524B File Offset: 0x0002344B
		public bool TreeHollowsEnableDeveloperTools { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00025254 File Offset: 0x00023454
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0002525C File Offset: 0x0002345C
		public int TreeHollowsMaxPerChunk { get; set; } = 1;

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00025265 File Offset: 0x00023465
		// (set) Token: 0x0600039D RID: 925 RVA: 0x0002526D File Offset: 0x0002346D
		public float TreeHollowsSpawnProbability { get; set; } = 0.15f;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00025276 File Offset: 0x00023476
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0002527E File Offset: 0x0002347E
		public double TreeHollowsUpdateMinutes { get; set; } = 360.0;

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00025287 File Offset: 0x00023487
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0002528F File Offset: 0x0002348F
		public int WeirTrapCatchPercent { get; set; } = 2;

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00025298 File Offset: 0x00023498
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x000252A0 File Offset: 0x000234A0
		public int WeirTrapEscapePercent { get; set; } = 15;

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000252A9 File Offset: 0x000234A9
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x000252B1 File Offset: 0x000234B1
		public double WeirTrapUpdateMinutes { get; set; } = 5.0;

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x000252BA File Offset: 0x000234BA
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x000252C2 File Offset: 0x000234C2
		public int WeirTrapRotRemovedPercent { get; set; } = 10;

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000252CB File Offset: 0x000234CB
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x000252D3 File Offset: 0x000234D3
		public int WormFoundPercentRock { get; set; } = 1;

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003AA RID: 938 RVA: 0x000252DC File Offset: 0x000234DC
		// (set) Token: 0x060003AB RID: 939 RVA: 0x000252E4 File Offset: 0x000234E4
		public int WormFoundPercentStickFlint { get; set; } = 10;
	}
}
