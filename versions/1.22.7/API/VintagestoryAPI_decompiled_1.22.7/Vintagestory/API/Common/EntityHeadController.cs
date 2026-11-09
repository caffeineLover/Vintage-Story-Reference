using System;

namespace Vintagestory.API.Common;

public class EntityHeadController : IHeadController
{
	protected readonly EntityAgent entity;

	protected readonly IAnimationManager animationManager;

	protected readonly ElementPose headPose;

	protected readonly ElementPose neckPose;

	protected float headYOffsetFactor = 0.45f;

	protected float headZOffsetFactor = 0.35f;

	protected float neckYOffsetFactor = 0.35f;

	protected float neckZOffsetFactor = 0.4f;

	public float YawOffset { get; set; }

	public float PitchOffset { get; set; }

	public EntityHeadController(IAnimationManager animationManager, EntityAgent entity, Shape entityShape)
	{
		this.entity = entity;
		this.animationManager = animationManager;
		headPose = GetPose("Head");
		neckPose = GetPose("Neck");
	}

	/// <summary>
	/// Called from AnimationManager each frame before frame.
	/// </summary>
	/// <param name="dt"></param>
	public virtual void OnFrame(float dt)
	{
		headPose.degOffY = 0f;
		headPose.degOffZ = 0f;
		neckPose.degOffZ = 0f;
		if (entity.Pos.HeadYaw != 0f || entity.Pos.HeadPitch != 0f)
		{
			var (yOffsetDeg, zOffsetDeg) = GetOffsets(dt);
			headPose.degOffY = yOffsetDeg * headYOffsetFactor;
			headPose.degOffZ = zOffsetDeg * headZOffsetFactor;
			neckPose.degOffY = yOffsetDeg * neckYOffsetFactor;
			neckPose.degOffZ = zOffsetDeg * neckZOffsetFactor;
		}
	}

	protected virtual (float yOffsetDeg, float zOffsetDeg) GetOffsets(float dt)
	{
		return (yOffsetDeg: (entity.Pos.HeadYaw + YawOffset) * (180f / (float)Math.PI), zOffsetDeg: (entity.Pos.HeadPitch + PitchOffset) * (180f / (float)Math.PI));
	}

	protected ElementPose GetPose(string name)
	{
		return animationManager.Animator.GetPosebyName(name) ?? throw new InvalidOperationException($"[Head Controller] Entity '{entity.Code}' shape does not have '{name}' element.");
	}
}
