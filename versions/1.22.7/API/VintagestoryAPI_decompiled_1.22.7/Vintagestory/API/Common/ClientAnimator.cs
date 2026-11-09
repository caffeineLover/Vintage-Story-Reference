using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

/// <summary>
/// Syncs every frame with entity.ActiveAnimationsByAnimCode, starts and stops animations when necessary
/// and does recursive interpolation on the rotation, position and scale value for each frame, for each element and for each active element
/// this produces always correctly blended animations but is significantly more costly for the cpu when compared to the technique used by the <see cref="T:Vintagestory.API.Common.AnimatorBase" />.
/// </summary>
public class ClientAnimator : AnimatorBase
{
	[ThreadStatic]
	private static float[] ReusableIdentityMatrix;

	[ThreadStatic]
	private static float[] localTransformMatrix;

	[ThreadStatic]
	private static float[] tmpMatrix;

	public Dictionary<int, AnimationJoint> jointsById;

	protected HashSet<int> jointsDone = new HashSet<int>();

	public static int MaxConcurrentAnimations = 16;

	private int maxDepth;

	private List<ElementPose>[][] frameByDepthByAnimation;

	private List<ElementPose>[][] nextFrameTransformsByAnimation;

	private ShapeElementWeights[][][] weightsByAnimationAndElement;

	private Action<string, AnimationSound> onShouldPlaySoundListener;

	private int[] prevFrame = new int[MaxConcurrentAnimations];

	private int[] nextFrame = new int[MaxConcurrentAnimations];

	private Dictionary<string, string> eleWeights = new Dictionary<string, string>();

	public override int MaxJointId => jointsById.Count + 1;

	public static ClientAnimator CreateForEntity(Entity entity, List<ElementPose> rootPoses, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById)
	{
		if (entity is EntityAgent)
		{
			EntityAgent entityag = entity as EntityAgent;
			return new ClientAnimator(() => (double)entityag.Controls.MovespeedMultiplier * entityag.GetWalkSpeedMultiplier(), rootPoses, animations, rootElements, jointsById, entity.AnimManager.TriggerAnimationStopped, entity.AnimManager.ShouldPlaySound);
		}
		return new ClientAnimator(() => 1.0, rootPoses, animations, rootElements, jointsById, entity.AnimManager.TriggerAnimationStopped, entity.AnimManager.ShouldPlaySound);
	}

	public static ClientAnimator CreateForEntity(Entity entity, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById)
	{
		if (entity is EntityAgent)
		{
			EntityAgent entityag = entity as EntityAgent;
			return new ClientAnimator(() => (double)entityag.Controls.MovespeedMultiplier * entityag.GetWalkSpeedMultiplier(), animations, rootElements, jointsById, entity.AnimManager.TriggerAnimationStopped, entity.AnimManager.ShouldPlaySound);
		}
		return new ClientAnimator(() => 1.0, animations, rootElements, jointsById, entity.AnimManager.TriggerAnimationStopped, entity.AnimManager.ShouldPlaySound);
	}

