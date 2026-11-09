using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000108 RID: 264
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DamageBlockEventArgs
	{
		// Token: 0x06000D31 RID: 3377 RVA: 0x00041298 File Offset: 0x0003F498
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

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x000412F0 File Offset: 0x0003F4F0
		public EntityPlayer Player { get; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x000412F8 File Offset: 0x0003F4F8
		public DamageSource DamageSource { get; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x00041300 File Offset: 0x0003F500
		public PlayerBodyPart BodyPart { get; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00041308 File Offset: 0x0003F508
		public EnumDamageBlockKind Kind { get; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000D36 RID: 3382 RVA: 0x00041310 File Offset: 0x0003F510
		public bool MainHand { get; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x00041318 File Offset: 0x0003F518
		public float InitialDamage { get; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00041320 File Offset: 0x0003F520
		public float DamageAfterBlock { get; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00041328 File Offset: 0x0003F528
		public float DamageBlocked
		{
			get
			{
				return this.InitialDamage - this.DamageAfterBlock;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x00041337 File Offset: 0x0003F537
		public int AttackTier { get; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0004133F File Offset: 0x0003F53F
		public int BlockTier { get; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000D3C RID: 3388 RVA: 0x00041347 File Offset: 0x0003F547
		public bool FullyBlocked
		{
			get
			{
				return this.DamageAfterBlock <= 0f;
			}
		}
	}
}
