using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018F RID: 399
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct PLayerKeyFrame
	{
		// Token: 0x0600136A RID: 4970 RVA: 0x0005E4E1 File Offset: 0x0005C6E1
		public PLayerKeyFrame(PlayerFrame frame, TimeSpan easingTime, EasingFunctionType easeFunction, EasingFunctionType easingType, Vector2 frameProgressRange)
		{
			this.Frame = frame;
			this.Time = easingTime;
			this.EasingFunction = easeFunction;
			this.EasingType = easingType;
			this.FrameProgressRange = frameProgressRange;
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0005E508 File Offset: 0x0005C708
		public PLayerKeyFrame(PlayerFrame frame, TimeSpan easingTime, EasingFunctionType easeFunction)
		{
			this.Frame = frame;
			this.Time = easingTime;
			this.EasingFunction = easeFunction;
			this.EasingType = easeFunction;
			this.FrameProgressRange = new Vector2(0f, 1f);
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0005E53C File Offset: 0x0005C73C
		public PlayerFrame Interpolate(PlayerFrame frame, float frameProgress)
		{
			float interpolatedProgress = this.GetInterpolatedProgress(frameProgress);
			return PlayerFrame.Interpolate(frame, this.Frame, interpolatedProgress);
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0005E55E File Offset: 0x0005C75E
		public bool Reached(TimeSpan currentDuration)
		{
			return currentDuration >= this.Time;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0005E56C File Offset: 0x0005C76C
		public unsafe static List<PLayerKeyFrame> FromVanillaAnimation(Animation animation, out bool hasPlayerFrames)
		{
			hasPlayerFrames = false;
			if (animation.KeyFrames == null || animation.KeyFrames.Length == 0)
			{
				int i = 1;
				List<PLayerKeyFrame> list = new List<PLayerKeyFrame>(i);
				CollectionsMarshal.SetCount<PLayerKeyFrame>(list, i);
				*CollectionsMarshal.AsSpan<PLayerKeyFrame>(list)[0] = PLayerKeyFrame.Zero;
				return list;
			}
			AnimationKeyFrame[] vanillaKeyFrames = Enumerable.ToArray<AnimationKeyFrame>(Enumerable.OrderBy<AnimationKeyFrame, int>(animation.KeyFrames, (AnimationKeyFrame keyFrame) => keyFrame.Frame));
			HashSet<string> playerElements = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (AnimationKeyFrame keyFrame2 in vanillaKeyFrames)
			{
				if (keyFrame2.Elements != null)
				{
					using (Dictionary<string, AnimationKeyFrameElement>.KeyCollection.Enumerator enumerator = keyFrame2.Elements.Keys.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string elementName = enumerator.Current;
							string canonicalName = Enumerable.FirstOrDefault<string>(PLayerKeyFrame.PlayerElementNames, (string name) => string.Equals(name, elementName, 5));
							if (canonicalName != null)
							{
								playerElements.Add(canonicalName);
							}
						}
					}
				}
			}
			AnimationKeyFrame[] vanillaKeyFrames2 = vanillaKeyFrames;
			TimeSpan duration = PLayerKeyFrame.FromVanillaFrameNumber(vanillaKeyFrames2[vanillaKeyFrames2.Length - 1].Frame);
			if (playerElements.Count == 0)
			{
				return PLayerKeyFrame.CreateDurationAnchorFrames(duration);
			}
			hasPlayerFrames = true;
			Dictionary<string, List<AnimationElement>> frames = Enumerable.ToDictionary<string, string, List<AnimationElement>>(playerElements, (string element) => element, (string element) => ItemKeyFrame.GetElementFrames(element, vanillaKeyFrames));
			List<PLayerKeyFrame> result = new List<PLayerKeyFrame>();
			if (vanillaKeyFrames[0].Frame != 0)
			{
				result.Add(new PLayerKeyFrame(PLayerKeyFrame.BuildPlayerFrame(frames, 0), TimeSpan.Zero, EasingFunctionType.Linear));
			}
			for (int index = 0; index < vanillaKeyFrames.Length; index++)
			{
				result.Add(new PLayerKeyFrame(PLayerKeyFrame.BuildPlayerFrame(frames, index), PLayerKeyFrame.FromVanillaFrameNumber(vanillaKeyFrames[index].Frame), EasingFunctionType.Linear));
			}
			return result;
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0005E774 File Offset: 0x0005C974
		private unsafe static List<PLayerKeyFrame> CreateDurationAnchorFrames(TimeSpan duration)
		{
			int num;
			if (duration <= TimeSpan.Zero)
			{
				num = 1;
				List<PLayerKeyFrame> list = new List<PLayerKeyFrame>(num);
				CollectionsMarshal.SetCount<PLayerKeyFrame>(list, num);
				*CollectionsMarshal.AsSpan<PLayerKeyFrame>(list)[0] = PLayerKeyFrame.Zero;
				return list;
			}
			num = 2;
			List<PLayerKeyFrame> list2 = new List<PLayerKeyFrame>(num);
			CollectionsMarshal.SetCount<PLayerKeyFrame>(list2, num);
			Span<PLayerKeyFrame> span = CollectionsMarshal.AsSpan<PLayerKeyFrame>(list2);
			*span[0] = PLayerKeyFrame.Zero;
			*span[1] = new PLayerKeyFrame(PlayerFrame.Zero, duration, EasingFunctionType.Linear);
			return list2;
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0005E7F8 File Offset: 0x0005C9F8
		private static TimeSpan FromVanillaFrameNumber(int frame)
		{
			return TimeSpan.FromMilliseconds((double)Math.Max(0, frame) * 1000.0 / 30.0);
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0005E81C File Offset: 0x0005CA1C
		private static PlayerFrame BuildPlayerFrame(Dictionary<string, List<AnimationElement>> frames, int index)
		{
			RightHandFrame? rightHand = (frames.ContainsKey("ItemAnchor") || frames.ContainsKey("LowerArmR") || frames.ContainsKey("UpperArmR")) ? new RightHandFrame?(new RightHandFrame(PLayerKeyFrame.GetFrameElement(frames, "ItemAnchor", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "LowerArmR", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "UpperArmR", index) ?? AnimationElement.Zero)) : default(RightHandFrame?);
			LeftHandFrame? leftHand = (frames.ContainsKey("ItemAnchorL") || frames.ContainsKey("LowerArmL") || frames.ContainsKey("UpperArmL")) ? new LeftHandFrame?(new LeftHandFrame(PLayerKeyFrame.GetFrameElement(frames, "ItemAnchorL", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "LowerArmL", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "UpperArmL", index) ?? AnimationElement.Zero)) : default(LeftHandFrame?);
			OtherPartsFrame? otherParts = (frames.ContainsKey("Neck") || frames.ContainsKey("Head") || frames.ContainsKey("UpperFootR") || frames.ContainsKey("UpperFootL") || frames.ContainsKey("LowerFootR") || frames.ContainsKey("LowerFootL")) ? new OtherPartsFrame?(new OtherPartsFrame(PLayerKeyFrame.GetFrameElement(frames, "Neck", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "Head", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "UpperFootR", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "UpperFootL", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "LowerFootR", index) ?? AnimationElement.Zero, PLayerKeyFrame.GetFrameElement(frames, "LowerFootL", index) ?? AnimationElement.Zero)) : default(OtherPartsFrame?);
			LeftHandFrame? leftHand2 = leftHand;
			OtherPartsFrame? otherParts2 = otherParts;
			AnimationElement? frameElement = PLayerKeyFrame.GetFrameElement(frames, "UpperTorso", index);
			AnimationElement? frameElement2 = PLayerKeyFrame.GetFrameElement(frames, "DetachedAnchor", index);
			bool detachedAnchor = false;
			bool switchArms = false;
			float pitchFollow = 0.8f;
			float fovMultiplier = 1f;
			float bobbingAmplitude = 1f;
			AnimationElement? frameElement3 = PLayerKeyFrame.GetFrameElement(frames, "LowerTorso", index);
			return new PlayerFrame(rightHand, leftHand2, otherParts2, frameElement, frameElement2, detachedAnchor, switchArms, pitchFollow, fovMultiplier, bobbingAmplitude, default(float?), frameElement3);
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0005EB0C File Offset: 0x0005CD0C
		private static AnimationElement? GetFrameElement(Dictionary<string, List<AnimationElement>> frames, string elementName, int index)
		{
			List<AnimationElement> values;
			if (!frames.TryGetValue(elementName, ref values) || index < 0 || index >= values.Count)
			{
				return default(AnimationElement?);
			}
			return new AnimationElement?(values[index]);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0005EB48 File Offset: 0x0005CD48
		private float GetInterpolatedProgress(float frameProgress)
		{
			EasingFunctions.EasingFunctionDelegate easingFunctionDelegate = EasingFunctions.Get(this.EasingFunction);
			float currentProgress = easingFunctionDelegate(this.GetAdjustedFrameProgress(frameProgress));
			float startProgress = easingFunctionDelegate(this.FrameProgressRange.X);
			float endProgress = easingFunctionDelegate(this.FrameProgressRange.Y);
			return (currentProgress - startProgress) / (endProgress - startProgress);
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0005EB98 File Offset: 0x0005CD98
		private float GetAdjustedFrameProgress(float frameProgress)
		{
			return this.FrameProgressRange.X + frameProgress * (this.FrameProgressRange.Y - this.FrameProgressRange.X);
		}

		// Token: 0x0400096E RID: 2414
		public readonly PlayerFrame Frame;

		// Token: 0x0400096F RID: 2415
		public readonly TimeSpan Time;

		// Token: 0x04000970 RID: 2416
		public readonly EasingFunctionType EasingFunction;

		// Token: 0x04000971 RID: 2417
		public readonly EasingFunctionType EasingType;

		// Token: 0x04000972 RID: 2418
		public readonly Vector2 FrameProgressRange;

		// Token: 0x04000973 RID: 2419
		public static readonly PLayerKeyFrame Zero = new PLayerKeyFrame(PlayerFrame.Zero, TimeSpan.Zero, EasingFunctionType.Linear);

		// Token: 0x04000974 RID: 2420
		private static readonly string[] PlayerElementNames = new string[]
		{
			"DetachedAnchor",
			"UpperTorso",
			"LowerTorso",
			"Neck",
			"Head",
			"UpperFootR",
			"UpperFootL",
			"LowerFootR",
			"LowerFootL",
			"ItemAnchor",
			"LowerArmR",
			"UpperArmR",
			"ItemAnchorL",
			"LowerArmL",
			"UpperArmL"
		};
	}
}
