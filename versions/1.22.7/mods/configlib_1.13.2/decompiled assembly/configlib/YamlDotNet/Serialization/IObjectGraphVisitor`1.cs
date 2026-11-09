using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009B RID: 155
	[NullableContext(1)]
	public interface IObjectGraphVisitor<[Nullable(2)] TContext>
	{
		// Token: 0x0600039C RID: 924
		bool Enter(IObjectDescriptor value, TContext context);

		// Token: 0x0600039D RID: 925
		bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, TContext context);

		// Token: 0x0600039E RID: 926
		bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, TContext context);

		// Token: 0x0600039F RID: 927
		void VisitScalar(IObjectDescriptor scalar, TContext context);

		// Token: 0x060003A0 RID: 928
		void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, TContext context);

		// Token: 0x060003A1 RID: 929
		void VisitMappingEnd(IObjectDescriptor mapping, TContext context);

		// Token: 0x060003A2 RID: 930
		void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, TContext context);

		// Token: 0x060003A3 RID: 931
		void VisitSequenceEnd(IObjectDescriptor sequence, TContext context);
	}
}
