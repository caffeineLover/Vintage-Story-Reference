using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000194 RID: 404
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct AnimationElement
	{
		// Token: 0x0600138B RID: 5003 RVA: 0x0005FA40 File Offset: 0x0005DC40
		public AnimationElement(float?[] values)
		{
			this.OffsetX = values[0];
			this.OffsetY = values[1];
			this.OffsetZ = values[2];
			this.RotationX = values[3];
			this.RotationY = values[4];
			this.RotationZ = values[5];
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0005FA9B File Offset: 0x0005DC9B
		public AnimationElement(float? offsetX, float? offsetY, float? offsetZ, float? rotationX, float? rotationY, float? rotationZ)
		{
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
			this.OffsetZ = offsetZ;
			this.RotationX = rotationX;
			this.RotationY = rotationY;
			this.RotationZ = rotationZ;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0005FACC File Offset: 0x0005DCCC
		public void Apply(ElementPose pose)
		{
			pose.translateX = (this.OffsetX / (float)16).GetValueOrDefault();
			pose.translateY = (this.OffsetY / (float)16).GetValueOrDefault();
			pose.translateZ = (this.OffsetZ / (float)16).GetValueOrDefault();
			pose.degX = this.RotationX.GetValueOrDefault();
			pose.degY = this.RotationY.GetValueOrDefault();
			pose.degZ = this.RotationZ.GetValueOrDefault();
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0005FBC0 File Offset: 0x0005DDC0
		public float?[] ToArray()
		{
			return new float?[]
			{
				this.OffsetX,
				this.OffsetY,
				this.OffsetZ,
				this.RotationX,
				this.RotationY,
				this.RotationZ
			};
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0005FC24 File Offset: 0x0005DE24
		public static AnimationElement Interpolate(AnimationElement from, AnimationElement to, float progress)
		{
			return new AnimationElement(from.OffsetX + (to.OffsetX - from.OffsetX) * progress, from.OffsetY + (to.OffsetY - from.OffsetY) * progress, from.OffsetZ + (to.OffsetZ - from.OffsetZ) * progress, from.RotationX + (to.RotationX - from.RotationX) * progress, from.RotationY + (to.RotationY - from.RotationY) * progress, from.RotationZ + (to.RotationZ - from.RotationZ) * progress);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0005FFF0 File Offset: 0x0005E1F0
		public static AnimationElement Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<ValueTuple<AnimationElement, float>> elements)
		{
			AnimationElement.Accumulator accumulator = default(AnimationElement.Accumulator);
			foreach (ValueTuple<AnimationElement, float> valueTuple in elements)
			{
				AnimationElement element = valueTuple.Item1;
				float weight = valueTuple.Item2;
				accumulator.Add(element, weight);
			}
			return accumulator.ToElement();
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00060058 File Offset: 0x0005E258
		public static AnimationElement FromVanilla(AnimationKeyFrameElement frame)
		{
			double? offsetX = frame.OffsetX;
			float? offsetX2 = new float?(((offsetX != null) ? new float?((float)offsetX.GetValueOrDefault()) : default(float?)).GetValueOrDefault());
			double? offsetY = frame.OffsetY;
			float? offsetY2 = new float?(((offsetY != null) ? new float?((float)offsetY.GetValueOrDefault()) : default(float?)).GetValueOrDefault());
			double? offsetZ = frame.OffsetZ;
			float? offsetZ2 = new float?(((offsetZ != null) ? new float?((float)offsetZ.GetValueOrDefault()) : default(float?)).GetValueOrDefault());
			double? rotationX = frame.RotationX;
			float? rotationX2 = new float?(((rotationX != null) ? new float?((float)rotationX.GetValueOrDefault()) : default(float?)).GetValueOrDefault());
			double? rotationY = frame.RotationY;
			float? rotationY2 = new float?(((rotationY != null) ? new float?((float)rotationY.GetValueOrDefault()) : default(float?)).GetValueOrDefault());
			double? rotationZ = frame.RotationZ;
			return new AnimationElement(offsetX2, offsetY2, offsetZ2, rotationX2, rotationY2, new float?(((rotationZ != null) ? new float?((float)rotationZ.GetValueOrDefault()) : default(float?)).GetValueOrDefault()));
		}

		// Token: 0x04000998 RID: 2456
		public readonly float? OffsetX;

		// Token: 0x04000999 RID: 2457
		public readonly float? OffsetY;

		// Token: 0x0400099A RID: 2458
		public readonly float? OffsetZ;

		// Token: 0x0400099B RID: 2459
		public readonly float? RotationX;

		// Token: 0x0400099C RID: 2460
		public readonly float? RotationY;

		// Token: 0x0400099D RID: 2461
		public readonly float? RotationZ;

		// Token: 0x0400099E RID: 2462
		public static readonly AnimationElement Zero = new AnimationElement(new float?(0f), new float?(0f), new float?(0f), new float?(0f), new float?(0f), new float?(0f));

		// Token: 0x020002D6 RID: 726
		[NullableContext(0)]
		internal struct Accumulator
		{
			// Token: 0x06001919 RID: 6425 RVA: 0x0006F9D0 File Offset: 0x0006DBD0
			public void Add(AnimationElement element, float weight)
			{
				if (weight > 0f)
				{
					if (weight >= this._offsetXMaxWeight && element.OffsetX != null)
					{
						this._offsetXMaxWeight = weight;
						this._offsetX = element.OffsetX.Value;
					}
					if (weight >= this._offsetYMaxWeight && element.OffsetY != null)
					{
						this._offsetYMaxWeight = weight;
						this._offsetY = element.OffsetY.Value;
					}
					if (weight >= this._offsetZMaxWeight && element.OffsetZ != null)
					{
						this._offsetZMaxWeight = weight;
						this._offsetZ = element.OffsetZ.Value;
					}
					if (weight >= this._rotationXMaxWeight && element.RotationX != null)
					{
						this._rotationXMaxWeight = weight;
						this._rotationX = element.RotationX.Value;
					}
					if (weight >= this._rotationYMaxWeight && element.RotationY != null)
					{
						this._rotationYMaxWeight = weight;
						this._rotationY = element.RotationY.Value;
					}
					if (weight >= this._rotationZMaxWeight && element.RotationZ != null)
					{
						this._rotationZMaxWeight = weight;
						this._rotationZ = element.RotationZ.Value;
					}
					return;
				}
				if (element.OffsetX != null)
				{
					this._offsetXAdditive += element.OffsetX.Value;
				}
				if (element.OffsetY != null)
				{
					this._offsetYAdditive += element.OffsetY.Value;
				}
				if (element.OffsetZ != null)
				{
					this._offsetZAdditive += element.OffsetZ.Value;
				}
				if (element.RotationX != null)
				{
					this._rotationXAdditive += element.RotationX.Value;
				}
				if (element.RotationY != null)
				{
					this._rotationYAdditive += element.RotationY.Value;
				}
				if (element.RotationZ != null)
				{
					this._rotationZAdditive += element.RotationZ.Value;
				}
			}

			// Token: 0x0600191A RID: 6426 RVA: 0x0006FBF4 File Offset: 0x0006DDF4
			public AnimationElement ToElement()
			{
				return new AnimationElement(new float?(this._offsetX + this._offsetXAdditive), new float?(this._offsetY + this._offsetYAdditive), new float?(this._offsetZ + this._offsetZAdditive), new float?(this._rotationX + this._rotationXAdditive), new float?(this._rotationY + this._rotationYAdditive), new float?(this._rotationZ + this._rotationZAdditive));
			}

			// Token: 0x04000E56 RID: 3670
			private float _offsetX;

			// Token: 0x04000E57 RID: 3671
			private float _offsetY;

			// Token: 0x04000E58 RID: 3672
			private float _offsetZ;

			// Token: 0x04000E59 RID: 3673
			private float _rotationX;

			// Token: 0x04000E5A RID: 3674
			private float _rotationY;

			// Token: 0x04000E5B RID: 3675
			private float _rotationZ;

			// Token: 0x04000E5C RID: 3676
			private float _offsetXMaxWeight;

			// Token: 0x04000E5D RID: 3677
			private float _offsetYMaxWeight;

			// Token: 0x04000E5E RID: 3678
			private float _offsetZMaxWeight;

			// Token: 0x04000E5F RID: 3679
			private float _rotationXMaxWeight;

			// Token: 0x04000E60 RID: 3680
			private float _rotationYMaxWeight;

			// Token: 0x04000E61 RID: 3681
			private float _rotationZMaxWeight;

			// Token: 0x04000E62 RID: 3682
			private float _offsetXAdditive;

			// Token: 0x04000E63 RID: 3683
			private float _offsetYAdditive;

			// Token: 0x04000E64 RID: 3684
			private float _offsetZAdditive;

			// Token: 0x04000E65 RID: 3685
			private float _rotationXAdditive;

			// Token: 0x04000E66 RID: 3686
			private float _rotationYAdditive;

			// Token: 0x04000E67 RID: 3687
			private float _rotationZAdditive;
		}
	}
}
