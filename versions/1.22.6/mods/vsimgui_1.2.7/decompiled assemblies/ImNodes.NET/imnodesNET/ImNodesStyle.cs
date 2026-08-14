using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace imnodesNET
{
	// Token: 0x0200000C RID: 12
	public struct ImNodesStyle
	{
		// Token: 0x04000038 RID: 56
		public float GridSpacing;

		// Token: 0x04000039 RID: 57
		public float NodeCornerRounding;

		// Token: 0x0400003A RID: 58
		public Vector2 NodePadding;

		// Token: 0x0400003B RID: 59
		public float NodeBorderThickness;

		// Token: 0x0400003C RID: 60
		public float LinkThickness;

		// Token: 0x0400003D RID: 61
		public float LinkLineSegmentsPerLength;

		// Token: 0x0400003E RID: 62
		public float LinkHoverDistance;

		// Token: 0x0400003F RID: 63
		public float PinCircleRadius;

		// Token: 0x04000040 RID: 64
		public float PinQuadSideLength;

		// Token: 0x04000041 RID: 65
		public float PinTriangleSideLength;

		// Token: 0x04000042 RID: 66
		public float PinLineThickness;

		// Token: 0x04000043 RID: 67
		public float PinHoverRadius;

		// Token: 0x04000044 RID: 68
		public float PinOffset;

		// Token: 0x04000045 RID: 69
		public Vector2 MiniMapPadding;

		// Token: 0x04000046 RID: 70
		public Vector2 MiniMapOffset;

		// Token: 0x04000047 RID: 71
		public ImNodesStyleFlags Flags;

		// Token: 0x04000048 RID: 72
		[FixedBuffer(typeof(uint), 29)]
		public ImNodesStyle.<Colors>e__FixedBuffer Colors;

		// Token: 0x02000014 RID: 20
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 116)]
		public struct <Colors>e__FixedBuffer
		{
			// Token: 0x04000065 RID: 101
			public uint FixedElementField;
		}
	}
}
