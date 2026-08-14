using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000037 RID: 55
	public class EntityFireflies : EntityGenericGlowingAgent
	{
		// Token: 0x0600025F RID: 607 RVA: 0x0001C1E1 File Offset: 0x0001A3E1
		public override void Initialize(EntityProperties properties, ICoreAPI api, long inChunkIndex3d)
		{
			base.Initialize(properties, api, inChunkIndex3d);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0001C1EC File Offset: 0x0001A3EC
		public override void OnInteract(EntityAgent byEntity, ItemSlot slot, Vec3d hitPosition, EnumInteractMode mode)
		{
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer plr = (entityPlayer != null) ? entityPlayer.Player : null;
			if (plr != null && !byEntity.World.Claims.TryAccess(plr, base.Pos.AsBlockPos, 2))
			{
				return;
			}
			if (!this.Alive || this.World.Side == 2 || mode == null)
			{
				base.OnInteract(byEntity, slot, hitPosition, mode);
				return;
			}
			string location = new AssetLocation(this.Code.Domain, this.Code.Path).ToString() + "-straight";
			Block block = byEntity.World.GetBlock(new AssetLocation(location));
			if (block != null)
			{
				ItemStack stack = new ItemStack(block, 1);
				if (!byEntity.TryGiveItemStack(stack))
				{
					byEntity.World.SpawnItemEntity(stack, base.Pos.XYZ, null);
				}
			}
			this.World.PlaySoundAt(new AssetLocation("game:sounds/effect/latch"), base.Pos.X + 0.5, base.Pos.Y + 0.5, base.Pos.Z + 0.5, null, false, 16f, 1f);
			this.Die(0, null);
		}
	}
}
