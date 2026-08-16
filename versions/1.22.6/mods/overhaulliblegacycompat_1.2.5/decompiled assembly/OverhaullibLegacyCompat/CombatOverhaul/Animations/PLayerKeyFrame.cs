using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000182 RID: 386
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct PLayerKeyFrame
	{
		// Token: 0x060012F3 RID: 4851 RVA: 0x0005CA8D File Offset: 0x0005AC8D
		public PLayerKeyFrame(PlayerFrame frame, TimeSpan easingTime, EasingFunctionType easeFunction, EasingFunctionType easingType, Vector2 frameProgressRange)
		{
			this.Frame = frame;
			this.Time = easingTime;
			this.EasingFunction = easeFunction;
			this.EasingType = easingType;
			this.FrameProgressRange = frameProgressRange;
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0005CAB4 File Offset: 0x0005ACB4
		public PLayerKeyFrame(PlayerFrame frame, TimeSpan easingTime, EasingFunctionType easeFunction)
		{
			this.Frame = frame;
			this.Time = easingTime;
			this.EasingFunction = easeFunction;
			this.EasingType = easeFunction;
			this.FrameProgressRange = new Vector2(0f, 1f);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0005CAE8 File Offset: 0x0005ACE8
		public PlayerFrame Interpolate(PlayerFrame frame, float frameProgress)
		{
			float interpolatedProgress = this.GetInterpolatedProgress(frameProgress);
			return PlayerFrame.Interpolate(frame, this.Frame, interpolatedProgress);
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0005CB0A File Offset: 0x0005AD0A
		public bool Reached(TimeSpan currentDuration)
		{
			return currentDuration >= this.Time;
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0005CB18 File Offset: 0x0005AD18
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

		// Token: 0x060012F8 RID: 4856 RVA: 0x0005CD20 File Offset: 0x0005AF20
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

		// Token: 0x060012F9 RID: 4857 RVA: 0x0005CDA4 File Offset: 0x0005AFA4
		private static TimeSpan FromVanillaFrameNumber(int frame)
		{
			return TimeSpan.FromMilliseconds((double)Math.Max(0, frame) * 1000.0 / 30.0);
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0005CDC8 File Offset: 0x0005AFC8
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

		// Token: 0x060012FB RID: 4859 RVA: 0x0005D0B8 File Offset: 0x0005B2B8
		private static AnimationElement? GetFrameElement(Dictionary<string, List<AnimationElement>> frames, string elementName, int index)
		{
			List<AnimationElement> values;
			if (!frames.TryGetValue(elementName, ref values) || index < 0 || index >= values.Count)
			{
				return default(AnimationElement?);
			}
			return new AnimationElement?(values[index]);
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x0005D0F4 File Offset: 0x0005B2F4
		private float GetInterpolatedProgress(float frameProgress)
		{
			EasingFunctions.EasingFunctionDelegate easingFunctionDelegate = EasingFunctions.Get(this.EasingFunction);
			float currentProgress = easingFunctionDelegate(this.GetAdjustedFrameProgress(frameProgress));
			float startProgress = easingFunctionDelegate(this.FrameProgressRange.X);
			float endProgress = easingFunctionDelegate(this.FrameProgressRange.Y);
			return (currentProgress - startProgress) / (endProgress - startProgress);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0005D144 File Offset: 0x0005B344
		private float GetAdjustedFrameProgress(float frameProgress)
		{
			return this.FrameProgressRange.X + frameProgress * (this.FrameProgressRange.Y - this.FrameProgressRange.X);
		}

		// Token: 0x04000946 RID: 2374
		public readonly PlayerFrame Frame;

		// Token: 0x04000947 RID: 2375
		public readonly TimeSpan Time;

		// Token: 0x04000948 RID: 2376
		public readonly EasingFunctionType EasingFunction;

		// Token: 0x04000949 RID: 2377
		public readonly EasingFunctionType EasingType;

		// Token: 0x0400094A RID: 2378
		public readonly Vector2 FrameProgressRange;

		// Token: 0x0400094B RID: 2379
		public static readonly PLayerKeyFrame Zero = new PLayerKeyFrame(PlayerFrame.Zero, TimeSpan.Zero, EasingFunctionType.Linear);

		// Token: 0x0400094C RID: 2380
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
