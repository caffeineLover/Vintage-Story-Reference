using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F1 RID: 497
	[NullableContext(1)]
	public interface IParsingEventVisitor
	{
		// Token: 0x06000B0D RID: 2829
		void Visit(AnchorAlias e);

		// Token: 0x06000B0E RID: 2830
		void Visit(StreamStart e);

		// Token: 0x06000B0F RID: 2831
		void Visit(StreamEnd e);

		// Token: 0x06000B10 RID: 2832
		void Visit(DocumentStart e);

		// Token: 0x06000B11 RID: 2833
		void Visit(DocumentEnd e);

		// Token: 0x06000B12 RID: 2834
		void Visit(Scalar e);

		// Token: 0x06000B13 RID: 2835
		void Visit(SequenceStart e);

		// Token: 0x06000B14 RID: 2836
		void Visit(SequenceEnd e);

		// Token: 0x06000B15 RID: 2837
		void Visit(MappingStart e);

		// Token: 0x06000B16 RID: 2838
		void Visit(MappingEnd e);

		// Token: 0x06000B17 RID: 2839
		void Visit(Comment e);
	}
}
