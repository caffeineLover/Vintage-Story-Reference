using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000192 RID: 402
	public readonly struct LeftHandFrame
	{
		// Token: 0x06001381 RID: 4993 RVA: 0x0005F66F File Offset: 0x0005D86F
		public LeftHandFrame(AnimationElement anchor, AnimationElement lower, AnimationElement upper)
		{
			this.ItemAnchorL = anchor;
			this.LowerArmL = lower;
			this.UpperArmL = upper;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0005F686 File Offset: 0x0005D886
		[NullableContext(1)]
		public void Apply(ElementPose pose, EnumAnimatedElement element)
		{
			switch (element)
			{
			case EnumAnimatedElement.ItemAnchorL:
				this.ItemAnchorL.Apply(pose);
				return;
			case EnumAnimatedElement.LowerArmL:
				this.LowerArmL.Apply(pose);
				return;
			case EnumAnimatedElement.UpperArmL:
				this.UpperArmL.Apply(pose);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0005F6C4 File Offset: 0x0005D8C4
		public static LeftHandFrame Interpolate(LeftHandFrame from, LeftHandFrame to, float progress)
		{
			return new LeftHandFrame(AnimationElement.Interpolate(from.ItemAnchorL, to.ItemAnchorL, progress), AnimationElement.Interpolate(from.LowerArmL, to.LowerArmL, progress), AnimationElement.Interpolate(from.UpperArmL, to.UpperArmL, progress));
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0005F704 File Offset: 0x0005D904
		public static LeftHandFrame Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<ValueTuple<LeftHandFrame, float>> frames)
		{
			AnimationElement.Accumulator itemAnchor = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator lowerArm = default(AnimationElement.Accumulator);
			AnimationElement.Accumulator upperArm = default(AnimationElement.Accumulator);
			foreach (ValueTuple<LeftHandFrame, float> valueTuple in frames)
			{
				LeftHandFrame element = valueTuple.Item1;
				float weight = valueTuple.Item2;
				itemAnchor.Add(element.ItemAnchorL, weight);
				lowerArm.Add(element.LowerArmL, weight);
				upperArm.Add(element.UpperArmL, weight);
			}
			return new LeftHandFrame(itemAnchor.ToElement(), lowerArm.ToElement(), upperArm.ToElement());
		}

		// Token: 0x0400098D RID: 2445
		public readonly AnimationElement ItemAnchorL;

		// Token: 0x0400098E RID: 2446
		public readonly AnimationElement LowerArmL;

		// Token: 0x0400098F RID: 2447
		public readonly AnimationElement UpperArmL;

		// Token: 0x04000990 RID: 2448
		public static readonly LeftHandFrame Zero = new LeftHandFrame(AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero);
	}
}
