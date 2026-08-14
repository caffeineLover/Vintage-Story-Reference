using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001D3 RID: 467
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlException : Exception
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x000255AA File Offset: 0x000237AA
		public Mark Start { get; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x000255B2 File Offset: 0x000237B2
		public Mark End { get; }

		// Token: 0x06000AA4 RID: 2724 RVA: 0x000255BA File Offset: 0x000237BA
		public YamlException(string message) : this(Mark.Empty, Mark.Empty, message)
		{
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x000255CD File Offset: 0x000237CD
		public YamlException(in Mark start, in Mark end, string message) : this(start, end, message, null)
		{
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000255D9 File Offset: 0x000237D9
		public YamlException(in Mark start, in Mark end, string message, [Nullable(2)] Exception innerException) : base(message, innerException)
		{
			this.Start = start;
			this.End = end;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000255FC File Offset: 0x000237FC
		public YamlException(string message, Exception inner) : this(Mark.Empty, Mark.Empty, message, inner)
		{
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00025610 File Offset: 0x00023810
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(9, 3);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<Mark>(this.Start);
			defaultInterpolatedStringHandler.AppendLiteral(") - (");
			defaultInterpolatedStringHandler.AppendFormatted<Mark>(this.End);
			defaultInterpolatedStringHandler.AppendLiteral("): ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Message);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
