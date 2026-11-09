using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000193 RID: 403
	public readonly struct OtherPartsFrame
	{
		// Token: 0x06001386 RID: 4998 RVA: 0x0005F7D3 File Offset: 0x0005D9D3
		public OtherPartsFrame(AnimationElement neck, AnimationElement head, AnimationElement upperFootR, AnimationElement upperFootL, AnimationElement lowerFootR, AnimationElement lowerFootL)
		{
			this.Neck = neck;
			this.Head = head;
			this.UpperFootR = upperFootR;
			this.UpperFootL = upperFootL;
			this.LowerFootR = lowerFootR;
			this.LowerFootL = lowerFootL;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0005F804 File Offset: 0x0005DA04
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

		// Token: 0x06001388 RID: 5000 RVA: 0x0005F880 File Offset: 0x0005DA80
		public static OtherPartsFrame Interpolate(OtherPartsFrame from, OtherPartsFrame to, float progress)
		{
			return new OtherPartsFrame(AnimationElement.Interpolate(from.Neck, to.Neck, progress), AnimationElement.Interpolate(from.Head, to.Head, progress), AnimationElement.Interpolate(from.UpperFootR, to.UpperFootR, progress), AnimationElement.Interpolate(from.UpperFootL, to.UpperFootL, progress), AnimationElement.Interpolate(from.LowerFootR, to.LowerFootR, progress), AnimationElement.Interpolate(from.LowerFootL, to.LowerFootL, progress));
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0005F900 File Offset: 0x0005DB00
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

		// Token: 0x04000991 RID: 2449
		public readonly AnimationElement Neck;

		// Token: 0x04000992 RID: 2450
		public readonly AnimationElement Head;

		// Token: 0x04000993 RID: 2451
		public readonly AnimationElement UpperFootR;

		// Token: 0x04000994 RID: 2452
		public readonly AnimationElement UpperFootL;

		// Token: 0x04000995 RID: 2453
		public readonly AnimationElement LowerFootR;

		// Token: 0x04000996 RID: 2454
		public readonly AnimationElement LowerFootL;

		// Token: 0x04000997 RID: 2455
		public static readonly OtherPartsFrame Zero = new OtherPartsFrame(AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero);
	}
}
