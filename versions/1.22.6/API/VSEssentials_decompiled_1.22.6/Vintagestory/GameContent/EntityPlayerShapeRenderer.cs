using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class EntityPlayerShapeRenderer : EntityShapeRenderer
{
	protected MultiTextureMeshRef firstPersonMeshRef;

	protected MultiTextureMeshRef thirdPersonMeshRef;

	protected bool watcherRegistered;

	protected EntityPlayer entityPlayer;

	protected ModSystemFpHands modSys;

	protected RenderMode renderMode;

	protected bool guiAnimations;

	protected float smoothedBodyYaw;

	private bool previfpMode;

	public static ModelTransform DefaultTongTransform = new ModelTransform
	{
		Translation = new FastVec3f(-0.68f, -0.52f, -0.6f),
		Rotation = new FastVec3f(-26f, -13f, -88f),
		Origin = new FastVec3f(0.5f, 0f, 0.5f),
		Scale = 0.7f
	};

	public float? HeldItemPitchFollowOverride { get; set; }

	protected bool IsSelf => entity.EntityId == capi.World.Player.Entity.EntityId;

	public override bool DisplayChatMessages => true;

	public virtual float HandRenderFov => (float)capi.Settings.Int["fpHandsFoV"] * ((float)Math.PI / 180f);

	public EntityPlayerShapeRenderer(Entity entity, ICoreClientAPI api)
		: base(entity, api)
	{
		entityPlayer = entity as EntityPlayer;
		guiAnimations = entity.Attributes.GetBool("guiAnimations");
		modSys = api.ModLoader.GetModSystem<ModSystemFpHands>();
		api.Event.ReloadTextures += entity.MarkShapeModified;
	}

	public override void OnEntityLoaded()
	{
		base.OnEntityLoaded();
	}

	public override void TesselateShape()
	{
		if (entityPlayer.GetBehavior<EntityBehaviorPlayerInventory>().Inventory == null)
		{
			return;
		}
		defaultTexSource = GetTextureSource();
		Tesselate();
		if (watcherRegistered)
		{
			return;
		}
		previfpMode = capi.Settings.Bool["immersiveFpMode"];
		if (IsSelf)
		{
			capi.Settings.Bool.AddWatcher("immersiveFpMode", delegate(bool on)
			{
				entity.MarkShapeModified();
				(entityPlayer.AnimManager as PlayerAnimationManager).OnIfpModeChanged(previfpMode, on);
			});
		}
		watcherRegistered = true;
	}

	protected override void onMeshReady(MeshData meshData)
	{
		base.onMeshReady(meshData);
		if (!IsSelf)
		{
			thirdPersonMeshRef = meshRefOpaque;
		}
	}

	public void Tesselate()
	{
		if (!IsSelf)
		{
			base.TesselateShape();
		}
		else
		{
			if (!loaded)
			{
				return;
			}
			TesselateShape(delegate(MeshData meshData)
			{
				disposeMeshes();
				IAnimator animator = entity.AnimManager?.Animator;
				if (!capi.IsShuttingDown && animator != null && meshData.VerticesCount > 0)
				{
					MeshData meshData2 = meshData.EmptyClone();
					thirdPersonMeshRef = capi.Render.UploadMultiTextureMesh(meshData);
					determineRenderMode();
					if (renderMode == RenderMode.ImmersiveFirstPerson)
					{
						HashSet<int> skipJointIds = new HashSet<int>();
						loadJointIdsRecursive(animator.GetPosebyName("Neck"), skipJointIds);
						meshData2.AddMeshData(meshData, (int i) => !skipJointIds.Contains(meshData.CustomInts.Values[i * 4]));
					}
					else
					{
						HashSet<int> includeJointIds = new HashSet<int>();
						loadJointIdsRecursive(animator.GetPosebyName(entity.GetBoneName("upperArmRBoneName", "UpperArmR")), includeJointIds);
						loadJointIdsRecursive(animator.GetPosebyName(entity.GetBoneName("upperArmLBoneName", "UpperArmL")), includeJointIds);
						meshData2.AddMeshData(meshData, (int i) => includeJointIds.Contains(meshData.CustomInts.Values[i * 4]));
					}
					firstPersonMeshRef = capi.Render.UploadMultiTextureMesh(meshData2);
				}
			});
		}
	}

	private void loadJointIdsRecursive(ElementPose elementPose, HashSet<int> outList)
	{
		outList.Add(elementPose.ForElement.JointId);
		foreach (ElementPose childpose in elementPose.ChildElementPoses)
		{
			loadJointIdsRecursive(childpose, outList);
		}
	}

	private void disposeMeshes()
	{
		if (firstPersonMeshRef != null)
		{
			firstPersonMeshRef.Dispose();
			firstPersonMeshRef = null;
		}
		if (thirdPersonMeshRef != null)
		{
			thirdPersonMeshRef.Dispose();
			thirdPersonMeshRef = null;
		}
		if (meshRefOpaque != null)
		{
			meshRefOpaque.Dispose();
			meshRefOpaque = null;
		}
	}

	public override void BeforeRender(float dt)
	{
		RenderMode prevRenderMode = renderMode;
		determineRenderMode();
		if ((prevRenderMode == RenderMode.FirstPerson && renderMode == RenderMode.ImmersiveFirstPerson) || (prevRenderMode == RenderMode.ImmersiveFirstPerson && renderMode == RenderMode.FirstPerson))
		{
			entity.MarkShapeModified();
			(entityPlayer.AnimManager as PlayerAnimationManager).OnIfpModeChanged(previfpMode, renderMode == RenderMode.ImmersiveFirstPerson);
		}
		base.BeforeRender(dt);
	}

	private void determineRenderMode()
	{
		if (IsSelf)
		{
			IClientPlayer clientPlayer = player;
			if (clientPlayer != null && clientPlayer.CameraMode == EnumCameraMode.FirstPerson)
			{
				if (capi.Settings.Bool["immersiveFpMode"] && !capi.Render.CameraStuck)
				{
					ITreeAttribute treeAttribute = entity.WatchedAttributes.GetTreeAttribute("tiredness");
					if (treeAttribute == null || treeAttribute.GetInt("isSleeping") != 1)
					{
						renderMode = RenderMode.ImmersiveFirstPerson;
						return;
					}
				}
				renderMode = RenderMode.FirstPerson;
				return;
			}
		}
		renderMode = RenderMode.ThirdPerson;
	}

	public override void RenderToGui(float dt, double posX, double posY, double posZ, float yawDelta, float size)
	{
		if (entityPlayer.TpAnimManager?.Animator != null && entity.AnimManager?.Animator != null)
		{
			IShaderProgram currentActiveShader = capi.Render.CurrentActiveShader;
			if (capi.IsGamePaused)
			{
				entityPlayer.TpAnimManager.StopAllAnimations();
				entityPlayer.TpAnimManager.StartAnimation("idle");
				bool prevValue = entity.IsRendered;
				entity.IsRendered = true;
				entityPlayer.TpAnimManager.RunWhilePaused = true;
				entityPlayer.TpAnimManager.OnClientFrame(dt);
				entity.IsRendered = prevValue;
				entityPlayer.TpAnimManager.RunWhilePaused = false;
			}
			currentActiveShader.Uniform("applyAnimation", guiAnimations ? 1 : 0);
			currentActiveShader.UBOs["Animation"].Update((object)entityPlayer.TpAnimManager.Animator.Matrices, 0, entity.AnimManager.Animator.MaxJointId * 16 * 4);
			if (IsSelf)
			{
				meshRefOpaque = thirdPersonMeshRef;
			}
			base.RenderToGui(dt, posX, posY, posZ, yawDelta, size);
			currentActiveShader.Uniform("applyAnimation", 0);
		}
	}

	public override void DoRender2D(float dt)
	{
		if (IsSelf)
		{
			IClientPlayer clientPlayer = player;
			if (clientPlayer != null && clientPlayer.CameraMode == EnumCameraMode.FirstPerson)
			{
				return;
			}
		}
		base.DoRender2D(dt);
	}

	public override Vec3d getAboveHeadPosition(EntityPlayer entityPlayer)
	{
		if (IsSelf)
		{
			return new Vec3d(entityPlayer.CameraPos.X + entityPlayer.LocalEyePos.X, entityPlayer.CameraPos.Y + 0.4 + entityPlayer.LocalEyePos.Y, entityPlayer.CameraPos.Z + entityPlayer.LocalEyePos.Z);
		}
		return base.getAboveHeadPosition(entityPlayer);
	}

	public override void DoRender3DOpaque(float dt, bool isShadowPass)
	{
		if (IsSelf)
		{
			entityPlayer.selfNowShadowPass = isShadowPass;
		}
		bool isHandRender = renderMode == RenderMode.FirstPerson && !isShadowPass;
		loadModelMatrixForPlayer(entity, IsSelf, dt, isShadowPass);
		if (IsSelf && (renderMode == RenderMode.ImmersiveFirstPerson || isShadowPass))
		{
			OriginPos.Set(0f, 0f, 0f);
		}
		if (isHandRender && capi.HideGuis)
		{
			return;
		}
		if (isHandRender)
		{
			pMatrixNormalFov = (float[])capi.Render.CurrentProjectionMatrix.Clone();
			capi.Render.Set3DProjection(capi.Render.ShaderUniforms.ZFar, HandRenderFov);
			pMatrixHandFov = (float[])capi.Render.CurrentProjectionMatrix.Clone();
		}
		else
		{
			pMatrixHandFov = null;
			pMatrixNormalFov = null;
		}
		if (isShadowPass)
		{
			DoRender3DAfterOIT(dt, isShadowPass: true);
		}
		if (DoRenderHeldItem && !entity.AnimManager.ActiveAnimationsByAnimCode.ContainsKey("lie") && !isSpectator)
		{
			RenderHeldItem(dt, isShadowPass, right: false);
			RenderHeldItem(dt, isShadowPass, right: true);
		}
		if (isHandRender)
		{
			if (!capi.Settings.Bool["hideFpHands"] && !entityPlayer.GetBehavior<EntityBehaviorTiredness>().IsSleeping)
			{
				IShaderProgram fpModeHandShader = modSys.fpModeHandShader;
				meshRefOpaque = firstPersonMeshRef;
				fpModeHandShader.Use();
				fpModeHandShader.Uniform("rgbaAmbientIn", capi.Render.AmbientColor);
				fpModeHandShader.Uniform("rgbaFogIn", capi.Render.FogColor);
				fpModeHandShader.Uniform("fogMinIn", capi.Render.FogMin);
				fpModeHandShader.Uniform("fogDensityIn", capi.Render.FogDensity);
				fpModeHandShader.UniformMatrix("projectionMatrix", capi.Render.CurrentProjectionMatrix);
				fpModeHandShader.Uniform("alphaTest", 0.05f);
				fpModeHandShader.Uniform("lightPosition", capi.Render.ShaderUniforms.LightPosition3D);
				fpModeHandShader.Uniform("depthOffset", -0.3f - GameMath.Max(0f, (float)capi.Settings.Int["fieldOfView"] / 90f - 1f) / 2f);
				capi.Render.GlPushMatrix();
				capi.Render.GlLoadMatrix(capi.Render.CameraMatrixOrigin);
				base.DoRender3DOpaqueBatched(dt, false);
				capi.Render.GlPopMatrix();
				fpModeHandShader.Stop();
			}
			capi.Render.Reset3DProjection();
		}
	}

	protected override IShaderProgram getReadyShader()
	{
		if (!entityPlayer.selfNowShadowPass && renderMode == RenderMode.FirstPerson)
		{
			IShaderProgram fpModeItemShader = modSys.fpModeItemShader;
			fpModeItemShader.Use();
			fpModeItemShader.Uniform("depthOffset", -0.3f - GameMath.Max(0f, (float)capi.Settings.Int["fieldOfView"] / 90f - 1f) / 2f);
			fpModeItemShader.Uniform("ssaoAttn", 1f);
			return fpModeItemShader;
		}
		return base.getReadyShader();
	}

	protected override void RenderHeldItem(float dt, bool isShadowPass, bool right)
	{
		if (IsSelf)
		{
			entityPlayer.selfNowShadowPass = isShadowPass;
		}
		if (right)
		{
			ItemSlot slot = eagent?.RightHandItemSlot;
			if (slot is ItemSlotSkill)
			{
				return;
			}
			ItemStack stack = slot?.Itemstack;
			ItemStack tongStack = eagent?.LeftHandItemSlot?.Itemstack;
			if (stack != null && stack.Collectible.GetTemperature(entity.World, stack) > (float)GlobalConstants.TooHotToTouchTemperature && tongStack != null && tongStack.ItemAttributes?.IsTrue("heatResistant") == true)
			{
				AttachmentPointAndPose apap = entity.AnimManager?.Animator?.GetAttachmentPointPose("LeftHand");
				ItemRenderInfo renderInfo = capi.Render.GetItemStackRenderInfo(slot, EnumItemRenderTarget.HandTpOff, dt);
				string transformCode = tongStack?.ItemAttributes["transformCode"].AsString();
				JsonObject itemAttributes = stack.ItemAttributes;
				if (itemAttributes == null || itemAttributes[transformCode]?.Exists != true)
				{
					transformCode = "onTongTransform";
				}
				renderInfo.Transform = stack.ItemAttributes?[transformCode].AsObject(DefaultTongTransform) ?? DefaultTongTransform;
				RenderItem(dt, isShadowPass, stack, apap, renderInfo);
				return;
			}
		}
		bool ishandrender = renderMode == RenderMode.FirstPerson;
		if ((ishandrender && !capi.Settings.Bool["hideFpHands"]) || !ishandrender)
		{
			try
			{
				base.RenderHeldItem(dt, isShadowPass, right);
			}
			catch (ArgumentException)
			{
				capi.Logger.Error("Exception thrown trying to render player held item {0} on {1}", (!right) ? eagent.LeftHandItemSlot?.Itemstack : eagent.RightHandItemSlot?.Itemstack, eagent.GetName());
				throw;
			}
		}
	}

	public override void DoRender3DOpaqueBatched(float dt, bool isShadowPass)
	{
		if (renderMode != RenderMode.FirstPerson || isShadowPass)
		{
			if (isShadowPass)
			{
				meshRefOpaque = thirdPersonMeshRef;
			}
			else
			{
				meshRefOpaque = ((renderMode == RenderMode.ImmersiveFirstPerson) ? firstPersonMeshRef : thirdPersonMeshRef);
			}
			base.DoRender3DOpaqueBatched(dt, isShadowPass);
		}
	}

	public void loadModelMatrixForPlayer(Entity entity, bool isSelf, float dt, bool isShadowPass)
	{
		EntityPlayer selfEplr = capi.World.Player.Entity;
		Mat4f.Identity(ModelMat);
		if (isSelf)
		{
			Matrixf tf = selfEplr.MountedOn?.RenderTransform;
			if (tf != null)
			{
				ModelMat = Mat4f.Mul(ModelMat, ModelMat, tf.Values);
			}
		}
		else
		{
			Vec3f off = GetOtherPlayerRenderOffset();
			Mat4f.Translate(ModelMat, ModelMat, off.X, off.Y, off.Z);
			Matrixf tf2 = entityPlayer.MountedOn?.RenderTransform;
			if (tf2 != null)
			{
				ModelMat = Mat4f.Mul(ModelMat, ModelMat, tf2.Values);
			}
		}
		float rotX = ((entity.Properties.Client.Shape != null) ? entity.Properties.Client.Shape.rotateX : 0f);
		float rotY = ((entity.Properties.Client.Shape != null) ? entity.Properties.Client.Shape.rotateY : 0f);
		float rotZ = ((entity.Properties.Client.Shape != null) ? entity.Properties.Client.Shape.rotateZ : 0f);
		float mdt = Math.Min(0.05f, dt);
		if (!isSelf && entityPlayer.MountedOn?.Entity != null)
		{
			smoothedBodyYaw = (bodyYawLerped = entityPlayer.MountedOn.Entity.Pos.Yaw);
		}
		else
		{
			IClientPlayer clientPlayer = player;
			if (clientPlayer == null || clientPlayer.CameraMode != EnumCameraMode.FirstPerson)
			{
				float yawDist = GameMath.AngleRadDistance(bodyYawLerped, eagent.BodyYaw);
				bodyYawLerped += GameMath.Clamp(yawDist, (0f - mdt) * 8f, mdt * 8f);
				float bodyYaw = bodyYawLerped;
				smoothedBodyYaw = bodyYaw;
			}
			else
			{
				float bodyYaw = ((renderMode != RenderMode.ThirdPerson) ? eagent.BodyYaw : eagent.Pos.Yaw);
				if (!isShadowPass)
				{
					smoothCameraTurning(bodyYaw, mdt);
				}
			}
		}
		float bodyPitch = ((entityPlayer == null) ? 0f : entityPlayer.WalkPitch);
		float seatPitch = (entityPlayer.MountedOn?.SeatPosition?.Pitch).GetValueOrDefault();
		Mat4f.RotateX(ModelMat, ModelMat, entity.Pos.Roll + rotX * ((float)Math.PI / 180f));
		Mat4f.RotateY(ModelMat, ModelMat, smoothedBodyYaw + (90f + rotY) * ((float)Math.PI / 180f));
		if ((!isSelf || !eagent.Swimming || renderMode != RenderMode.FirstPerson) && (((selfEplr == null || !selfEplr.Controls.Gliding) && selfEplr.MountedOn == null) || renderMode != RenderMode.FirstPerson))
		{
			Mat4f.RotateZ(ModelMat, ModelMat, bodyPitch + rotZ * ((float)Math.PI / 180f));
			if (seatPitch != 0f)
			{
				Mat4f.Translate(ModelMat, ModelMat, 0f, -0.5f, 0f);
				Mat4f.RotateZ(ModelMat, ModelMat, seatPitch);
				Mat4f.Translate(ModelMat, ModelMat, 0f, 0.5f, 0f);
			}
		}
		Mat4f.RotateX(ModelMat, ModelMat, nowSwivelRad);
		if (selfEplr != null && renderMode == RenderMode.FirstPerson && !isShadowPass)
		{
			float itemSpecificPitchFollow = eagent.RightHandItemSlot?.Itemstack?.ItemAttributes?["heldItemPitchFollow"].AsFloat(0.75f) ?? 0.75f;
			float ridingSpecificPitchFollow = eagent.MountedOn?.FpHandPitchFollow ?? 1f;
			float f = ((selfEplr != null && selfEplr.Controls.IsFlying) ? 1f : (HeldItemPitchFollowOverride ?? (itemSpecificPitchFollow * ridingSpecificPitchFollow)));
			Mat4f.Translate(ModelMat, ModelMat, 0f, (float)entity.LocalEyePos.Y, 0f);
			Mat4f.RotateZ(ModelMat, ModelMat, (entity.Pos.Pitch - (float)Math.PI) * f);
			Mat4f.Translate(ModelMat, ModelMat, 0f, 0f - (float)entity.LocalEyePos.Y, 0f);
		}
		if (renderMode == RenderMode.FirstPerson && !isShadowPass)
		{
			Mat4f.Translate(ModelMat, ModelMat, 0f, capi.Settings.Float["fpHandsYOffset"], 0f);
		}
		float targetIntensity = entity.WatchedAttributes.GetFloat("intoxication");
		intoxIntensity += (targetIntensity - intoxIntensity) * dt / 3f;
		capi.Render.PerceptionEffects.ApplyToTpPlayer(entity as EntityPlayer, ModelMat, intoxIntensity);
		float scale = entity.Properties.Client.Size;
		Mat4f.Scale(ModelMat, ModelMat, new float[3] { scale, scale, scale });
		Mat4f.Translate(ModelMat, ModelMat, -0.5f, 0f, -0.5f);
	}

	private void smoothCameraTurning(float bodyYaw, float mdt)
	{
		float yawDist = GameMath.AngleRadDistance(smoothedBodyYaw, bodyYaw);
		smoothedBodyYaw += Math.Max(0f, Math.Abs(yawDist) - 0.6f) * (float)Math.Sign(yawDist);
		yawDist = GameMath.AngleRadDistance(smoothedBodyYaw, eagent.BodyYaw);
		smoothedBodyYaw += yawDist * mdt * 25f;
	}

	protected Vec3f GetOtherPlayerRenderOffset()
	{
		EntityPlayer selfEplr = capi.World.Player.Entity;
		IMountable selfMountedOn = selfEplr.MountedOn?.MountSupplier;
		IMountableSeat hisSeat = (entity as EntityAgent).MountedOn;
		IMountable heMountedOn = hisSeat?.MountSupplier;
		if (selfMountedOn != null && selfMountedOn == heMountedOn)
		{
			EntityPos selfMountPos = selfEplr.MountedOn.SeatPosition;
			EntityPos heMountPos = hisSeat.SeatPosition;
			return new Vec3f((float)(0.0 - selfMountPos.X + heMountPos.X), (float)(0.0 - selfMountPos.Y + heMountPos.Y), (float)(0.0 - selfMountPos.Z + heMountPos.Z));
		}
		if (hisSeat != null)
		{
			return new Vec3f((float)(hisSeat.SeatPosition.X - selfEplr.CameraPos.X), (float)(hisSeat.SeatPosition.InternalY - selfEplr.CameraPos.Y), (float)(hisSeat.SeatPosition.Z - selfEplr.CameraPos.Z));
		}
		return new Vec3f((float)(entity.Pos.X - selfEplr.CameraPos.X), (float)(entity.Pos.InternalY - selfEplr.CameraPos.Y), (float)(entity.Pos.Z - selfEplr.CameraPos.Z));
	}

	protected override void determineSidewaysSwivel(float dt)
	{
		if (entityPlayer.MountedOn != null)
		{
			entityPlayer.sidewaysSwivelAngle = (nowSwivelRad = 0f);
			return;
		}
		double nowAngle = Math.Atan2(entity.Pos.Motion.Z, entity.Pos.Motion.X);
		double walkspeedsq = entity.Pos.Motion.LengthSq();
		if (walkspeedsq > 0.001 && entity.OnGround)
		{
			float angledist = GameMath.AngleRadDistance((float)prevAngleSwing, (float)nowAngle);
			float num = nowSwivelRad;
			float num2 = GameMath.Clamp(angledist, -0.05f, 0.05f) * dt * 40f * (float)Math.Min(0.02500000037252903, walkspeedsq) * 80f;
			EntityAgent entityAgent = eagent;
			nowSwivelRad = num - num2 * (float)((entityAgent == null || !entityAgent.Controls.Backward) ? 1 : (-1));
			nowSwivelRad = GameMath.Clamp(nowSwivelRad, -0.3f, 0.3f);
		}
		nowSwivelRad *= Math.Min(0.99f, 1f - 0.1f * dt * 60f);
		prevAngleSwing = nowAngle;
		entityPlayer.sidewaysSwivelAngle = nowSwivelRad;
	}

	public override void Dispose()
	{
		base.Dispose();
		capi.Event.ReloadTextures -= entity.MarkShapeModified;
		firstPersonMeshRef?.Dispose();
		thirdPersonMeshRef?.Dispose();
	}
}
