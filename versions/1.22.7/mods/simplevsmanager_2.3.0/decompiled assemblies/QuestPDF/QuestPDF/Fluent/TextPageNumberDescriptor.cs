using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements.Text.Items;

namespace QuestPDF.Fluent
{
	// Token: 0x020000AC RID: 172
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TextPageNumberDescriptor : TextSpanDescriptor
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x0000C62F File Offset: 0x0000A82F
		internal Action<PageNumberFormatter> AssignFormatFunction { get; }

		// Token: 0x060004D1 RID: 1233 RVA: 0x0000C637 File Offset: 0x0000A837
		internal TextPageNumberDescriptor(TextBlockSpan textBlockSpan, Action<PageNumberFormatter> assignFormatFunction) : base(textBlockSpan)
		{
			this.AssignFormatFunction = assignFormatFunction;
			this.AssignFormatFunction.Invoke(delegate(int? x)
			{
				if (x == null)
				{
					return null;
				}
				return x.GetValueOrDefault().ToString();
			});
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000C671 File Offset: 0x0000A871
		public TextPageNumberDescriptor Format(PageNumberFormatter formatter)
		{
			this.AssignFormatFunction.Invoke(formatter);
			return this;
		}
	}
}
