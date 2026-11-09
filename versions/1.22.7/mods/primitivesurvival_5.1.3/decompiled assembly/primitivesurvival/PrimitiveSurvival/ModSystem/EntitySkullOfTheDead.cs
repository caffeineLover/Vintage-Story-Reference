using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200003E RID: 62
	public class EntitySkullOfTheDead : EntityGenericGlowingAgent
	{
		// Token: 0x06000277 RID: 631 RVA: 0x0001CFA1 File Offset: 0x0001B1A1
		public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
		{
			base.Initialize(properties, api, InChunkIndex3d);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0001CFAC File Offset: 0x0001B1AC
		public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
		{
			if (!this.Alive || this.World.Side == 2 || mode == null)
			{
				base.OnInteract(byEntity, slot, hitPosition, mode);
				return;
			}
			Block block = byEntity.World.GetBlock(new AssetLocation("primitivesurvival:skullofthedead-normal"));
			if (block != null)
			{
				ItemStack stack = new ItemStack(block, 1);
				if (!byEntity.TryGiveItemStack(stack))
				{
					byEntity.World.SpawnItemEntity(stack, base.Pos.XYZ, null);
				}
			}
			this.Die(0, null);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0001D02A File Offset: 0x0001B22A
		public override bool ShouldReceiveDamage(DamageSource damageSource, float damage)
		{
			return false;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0001D030 File Offset: 0x0001B230
		public override void OnGameTick(float dt)
		{
			base.OnGameTick(dt);
			if (this.Api.Side == 1)
			{
				int num = this.cnt;
				this.cnt = num + 1;
				if (num > 250)
				{
					this.cnt = 0;
					Entity targetEntity = this.Api.World.GetNearestEntity(base.Pos.XYZ, 15f, 5f, delegate(Entity e)
					{
						if (e == null)
						{
							return false;
						}
						if (!(e is EntityAgent))
						{
							return false;
						}
						if (!e.Alive)
						{
							return false;
						}
						string p = e.FirstCodePart(0);
						return p == "strawdummy" || (!(p == "player") && !(p == "livingdead") && !(p == "skullofthedead") && !(p == "fireflies") && !(p == "butterfly") && !(p == "nightcrawler") && !(p == "beemob"));
					});
					if (targetEntity != null && targetEntity.FirstCodePart(0) != "strawdummy")
					{
						targetEntity.ReceiveDamage(new DamageSource
						{
							SourceEntity = null,
							Type = 3
						}, 2f);
					}
				}
			}
		}

		// Token: 0x04000118 RID: 280
		private int cnt;
	}
}
