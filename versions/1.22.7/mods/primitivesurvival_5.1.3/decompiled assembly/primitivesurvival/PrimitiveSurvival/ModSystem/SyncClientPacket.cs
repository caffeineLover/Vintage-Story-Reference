using System;
using ProtoBuf;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000052 RID: 82
	[ProtoContract(ImplicitFields = 1)]
	public class SyncClientPacket
	{
		// Token: 0x04000142 RID: 322
		public int DeadfallBaitStolenPercent;

		// Token: 0x04000143 RID: 323
		public float DeadfallMaxAnimalHeight;

		// Token: 0x04000144 RID: 324
		public int DeadfallMaxDamageSet;

		// Token: 0x04000145 RID: 325
		public int DeadfallMaxDamageBaited;

		// Token: 0x04000146 RID: 326
		public int DeadfallTrippedPercent;

		// Token: 0x04000147 RID: 327
		public int FallDamageMultiplierWoodSpikes;

		// Token: 0x04000148 RID: 328
		public int FallDamageMultiplierMetalSpikes;

		// Token: 0x04000149 RID: 329
		public int FishBasketCatchPercent;

		// Token: 0x0400014A RID: 330
		public int FishBasketBaitedCatchPercent;

		// Token: 0x0400014B RID: 331
		public int FishBasketBaitStolenPercent;

		// Token: 0x0400014C RID: 332
		public int FishBasketEscapePercent;

		// Token: 0x0400014D RID: 333
		public double FishBasketUpdateMinutes;

		// Token: 0x0400014E RID: 334
		public int FishBasketRotRemovedPercent;

		// Token: 0x0400014F RID: 335
		public int FishChanceOfEggsPercent;

		// Token: 0x04000150 RID: 336
		public int FishChunkDepletionRate;

		// Token: 0x04000151 RID: 337
		public int FishChunkRepletionRate;

		// Token: 0x04000152 RID: 338
		public int FishChunkRepletionMinutes;

		// Token: 0x04000153 RID: 339
		public int FishEggsChunkRepletionRate;

		// Token: 0x04000154 RID: 340
		public int FishChunkMaxDepletionPercent;

		// Token: 0x04000155 RID: 341
		public int FurrowedLandUpdateFrequency;

		// Token: 0x04000156 RID: 342
		public double FurrowedLandBlockageChancePercent;

		// Token: 0x04000157 RID: 343
		public double FurrowedLandMinMoistureClose;

		// Token: 0x04000158 RID: 344
		public double FurrowedLandMinMoistureFar;

		// Token: 0x04000159 RID: 345
		public int LimbTrotlineCatchPercent;

		// Token: 0x0400015A RID: 346
		public int LimbTrotlineBaitedCatchPercent;

		// Token: 0x0400015B RID: 347
		public int LimbTrotlineLuredCatchPercent;

		// Token: 0x0400015C RID: 348
		public int LimbTrotlineBaitedLuredCatchPercent;

		// Token: 0x0400015D RID: 349
		public int LimbTrotlineBaitStolenPercent;

		// Token: 0x0400015E RID: 350
		public double LimbTrotlineUpdateMinutes;

		// Token: 0x0400015F RID: 351
		public int LimbTrotlineRotRemovedPercent;

		// Token: 0x04000160 RID: 352
		public int MonkeyBridgeMaxLength;

		// Token: 0x04000161 RID: 353
		public float RaftWaterSpeedModifier;

		// Token: 0x04000162 RID: 354
		public float RaftFlotationModifier;

		// Token: 0x04000163 RID: 355
		public int SnareBaitStolenPercent;

		// Token: 0x04000164 RID: 356
		public float SnareMaxAnimalHeight;

		// Token: 0x04000165 RID: 357
		public int SnareMaxDamageSet;

		// Token: 0x04000166 RID: 358
		public int SnareMaxDamageBaited;

		// Token: 0x04000167 RID: 359
		public int SnareTrippedPercent;

		// Token: 0x04000168 RID: 360
		public double SpawnMultiplierBioluminescentGlobe;

		// Token: 0x04000169 RID: 361
		public double SpawnMultiplierBioluminescentJelly;

		// Token: 0x0400016A RID: 362
		public double SpawnMultiplierBioluminescentOrangeJelly;

		// Token: 0x0400016B RID: 363
		public double SpawnMultiplierBioluminescentWorm;

		// Token: 0x0400016C RID: 364
		public double SpawnMultiplierCrabBairdi;

		// Token: 0x0400016D RID: 365
		public double SpawnMultiplierCrabLand;

		// Token: 0x0400016E RID: 366
		public double SpawnMultiplierLivingDead;

		// Token: 0x0400016F RID: 367
		public double SpawnMultiplierSnakeBlackRat;

		// Token: 0x04000170 RID: 368
		public double SpawnMultiplierSnakeChainViper;

		// Token: 0x04000171 RID: 369
		public double SpawnMultiplierSnakeCoachWhip;

		// Token: 0x04000172 RID: 370
		public double SpawnMultiplierSnakePitViper;

		// Token: 0x04000173 RID: 371
		public double SpawnMultiplierWillowispGreen;

		// Token: 0x04000174 RID: 372
		public double SpawnMultiplierWillowispWhite;

		// Token: 0x04000175 RID: 373
		public double SpawnMultiplierWillowispYellow;

		// Token: 0x04000176 RID: 374
		public int TreeHollowsMaxItems;

		// Token: 0x04000177 RID: 375
		public bool TreeHollowsEnableDeveloperTools;

		// Token: 0x04000178 RID: 376
		public int TreeHollowsMaxPerChunk;

		// Token: 0x04000179 RID: 377
		public float TreeHollowsSpawnProbability;

		// Token: 0x0400017A RID: 378
		public double TreeHollowsUpdateMinutes;

		// Token: 0x0400017B RID: 379
		public int WeirTrapCatchPercent;

		// Token: 0x0400017C RID: 380
		public int WeirTrapEscapePercent;

		// Token: 0x0400017D RID: 381
		public double WeirTrapUpdateMinutes;

		// Token: 0x0400017E RID: 382
		public int WeirTrapRotRemovedPercent;

		// Token: 0x0400017F RID: 383
		public int WormFoundPercentRock;

		// Token: 0x04000180 RID: 384
		public int WormFoundPercentStickFlint;
	}
}
