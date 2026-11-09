using System;
using ProtoBuf;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[ProtoContract]
public class ClothPoint
{
	public static bool PushingPhysics;

	[ProtoMember(1)]
	public int PointIndex;

	[ProtoMember(2)]
	public float Mass;

	[ProtoMember(3)]
	public float InvMass;

	[ProtoMember(4)]
	public Vec3d Pos;

	[ProtoMember(5)]
	public Vec3f Velocity = new Vec3f();

	[ProtoMember(6)]
	public Vec3f Tension = new Vec3f();

	[ProtoMember(7)]
	private float GravityStrength = 1f;

	[ProtoMember(8)]
	private bool pinned;

	[ProtoMember(9)]
	public long pinnedToEntityId;

	[ProtoMember(10)]
	private BlockPos pinnedToBlockPos;

	[ProtoMember(11)]
	public Vec3f pinnedToOffset;

	[ProtoMember(12)]
	private float pinnedToOffsetStartYaw;

	[ProtoMember(13)]
	private string pinnedToPlayerUid;

	[ProtoMember(14)]
	public bool NoAttachTransform;

	public EnumCollideFlags CollideFlags;

	public float YCollideRestMul;

	private Vec4f tmpvec = new Vec4f();

	private ClothSystem cs;

	private Entity pinnedTo;

	private Matrixf pinOffsetTransform;

	public Vec3d TensionDirection = new Vec3d();

	public double extension;

	private float dampFactor = 0.9f;

	private float accum1s;

	public bool Dirty { get; internal set; }

	public Entity PinnedToEntity => pinnedTo;

	public BlockPos PinnedToBlockPos => pinnedToBlockPos;

	public bool Pinned => pinned;

	public ClothPoint(ClothSystem cs)
	{
		this.cs = cs;
		Pos = new Vec3d();
		init();
	}

	protected ClothPoint()
	{
	}

	public ClothPoint(ClothSystem cs, int pointIndex, double x, double y, double z)
	{
		this.cs = cs;
		PointIndex = pointIndex;
		Pos = new Vec3d(x, y, z);
		init();
	}

	public void setMass(float mass)
	{
		Mass = mass;
		InvMass = 1f / mass;
	}

	private void init()
	{
		setMass(1f);
	}

	public void PinTo(Entity toEntity, Vec3f pinOffset)
	{
		pinned = true;
		pinnedTo = toEntity;
		pinnedToEntityId = toEntity.EntityId;
		pinnedToOffset = pinOffset;
		pinnedToOffsetStartYaw = toEntity.Pos.Yaw;
		pinOffsetTransform = Matrixf.Create();
		pinnedToBlockPos = null;
		if (toEntity is EntityPlayer eplr)
		{
			pinnedToPlayerUid = eplr.PlayerUID;
		}
		MarkDirty();
	}

	public void PinTo(BlockPos blockPos, Vec3f offset)
	{
		pinnedToBlockPos = blockPos;
		pinnedToOffset = offset;
		pinnedToPlayerUid = null;
		pinned = true;
		Pos.Set(pinnedToBlockPos).Add(pinnedToOffset);
		pinnedTo = null;
		pinnedToEntityId = 0L;
		MarkDirty();
	}

	public void UnPin()
	{
		pinned = false;
		pinnedTo = null;
		pinnedToPlayerUid = null;
		pinnedToEntityId = 0L;
		MarkDirty();
	}

	public void MarkDirty()
	{
		Dirty = true;
	}

