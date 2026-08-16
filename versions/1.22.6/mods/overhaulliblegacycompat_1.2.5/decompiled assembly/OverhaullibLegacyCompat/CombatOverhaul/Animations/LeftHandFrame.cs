using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000185 RID: 389
	public readonly struct LeftHandFrame
	{
		// Token: 0x0600130A RID: 4874 RVA: 0x0005DC1B File Offset: 0x0005BE1B
		public LeftHandFrame(AnimationElement anchor, AnimationElement lower, AnimationElement upper)
		{
			this.ItemAnchorL = anchor;
			this.LowerArmL = lower;
			this.UpperArmL = upper;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0005DC32 File Offset: 0x0005BE32
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

		// Token: 0x0600130C RID: 4876 RVA: 0x0005DC70 File Offset: 0x0005BE70
		public static LeftHandFrame Interpolate(LeftHandFrame from, LeftHandFrame to, float progress)
		{
			return new LeftHandFrame(AnimationElement.Interpolate(from.ItemAnchorL, to.ItemAnchorL, progress), AnimationElement.Interpolate(from.LowerArmL, to.LowerArmL, progress), AnimationElement.Interpolate(from.UpperArmL, to.UpperArmL, progress));
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0005DCB0 File Offset: 0x0005BEB0
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

		// Token: 0x04000965 RID: 2405
		public readonly AnimationElement ItemAnchorL;

		// Token: 0x04000966 RID: 2406
		public readonly AnimationElement LowerArmL;

		// Token: 0x04000967 RID: 2407
		public readonly AnimationElement UpperArmL;

		// Token: 0x04000968 RID: 2408
		public static readonly LeftHandFrame Zero = new LeftHandFrame(AnimationElement.Zero, AnimationElement.Zero, AnimationElement.Zero);
	}
}
