using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000108 RID: 264
	[NullableContext(1)]
	[Nullable(0)]
	public class MappingEnd : ParsingEvent
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00017750 File Offset: 0x00015950
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x00017753 File Offset: 0x00015953
		internal override EventType Type
		{
			get
			{
				return EventType.MappingEnd;
			}
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00017757 File Offset: 0x00015957
		public MappingEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00017761 File Offset: 0x00015961
		public MappingEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00017773 File Offset: 0x00015973
		public override string ToString()
		{
			return "Mapping end";
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001777A File Offset: 0x0001597A
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
