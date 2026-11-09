using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000034 RID: 52
	public class fishDepletionProbe : CollectibleBehavior
	{
		// Token: 0x0600024C RID: 588 RVA: 0x0001B6EE File Offset: 0x000198EE
		public fishDepletionProbe(CollectibleObject collObj) : base(collObj)
		{
			this.collObj = collObj;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0001B6FE File Offset: 0x000198FE
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.SplashTime = 1f;
			this.AnimationCode = "heavyimpact";
			this.SplashingSound = AssetLocation.Create("primitivesurvival:sounds/sticksplash", this.collObj.Code.Domain);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0001B740 File Offset: 0x00019940
		private void GenerateWaterParticles(BlockPos pos, IWorldAccessor world)
		{
			Random random = new Random();
			int loc = random.Next(-2, 2);
			int loc2 = random.Next(-2, 2);
			float num = 1f;
			float maxQuantity = 1f;
			int color = ColorUtil.ToRgba(40, 165, 185, 255);
			Vec3d minPos = new Vec3d();
			Vec3d addPos = new Vec3d();
			Vec3f minVelocity = new Vec3f(0.2f, 0f, 0.2f);
			Vec3f maxVelocity = new Vec3f(0.6f, 0.4f, 0.6f);
			float lifeLength = 5f;
			float gravityEffect = -0.1f;
			float minSize = 0.1f;
			float maxSize = 0.5f;
			SimpleParticleProperties waterParticles = new SimpleParticleProperties(num, maxQuantity, color, minPos, addPos, minVelocity, maxVelocity, lifeLength, gravityEffect, minSize, maxSize, 0);
			waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(0.5, 0.2, 0.5));
			waterParticles.AddPos.Set(new Vec3d((double)(0.1f + (float)loc), 0.0, (double)(0f + (float)loc2)));
			waterParticles.SizeEvolve = new EvolvingNatFloat(4, 0.5f);
			waterParticles.ShouldDieInAir = true;
			waterParticles.SelfPropelled = true;
			world.SpawnParticles(waterParticles, null);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0001B884 File Offset: 0x00019A84
		private BlockSelection GetWaterAtPos(EntityAgent entity, double range)
		{
			Vec3d pos = entity.Pos.XYZ.Add(0.0, entity.LocalEyePos.Y, 0.0);
			Vec3f dir = entity.Pos.GetViewVector();
			double i = 0.0;
			while (i < range)
			{
				BlockPos checkPos = new BlockPos((int)(pos.X + (double)dir.X * i), (int)(pos.Y + (double)dir.Y * i), (int)(pos.Z + (double)dir.Z * i));
				Block b = entity.World.BlockAccessor.GetBlock(checkPos);
				if (!(b.Code.Path == "air"))
				{
					if (b.Code.Path.Contains("water"))
					{
						this.GenerateWaterParticles(checkPos, entity.World);
						return new BlockSelection
						{
							Position = checkPos,
							Face = BlockFacing.UP
						};
					}
					return null;
				}
				else
				{
					i += 0.1;
				}
			}
			return null;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0001B994 File Offset: 0x00019B94
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
		{
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer byPlayer = (entityPlayer != null) ? entityPlayer.Player : null;
			IBlockAccessor blockAccessor = byEntity.World.BlockAccessor;
			BlockSelection waterAtPos = this.GetWaterAtPos(byEntity, 5.0);
			if (waterAtPos != null)
			{
				handling = 2;
				handHandling = 4;
				if (byEntity.World.Side == 2)
				{
					byEntity.World.PlaySoundAt(this.SplashingSound, waterAtPos.Position, -0.5, byPlayer, false, 8f, 10f);
					return;
				}
			}
			else
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling, ref handling);
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0001BA28 File Offset: 0x00019C28
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
		{
			if (this.GetWaterAtPos(byEntity, 5.0) != null)
			{
				handling = 2;
				if (byEntity.World is IClientWorldAccessor)
				{
					byEntity.StartAnimation(this.AnimationCode);
				}
				return secondsUsed < this.SplashTime;
			}
			return base.OnHeldInteractStep(secondsUsed, slot, byEntity, blockSel, entitySel, ref handling);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0001BA80 File Offset: 0x00019C80
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandling handling)
		{
			byEntity.StopAnimation(this.AnimationCode);
			BlockSelection waterAtPos = this.GetWaterAtPos(byEntity, 5.0);
			if (waterAtPos == null)
			{
				base.OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel, ref handling);
				return;
			}
			handling = 2;
			if (secondsUsed < this.SplashTime - 0.05f)
			{
				return;
			}
			if (byEntity.World.Side == 2)
			{
				return;
			}
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IServerPlayer splr = ((entityPlayer != null) ? entityPlayer.Player : null) as IServerPlayer;
			if (splr == null)
			{
				return;
			}
			waterAtPos = this.GetWaterAtPos(byEntity, 5.0);
			if (waterAtPos != null)
			{
				int rate = PrimitiveSurvivalSystem.FishDepletedPercent(byEntity.Api as ICoreServerAPI, waterAtPos.Position);
				string msg = "<strong><font size=16 color=yellow>" + Lang.Get("primitivesurvival:fishingstocksat", new object[]
				{
					rate
				}) + "</font></strong>";
				splr.SendIngameError("depleted", msg, Array.Empty<object>());
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0001BB64 File Offset: 0x00019D64
		public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason, ref EnumHandling handling)
		{
			byEntity.StopAnimation(this.AnimationCode);
			return base.OnHeldInteractCancel(secondsUsed, slot, byEntity, blockSel, entitySel, cancelReason, ref handling);
		}

		// Token: 0x04000103 RID: 259
		public float SplashTime;

		// Token: 0x04000104 RID: 260
		public AssetLocation SplashingSound;

		// Token: 0x04000105 RID: 261
		public string AnimationCode;
	}
}
