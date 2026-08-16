using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000186 RID: 390
	public readonly struct OtherPartsFrame
	{
		// Token: 0x0600130F RID: 4879 RVA: 0x0005DD7F File Offset: 0x0005BF7F
		public OtherPartsFrame(AnimationElement neck, AnimationElement head, AnimationElement upperFootR, AnimationElement upperFootL, AnimationElement lowerFootR, AnimationElement lowerFootL)
		{
			this.Neck = neck;
			this.Head = head;
			this.UpperFootR = upperFootR;
			this.UpperFootL = upperFootL;
			this.LowerFootR = lowerFootR;
			this.LowerFootL = lowerFootL;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0005DDB0 File Offset: 0x0005BFB0
		[NullableContext(1)]
		public void Apply(ElementPose pose, EnumAnimatedElement element)
		{
			switch (element)
			{
			case EnumAnimatedElement.Neck:
				this.Neck.Apply(pose);
				return;
			case EnumAnimatedElement.Head:
				this.Head.Apply(pose);
				return;
			case EnumAnimatedElement.UpperFootR:
				this.UpperFootR.Apply(pose);
				return;
			case EnumAnimatedElement.UpperFootL:
				this.UpperFootL.Apply(pose);
				return;
			case EnumAnimatedElement.LowerFootR:
				this.LowerFootR.Apply(pose);
				return;
			case EnumAnimatedElement.LowerFootL:
				this.LowerFootL.Apply(pose);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0005DE2C File Offset: 0x0005C02C
		public static OtherPartsFrame Interpolate(OtherPartsFrame from, OtherPartsFrame to, float progress)
		{
			return new OtherPartsFrame(AnimationElement.Interpolate(from.Neck, to.Neck, progress), AnimationElement.Interpolate(from.Head, to.Head, progress), AnimationElement.Interpolate(from.UpperFootR, to.UpperFootR, progress), AnimationElement.Interpolate(from.UpperFootL, to.UpperFootL, progress), AnimationElement.Interpolate(from.LowerFootR, to.LowerFootR, progress), AnimationElement.Interpolate(from.LowerFootL, to.LowerFootL, progress));
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0005DEAC File Offset: 0x0005C0AC
		public static OtherPartsFrame Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<ValueTuple<OtherPartsFrame, float>> frames)
		{
			AnimationElement.Accumulator neck = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator head = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator upperFootR = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator upperFootL = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator lowerFootR = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator lowerFootL = default(AnimationElement.Accumulator);
			foreach (ValueTuple<OtherPartsFrame, float> valueTuple in frames)
			{
				OtherPartsFrame element = valueTuple.Item1;
				float weight = valueTuple.Item2;
				neck.Add(element.Neck, weight);
				head.Add(element.Head, weight);
				upperFootR.Add(element.UpperFootR, weight);
				upperFootL.Add(element.UpperFootL, weight);
				lowerFootR.Add(element.LowerFootR, weight);
				lowerFootL.Add(element.LowerFootL, weight);
			}
			return new OtherPartsFrame(neck.ToElement(), head.ToElement(), upperFootR.ToElement(), upperFootL.ToElement(), lowerFootR.ToElement(), lowerFootL.ToElement());
		}

		// Token: 0x04000969 RID: 2409
		public readonly AnimationElement Neck;

		// Token: 0x0400096A RID: 2410
		public readonly AnimationElement Head;

		// Token: 0x0400096B RID: 2411
		public readonly AnimationElement UpperFootR;

		// Token: 0x0400096C RID: 2412
		public readonly AnimationElement UpperFootL;

		// Token: 0x0400096D RID: 2413
		public readonly AnimationElement LowerFootR;

		// Token: 0x0400096E RID: 2414
		public readonly AnimationElement LowerFootL;

		// Token: 0x0400096F RID: 2415
		public static readonly OtherPartsFrame Zero = new OtherPartsFrame(AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero);
	}
}
