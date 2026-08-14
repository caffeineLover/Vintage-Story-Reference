using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x0200001B RID: 27
	public class ItemPetWhistle : Item
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00006754 File Offset: 0x00004954
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			if (byEntity != null)
			{
				EntityControls controls = byEntity.Controls;
				if (((controls != null) ? new bool?(controls.Sneak) : default(bool?)).GetValueOrDefault())
				{
					if (byEntity == null)
					{
						return;
					}
					ICoreAPI api = byEntity.Api;
					if (((api != null) ? new EnumAppSide?(api.Side) : default(EnumAppSide?)).GetValueOrDefault() != 2 || !(byEntity is EntityPlayer))
					{
						return;
					}
					bool flag;
					if (entitySel == null)
					{
						flag = true;
					}
					else
					{
						Entity entity = entitySel.Entity;
						flag = !((entity != null) ? new bool?(entity.HasBehavior<EntityBehaviorTameable>()) : default(bool?)).GetValueOrDefault();
					}
					if (flag)
					{
						new TaskSelectionGui(byEntity.Api as ICoreClientAPI, byEntity as EntityPlayer, null).TryOpen();
						return;
					}
					return;
				}
			}
			handling = 2;
			IAnimationManager animManager = byEntity.AnimManager;
			if (animManager != null)
			{
				animManager.StartAnimation("eat");
			}
			ICoreAPI api2 = byEntity.Api;
			if (api2 != null && api2.Side == 1)
			{
				IWorldAccessor world = byEntity.World;
				if (world != null)
				{
					world.PlaySoundAt(new AssetLocation("petai:sounds/whistling.ogg"), byEntity.Pos.X, byEntity.Pos.Y, byEntity.Pos.Z, null, true, 32f, 1f);
				}
			}
			this.NotifyNearbyPets(byEntity);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000068A8 File Offset: 0x00004AA8
		private void NotifyNearbyPets(EntityAgent byEntity)
		{
			EntityBehaviorGiveCommand giveBehavior = byEntity.GetBehavior<EntityBehaviorGiveCommand>();
			if (giveBehavior == null)
			{
				return;
			}
			Command command = giveBehavior.ActiveCommand;
			if (command == null)
			{
				return;
			}
			Entity[] entitiesAround = byEntity.World.GetEntitiesAround(byEntity.Pos.XYZ, 15f, 5f, (Entity entity) => entity.HasBehavior<EntityBehaviorReceiveCommand>());
			Entity target = null;
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null && command.CommandName == "settarget")
			{
				EntitySelection entitySel = null;
				BlockSelection blockSel = null;
				Vec3d pos = player.Pos.XYZ.Add(player.LocalEyePos);
				player.World.RayTraceForSelection(pos, player.Pos.Pitch, player.Pos.Yaw, 50f, ref blockSel, ref entitySel, null, null);
				string text;
				if (entitySel == null)
				{
					text = null;
				}
				else
				{
					Entity entity3 = entitySel.Entity;
					if (entity3 == null)
					{
						text = null;
					}
					else
					{
						EntityBehaviorTameable behavior = entity3.GetBehavior<EntityBehaviorTameable>();
						text = ((behavior != null) ? behavior.OwnerId : null);
					}
				}
				if (text != player.PlayerUID)
				{
					giveBehavior.Victim = ((entitySel != null) ? entitySel.Entity : null);
					target = ((entitySel != null) ? entitySel.Entity : null);
				}
			}
			if (command.CommandName == "removetarget")
			{
				giveBehavior.Victim = null;
				giveBehavior.Attacker = null;
			}
			foreach (Entity entity2 in entitiesAround)
			{
				EntityBehaviorReceiveCommand receiveBehavior = entity2.GetBehavior<EntityBehaviorReceiveCommand>();
				receiveBehavior.SetCommand(command, byEntity as EntityPlayer);
				EntityBehaviorTaskAI behavior2 = entity2.GetBehavior<EntityBehaviorTaskAI>();
				AiTaskManager taskManager = (behavior2 != null) ? behavior2.TaskManager : null;
				AiTaskPetSeekEntity seekTask = (taskManager != null) ? taskManager.GetTask<AiTaskPetSeekEntity>() : null;
				AiTaskPetMeleeAttack attackTask = (taskManager != null) ? taskManager.GetTask<AiTaskPetMeleeAttack>() : null;
				if (target != null && seekTask != null && attackTask != null && (receiveBehavior.AggressionLevel == EnumAggressionLevel.PROTECTIVE || receiveBehavior.AggressionLevel == EnumAggressionLevel.AGGRESSIVE))
				{
					seekTask.targetEntity = target;
				}
				if (command.CommandName == "removetarget" && seekTask != null && attackTask != null)
				{
					if (taskManager != null)
					{
						taskManager.StopTask(typeof(AiTaskPetMeleeAttack));
					}
					if (taskManager != null)
					{
						taskManager.StopTask(typeof(AiTaskPetSeekEntity));
					}
					seekTask.targetEntity = null;
					seekTask.ClearAttacker();
					attackTask.targetEntity = null;
					attackTask.ClearAttacker();
				}
				foreach (IAiTask aiTask in taskManager.AllTasks)
				{
					AiTaskBaseTargetable aiTaskBaseTargetable = aiTask as AiTaskBaseTargetable;
					if (aiTaskBaseTargetable != null)
					{
						aiTaskBaseTargetable.ClearAttacker();
					}
				}
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00006B34 File Offset: 0x00004D34
		public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
		{
			IAnimationManager animManager = byEntity.AnimManager;
			if (animManager != null)
			{
				animManager.StopAnimation("PlaceBlock");
			}
			return secondsUsed < 3f;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00006B54 File Offset: 0x00004D54
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "petai:interact-whistle-select",
					HotKeyCode = "sneak",
					MouseButton = 2
				},
				new WorldInteraction
				{
					ActionLangCode = "petai:interact-whistle-command",
					MouseButton = 2
				}
			};
		}
	}
}
