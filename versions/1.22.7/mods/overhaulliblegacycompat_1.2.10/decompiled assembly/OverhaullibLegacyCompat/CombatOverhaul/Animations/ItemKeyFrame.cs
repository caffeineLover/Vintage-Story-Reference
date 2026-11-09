using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x0200018D RID: 397
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ItemKeyFrame
	{
		// Token: 0x06001353 RID: 4947 RVA: 0x0005DA9C File Offset: 0x0005BC9C
		public ItemKeyFrame(ItemFrame frame, float durationFraction, EasingFunctionType easeFunction)
		{
			this.Frame = frame;
			this.DurationFraction = durationFraction;
			this.EasingFunction = easeFunction;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0005DAB4 File Offset: 0x0005BCB4
		public ItemFrame Interpolate(ItemFrame frame, float frameProgress)
		{
			float interpolatedProgress = EasingFunctions.Get(this.EasingFunction)(frameProgress);
			return ItemFrame.Interpolate(frame, this.Frame, interpolatedProgress);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0005DAE1 File Offset: 0x0005BCE1
		public bool Reached(float animationProgress)
		{
			return animationProgress >= this.DurationFraction;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0005DAEF File Offset: 0x0005BCEF
		public static List<ItemKeyFrame> FromVanillaAnimation(string code, Shape shape)
		{
			return ItemKeyFrame.FromVanillaAnimation(ItemKeyFrame.GetVanillaKeyFrames(code, shape));
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0005DAFD File Offset: 0x0005BCFD
		public static List<ItemKeyFrame> FromVanillaAnimation(Animation animation)
		{
			return ItemKeyFrame.FromVanillaAnimation(animation.KeyFrames);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005DB0C File Offset: 0x0005BD0C
		private static List<ItemKeyFrame> FromVanillaAnimation(AnimationKeyFrame[] vanillaKeyFrames)
		{
			if (vanillaKeyFrames == null || vanillaKeyFrames.Length == 0)
			{
				return new List<ItemKeyFrame>();
			}
			vanillaKeyFrames = Enumerable.ToArray<AnimationKeyFrame>(Enumerable.OrderBy<AnimationKeyFrame, int>(vanillaKeyFrames, (AnimationKeyFrame keyFrame) => keyFrame.Frame));
			HashSet<string> elements = new HashSet<string>();
			foreach (AnimationKeyFrame keyFrame2 in vanillaKeyFrames)
			{
				if (keyFrame2.Elements != null)
				{
					foreach (KeyValuePair<string, AnimationKeyFrameElement> keyValuePair in keyFrame2.Elements)
					{
						string text;
						AnimationKeyFrameElement animationKeyFrameElement;
						keyValuePair.Deconstruct(ref text, ref animationKeyFrameElement);
						string element = text;
						elements.Add(element);
					}
				}
			}
			Dictionary<string, List<AnimationElement>> frames = new Dictionary<string, List<AnimationElement>>();
			foreach (string element2 in elements)
			{
				frames.Add(element2, ItemKeyFrame.GetElementFrames(element2, vanillaKeyFrames));
			}
			List<ItemKeyFrame> result = new List<ItemKeyFrame>();
			AnimationKeyFrame[] array2 = vanillaKeyFrames;
			int num;
			if (array2[array2.Length - 1].Frame != 0)
			{
				AnimationKeyFrame[] array3 = vanillaKeyFrames;
				num = array3[array3.Length - 1].Frame;
			}
			else
			{
				num = 1;
			}
			int lastFrame = num;
			if (vanillaKeyFrames[0].Frame != 0)
			{
				result.Add(new ItemKeyFrame(new ItemFrame(Enumerable.ToDictionary<KeyValuePair<string, List<AnimationElement>>, string, AnimationElement>(frames, (KeyValuePair<string, List<AnimationElement>> entry) => entry.Key, (KeyValuePair<string, List<AnimationElement>> entry) => entry.Value[0])), 0f, EasingFunctionType.Linear));
			}
			int index;
			int i;
			for (index = 0; index < vanillaKeyFrames.Length; index = i + 1)
			{
				float durationFraction = (float)vanillaKeyFrames[index].Frame / (float)lastFrame;
				result.Add(new ItemKeyFrame(new ItemFrame(Enumerable.ToDictionary<KeyValuePair<string, List<AnimationElement>>, string, AnimationElement>(frames, (KeyValuePair<string, List<AnimationElement>> entry) => entry.Key, (KeyValuePair<string, List<AnimationElement>> entry) => entry.Value[index])), durationFraction, EasingFunctionType.Linear));
				i = index;
			}
			return result;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0005DD3C File Offset: 0x0005BF3C
		private static uint ToCrc32(string value)
		{
			return GameMath.Crc32(value.ToLowerInvariant()) & 2147483647U;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0005DD50 File Offset: 0x0005BF50
		private static AnimationKeyFrame[] GetVanillaKeyFrames(string code, Shape shape)
		{
			Dictionary<uint, Animation> animationsByCrc = shape.AnimationsByCrc32;
			uint crc32 = ItemKeyFrame.ToCrc32(code);
			Animation value;
			if (!animationsByCrc.TryGetValue(crc32, ref value))
			{
				throw new InvalidOperationException("Animation '" + code + "' not found");
			}
			return value.KeyFrames;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0005DD90 File Offset: 0x0005BF90
		internal static List<AnimationElement> GetElementFrames(string elementName, AnimationKeyFrame[] vanillaFrames)
		{
			IEnumerable<AnimationKeyFrameElement> enumerable = Enumerable.Select<AnimationKeyFrame, AnimationKeyFrameElement>(Enumerable.Where<AnimationKeyFrame>(vanillaFrames, (AnimationKeyFrame element) => element.Elements != null && element.Elements.ContainsKey(elementName)), (AnimationKeyFrame element) => element.Elements[elementName]);
			AnimationElement firstFrame = AnimationElement.FromVanilla(Enumerable.First<AnimationKeyFrameElement>(enumerable));
			AnimationElement lastFrame = AnimationElement.FromVanilla(Enumerable.Last<AnimationKeyFrameElement>(enumerable));
			List<AnimationElement> result = new List<AnimationElement>();
			int previousFrameFrame = 0;
			AnimationElement previousFoundElement = firstFrame;
			int startingIndex = 0;
			if (vanillaFrames[0].Frame == 0)
			{
				startingIndex = 1;
			}
			result.Add(firstFrame);
			bool reachedLast = false;
			for (int index = startingIndex; index < vanillaFrames.Length; index++)
			{
				if (reachedLast)
				{
					result.Add(lastFrame);
				}
				else if (vanillaFrames[index].Elements != null && vanillaFrames[index].Elements.ContainsKey(elementName))
				{
					AnimationElement frame = AnimationElement.FromVanilla(vanillaFrames[index].Elements[elementName]);
					result.Add(frame);
					previousFoundElement = frame;
					previousFrameFrame = vanillaFrames[index].Frame;
				}
				else
				{
					int nextFrameFrame = 0;
					AnimationElement nextFrameElement = previousFoundElement;
					bool found = false;
					for (int nextIndex = index + 1; nextIndex < vanillaFrames.Length; nextIndex++)
					{
						if (vanillaFrames[nextIndex].Elements != null && vanillaFrames[nextIndex].Elements.ContainsKey(elementName))
						{
							nextFrameFrame = vanillaFrames[nextIndex].Frame;
							nextFrameElement = AnimationElement.FromVanilla(vanillaFrames[nextIndex].Elements[elementName]);
							found = true;
							break;
						}
					}
					if (!found)
					{
						reachedLast = true;
						result.Add(lastFrame);
					}
					else
					{
						float interpolationProgress = (float)(vanillaFrames[index].Frame - previousFrameFrame) / (float)(nextFrameFrame - previousFrameFrame);
						result.Add(AnimationElement.Interpolate(previousFoundElement, nextFrameElement, interpolationProgress));
					}
				}
			}
			return result;
		}

		// Token: 0x04000964 RID: 2404
		public readonly ItemFrame Frame;

		// Token: 0x04000965 RID: 2405
		public readonly float DurationFraction;

		// Token: 0x04000966 RID: 2406
		public readonly EasingFunctionType EasingFunction;

		// Token: 0x04000967 RID: 2407
		public static readonly ItemKeyFrame Empty = new ItemKeyFrame(ItemFrame.Empty, 0f, EasingFunctionType.Linear);
	}
}