	public void update(float dt, IWorldAccessor world)
	{
		if (pinnedTo == null && pinnedToPlayerUid != null)
		{
			EntityPlayer eplr = world.PlayerByUid(pinnedToPlayerUid)?.Entity;
			if (eplr?.World != null)
			{
				PinTo(eplr, pinnedToOffset);
			}
		}
		if (pinned)
		{
			if (pinnedTo != null)
			{
				Entity pinnedToMounted = pinnedTo;
				pinnedToMounted = (pinnedToMounted as EntityAgent)?.MountedOn?.Entity ?? pinnedToMounted;
				if (pinnedToMounted.ShouldDespawn)
				{
					EntityDespawnData despawnReason = pinnedToMounted.DespawnReason;
					if (despawnReason == null || despawnReason.Reason != EnumDespawnReason.Unload)
					{
						UnPin();
						return;
					}
				}
				float weight = pinnedToMounted.Properties.Weight;
				float counterTensionStrength = GameMath.Clamp(50f / weight, 0.1f, 2f);
				EntityAgent obj = pinnedToMounted as EntityAgent;
				int num;
				if (obj == null || !obj.Controls.Sneak)
				{
					if (pinnedToMounted is EntityPlayer)
					{
						IAnimationManager animManager = pinnedToMounted.AnimManager;
						num = (((animManager != null && animManager.IsAnimationActive("sit")) || (pinnedToMounted.AnimManager?.IsAnimationActive("sleep") ?? false)) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 1;
				}
				bool extraResist = (byte)num != 0;
				float tensionResistStrength = weight / 10f * (float)((!extraResist) ? 1 : 200);
				EntityPlayer eplr2 = pinnedTo as EntityPlayer;
				EntityAgent eagent = pinnedTo as EntityAgent;
				AttachmentPointAndPose apap = eplr2?.AnimManager?.Animator?.GetAttachmentPointPose("RightHand");
				if (apap == null)
				{
					apap = pinnedTo?.AnimManager?.Animator?.GetAttachmentPointPose("rope");
				}
				Vec4f outvec;
				if (apap == null || NoAttachTransform)
				{
					pinOffsetTransform.Identity();
					if (!NoAttachTransform)
					{
						pinOffsetTransform.RotateY(pinnedTo.Pos.Yaw - pinnedToOffsetStartYaw);
					}
					tmpvec.Set(pinnedToOffset.X, pinnedToOffset.Y, pinnedToOffset.Z, 1f);
					outvec = pinOffsetTransform.TransformVector(tmpvec);
				}
				else
				{
					Matrixf modelmat = new Matrixf();
					if (eplr2 != null)
					{
						modelmat.RotateY(eagent.BodyYaw + (float)Math.PI / 2f);
					}
					else
					{
						modelmat.RotateY(pinnedTo.Pos.Yaw + (float)Math.PI / 2f);
					}
					modelmat.Translate(-0.5, 0.0, -0.5);
					apap.MulUncentered(modelmat);
					outvec = modelmat.TransformVector(new Vec4f(pinnedToOffset.X, pinnedToOffset.Y, pinnedToOffset.Z, 1f));
				}
				EntityPos pos = pinnedTo.Pos;
				Pos.Set(pos.X + (double)outvec.X, pos.Y + (double)outvec.Y, pos.Z + (double)outvec.Z);
				if (!cs.CanPull)
				{
					return;
				}
				if (extension > 0.0)
				{
					float f = counterTensionStrength * dt * 0.006f;
					Vec3d direction = TensionDirection.Clone();
					direction.Normalize();
					double tensionForce = tensionResistStrength * 1.65f;
					double gravityForce = tensionResistStrength;
					Vec3d dirXZ = new Vec3d(direction.X, 0.0, direction.Z);
					double dirXZLen = dirXZ.Length();
					if (dirXZLen > 0.001)
					{
						dirXZ /= dirXZLen;
					}
					else
					{
						dirXZ.Set(0.0, 0.0, 0.0);
					}
					_ = direction.Y;
					double pullBackDiag = Math.Sqrt(tensionForce * tensionForce + gravityForce * gravityForce);
					double velocityForce = tensionResistStrength * 1.65f;
					double extraForce = tensionForce - velocityForce;
					double num2 = velocityForce + extraForce;
					double horizontalForce = Math.Sqrt(num2 * num2 + gravityForce * gravityForce);
					double verticalForce = 0.0 - gravityForce;
					bool isGrounded = pinnedToMounted.OnGround;
					bool isTaut = extension > 0.05;
					if (isTaut && isGrounded)
					{
						Vec3d tensionDrag = new Vec3d(GameMath.Clamp(Math.Abs(TensionDirection.X) + horizontalForce - pullBackDiag, 0.0, 400.0) * (double)Math.Sign(TensionDirection.X), GameMath.Clamp(Math.Abs(TensionDirection.Y) + horizontalForce - pullBackDiag + verticalForce, 0.0, 400.0) * (double)Math.Sign(TensionDirection.Y), GameMath.Clamp(Math.Abs(TensionDirection.Z) + horizontalForce - pullBackDiag, 0.0, 400.0) * (double)Math.Sign(TensionDirection.Z)) * f;
						pinnedToMounted.Pos.Motion.Add(tensionDrag);
					}
					else if (isTaut)
					{
						Vec3d tensionDrag2 = new Vec3d(GameMath.Clamp(Math.Abs(TensionDirection.X * 0.1) + (horizontalForce - pullBackDiag) * 0.5, 0.0, 400.0) * (double)Math.Sign(TensionDirection.X), GameMath.Clamp(Math.Abs(TensionDirection.Y * 0.3) + horizontalForce - pullBackDiag + verticalForce, 0.0, 400.0) * (double)Math.Sign(TensionDirection.Y), GameMath.Clamp(Math.Abs(TensionDirection.Z * 0.1) + (horizontalForce - pullBackDiag) * 0.5, 0.0, 400.0) * (double)Math.Sign(TensionDirection.Z)) * f;
						pinnedToMounted.Pos.Motion.Add(tensionDrag2);
					}
				}
				Velocity.Set(0f, 0f, 0f);
				return;
			}
			Velocity.Set(0f, 0f, 0f);
			if (!(pinnedToBlockPos != null))
			{
				return;
			}
			accum1s += dt;
			if (accum1s >= 1f)
			{
				accum1s = 0f;
				if (!cs.api.World.BlockAccessor.GetBlock(PinnedToBlockPos).HasBehavior<BlockBehaviorRopeTieable>())
				{
					UnPin();
				}
			}
		}
		else
		{
			Vec3f vec3f = Tension.Clone();
			vec3f.Y -= GravityStrength * 10f;
			Vec3f acceleration = vec3f * InvMass;
			if (CollideFlags == (EnumCollideFlags)0)
			{
				acceleration.X += (float)cs.windSpeed.X * InvMass;
			}
			Vec3f nextVelocity = Velocity + acceleration * dt;
			nextVelocity *= dampFactor;
			float size = 0.1f;
			cs.pp.HandleBoyancy(Pos, nextVelocity, cs.boyant, GravityStrength, dt, size);
			CollideFlags = cs.pp.UpdateMotion(Pos, nextVelocity, size);
			dt *= 0.99f;
			Pos.Add(nextVelocity.X * dt, nextVelocity.Y * dt, nextVelocity.Z * dt);
			Velocity.Set(nextVelocity);
			Tension.Set(0f, 0f, 0f);
		}
	}

	public void restoreReferences(ClothSystem cs, IWorldAccessor world)
	{
		this.cs = cs;
		if (pinnedToEntityId != 0L)
		{
			pinnedTo = world.GetEntityById(pinnedToEntityId);
			if (pinnedTo != null)
			{
				PinTo(pinnedTo, pinnedToOffset);
			}
		}
		if (pinnedToBlockPos != null)
		{
			PinTo(pinnedToBlockPos, pinnedToOffset);
		}
	}

	public void restoreReferences(Entity entity)
	{
		if (pinnedToEntityId == entity.EntityId)
		{
			PinTo(entity, pinnedToOffset);
		}
	}

	public void updateFromPoint(ClothPoint point, IWorldAccessor world)
	{
		PointIndex = point.PointIndex;
		Mass = point.Mass;
		InvMass = point.InvMass;
		Pos.Set(point.Pos);
		Velocity.Set(point.Velocity);
		Tension.Set(point.Tension);
		GravityStrength = point.GravityStrength;
		pinned = point.pinned;
		pinnedToPlayerUid = point.pinnedToPlayerUid;
		pinnedToOffsetStartYaw = point.pinnedToOffsetStartYaw;
		pinnedToEntityId = point.pinnedToEntityId;
		pinnedToBlockPos = pinnedToBlockPos.SetOrCreate(point.PinnedToBlockPos);
		pinnedToOffset = pinnedToOffset.SetOrCreate(point.pinnedToOffset);
		CollideFlags = point.CollideFlags;
		YCollideRestMul = point.YCollideRestMul;
		if (pinnedToEntityId != 0L)
		{
			pinnedTo = world.GetEntityById(pinnedToEntityId);
			if (pinnedTo != null)
			{
				PinTo(pinnedTo, pinnedToOffset);
			}
			else
			{
				UnPin();
			}
		}
		else if (pinnedToBlockPos != null)
		{
			PinTo(pinnedToBlockPos, pinnedToOffset);
		}
	}
}
