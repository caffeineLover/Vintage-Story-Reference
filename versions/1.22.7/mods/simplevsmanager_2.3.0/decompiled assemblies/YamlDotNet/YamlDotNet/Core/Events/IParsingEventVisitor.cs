using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000107 RID: 263
	[NullableContext(1)]
	public interface IParsingEventVisitor
	{
		// Token: 0x06000667 RID: 1639
		void Visit(AnchorAlias e);

		// Token: 0x06000668 RID: 1640
		void Visit(StreamStart e);

		// Token: 0x06000669 RID: 1641
		void Visit(StreamEnd e);

		// Token: 0x0600066A RID: 1642
		void Visit(DocumentStart e);

		// Token: 0x0600066B RID: 1643
		void Visit(DocumentEnd e);

		// Token: 0x0600066C RID: 1644
		void Visit(Scalar e);

		// Token: 0x0600066D RID: 1645
		void Visit(SequenceStart e);

		// Token: 0x0600066E RID: 1646
		void Visit(SequenceEnd e);

		// Token: 0x0600066F RID: 1647
		void Visit(MappingStart e);

		// Token: 0x06000670 RID: 1648
		void Visit(MappingEnd e);

		// Token: 0x06000671 RID: 1649
		void Visit(Comment e);
	}
}
