using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class ItemEntityMoverTool : Item, IHeldItemOnMouseWheel
{
	protected const string distanceAttribute = "distance";

	protected const string offsetAttribute = "offset";

	protected const string entityIdAttribute = "entity-id";

	protected const double mouseWheelSensitivity = 1.0;

	protected const double shiftMouseWheelSensitivity = 0.1;

	protected readonly Dictionary<long, long> affectedEntities = new Dictionary<long, long>();

	protected WorldInteraction[]? interactionHelp;

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (byEntity.Api.Side == EnumAppSide.Client)
		{
			OnHeldInteractStartClient(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		}
		else
		{
			OnHeldInteractStartServer(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (slot.Itemstack?.Item == null)
		{
			return false;
		}
		if (byEntity.Api.Side == EnumAppSide.Server)
		{
			OnHeldInteractStepServer(secondsUsed, slot, byEntity, blockSel, entitySel);
		}
		return true;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (byEntity.Api.Side == EnumAppSide.Server)
		{
			OnHeldInteractStopServer(secondsUsed, slot, byEntity, blockSel, entitySel);
		}
	}

	public void OnMouseWheel(EntityPlayer byPlayer, ItemSlot inSlot, MouseWheelEventArgs args)
	{
		if (byPlayer.Api is ICoreServerAPI serverApi)
		{
			OnMouseWheelServer(serverApi, byPlayer, inSlot, args);
		}
		else if (byPlayer.Api is ICoreClientAPI clientApi)
		{
			OnMouseWheelClient(clientApi, byPlayer, inSlot, args);
		}
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		if (interactionHelp != null)
		{
			return interactionHelp;
		}
		interactionHelp = new WorldInteraction[2]
		{
			new WorldInteraction
			{
				ActionLangCode = Lang.Get("Grab entity"),
				MouseButton = EnumMouseButton.Right
			},
			new WorldInteraction
			{
				ActionLangCode = Lang.Get("Use mouse wheel to adjust entity distance (hold 'Shift' for more precise adjustments)"),
				MouseButton = EnumMouseButton.Wheel
			}
		};
		return interactionHelp;
	}

	protected virtual void OnHeldInteractStartClient(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (entitySel?.Entity != null && slot.Itemstack != null)
		{
			handling = EnumHandHandling.PreventDefaultAction;
		}
	}

	protected virtual void OnHeldInteractStartServer(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (entitySel.Entity == null || slot.Itemstack == null)
		{
			return;
		}
		StartDisableEntityMovement(entitySel.Entity);
		handling = EnumHandHandling.PreventDefaultAction;
		double distance = byEntity.Pos.Add(byEntity.LocalEyePos.ToVec3f()).DistanceTo(entitySel.Entity.Pos.XYZ);
		slot.Itemstack.Attributes.SetDouble("distance", distance);
		Vec3d offset = entitySel.Entity.Pos.XYZ - GetViewPosition(byEntity, distance);
		slot.Itemstack.Attributes.SetDouble("offsetX", offset.X);
		slot.Itemstack.Attributes.SetDouble("offsetY", offset.Y);
		slot.Itemstack.Attributes.SetDouble("offsetZ", offset.Z);
		slot.Itemstack.Attributes.SetLong("entity-id", entitySel.Entity.EntityId);
		if (affectedEntities.TryGetValue(byEntity.EntityId, out var targetId))
		{
			Entity previousTarget = byEntity.Api.World.GetEntityById(targetId);
			if (previousTarget != null)
			{
				StopDisableEntityMovement(previousTarget);
			}
			affectedEntities.Remove(byEntity.EntityId);
		}
		affectedEntities.Add(byEntity.EntityId, entitySel.Entity.EntityId);
	}

	protected virtual void OnHeldInteractStepServer(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (slot.Itemstack != null)
		{
			double distance = slot.Itemstack.Attributes.GetDouble("distance");
			new Vec3d
			{
				X = slot.Itemstack.Attributes.GetDouble("offsetX"),
				Y = slot.Itemstack.Attributes.GetDouble("offsetY"),
				Z = slot.Itemstack.Attributes.GetDouble("offsetZ")
			};
			Vec3d targetPosition = GetViewPosition(byEntity, distance);
			byEntity.Api.World.SpawnParticles(1f, -1, targetPosition, targetPosition, new Vec3f(), new Vec3f(), 0.1f, 0f, 0.5f, EnumParticleModel.Cube);
			long entityId = slot.Itemstack.Attributes.GetLong("entity-id", 0L);
			Entity entityById = byEntity.Api.World.GetEntityById(entityId);
			entityById?.Pos.SetPos(targetPosition);
			entityById?.GetBehavior<EntityBehaviorControlledPhysics>()?.SetState(new EntityPos(targetPosition.X, targetPosition.Y, targetPosition.Z), 0f);
			entityById?.GetBehavior<EntityBehaviorTaskAI>()?.TaskManager?.StopTasks();
		}
	}

	protected virtual void OnHeldInteractStopServer(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (affectedEntities.TryGetValue(byEntity.EntityId, out var targetId))
		{
			Entity target = byEntity.Api.World.GetEntityById(targetId);
			if (target != null)
			{
				StopDisableEntityMovement(target);
			}
			affectedEntities.Remove(byEntity.EntityId);
		}
	}

	protected virtual void StartDisableEntityMovement(Entity target)
	{
		EntityBehaviorTaskAI aiTaskBehavior = target.GetBehavior<EntityBehaviorTaskAI>();
		if (aiTaskBehavior != null)
		{
			aiTaskBehavior.TaskManager.OnShouldExecuteTask += PreventAiTask;
			aiTaskBehavior.TaskManager.OnExecuteTask += PreventAiTaskExecution;
			aiTaskBehavior.TaskManager.StopTasks();
		}
		EntityBehaviorControlledPhysics physicsBehavior = target.GetBehavior<EntityBehaviorControlledPhysics>();
		if (physicsBehavior != null)
		{
			physicsBehavior.EnableModulesApplication = false;
			target.Pos.Motion.Set(0.0, 0.0, 0.0);
		}
	}

	protected virtual void StopDisableEntityMovement(Entity target)
	{
		EntityBehaviorTaskAI aiTaskBehavior = target.GetBehavior<EntityBehaviorTaskAI>();
		if (aiTaskBehavior != null)
		{
			aiTaskBehavior.TaskManager.OnShouldExecuteTask -= PreventAiTask;
			aiTaskBehavior.TaskManager.OnExecuteTask -= PreventAiTaskExecution;
		}
		EntityBehaviorControlledPhysics physicsBehavior = target.GetBehavior<EntityBehaviorControlledPhysics>();
		if (physicsBehavior != null)
		{
			physicsBehavior.EnableModulesApplication = true;
		}
	}

	protected virtual bool PreventAiTask(IAiTask task)
	{
		return false;
	}

	protected virtual bool PreventAiTaskExecution(IAiTask task, IAiTask? activeTask)
	{
		return false;
	}

	protected virtual Vec3d GetViewPosition(EntityAgent byEntity, double distance)
	{
		return byEntity.Pos.XYZ.Add(byEntity.LocalEyePos.ToVec3f()) + byEntity.Pos.GetViewVector().ToVec3d().Normalize() * distance;
	}

	protected virtual void OnMouseWheelServer(ICoreServerAPI api, EntityPlayer byPlayer, ItemSlot inSlot, MouseWheelEventArgs args)
	{
		if (inSlot.Itemstack?.Attributes != null)
		{
			args.SetHandled();
			if (byPlayer.Controls.CtrlKey)
			{
				long entityId = inSlot.Itemstack.Attributes.GetLong("entity-id", 0L);
				api.World.GetEntityById(entityId).Pos.Yaw += args.deltaPrecise / 3f;
				return;
			}
			double factor = (byPlayer.Controls.ShiftKey ? 0.1 : 1.0);
			double offset = (double)args.deltaPrecise * factor;
			double distance = inSlot.Itemstack.Attributes.GetDouble("distance");
			distance = Math.Max(distance + offset, 0.0);
			inSlot.Itemstack.Attributes.SetDouble("distance", distance);
		}
	}

	protected virtual void OnMouseWheelClient(ICoreClientAPI api, EntityPlayer byPlayer, ItemSlot inSlot, MouseWheelEventArgs args)
	{
		if (api.Input.MouseButton.Right)
		{
			args.SetHandled();
		}
	}
}
