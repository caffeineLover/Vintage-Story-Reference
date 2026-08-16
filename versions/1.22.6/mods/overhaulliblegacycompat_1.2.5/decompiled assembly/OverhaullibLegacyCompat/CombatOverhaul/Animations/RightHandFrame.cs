using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000184 RID: 388
	public readonly struct RightHandFrame
	{
		// Token: 0x06001305 RID: 4869 RVA: 0x0005DAB9 File Offset: 0x0005BCB9
		public RightHandFrame(AnimationElement anchor, AnimationElement lower, AnimationElement upper)
		{
			this.ItemAnchor = anchor;
			this.LowerArmR = lower;
			this.UpperArmR = upper;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0005DAD0 File Offset: 0x0005BCD0
		[NullableContext(1)]
		public void Apply(ElementPose pose, EnumAnimatedElement element)
		{
			switch (element)
			{
			case EnumAnimatedElement.ItemAnchor:
				this.ItemAnchor.Apply(pose);
				return;
			case EnumAnimatedElement.LowerArmR:
				this.LowerArmR.Apply(pose);
				return;
			case EnumAnimatedElement.UpperArmR:
				this.UpperArmR.Apply(pose);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0005DB0E File Offset: 0x0005BD0E
		public static RightHandFrame Interpolate(RightHandFrame from, RightHandFrame to, float progress)
		{
			return new RightHandFrame(AnimationElement.Interpolate(from.ItemAnchor, to.ItemAnchor, progress), AnimationElement.Interpolate(from.LowerArmR, to.LowerArmR, progress), AnimationElement.Interpolate(from.UpperArmR, to.UpperArmR, progress));
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0005DB4C File Offset: 0x0005BD4C
		public static RightHandFrame Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<ValueTuple<RightHandFrame, float>> frames)
		{
			AnimationElement.Accumulator itemAnchor = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator lowerArm = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator upperArm = default(AnimationElement.Accumulator);
			foreach (ValueTuple<RightHandFrame, float> valueTuple in frames)
			{
				RightHandFrame element = valueTuple.Item1;
				float weight = valueTuple.Item2;
				itemAnchor.Add(element.ItemAnchor, weight);
				lowerArm.Add(element.LowerArmR, weight);
				upperArm.Add(element.UpperArmR, weight);
			}
			return new RightHandFrame(itemAnchor.ToElement(), lowerArm.ToElement(), upperArm.ToElement());
		}

		// Token: 0x04000961 RID: 2401
		public readonly AnimationElement ItemAnchor;

		// Token: 0x04000962 RID: 2402
		public readonly AnimationElement LowerArmR;

		// Token: 0x04000963 RID: 2403
		public readonly AnimationElement UpperArmR;

		// Token: 0x04000964 RID: 2404
		public static readonly RightHandFrame Zero = new RightHandFrame(AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero);
	}
}
