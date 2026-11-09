using System;
using Windows.Foundation;

namespace System.Numerics
{
	// Token: 0x02000017 RID: 23
	internal static class VectorExtensions
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00003A14 File Offset: 0x00001C14
		public static Point ToPoint(this Vector2 vector)
		{
			return new Point(vector.X, vector.Y);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003A27 File Offset: 0x00001C27
		public static Size ToSize(this Vector2 vector)
		{
			return new Size(vector.X, vector.Y);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003A3A File Offset: 0x00001C3A
		public static Vector2 ToVector2(this Point point)
		{
			return new Vector2((float)point.X, (float)point.Y);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003A51 File Offset: 0x00001C51
		public static Vector2 ToVector2(this Size size)
		{
			return new Vector2((float)size.Width, (float)size.Height);
		}
	}
}
