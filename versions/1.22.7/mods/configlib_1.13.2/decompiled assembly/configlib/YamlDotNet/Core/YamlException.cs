using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001CF RID: 463
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlException : Exception
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x00024C62 File Offset: 0x00022E62
		public Mark Start { get; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00024C6A File Offset: 0x00022E6A
		public Mark End { get; }

		// Token: 0x06000A68 RID: 2664 RVA: 0x00024C72 File Offset: 0x00022E72
		public YamlException(string message) : this(Mark.Empty, Mark.Empty, message)
		{
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00024C85 File Offset: 0x00022E85
		public YamlException(in Mark start, in Mark end, string message) : this(start, end, message, null)
		{
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00024C91 File Offset: 0x00022E91
		public YamlException(in Mark start, in Mark end, string message, [Nullable(2)] Exception innerException) : base(message, innerException)
		{
			this.Start = start;
			this.End = end;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00024CB4 File Offset: 0x00022EB4
		public YamlException(string message, Exception inner) : this(Mark.Empty, Mark.Empty, message, inner)
		{
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00024CC8 File Offset: 0x00022EC8
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
