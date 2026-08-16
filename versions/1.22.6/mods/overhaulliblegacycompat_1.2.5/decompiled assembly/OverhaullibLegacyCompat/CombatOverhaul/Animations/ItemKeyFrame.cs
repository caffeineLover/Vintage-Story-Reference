using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000180 RID: 384
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ItemKeyFrame
	{
		// Token: 0x060012DC RID: 4828 RVA: 0x0005C048 File Offset: 0x0005A248
		public ItemKeyFrame(ItemFrame frame, float durationFraction, EasingFunctionType easeFunction)
		{
			this.Frame = frame;
			this.DurationFraction = durationFraction;
			this.EasingFunction = easeFunction;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0005C060 File Offset: 0x0005A260
		public ItemFrame Interpolate(ItemFrame frame, float frameProgress)
		{
			float interpolatedProgress = EasingFunctions.Get(this.EasingFunction)(frameProgress);
			return ItemFrame.Interpolate(frame, this.Frame, interpolatedProgress);
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0005C08D File Offset: 0x0005A28D
		public bool Reached(float animationProgress)
		{
			return animationProgress >= this.DurationFraction;
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0005C09B File Offset: 0x0005A29B
		public static List<ItemKeyFrame> FromVanillaAnimation(string code, Shape shape)
		{
			return ItemKeyFrame.FromVanillaAnimation(ItemKeyFrame.GetVanillaKeyFrames(code, shape));
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0005C0A9 File Offset: 0x0005A2A9
		public static List<ItemKeyFrame> FromVanillaAnimation(Animation animation)
		{
			return ItemKeyFrame.FromVanillaAnimation(animation.KeyFrames);
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0005C0B8 File Offset: 0x0005A2B8
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

		// Token: 0x060012E2 RID: 4834 RVA: 0x0005C2E8 File Offset: 0x0005A4E8
		private static uint ToCrc32(string value)
		{
			return GameMath.Crc32(value.ToLowerInvariant()) & 2147483647U;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0005C2FC File Offset: 0x0005A4FC
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

		// Token: 0x060012E4 RID: 4836 RVA: 0x0005C33C File Offset: 0x0005A53C
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

		// Token: 0x0400093C RID: 2364
		public readonly ItemFrame Frame;

		// Token: 0x0400093D RID: 2365
		public readonly float DurationFraction;

		// Token: 0x0400093E RID: 2366
		public readonly EasingFunctionType EasingFunction;

		// Token: 0x0400093F RID: 2367
		public static readonly ItemKeyFrame Empty = new ItemKeyFrame(ItemFrame.Empty, 0f, EasingFunctionType.Linear);
	}
}
