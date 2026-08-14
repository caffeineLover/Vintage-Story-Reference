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
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00024DFB File Offset: 0x00022FFB
		// (set) Token: 0x06000325 RID: 805 RVA: 0x00024E02 File Offset: 0x00023002
		public static ModConfig Loaded { get; set; } = new ModConfig();

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00024E0A File Offset: 0x0002300A
		// (set) Token: 0x06000327 RID: 807 RVA: 0x00024E12 File Offset: 0x00023012
		public int DeadfallBaitStolenPercent { get; set; } = 10;

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00024E1B File Offset: 0x0002301B
		// (set) Token: 0x06000329 RID: 809 RVA: 0x00024E23 File Offset: 0x00023023
		public float DeadfallMaxAnimalHeight { get; set; } = 0.7f;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00024E2C File Offset: 0x0002302C
		// (set) Token: 0x0600032B RID: 811 RVA: 0x00024E34 File Offset: 0x00023034
		public int DeadfallMaxDamageSet { get; set; } = 10;

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00024E3D File Offset: 0x0002303D
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00024E45 File Offset: 0x00023045
		public int DeadfallMaxDamageBaited { get; set; } = 20;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00024E4E File Offset: 0x0002304E
		// (set) Token: 0x0600032F RID: 815 RVA: 0x00024E56 File Offset: 0x00023056
		public int DeadfallTrippedPercent { get; set; } = 10;

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000330 RID: 816 RVA: 0x00024E5F File Offset: 0x0002305F
		// (set) Token: 0x06000331 RID: 817 RVA: 0x00024E67 File Offset: 0x00023067
		public int FallDamageMultiplierWoodSpikes { get; set; } = 25;

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00024E70 File Offset: 0x00023070
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00024E78 File Offset: 0x00023078
		public int FallDamageMultiplierMetalSpikes { get; set; } = 80;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00024E81 File Offset: 0x00023081
		// (set) Token: 0x06000335 RID: 821 RVA: 0x00024E89 File Offset: 0x00023089
		public int FishBasketCatchPercent { get; set; } = 5;

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000336 RID: 822 RVA: 0x00024E92 File Offset: 0x00023092
		// (set) Token: 0x06000337 RID: 823 RVA: 0x00024E9A File Offset: 0x0002309A
		public int FishBasketBaitedCatchPercent { get; set; } = 10;

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000338 RID: 824 RVA: 0x00024EA3 File Offset: 0x000230A3
		// (set) Token: 0x06000339 RID: 825 RVA: 0x00024EAB File Offset: 0x000230AB
		public int FishBasketBaitStolenPercent { get; set; } = 5;

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600033A RID: 826 RVA: 0x00024EB4 File Offset: 0x000230B4
		// (set) Token: 0x0600033B RID: 827 RVA: 0x00024EBC File Offset: 0x000230BC
		public int FishBasketEscapePercent { get; set; } = 15;

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00024EC5 File Offset: 0x000230C5
		// (set) Token: 0x0600033D RID: 829 RVA: 0x00024ECD File Offset: 0x000230CD
		public double FishBasketUpdateMinutes { get; set; } = 2.2;

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00024ED6 File Offset: 0x000230D6
		// (set) Token: 0x0600033F RID: 831 RVA: 0x00024EDE File Offset: 0x000230DE
		public int FishBasketRotRemovedPercent { get; set; } = 10;

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000340 RID: 832 RVA: 0x00024EE7 File Offset: 0x000230E7
		// (set) Token: 0x06000341 RID: 833 RVA: 0x00024EEF File Offset: 0x000230EF
		public int FishChanceOfEggsPercent { get; set; } = 20;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00024EF8 File Offset: 0x000230F8
		// (set) Token: 0x06000343 RID: 835 RVA: 0x00024F00 File Offset: 0x00023100
		public int FishChunkDepletionRate { get; set; } = 10;

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00024F09 File Offset: 0x00023109
		// (set) Token: 0x06000345 RID: 837 RVA: 0x00024F11 File Offset: 0x00023111
		public int FishChunkRepletionRate { get; set; } = 1;

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00024F1A File Offset: 0x0002311A
		// (set) Token: 0x06000347 RID: 839 RVA: 0x00024F22 File Offset: 0x00023122
		public int FishChunkRepletionMinutes { get; set; } = 20;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00024F2B File Offset: 0x0002312B
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00024F33 File Offset: 0x00023133
		public int FishEggsChunkRepletionRate { get; set; } = 10;

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00024F3C File Offset: 0x0002313C
		// (set) Token: 0x0600034B RID: 843 RVA: 0x00024F44 File Offset: 0x00023144
		public int FishChunkMaxDepletionPercent { get; set; } = 95;

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00024F4D File Offset: 0x0002314D
		// (set) Token: 0x0600034D RID: 845 RVA: 0x00024F55 File Offset: 0x00023155
		public int FurrowedLandUpdateFrequency { get; set; } = 120;

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00024F5E File Offset: 0x0002315E
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00024F66 File Offset: 0x00023166
		public double FurrowedLandBlockageChancePercent { get; set; } = 0.05;

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00024F6F File Offset: 0x0002316F
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00024F77 File Offset: 0x00023177
		public bool FurrowedLandEnabled { get; set; } = true;

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00024F80 File Offset: 0x00023180
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00024F88 File Offset: 0x00023188
		public double FurrowedLandMinMoistureClose { get; set; } = 0.85;

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00024F91 File Offset: 0x00023191
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00024F99 File Offset: 0x00023199
		public double FurrowedLandMinMoistureFar { get; set; } = 0.6;

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00024FA2 File Offset: 0x000231A2
		// (set) Token: 0x06000357 RID: 855 RVA: 0x00024FAA File Offset: 0x000231AA
		public int LimbTrotlineCatchPercent { get; set; } = 1;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00024FB3 File Offset: 0x000231B3
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00024FBB File Offset: 0x000231BB
		public int LimbTrotlineBaitedCatchPercent { get; set; } = 3;

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600035A RID: 858 RVA: 0x00024FC4 File Offset: 0x000231C4
		// (set) Token: 0x0600035B RID: 859 RVA: 0x00024FCC File Offset: 0x000231CC
		public int LimbTrotlineLuredCatchPercent { get; set; } = 3;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00024FD5 File Offset: 0x000231D5
		// (set) Token: 0x0600035D RID: 861 RVA: 0x00024FDD File Offset: 0x000231DD
		public int LimbTrotlineBaitedLuredCatchPercent { get; set; } = 6;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00024FE6 File Offset: 0x000231E6
		// (set) Token: 0x0600035F RID: 863 RVA: 0x00024FEE File Offset: 0x000231EE
		public int LimbTrotlineBaitStolenPercent { get; set; } = 5;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00024FF7 File Offset: 0x000231F7
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00024FFF File Offset: 0x000231FF
		public double LimbTrotlineUpdateMinutes { get; set; } = 2.4;

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00025008 File Offset: 0x00023208
		// (set) Token: 0x06000363 RID: 867 RVA: 0x00025010 File Offset: 0x00023210
		public int LimbTrotlineRotRemovedPercent { get; set; } = 10;

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00025019 File Offset: 0x00023219
		// (set) Token: 0x06000365 RID: 869 RVA: 0x00025021 File Offset: 0x00023221
		public int MonkeyBridgeMaxLength { get; set; } = 120;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0002502A File Offset: 0x0002322A
		// (set) Token: 0x06000367 RID: 871 RVA: 0x00025032 File Offset: 0x00023232
		public bool RaftEnabled { get; set; } = true;

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0002503B File Offset: 0x0002323B
		// (set) Token: 0x06000369 RID: 873 RVA: 0x00025043 File Offset: 0x00023243
		public float RaftWaterSpeedModifier { get; set; } = 0.1f;

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600036A RID: 874 RVA: 0x0002504C File Offset: 0x0002324C
		// (set) Token: 0x0600036B RID: 875 RVA: 0x00025054 File Offset: 0x00023254
		public float RaftFlotationModifier { get; set; } = 0.025f;

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0002505D File Offset: 0x0002325D
		// (set) Token: 0x0600036D RID: 877 RVA: 0x00025065 File Offset: 0x00023265
		public bool ShowModNameInHud { get; set; } = true;

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0002506E File Offset: 0x0002326E
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00025076 File Offset: 0x00023276
		public bool ShowModNameInGuis { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0002507F File Offset: 0x0002327F
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00025087 File Offset: 0x00023287
		public int SnareBaitStolenPercent { get; set; } = 10;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00025090 File Offset: 0x00023290
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00025098 File Offset: 0x00023298
		public float SnareMaxAnimalHeight { get; set; } = 0.8f;

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000250A1 File Offset: 0x000232A1
		// (set) Token: 0x06000375 RID: 885 RVA: 0x000250A9 File Offset: 0x000232A9
		public int SnareMaxDamageSet { get; set; } = 12;

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000376 RID: 886 RVA: 0x000250B2 File Offset: 0x000232B2
		// (set) Token: 0x06000377 RID: 887 RVA: 0x000250BA File Offset: 0x000232BA
		public int SnareMaxDamageBaited { get; set; } = 24;

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000378 RID: 888 RVA: 0x000250C3 File Offset: 0x000232C3
		// (set) Token: 0x06000379 RID: 889 RVA: 0x000250CB File Offset: 0x000232CB
		public int SnareTrippedPercent { get; set; } = 10;

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600037A RID: 890 RVA: 0x000250D4 File Offset: 0x000232D4
		// (set) Token: 0x0600037B RID: 891 RVA: 0x000250DC File Offset: 0x000232DC
		public double SpawnMultiplierBioluminescentGlobe { get; set; } = 1.0;

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600037C RID: 892 RVA: 0x000250E5 File Offset: 0x000232E5
		// (set) Token: 0x0600037D RID: 893 RVA: 0x000250ED File Offset: 0x000232ED
		public double SpawnMultiplierBioluminescentJelly { get; set; } = 1.0;

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600037E RID: 894 RVA: 0x000250F6 File Offset: 0x000232F6
		// (set) Token: 0x0600037F RID: 895 RVA: 0x000250FE File Offset: 0x000232FE
		public double SpawnMultiplierBioluminescentOrangeJelly { get; set; } = 1.0;

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00025107 File Offset: 0x00023307
		// (set) Token: 0x06000381 RID: 897 RVA: 0x0002510F File Offset: 0x0002330F
		public double SpawnMultiplierBioluminescentWorm { get; set; } = 1.0;

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00025118 File Offset: 0x00023318
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00025120 File Offset: 0x00023320
		public double SpawnMultiplierCrabBairdi { get; set; } = 1.0;

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00025129 File Offset: 0x00023329
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00025131 File Offset: 0x00023331
		public double SpawnMultiplierCrabLand { get; set; } = 1.0;

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0002513A File Offset: 0x0002333A
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00025142 File Offset: 0x00023342
		public double SpawnMultiplierLivingDead { get; set; } = 1.0;

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0002514B File Offset: 0x0002334B
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00025153 File Offset: 0x00023353
		public double SpawnMultiplierSnakeBlackRat { get; set; } = 1.0;

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0002515C File Offset: 0x0002335C
		// (set) Token: 0x0600038B RID: 907 RVA: 0x00025164 File Offset: 0x00023364
		public double SpawnMultiplierSnakeChainViper { get; set; } = 1.0;

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0002516D File Offset: 0x0002336D
		// (set) Token: 0x0600038D RID: 909 RVA: 0x00025175 File Offset: 0x00023375
		public double SpawnMultiplierSnakeCoachWhip { get; set; } = 1.0;

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0002517E File Offset: 0x0002337E
		// (set) Token: 0x0600038F RID: 911 RVA: 0x00025186 File Offset: 0x00023386
		public double SpawnMultiplierSnakePitViper { get; set; } = 1.0;

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0002518F File Offset: 0x0002338F
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00025197 File Offset: 0x00023397
		public double SpawnMultiplierWillowispGreen { get; set; } = 1.0;

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000392 RID: 914 RVA: 0x000251A0 File Offset: 0x000233A0
		// (set) Token: 0x06000393 RID: 915 RVA: 0x000251A8 File Offset: 0x000233A8
		public double SpawnMultiplierWillowispWhite { get; set; } = 1.0;

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000394 RID: 916 RVA: 0x000251B1 File Offset: 0x000233B1
		// (set) Token: 0x06000395 RID: 917 RVA: 0x000251B9 File Offset: 0x000233B9
		public double SpawnMultiplierWillowispYellow { get; set; } = 1.0;

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000396 RID: 918 RVA: 0x000251C2 File Offset: 0x000233C2
		// (set) Token: 0x06000397 RID: 919 RVA: 0x000251CA File Offset: 0x000233CA
		public int TreeHollowsMaxItems { get; set; } = 8;

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000398 RID: 920 RVA: 0x000251D3 File Offset: 0x000233D3
		// (set) Token: 0x06000399 RID: 921 RVA: 0x000251DB File Offset: 0x000233DB
		public bool TreeHollowsEnableDeveloperTools { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600039A RID: 922 RVA: 0x000251E4 File Offset: 0x000233E4
		// (set) Token: 0x0600039B RID: 923 RVA: 0x000251EC File Offset: 0x000233EC
		public int TreeHollowsMaxPerChunk { get; set; } = 1;

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600039C RID: 924 RVA: 0x000251F5 File Offset: 0x000233F5
		// (set) Token: 0x0600039D RID: 925 RVA: 0x000251FD File Offset: 0x000233FD
		public float TreeHollowsSpawnProbability { get; set; } = 0.15f;

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00025206 File Offset: 0x00023406
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0002520E File Offset: 0x0002340E
		public double TreeHollowsUpdateMinutes { get; set; } = 360.0;

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00025217 File Offset: 0x00023417
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0002521F File Offset: 0x0002341F
		public int WeirTrapCatchPercent { get; set; } = 2;

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00025228 File Offset: 0x00023428
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x00025230 File Offset: 0x00023430
		public int WeirTrapEscapePercent { get; set; } = 15;

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00025239 File Offset: 0x00023439
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00025241 File Offset: 0x00023441
		public double WeirTrapUpdateMinutes { get; set; } = 5.0;

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0002524A File Offset: 0x0002344A
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00025252 File Offset: 0x00023452
		public int WeirTrapRotRemovedPercent { get; set; } = 10;

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0002525B File Offset: 0x0002345B
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00025263 File Offset: 0x00023463
		public int WormFoundPercentRock { get; set; } = 1;

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0002526C File Offset: 0x0002346C
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00025274 File Offset: 0x00023474
		public int WormFoundPercentStickFlint { get; set; } = 10;
	}
}
