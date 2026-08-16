using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x020000FD RID: 253
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageBlockEventArgs
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x000402A8 File Offset: 0x0003E4A8
		public DamageBlockEventArgs(EntityPlayer player, DamageSource damageSource, PlayerBodyPart bodyPart, EnumDamageBlockKind kind, bool mainHand, float initialDamage, float damageAfterBlock, int attackTier, int blockTier)
		{
			this.Player = player;
			this.DamageSource = damageSource;
			this.BodyPart = bodyPart;
			this.Kind = kind;
			this.MainHand = mainHand;
			this.InitialDamage = initialDamage;
			this.DamageAfterBlock = damageAfterBlock;
			this.AttackTier = attackTier;
			this.BlockTier = blockTier;
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00040300 File Offset: 0x0003E500
		public EntityPlayer Player { get; }

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x00040308 File Offset: 0x0003E508
		public DamageSource DamageSource { get; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00040310 File Offset: 0x0003E510
		public PlayerBodyPart BodyPart { get; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x00040318 File Offset: 0x0003E518
		public EnumDamageBlockKind Kind { get; }

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00040320 File Offset: 0x0003E520
		public bool MainHand { get; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00040328 File Offset: 0x0003E528
		public float InitialDamage { get; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00040330 File Offset: 0x0003E530
		public float DamageAfterBlock { get; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00040338 File Offset: 0x0003E538
		public float DamageBlocked
		{
			get
			{
				return this.InitialDamage - this.DamageAfterBlock;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00040347 File Offset: 0x0003E547
		public int AttackTier { get; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0004034F File Offset: 0x0003E54F
		public int BlockTier { get; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00040357 File Offset: 0x0003E557
		public bool FullyBlocked
		{
			get
			{
				return this.DamageAfterBlock <= 0f;
			}
		}
	}
}
