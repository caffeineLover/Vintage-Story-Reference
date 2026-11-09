using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000191 RID: 401
	public readonly struct RightHandFrame
	{
		// Token: 0x0600137C RID: 4988 RVA: 0x0005F50D File Offset: 0x0005D70D
		public RightHandFrame(AnimationElement anchor, AnimationElement lower, AnimationElement upper)
		{
			this.ItemAnchor = anchor;
			this.LowerArmR = lower;
			this.UpperArmR = upper;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0005F524 File Offset: 0x0005D724
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

		// Token: 0x0600137E RID: 4990 RVA: 0x0005F562 File Offset: 0x0005D762
		public static RightHandFrame Interpolate(RightHandFrame from, RightHandFrame to, float progress)
		{
			return new RightHandFrame(AnimationElement.Interpolate(from.ItemAnchor, to.ItemAnchor, progress), AnimationElement.Interpolate(from.LowerArmR, to.LowerArmR, progress), AnimationElement.Interpolate(from.UpperArmR, to.UpperArmR, progress));
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0005F5A0 File Offset: 0x0005D7A0
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

		// Token: 0x04000989 RID: 2441
		public readonly AnimationElement ItemAnchor;

		// Token: 0x0400098A RID: 2442
		public readonly AnimationElement LowerArmR;

		// Token: 0x0400098B RID: 2443
		public readonly AnimationElement UpperArmR;

		// Token: 0x0400098C RID: 2444
		public static readonly RightHandFrame Zero = new RightHandFrame(AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero);
	}
}
