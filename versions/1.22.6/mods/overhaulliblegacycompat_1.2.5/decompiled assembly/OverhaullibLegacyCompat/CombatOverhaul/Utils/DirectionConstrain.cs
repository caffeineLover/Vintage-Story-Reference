using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000023 RID: 35
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct DirectionConstrain
	{
		// Token: 0x0600028F RID: 655 RVA: 0x0000ABD3 File Offset: 0x00008DD3
		public DirectionConstrain(Angle pitchTop, Angle pitchBottom, Angle yawRight, Angle yawLeft)
		{
			this.PitchTop = pitchTop;
			this.PitchBottom = pitchBottom;
			this.YawLeft = yawLeft;
			this.YawRight = yawRight;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000ABF2 File Offset: 0x00008DF2
		public static DirectionConstrain FromDegrees(float top, float bottom, float right, float left)
		{
			return new DirectionConstrain(Angle.FromDegrees(top), Angle.FromDegrees(-bottom), Angle.FromDegrees(right), Angle.FromDegrees(-left));
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000AC13 File Offset: 0x00008E13
		public static DirectionConstrain FromDegrees(float angle)
		{
			return new DirectionConstrain(Angle.FromDegrees(angle), Angle.FromDegrees(-angle), Angle.FromDegrees(angle), Angle.FromDegrees(-angle));
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000AC34 File Offset: 0x00008E34
		public bool Check(DirectionOffset offset)
		{
			return offset.Pitch <= this.PitchTop && offset.Pitch >= this.PitchBottom && offset.Yaw >= this.YawLeft && offset.Yaw <= this.YawRight;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000AC90 File Offset: 0x00008E90
		public float[] ToArray()
		{
			return new float[]
			{
				this.PitchTop.Degrees,
				-this.PitchBottom.Degrees,
				this.YawLeft.Degrees,
				-this.YawRight.Degrees
			};
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000ACDD File Offset: 0x00008EDD
		public DirectionConstrain Expand(Angle angle)
		{
			return new DirectionConstrain(this.PitchTop + angle, this.PitchBottom - angle, this.YawRight + angle, this.YawLeft - angle);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000AD14 File Offset: 0x00008F14
		public static DirectionConstrain FromArray(float[] array)
		{
			return new DirectionConstrain(Angle.FromDegrees(array[0]), Angle.FromDegrees(-array[1]), Angle.FromDegrees(array[2]), Angle.FromDegrees(-array[3]));
		}

		// Token: 0x040000FF RID: 255
		public readonly Angle PitchTop;

		// Token: 0x04000100 RID: 256
		public readonly Angle PitchBottom;

		// Token: 0x04000101 RID: 257
		public readonly Angle YawLeft;

		// Token: 0x04000102 RID: 258
		public readonly Angle YawRight;
	}
}
