using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001ED RID: 493
	[NullableContext(1)]
	public interface IParsingEventVisitor
	{
		// Token: 0x06000AD1 RID: 2769
		void Visit(AnchorAlias e);

		// Token: 0x06000AD2 RID: 2770
		void Visit(StreamStart e);

		// Token: 0x06000AD3 RID: 2771
		void Visit(StreamEnd e);

		// Token: 0x06000AD4 RID: 2772
		void Visit(DocumentStart e);

		// Token: 0x06000AD5 RID: 2773
		void Visit(DocumentEnd e);

		// Token: 0x06000AD6 RID: 2774
		void Visit(Scalar e);

		// Token: 0x06000AD7 RID: 2775
		void Visit(SequenceStart e);

		// Token: 0x06000AD8 RID: 2776
		void Visit(SequenceEnd e);

		// Token: 0x06000AD9 RID: 2777
		void Visit(MappingStart e);

		// Token: 0x06000ADA RID: 2778
		void Visit(MappingEnd e);

		// Token: 0x06000ADB RID: 2779
		void Visit(Comment e);
	}
}
