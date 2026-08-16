using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000187 RID: 391
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct AnimationElement
	{
		// Token: 0x06001314 RID: 4884 RVA: 0x0005DFEC File Offset: 0x0005C1EC
		public AnimationElement(float?[] values)
		{
			this.OffsetX = values[0];
			this.OffsetY = values[1];
			this.OffsetZ = values[2];
			this.RotationX = values[3];
			this.RotationY = values[4];
			this.RotationZ = values[5];
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0005E047 File Offset: 0x0005C247
		public AnimationElement(float? offsetX, float? offsetY, float? offsetZ, float? rotationX, float? rotationY, float? rotationZ)
		{
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
			this.OffsetZ = offsetZ;
			this.RotationX = rotationX;
			this.RotationY = rotationY;
			this.RotationZ = rotationZ;
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0005E078 File Offset: 0x0005C278
		public void Apply(ElementPose pose)
		{
			pose.translateX = (this.OffsetX / (float)16).GetValueOrDefault();
			pose.translateY = (this.OffsetY / (float)16).GetValueOrDefault();
			pose.translateZ = (this.OffsetZ / (float)16).GetValueOrDefault();
			pose.degX = this.RotationX.GetValueOrDefault();
			pose.degY = this.RotationY.GetValueOrDefault();
			pose.degZ = this.RotationZ.GetValueOrDefault();
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x0005E16C File Offset: 0x0005C36C
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

		// Token: 0x06001318 RID: 4888 RVA: 0x0005E1D0 File Offset: 0x0005C3D0
		public static AnimationElement Interpolate(AnimationElement from, AnimationElement to, float progress)
		{
			return new AnimationElement(from.OffsetX + (to.OffsetX - from.OffsetX) * progress, from.OffsetY + (to.OffsetY - from.OffsetY) * progress, from.OffsetZ + (to.OffsetZ - from.OffsetZ) * progress, from.RotationX + (to.RotationX - from.RotationX) * progress, from.RotationY + (to.RotationY - from.RotationY) * progress, from.RotationZ + (to.RotationZ - from.RotationZ) * progress);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x0005E59C File Offset: 0x0005C79C
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

		// Token: 0x0600131A RID: 4890 RVA: 0x0005E604 File Offset: 0x0005C804
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

		// Token: 0x04000970 RID: 2416
		public readonly float? OffsetX;

		// Token: 0x04000971 RID: 2417
		public readonly float? OffsetY;

		// Token: 0x04000972 RID: 2418
		public readonly float? OffsetZ;

		// Token: 0x04000973 RID: 2419
		public readonly float? RotationX;

		// Token: 0x04000974 RID: 2420
		public readonly float? RotationY;

		// Token: 0x04000975 RID: 2421
		public readonly float? RotationZ;

		// Token: 0x04000976 RID: 2422
		public static readonly AnimationElement Zero = new AnimationElement(new float?(0f), new float?(0f), new float?(0f), new float?(0f), new float?(0f), new float?(0f));

		// Token: 0x020002C3 RID: 707
		[NullableContext(0)]
		internal struct Accumulator
		{
			// Token: 0x06001874 RID: 6260 RVA: 0x0006D434 File Offset: 0x0006B634
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

			// Token: 0x06001875 RID: 6261 RVA: 0x0006D658 File Offset: 0x0006B858
			public AnimationElement ToElement()
			{
				return new AnimationElement(new float?(this._offsetX + this._offsetXAdditive), new float?(this._offsetY + this._offsetYAdditive), new float?(this._offsetZ + this._offsetZAdditive), new float?(this._rotationX + this._rotationXAdditive), new float?(this._rotationY + this._rotationYAdditive), new float?(this._rotationZ + this._rotationZAdditive));
			}

			// Token: 0x04000E0A RID: 3594
			private float _offsetX;

			// Token: 0x04000E0B RID: 3595
			private float _offsetY;

			// Token: 0x04000E0C RID: 3596
			private float _offsetZ;

			// Token: 0x04000E0D RID: 3597
			private float _rotationX;

			// Token: 0x04000E0E RID: 3598
			private float _rotationY;

			// Token: 0x04000E0F RID: 3599
			private float _rotationZ;

			// Token: 0x04000E10 RID: 3600
			private float _offsetXMaxWeight;

			// Token: 0x04000E11 RID: 3601
			private float _offsetYMaxWeight;

			// Token: 0x04000E12 RID: 3602
			private float _offsetZMaxWeight;

			// Token: 0x04000E13 RID: 3603
			private float _rotationXMaxWeight;

			// Token: 0x04000E14 RID: 3604
			private float _rotationYMaxWeight;

			// Token: 0x04000E15 RID: 3605
			private float _rotationZMaxWeight;

			// Token: 0x04000E16 RID: 3606
			private float _offsetXAdditive;

			// Token: 0x04000E17 RID: 3607
			private float _offsetYAdditive;

			// Token: 0x04000E18 RID: 3608
			private float _offsetZAdditive;

			// Token: 0x04000E19 RID: 3609
			private float _rotationXAdditive;

			// Token: 0x04000E1A RID: 3610
			private float _rotationYAdditive;

			// Token: 0x04000E1B RID: 3611
			private float _rotationZAdditive;
		}
	}
}
