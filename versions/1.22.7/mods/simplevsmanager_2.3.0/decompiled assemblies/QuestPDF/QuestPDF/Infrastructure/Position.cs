using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000061 RID: 97
	internal readonly struct Position
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00005843 File Offset: 0x00003A43
		public static Position Zero
		{
			get
			{
				return new Position(0f, 0f);
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00005854 File Offset: 0x00003A54
		public Position(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00005864 File Offset: 0x00003A64
		public Position Reverse()
		{
			return new Position(-this.X, -this.Y);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00005879 File Offset: 0x00003A79
		public static bool Equal(Position first, Position second)
		{
			return Math.Abs(first.X - second.X) <= 0.01f && Math.Abs(first.Y - second.Y) <= 0.01f;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000058B4 File Offset: 0x00003AB4
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(15, 2);
			defaultInterpolatedStringHandler.AppendLiteral("(Left: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.X, "N3");
			defaultInterpolatedStringHandler.AppendLiteral(", Top: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.Y, "N3");
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000138 RID: 312
		public readonly float X;

		// Token: 0x04000139 RID: 313
		public readonly float Y;
	}
}
