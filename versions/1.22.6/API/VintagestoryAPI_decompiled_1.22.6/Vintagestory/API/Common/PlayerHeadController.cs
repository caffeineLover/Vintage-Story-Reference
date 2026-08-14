using System;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

public class PlayerHeadController : EntityHeadController
{
	protected readonly EntityPlayer entityPlayer;

	protected ICoreClientAPI clientApi;

	protected bool turnOpposite;

	protected bool rotateTpYawNow;

	protected float upperTorsoYOffsetFactor = 0.3f;

	protected float upperTorsoZOffsetFactor = 0.2f;

	protected float lowerTorsoZOffsetFactor = 0.1f;

	protected float upperFootROffsetFactor = 0.1f;

	protected float upperFootLOffsetFactor = 0.1f;

	protected float bodyFollowSpeedFactor = 1f;

	protected float bodyFollowThresholdDeg = 69f;

	protected float headYawLimitsDeg = 43f;

	protected float headPitchLimitsDeg = 69f;

	protected float headFollowEntityPitchFactor = 0.75f;

	protected readonly ElementPose upperTorsoPose;

	protected readonly ElementPose lowerTorsoPose;

	protected readonly ElementPose upperFootLPose;

	protected readonly ElementPose upperFootRPose;

	protected IClientPlayer? player => entityPlayer.Player as IClientPlayer;

	public PlayerHeadController(IAnimationManager animator, EntityPlayer entity, Shape entityShape)
		: base(animator, entity, entityShape)
	{
		entityPlayer = entity;
		clientApi = (entity.Api as ICoreClientAPI) ?? throw new InvalidOperationException("PlayerHeadController have to be created client side");
		upperTorsoPose = GetPose(entity.GetBoneName("upperTorsoBoneName", "UpperTorso"));
		lowerTorsoPose = GetPose(entity.GetBoneName("lowerTorsoBoneName", "LowerTorso"));
		upperFootRPose = GetPose(entity.GetBoneName("upperFootRBoneName", "UpperFootR"));
		upperFootLPose = GetPose(entity.GetBoneName("upperFootLBoneName", "UpperFootL"));
	}

	public override void OnFrame(float dt)
	{
		if (player == null)
		{
			return;
		}
		upperTorsoPose.degOffY = 0f;
		upperTorsoPose.degOffZ = 0f;
		lowerTorsoPose.degOffZ = 0f;
		upperFootRPose.degOffZ = 0f;
		upperFootLPose.degOffZ = 0f;
		if (!IsSelf())
		{
			base.OnFrame(dt);
			if (entity.BodyYawServer == 0f)
			{
				entity.BodyYaw = entity.Pos.Yaw;
			}
			return;
		}
		if (clientApi.Input.MouseGrabbed)
		{
			AdjustAngles(dt);
		}
		base.OnFrame(dt);
		SetTorsoOffsets(dt);
	}

	protected virtual void AdjustAngles(float dt)
	{
		if (player != null)
		{
			EnumCameraMode cameraMode = player.CameraMode;
			EnumMountAngleMode mountAngleMode = player.Entity.MountedOn?.AngleMode ?? EnumMountAngleMode.Unaffected;
			bool num = mountAngleMode == EnumMountAngleMode.Fixate || mountAngleMode == EnumMountAngleMode.FixateYaw || cameraMode == EnumCameraMode.Overhead || cameraMode == EnumCameraMode.FirstPerson;
			AdjustHeadAngles(cameraMode, dt);
			if (num)
			{
				entity.BodyYaw = entity.Pos.Yaw;
			}
			else
			{
				AdjustBodyAngles(dt);
			}
		}
	}