	public ClientAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, Animation[] animations, Action<string> onAnimationStoppedListener = null, Action<string, AnimationSound> onShouldPlaySoundListener = null)
		: base(walkSpeedSupplier, animations, onAnimationStoppedListener)
	{
		this.onShouldPlaySoundListener = onShouldPlaySoundListener;
		initFields();
	}

	public ClientAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, List<ElementPose> rootPoses, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById, Action<string> onAnimationStoppedListener = null, Action<string, AnimationSound> onShouldPlaySoundListener = null)
		: base(walkSpeedSupplier, animations, onAnimationStoppedListener)
	{
		RootElements = rootElements;
		this.jointsById = jointsById;
		RootPoses = rootPoses;
		this.onShouldPlaySoundListener = onShouldPlaySoundListener;
		LoadAttachmentPoints(RootPoses);
		initFields();
		initMatrices(MaxJointId);
	}

	public ClientAnimator(WalkSpeedSupplierDelegate walkSpeedSupplier, Animation[] animations, ShapeElement[] rootElements, Dictionary<int, AnimationJoint> jointsById, Action<string> onAnimationStoppedListener = null, Action<string, AnimationSound> onShouldPlaySoundListener = null)
		: base(walkSpeedSupplier, animations, onAnimationStoppedListener)
	{
		RootElements = rootElements;
		this.jointsById = jointsById;
		RootPoses = new List<ElementPose>();
		LoadPosesAndAttachmentPoints(rootElements, RootPoses);
		this.onShouldPlaySoundListener = onShouldPlaySoundListener;
		initFields();
		initMatrices(MaxJointId);
	}

	protected virtual void initFields()
	{
		maxDepth = 2 + ((RootPoses != null) ? getMaxDepth(RootPoses, 1) : 0);
		frameByDepthByAnimation = new List<ElementPose>[maxDepth][];
		nextFrameTransformsByAnimation = new List<ElementPose>[maxDepth][];
		weightsByAnimationAndElement = new ShapeElementWeights[maxDepth][][];
		for (int i = 0; i < maxDepth; i++)
		{
			frameByDepthByAnimation[i] = new List<ElementPose>[MaxConcurrentAnimations];
			nextFrameTransformsByAnimation[i] = new List<ElementPose>[MaxConcurrentAnimations];
			weightsByAnimationAndElement[i] = new ShapeElementWeights[MaxConcurrentAnimations][];
		}
	}

	protected virtual void initMatrices(int maxJointId)
	{
		float[] identMat = AnimatorBase.identMat;
		float[] defaultPoseMatrices = new float[16 * maxJointId];
		for (int i = 0; i < defaultPoseMatrices.Length; i++)
		{
			defaultPoseMatrices[i] = identMat[i % 16];
		}
		TransformationMatricesDefaultPose = defaultPoseMatrices;
		TransformationMatrices = new float[defaultPoseMatrices.Length];
	}

	public override void ReloadAttachmentPoints()
	{
		LoadAttachmentPoints(RootPoses);
	}

	protected virtual void LoadAttachmentPoints(List<ElementPose> cachedPoses)
	{
		for (int i = 0; i < cachedPoses.Count; i++)
		{
			ElementPose elem = cachedPoses[i];
			AttachmentPoint[] attachmentPoints = elem.ForElement.AttachmentPoints;
			if (attachmentPoints != null)
			{
				foreach (AttachmentPoint apoint in attachmentPoints)
				{
					AttachmentPointByCode[apoint.Code] = new AttachmentPointAndPose
					{
						AttachPoint = apoint,
						CachedPose = elem
					};
				}
			}
			if (elem.ChildElementPoses != null)
			{
				LoadAttachmentPoints(elem.ChildElementPoses);
			}
		}
	}

	protected virtual void LoadPosesAndAttachmentPoints(ShapeElement[] elements, List<ElementPose> intoPoses)
	{
		foreach (ShapeElement elem in elements)
		{
			ElementPose pose;
			intoPoses.Add(pose = new ElementPose());
			pose.AnimModelMatrix = Mat4f.Create();
			pose.ForElement = elem;
			if (elem.AttachmentPoints != null)
			{
				for (int j = 0; j < elem.AttachmentPoints.Length; j++)
				{
					AttachmentPoint apoint = elem.AttachmentPoints[j];
					AttachmentPointByCode[apoint.Code] = new AttachmentPointAndPose
					{
						AttachPoint = apoint,
						CachedPose = pose
					};
				}
			}
			if (elem.Children != null)
			{
				pose.ChildElementPoses = new List<ElementPose>(elem.Children.Length);
				LoadPosesAndAttachmentPoints(elem.Children, pose.ChildElementPoses);
			}
		}
	}

	private int getMaxDepth(List<ElementPose> poses, int depth)
	{
		for (int i = 0; i < poses.Count; i++)
		{
			ElementPose pose = poses[i];
			if (pose.ChildElementPoses != null)
			{
				depth = getMaxDepth(pose.ChildElementPoses, depth);
			}
		}
		return depth + 1;
	}

	public override ElementPose GetPosebyName(string name, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
	{
		return getPosebyName(RootPoses, name);
	}

	private ElementPose getPosebyName(List<ElementPose> poses, string name, StringComparison stringComparison = StringComparison.InvariantCultureIgnoreCase)
	{
		for (int i = 0; i < poses.Count; i++)
		{
			ElementPose pose = poses[i];
			if (pose.ForElement.Name.Equals(name, stringComparison))
			{
				return pose;
			}
			if (pose.ChildElementPoses != null)
			{
				ElementPose foundPose = getPosebyName(pose.ChildElementPoses, name);
				if (foundPose != null)
				{
					return foundPose;
				}
			}
		}
		return null;
	}

	protected override void AnimNowActive(RunningAnimation anim, AnimationMetaData animData)
	{
		base.AnimNowActive(anim, animData);
		if (anim.Animation.PrevNextKeyFrameByFrame == null)
		{
			anim.Animation.GenerateAllFrames(RootElements, jointsById);
		}
		anim.LoadWeights(RootElements);
	}

	public override void OnFrame(Dictionary<string, AnimationMetaData> activeAnimationsByAnimCode, float dt)
	{
		for (int j = 0; j < activeAnimCount; j++)
		{
			RunningAnimation anim = CurAnims[j];
			if (anim.Animation.PrevNextKeyFrameByFrame == null && anim.Animation.KeyFrames.Length != 0)
			{
				anim.Animation.GenerateAllFrames(RootElements, jointsById);
			}
			if (anim.meta.AnimationSounds == null || anim.meta.AnimationSounds.Length == 0 || onShouldPlaySoundListener == null)
			{
				continue;
			}
			if (anim.SoundPlayedAtIteration == null)
			{
				anim.SoundPlayedAtIteration = new int[anim.meta.AnimationSounds.Length].Fill(-1);
			}
			for (int i = 0; i < anim.meta.AnimationSounds.Length; i++)
			{
				AnimationSound animsound = anim.meta.AnimationSounds[i];
				if (anim.CurrentFrame >= (float)animsound.Frame && anim.SoundPlayedAtIteration[i] != anim.Iterations && anim.Active && anim.NowPlaying)
				{
					onShouldPlaySoundListener(anim.meta.Code, animsound);
					anim.SoundPlayedAtIteration[i] = anim.Iterations;
				}
			}
		}
		base.OnFrame(activeAnimationsByAnimCode, dt);
	}

	protected override void calculateMatrices(float dt)
	{
		if (!base.CalculateMatrices)
		{
			return;
		}
		jointsDone.Clear();
		int animVersion = 0;
		for (int j = 0; j < activeAnimCount; j++)
		{
			RunningAnimation anim = CurAnims[j];
			weightsByAnimationAndElement[0][j] = anim.ElementWeights;
			animVersion = Math.Max(animVersion, anim.Animation.Version);
			AnimationFrame[] prevNextFrame = anim.Animation.PrevNextKeyFrameByFrame[(int)anim.CurrentFrame % anim.Animation.QuantityFrames];
			frameByDepthByAnimation[0][j] = prevNextFrame[0].RootElementTransforms;
			prevFrame[j] = prevNextFrame[0].FrameNumber;
			if (anim.Animation.OnAnimationEnd == EnumEntityAnimationEndHandling.Hold && (int)anim.CurrentFrame + 1 == anim.Animation.QuantityFrames)
			{
				nextFrameTransformsByAnimation[0][j] = prevNextFrame[0].RootElementTransforms;
				nextFrame[j] = prevNextFrame[0].FrameNumber;
			}
			else
			{
				nextFrameTransformsByAnimation[0][j] = prevNextFrame[1].RootElementTransforms;
				nextFrame[j] = prevNextFrame[1].FrameNumber;
			}
		}
		calculateMatrices(animVersion, dt, RootPoses, weightsByAnimationAndElement[0], ReusableIdentityMatrix ?? (ReusableIdentityMatrix = Mat4f.Create()), frameByDepthByAnimation[0], nextFrameTransformsByAnimation[0], 0);
		float[] TransformationMatrices = base.TransformationMatrices;
		if (TransformationMatrices != null)
		{
			for (int jointidMul16 = 0; jointidMul16 < TransformationMatrices.Length; jointidMul16 += 16)
			{
				if (!jointsById.ContainsKey(jointidMul16 / 16))
				{
					for (int i = 0; i < 16; i++)
					{
						TransformationMatrices[jointidMul16 + i] = AnimatorBase.identMat[i];
					}
				}
			}
		}
		foreach (KeyValuePair<string, AttachmentPointAndPose> val in AttachmentPointByCode)
		{
			float[] cachedMatrix = val.Value.CachedPose.AnimModelMatrix;
			float[] animMatrix = val.Value.AnimModelMatrix;
			for (int k = 0; k < 16; k++)
			{
				animMatrix[k] = cachedMatrix[k];
			}
		}
	}

	private void calculateMatrices(int animVersion, float dt, List<ElementPose> outFrame, ShapeElementWeights[][] weightsByAnimationAndElement, float[] modelMatrix, List<ElementPose>[] nowKeyFrameByAnimation, List<ElementPose>[] nextInKeyFrameByAnimation, int depth)
	{
		depth++;
		List<ElementPose>[] nowChildKeyFrameByAnimation = frameByDepthByAnimation[depth];
		List<ElementPose>[] nextChildKeyFrameByAnimation = nextFrameTransformsByAnimation[depth];
		ShapeElementWeights[][] childWeightsByAnimationAndElement = this.weightsByAnimationAndElement[depth];
		if (localTransformMatrix == null)
		{
			localTransformMatrix = Mat4f.Create();
		}
		for (int childPoseIndex = 0; childPoseIndex < outFrame.Count; childPoseIndex++)
		{
			ElementPose outFramePose = outFrame[childPoseIndex];
			ShapeElement elem = outFramePose.ForElement;
			outFramePose.SetMat(modelMatrix);
			Mat4f.Identity(localTransformMatrix);
			outFramePose.Clear();
			float weightSum = 0f;
			for (int animIndex = 0; animIndex < activeAnimCount; animIndex++)
			{
				RunningAnimation anim = CurAnims[animIndex];
				ShapeElementWeights sew = weightsByAnimationAndElement[animIndex][childPoseIndex];
				if (sew.BlendMode != EnumAnimationBlendMode.Add)
				{
					weightSum += sew.Weight * anim.EasingFactor;
				}
			}
			for (int i = 0; i < activeAnimCount; i++)
			{
				RunningAnimation anim2 = CurAnims[i];
				ShapeElementWeights sew2 = weightsByAnimationAndElement[i][childPoseIndex];
				anim2.CalcBlendedWeight(weightSum / sew2.Weight, sew2.BlendMode);
				ElementPose nowFramePose = nowKeyFrameByAnimation[i][childPoseIndex];
				ElementPose nextFramePose = nextInKeyFrameByAnimation[i][childPoseIndex];
				int prevFrame = this.prevFrame[i];
				int nextFrame = this.nextFrame[i];
				float keyFrameDist = ((nextFrame > prevFrame) ? (nextFrame - prevFrame) : (anim2.Animation.QuantityFrames - prevFrame + nextFrame));
				float lerp = ((anim2.CurrentFrame >= (float)prevFrame) ? (anim2.CurrentFrame - (float)prevFrame) : ((float)(anim2.Animation.QuantityFrames - prevFrame) + anim2.CurrentFrame)) / keyFrameDist;
				outFramePose.Add(nowFramePose, nextFramePose, lerp, anim2.BlendedWeight);
				nowChildKeyFrameByAnimation[i] = nowFramePose.ChildElementPoses;
				childWeightsByAnimationAndElement[i] = sew2.ChildElements;
				nextChildKeyFrameByAnimation[i] = nextFramePose.ChildElementPoses;
			}
			elem.GetLocalTransformMatrix(animVersion, localTransformMatrix, outFramePose);
			Mat4f.Mul(outFramePose.AnimModelMatrix, outFramePose.AnimModelMatrix, localTransformMatrix);
			if (TransformationMatrices != null && elem.JointId > 0 && !jointsDone.Contains(elem.JointId))
			{
				float[] tmpMatrixLocal = tmpMatrix ?? (tmpMatrix = Mat4f.Create());
				Mat4f.Mul(tmpMatrixLocal, outFramePose.AnimModelMatrix, elem.inverseModelTransform);
				int index = 16 * elem.JointId;
				float[] transformationMatrices = TransformationMatrices;
				if (index + 16 > transformationMatrices.Length)
				{
					float[] transformationMatricesDefault = TransformationMatricesDefaultPose;
					initMatrices(elem.JointId + 1);
					Array.Copy(transformationMatrices, TransformationMatrices, transformationMatrices.Length);
					Array.Copy(transformationMatricesDefault, TransformationMatricesDefaultPose, transformationMatricesDefault.Length);
					transformationMatrices = TransformationMatrices;
				}
				for (int j = 0; j < 16; j++)
				{
					transformationMatrices[index + j] = tmpMatrixLocal[j];
				}
				jointsDone.Add(elem.JointId);
			}
			if (outFramePose.ChildElementPoses != null)
			{
				calculateMatrices(animVersion, dt, outFramePose.ChildElementPoses, childWeightsByAnimationAndElement, outFramePose.AnimModelMatrix, nowChildKeyFrameByAnimation, nextChildKeyFrameByAnimation, depth);
			}
		}
	}

	public override string DumpCurrentState()
	{
		eleWeights.Clear();
		calculateMatrices(1f / 60f);
		return base.DumpCurrentState() + "\nElement weights:\n" + string.Join("\n", eleWeights.Select((KeyValuePair<string, string> x) => x.Key + ": " + x.Value));
	}
}
