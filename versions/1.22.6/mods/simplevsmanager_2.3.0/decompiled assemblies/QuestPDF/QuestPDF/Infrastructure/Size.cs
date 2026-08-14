using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000062 RID: 98
	public readonly struct Size
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000591A File Offset: 0x00003B1A
		public static Size Zero { get; } = new Size(0f, 0f);

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00005921 File Offset: 0x00003B21
		public static Size Max { get; } = new Size(14400f, 14400f);

		// Token: 0x0600025B RID: 603 RVA: 0x00005928 File Offset: 0x00003B28
		public Size(float width, float height)
		{
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00005938 File Offset: 0x00003B38
		internal static bool Equal(Size first, Size second)
		{
			return Math.Abs(first.Width - second.Width) <= 0.01f && Math.Abs(first.Height - second.Height) <= 0.01f;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00005974 File Offset: 0x00003B74
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(19, 2);
			defaultInterpolatedStringHandler.AppendLiteral("(Width: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.Width, "N3");
			defaultInterpolatedStringHandler.AppendLiteral(", Height: ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.Height, "N3");
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400013A RID: 314
		public const float Epsilon = 0.01f;

		// Token: 0x0400013B RID: 315
		public const float Infinity = 14400f;

		// Token: 0x0400013C RID: 316
		public readonly float Width;

		// Token: 0x0400013D RID: 317
		public readonly float Height;
	}
}