	protected virtual void AdjustHeadAngles(EnumCameraMode cameraMode, float dt)
	{
		float diff = GameMath.AngleRadDistance(entity.BodyYaw, entity.Pos.Yaw);
		if (Math.Abs(diff) > 1.8849558f)
		{
			turnOpposite = true;
		}
		if (turnOpposite)
		{
			if (Math.Abs(diff) < (float)Math.PI * 9f / 20f)
			{
				turnOpposite = false;
			}
			else
			{
				diff = 0f;
			}
		}
		if (clientApi.Settings.Bool["overheadLookAt"] && cameraMode == EnumCameraMode.Overhead)
		{
			TryLookIntoCamera(dt);
			return;
		}
		entity.Pos.HeadYaw += (diff - entity.Pos.HeadYaw) * dt * 6f;
		entity.Pos.HeadYaw = GameMath.Clamp(entity.Pos.HeadYaw, (0f - headYawLimitsDeg) * ((float)Math.PI / 180f), headYawLimitsDeg * ((float)Math.PI / 180f));
		entity.Pos.HeadPitch = GameMath.Clamp((entity.Pos.Pitch - (float)Math.PI) * headFollowEntityPitchFactor, (0f - headPitchLimitsDeg) * ((float)Math.PI / 180f), headPitchLimitsDeg * ((float)Math.PI / 180f));
	}

	protected virtual void TryLookIntoCamera(float dt)
	{
		float yawDistance = 0f - GameMath.AngleRadDistance(clientApi.Input.MouseYaw, entity.Pos.Yaw);
		float targetHeadYaw = (float)Math.PI + yawDistance;
		float targetPitch = GameMath.Clamp(0f - entity.Pos.Pitch - (float)Math.PI + (float)Math.PI * 2f, -1f, 0.8f);
		if (targetHeadYaw > (float)Math.PI)
		{
			targetHeadYaw -= (float)Math.PI * 2f;
		}
		float pitchOffset = 0f;
		if (targetHeadYaw < -1f || targetHeadYaw > 1f)
		{
			targetHeadYaw = 0f;
			pitchOffset = (GameMath.Clamp((entity.Pos.Pitch - (float)Math.PI) * 0.75f, -1.2f, 1.2f) - entity.Pos.HeadPitch) * dt * 6f;
		}
		else
		{
			pitchOffset = (targetPitch - entity.Pos.HeadPitch) * dt * 6f;
		}
		entity.Pos.HeadPitch += pitchOffset;
		entity.Pos.HeadYaw += (targetHeadYaw - entity.Pos.HeadYaw) * dt * 6f;
	}

	protected virtual void AdjustBodyAngles(float dt)
	{
		if (entityPlayer.Alive && player != null)
		{
			float threshold = ((player.Entity.Controls.TriesToMove || player.Entity.ServerControls.TriesToMove) ? 0.01f : (bodyFollowThresholdDeg * ((float)Math.PI / 180f)));
			if (entity.Controls.Gliding)
			{
				threshold = 0f;
			}
			float yawDistance = GameMath.AngleRadDistance(entity.BodyYaw, entity.Pos.Yaw);
			if (Math.Abs(yawDistance) > threshold || rotateTpYawNow)
			{
				float speed = 0.05f + Math.Abs(yawDistance) * 3.5f * bodyFollowSpeedFactor;
				entity.BodyYaw += GameMath.Clamp(yawDistance, (0f - dt) * speed, dt * speed);
				rotateTpYawNow = Math.Abs(yawDistance) > 0.01f;
			}
		}
	}

	protected virtual void SetTorsoOffsets(float dt)
	{
		if (IsSelfImmersiveFirstPerson())
		{
			var (yOffsetDeg, zOffsetDeg) = GetOffsets(dt);
			upperTorsoPose.degOffZ = zOffsetDeg * upperTorsoYOffsetFactor;
			upperTorsoPose.degOffY = yOffsetDeg * upperTorsoZOffsetFactor;
			lowerTorsoPose.degOffZ = zOffsetDeg * lowerTorsoZOffsetFactor;
			upperFootRPose.degOffZ = (0f - zOffsetDeg) * upperFootROffsetFactor;
			upperFootLPose.degOffZ = (0f - zOffsetDeg) * upperFootLOffsetFactor;
		}
	}

	protected bool IsSelf()
	{
		return clientApi?.World.Player.PlayerUID == player?.PlayerUID;
	}

	protected bool IsSelfImmersiveFirstPerson()
	{
		if (IsSelf())
		{
			return player?.ImmersiveFpMode ?? false;
		}
		return false;
	}
}
