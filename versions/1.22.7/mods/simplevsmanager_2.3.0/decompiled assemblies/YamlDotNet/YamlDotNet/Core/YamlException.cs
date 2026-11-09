using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000E0 RID: 224
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlException : Exception
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00016C8E File Offset: 0x00014E8E
		public Mark Start { get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00016C96 File Offset: 0x00014E96
		public Mark End { get; }

		// Token: 0x060005E3 RID: 1507 RVA: 0x00016C9E File Offset: 0x00014E9E
		public YamlException(string message) : this(Mark.Empty, Mark.Empty, message)
		{
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00016CB1 File Offset: 0x00014EB1
		public YamlException(in Mark start, in Mark end, string message) : this(start, end, message, null)
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00016CBD File Offset: 0x00014EBD
		public YamlException(in Mark start, in Mark end, string message, [Nullable(2)] Exception innerException) : base(message, innerException)
		{
			this.Start = start;
			this.End = end;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00016CE0 File Offset: 0x00014EE0
		public YamlException(string message, Exception inner) : this(Mark.Empty, Mark.Empty, message, inner)
		{
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00016CF4 File Offset: 0x00014EF4
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
