using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class AiTaskTurretModeR : AiTaskShootAtEntityR
{
	protected int searchWaitMs = 2000;

	protected EnumTurretStateR currentState;

	protected IProjectile? prevProjectile;

	protected float currentStateTime;

	private AiTaskTurretModeConfig Config => GetConfig<AiTaskTurretModeConfig>();

	protected virtual bool inFiringRange
	{
		get
		{
			double range = targetEntity?.Pos.DistanceTo(entity.Pos) ?? double.MaxValue;
			if (range >= (double)Config.FiringRangeMin)
			{
				return range <= (double)Config.FiringRangeMax;
			}
			return false;
		}
	}

	protected virtual bool inSensingRange => (targetEntity?.Pos.DistanceTo(entity.Pos) ?? 3.4028234663852886E+38) <= (double)GetSeekingRange();

	protected virtual bool inAbortRange => (targetEntity?.Pos.DistanceTo(entity.Pos) ?? 3.4028234663852886E+38) <= (double)Config.AbortRange;

	public AiTaskTurretModeR(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		baseConfig = AiTaskBaseR.LoadConfig<AiTaskTurretModeConfig>(entity, taskConfig, aiConfig);
	}

	public override void AfterInitialize()
	{
		base.AfterInitialize();
		entity.AnimManager.OnAnimationStopped += AnimManager_OnAnimationStopped;
	}

	public override bool ShouldExecute()
	{
		if (base.ShouldExecute())
		{
			return !inAbortRange;
		}
		return false;
	}

	public override void StartExecute()
	{
		base.StartExecute();
		currentState = EnumTurretStateR.Idle;
		currentStateTime = 0f;
	}

	public override bool ContinueExecute(float dt)
	{
		if (!base.ContinueExecuteChecks(dt))
		{
			return false;
		}
		if (targetEntity == null)
		{
			return false;
		}
		currentStateTime += dt;
		UpdateState();
		AdjustYaw(dt);
		return currentState != EnumTurretStateR.Stop;
	}

	public override void FinishExecute(bool cancelled)
	{
		base.FinishExecute(cancelled);
		entity.StopAnimation(Config.TurretAnimation);
		entity.StopAnimation(Config.HoldAnimation);
		prevProjectile = null;
	}

	protected virtual void UpdateState()
	{
		currentState = currentState switch
		{
			EnumTurretStateR.Idle => Idle(currentState), 
			EnumTurretStateR.TurretMode => TurretMode(currentState), 
			EnumTurretStateR.TurretModeLoad => TurretModeLoad(currentState), 
			EnumTurretStateR.TurretModeHold => TurretModeHold(currentState), 
			EnumTurretStateR.TurretModeFired => TurretModeFired(currentState), 
			EnumTurretStateR.TurretModeReload => TurretModeReload(currentState), 
			EnumTurretStateR.TurretModeUnload => TurretModeUnload(currentState), 
			EnumTurretStateR.Stop => currentState, 
			_ => currentState, 
		};
	}

	protected virtual EnumTurretStateR Idle(EnumTurretStateR state)
	{
		if (inFiringRange)
		{
			entity.StartAnimation(Config.LoadAnimation);
			currentStateTime = 0f;
			return EnumTurretStateR.TurretMode;
		}
		if (inSensingRange)
		{
			entity.StartAnimation(Config.TurretAnimation);
			currentStateTime = 0f;
			return EnumTurretStateR.TurretMode;
		}
		return state;
	}

	protected virtual EnumTurretStateR TurretMode(EnumTurretStateR state)
	{
		if (!IsAnimationFinished(Config.TurretAnimation))
		{
			return state;
		}
		if (inAbortRange)
		{
			Abort();
			return state;
		}
		if (inFiringRange)
		{
			entity.StopAnimation(Config.TurretAnimation);
			entity.StartAnimation(Config.LoadFromTurretAnimation);
			if (Config.DrawSound != null)
			{
				entity.World.PlaySoundAt(Config.DrawSound, entity, null, Config.RandomizePitch, Config.SoundRange, Config.SoundVolume);
			}
			currentStateTime = 0f;
			return EnumTurretStateR.TurretModeLoad;
		}
		if (currentStateTime > 5f)
		{
			entity.StopAnimation(Config.TurretAnimation);
			return EnumTurretStateR.Stop;
		}
		return state;
	}

	protected virtual EnumTurretStateR TurretModeLoad(EnumTurretStateR state)
	{
		if (!IsAnimationFinished(Config.LoadAnimation))
		{
			return state;
		}
		entity.StartAnimation(Config.HoldAnimation);
		currentStateTime = 0f;
		return EnumTurretStateR.TurretModeHold;
	}

	protected virtual EnumTurretStateR TurretModeHold(EnumTurretStateR state)
	{
		if (inFiringRange || inAbortRange)
		{
			if ((double)currentStateTime > 1.25)
			{
				SetOrAdjustDispersion();
				ShootProjectile();
				entity.StopAnimation(Config.HoldAnimation);
				entity.StartAnimation(Config.FireAnimation);
				return EnumTurretStateR.TurretModeFired;
			}
			return state;
		}
		if (currentStateTime > 2f)
		{
			entity.StopAnimation(Config.HoldAnimation);
			entity.StartAnimation(Config.UnloadAnimation);
			return EnumTurretStateR.TurretModeUnload;
		}
		return state;
	}

	protected virtual EnumTurretStateR TurretModeFired(EnumTurretStateR state)
	{
		if (targetEntity == null)
		{
			stopTask = true;
			return EnumTurretStateR.Stop;
		}
		float range = Config.SeekingRange;
		if (inAbortRange || !targetEntity.Alive || (targetEntity is EntityPlayer player && !TargetablePlayerMode(player)) || !HasDirectContact(targetEntity, range, range / 2f))
		{
			Abort();
			return state;
		}
		if (inSensingRange)
		{
			entity.StartAnimation(Config.ReloadAnimation);
			if (Config.ReloadSound != null)
			{
				entity.World.PlaySoundAt(Config.ReloadSound, entity, null, Config.RandomizePitch, Config.SoundRange, Config.SoundVolume);
			}
			return EnumTurretStateR.TurretModeReload;
		}
		return state;
	}

	protected virtual EnumTurretStateR TurretModeReload(EnumTurretStateR state)
	{
		if (!IsAnimationFinished(Config.ReloadAnimation))
		{
			return state;
		}
		if (inAbortRange)
		{
			Abort();
			return state;
		}
		if (Config.DrawSound != null)
		{
			entity.World.PlaySoundAt(Config.DrawSound, entity, null, Config.RandomizePitch, Config.SoundRange, Config.SoundVolume);
		}
		return EnumTurretStateR.TurretModeLoad;
	}

	protected virtual EnumTurretStateR TurretModeUnload(EnumTurretStateR state)
	{
		if (!IsAnimationFinished(Config.UnloadAnimation))
		{
			return state;
		}
		return EnumTurretStateR.Stop;
	}

	protected virtual void Abort()
	{
		currentState = EnumTurretStateR.Stop;
		entity.StopAnimation(Config.HoldAnimation);
		entity.StopAnimation(Config.TurretAnimation);
		AiTaskManager taskManager = entity.GetBehavior<EntityBehaviorTaskAI>()?.TaskManager ?? throw new InvalidOperationException("Failed to get task manager");
		AiTaskStayInRangeR stayInRangeTask = taskManager.GetTask<AiTaskStayInRangeR>();
		if (stayInRangeTask != null && targetEntity != null)
		{
			stayInRangeTask.TargetEntity = targetEntity;
			taskManager.ExecuteTask<AiTaskStayInRangeR>();
		}
	}

	protected virtual bool IsAnimationFinished(string animationCode)
	{
		RunningAnimation animation = entity.AnimManager.GetAnimationState(animationCode);
		if (animation == null)
		{
			return false;
		}
		if (animation.Running)
		{
			return animation.AnimProgress >= Config.FinishedAnimationProgress;
		}
		return true;
	}

	protected virtual void AnimManager_OnAnimationStopped(string anim)
	{
		if (active && targetEntity != null)
		{
			UpdateState();
		}
	}
}
