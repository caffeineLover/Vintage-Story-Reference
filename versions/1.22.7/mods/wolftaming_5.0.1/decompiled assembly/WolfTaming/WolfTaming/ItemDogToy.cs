using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace WolfTaming
{
	// Token: 0x02000004 RID: 4
	public class ItemDogToy : Item
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002598 File Offset: 0x00000798
		public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			if (!byEntity.Controls.TriesToMove && byEntity.Controls.Sprint)
			{
				return;
			}
			byEntity.Attributes.SetInt("aiming", 1);
			byEntity.Attributes.SetInt("aimingCancel", 0);
			byEntity.StartAnimation("aim");
			handling = 4;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025F1 File Offset: 0x000007F1
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			return true;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025F4 File Offset: 0x000007F4
		public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
		{
			byEntity.Attributes.SetInt("aiming", 0);
			byEntity.StopAnimation("aim");
			if (cancelReason != null)
			{
				byEntity.Attributes.SetInt("aimingCancel", 1);
			}
			return true;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002628 File Offset: 0x00000828
		public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			if (byEntity.Attributes.GetInt("aimingCancel", 0) == 1)
			{
				return;
			}
			byEntity.Attributes.SetInt("aiming", 0);
			byEntity.StopAnimation("aim");
			if (secondsUsed < 0.35f)
			{
				return;
			}
			ICoreClientAPI coreClientAPI = this.api as ICoreClientAPI;
			if (coreClientAPI != null)
			{
				coreClientAPI.World.AddCameraShake(0.17f);
			}
			slot.TakeOut(1);
			slot.MarkDirty();
			IPlayer byPlayer = null;
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				byPlayer = byEntity.World.PlayerByUid(player.PlayerUID);
			}
			byEntity.World.PlaySoundAt(new AssetLocation("sounds/player/throw"), byEntity, byPlayer, false, 8f, 1f);
			byEntity.World.GetEntityType(new AssetLocation("wolftaming:dogtoy"));
			float acc = 1f - byEntity.Attributes.GetFloat("aimingAccuracy", 0f);
			double rndpitch = byEntity.WatchedAttributes.GetDouble("aimingRandPitch", 1.0) * (double)acc * 0.75;
			double rndyaw = byEntity.WatchedAttributes.GetDouble("aimingRandYaw", 1.0) * (double)acc * 0.75;
			Vec3d pos = byEntity.Pos.XYZ.Add(0.0, byEntity.LocalEyePos.Y - 0.2, 0.0);
			Vec3d velocity = (pos.AheadCopy(1.0, (double)byEntity.Pos.Pitch + rndpitch, (double)byEntity.Pos.Yaw + rndyaw) - pos) * 0.65;
			Vec3d spawnPos = byEntity.Pos.BehindCopy(0.21).XYZ.Add(byEntity.LocalEyePos.X, byEntity.LocalEyePos.Y - 0.2, byEntity.LocalEyePos.Z);
			byEntity.StartAnimation("throw");
			this.NotifyDogs(byEntity.World.SpawnItemEntity(new ItemStack(this, 1), spawnPos, velocity) as EntityItem);
			if (byEntity is EntityPlayer)
			{
				this.RefillSlotIfEmpty(slot, byEntity, (ItemStack itemstack) => itemstack.Collectible is ItemDogToy);
			}
			byPlayer.Entity.World.PlaySoundAt(new AssetLocation("sounds/player/strike"), byPlayer.Entity, byPlayer, 0.9f + (float)this.api.World.Rand.NextDouble() * 0.2f, 16f, 0.5f);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000028D0 File Offset: 0x00000AD0
		private void NotifyDogs(EntityItem dogToy)
		{
			Entity[] array;
			if (dogToy == null)
			{
				array = null;
			}
			else
			{
				IWorldAccessor world = dogToy.World;
				array = ((world != null) ? world.GetEntitiesAround(dogToy.Pos.XYZ, 20f, 5f, null) : null);
			}
			Entity[] dogs = array;
			if (dogs == null)
			{
				return;
			}
			foreach (Entity entity in dogs)
			{
				AiTaskPlayFetch aiTaskPlayFetch;
				if (entity == null)
				{
					aiTaskPlayFetch = null;
				}
				else
				{
					EntityBehaviorTaskAI behavior = entity.GetBehavior<EntityBehaviorTaskAI>();
					if (behavior == null)
					{
						aiTaskPlayFetch = null;
					}
					else
					{
						AiTaskManager taskManager = behavior.TaskManager;
						aiTaskPlayFetch = ((taskManager != null) ? taskManager.GetTask<AiTaskPlayFetch>() : null);
					}
				}
				AiTaskPlayFetch task = aiTaskPlayFetch;
				if (task != null)
				{
					task.DogToy = dogToy;
				}
			}
		}
	}
}
